using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject DungeonUI;

    public int[] DungeonLevels = new int[3] { 1, 1, 1 };
    private int currentDungeonIndex;

    // ����� �������� ���� �ð�
    public float playDuration = 10.0f;

    public int baseGoldAmount = 100000;
    private Coroutine goldCoroutine;
    private bool isSuccess;
    private int gainGold = 100000;               // �⺻ ��� ȹ�淮
    // �ӽ� ��ü�� ����� cat�� enemy ������
    public Character catPrefab;
    public DungeonEnemy enemyPrefab;
    private Character catInstance;
    private DungeonEnemy enemyInstance;

    // �̱��� �ν��Ͻ�
    public static DungeonManager Instance { get; private set; }

    private void Awake()
    {
        // �̱��� ���� ����
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ����� �������� ����
    public void StartDungeon(int index, int level)
    {
        if (GameManager.isDungeonActive)
        {
            Debug.Log("����� ���������� �̹� Ȱ��ȭ�Ǿ� �ֽ��ϴ�.");
            return;
        }

        if (GameManager.isMiniGameActive)
        {
            Debug.Log("�̴ϰ����� ���� ���̹Ƿ� ����� ���������� ������ �� �����ϴ�.");
            return;
        }

        // Ƽ�� Ȯ��
        if (!DummyServerData.HasTicket(Player.GetUserID(), index))
        {
            Debug.Log("�������� �����Ͽ� ����� ���������� ������ �� �����ϴ�.");
            return;
        }

        // ���� Invoke ȣ���� ���� ��� ���
        CancelInvoke("TimeOut");

        // ���� Ȱ��ȭ
        GameManager.isDungeonActive = true;

        // ������ �ν��Ͻ� ����
        catInstance = Instantiate(catPrefab, new Vector3(-10, 40, 0), Quaternion.identity).GetComponent<Character>();
        enemyInstance = Instantiate(enemyPrefab, new Vector3(5, 40, 0), Quaternion.identity).GetComponent<DungeonEnemy>();
        
        // ���� �����, ���ݷ�, ���� ���� ���� (index�� level�� ���� �ٸ��� ����)
        enemyInstance.InitializeEnemyStats(index, level);

        // ����̿� ������ ���� ���·� ����
        catInstance.SetEnemy(enemyInstance);
        enemyInstance.SetEnemy(catInstance);

        currentDungeonIndex = index;
        DungeonUI.SetActive(true);  // UI Ȱ��ȭ

        // ���� �ð� ���� ��带 ȹ���ϴ� �ڷ�ƾ
        // goldCoroutine = StartCoroutine(GainGoldOverTime(level));

        DummyServerData.UseTicket(Player.GetUserID(), index); // Ƽ�� ����

        // ���� ��� üũ ����
        StartCoroutine(CheckBattleOutcome());
        // �������� ���� �ð� ����
        Invoke("TimeOut", playDuration);
    }

    private IEnumerator GainGoldOverTime(int level)
    {
        while (GameManager.isDungeonActive)
        {
            Player.AddGold(baseGoldAmount * level);
            yield return new WaitForSeconds(gainGold);
        }
    }

    // ���� ��� üũ
    private IEnumerator CheckBattleOutcome()
    {
        while (GameManager.isDungeonActive)
        {
            // ������ ������ Ŭ����
            if (enemyInstance != null && enemyInstance.IsDead())
            {
                isSuccess = true;
                EndDungeonStage();
                yield break;
            }

            // �÷��̾ ������ ����
            if (catInstance != null && catInstance.IsDead())
            {
                isSuccess = false;
                EndDungeonStage();
                yield break;
            }

            yield return null;
        }
    }

    // ���� �ð� �ʰ� �� ���� ó��
    private void TimeOut()
    {
        if (GameManager.isDungeonActive)
        {
            Debug.Log("���� �ð��� �ʰ��Ǿ����ϴ�.");
            isSuccess = false;
            EndDungeonStage(); // ���� ó��
        }
    }

    public void EndDungeonStage()
    {
        if (goldCoroutine != null)
            StopCoroutine(goldCoroutine);

        // ���� ó��
        if (isSuccess)
        {
            if (currentDungeonIndex >= 0 && currentDungeonIndex < DungeonLevels.Length)
            {
                DungeonLevels[currentDungeonIndex]++;
                Debug.Log($"���� {currentDungeonIndex + 1} Ŭ����! ���� ������ Ȱ��ȭ�˴ϴ�.");
                Player.AddGold(DungeonLevels[currentDungeonIndex] * gainGold);

                var DungeonPanel = FindObjectOfType<DungeonPanel>();
                if (DungeonPanel != null)
                {
                    DungeonPanel.OnStageCleared(currentDungeonIndex, DungeonLevels[currentDungeonIndex]);
                }
            }
        }
        else
        {
            Debug.Log($"����� �������� {currentDungeonIndex + 1} ����.");
        }

        StartCoroutine(DestroyObjectsWithDelay(2.0f));
    }

    private IEnumerator DestroyObjectsWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // cat�� enemy ��ü ����
        if (catInstance != null)
        {
            Destroy(catInstance.gameObject);
            catInstance = null;
        }

        if (enemyInstance != null)
        {
            Destroy(enemyInstance.gameObject);
            enemyInstance = null;
        }
        DungeonUI.SetActive(false);

        GameManager.isDungeonActive = false;
    }
}
public class DungeonRewardManager
{
    public void GiveDungeonReward(string userID, int index, int level)
    {
        int goldReward = CalculateGoldReward(index, level);
        int itemReward = CalculateItemReward(index, level);

        // ���� ����
        Player.AddGold(goldReward);
        //Player.AddItem(itemReward); // ������ ����... �߰� ����

        Debug.Log($"���� {userID}���� ���� {index + 1} ���� {level} ������ �����߽��ϴ�. ���: {goldReward}, ������: {itemReward}");
    }

    private int CalculateGoldReward(int index, int level)
    {
        // index�� level�� ���� ������ �ٸ��� ����
        return 10000 * (index + 1) * level; // �⺻ ��� ���� ����
    }

    private int CalculateItemReward(int index, int level)
    {
        // Ư�� ���� ���� �� ������ ���� ���� ����
        return (index + 1) * level; // ������ ���� ����
    }
}
