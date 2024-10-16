using System;
using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject DungeonUI;
    private GameObject[] dungeonPanels;
    private TextMeshProUGUI DungeonResultText;

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

        // DungeonUI >> Panel >> Dungeon �������� Dungeon �гε��� �������� ã��
        Transform panelTransform = DungeonUI.transform.Find("Panel");
        if (panelTransform != null)
        {
            dungeonPanels = new GameObject[3];
            for (int i = 0; i < 3; i++)
            {
                dungeonPanels[i] = panelTransform.Find($"Dungeon ({i})").gameObject;  // Dungeon[0], Dungeon[1], Dungeon[2]�� ã��
            }
        }
        else
        {
            Debug.LogError("Panel�� ã�� �� �����ϴ�.");
        }

        // DungeonUI >> Panel >> DungeonResultText �������� ã��
        DungeonResultText = panelTransform.Find("DungeonResultText").GetComponent<TextMeshProUGUI>();
        if (DungeonResultText == null)
        {
            Debug.LogError("DungeonResultText�� ã�� �� �����ϴ�.");
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

        ShowDungeonResultText("START!!", 1);
        // index�� �´� UI �гθ� Ȱ��ȭ
        for (int i = 0; i < dungeonPanels.Length; i++)
        {
            dungeonPanels[i].SetActive(i == index); // ���� ������ index�� �ش��ϴ� �гθ� Ȱ��ȭ
        }

        // ������ �ν��Ͻ� ����
        catInstance = Instantiate(catPrefab, new Vector3(-10, 40, 0), Quaternion.identity).GetComponent<Character>();
        enemyInstance = Instantiate(enemyPrefab, new Vector3(5, 40, 0), Quaternion.identity).GetComponent<DungeonEnemy>();
        
        // ���� �����, ���ݷ�, ���� ���� ���� (index�� level�� ���� �ٸ��� ����)
        enemyInstance.InitializeEnemyStats(index, level);


        currentDungeonIndex = index;
        DungeonUI.SetActive(true);  // UI Ȱ��ȭ

        // ���� �ð� ���� ��带 ȹ���ϴ� �ڷ�ƾ
        // goldCoroutine = StartCoroutine(GainGoldOverTime(level));

        DummyServerData.UseTicket(Player.GetUserID(), index); // Ƽ�� ����

        StartCoroutine(StartCombatAfterDelay(1.0f));
        StartCoroutine(CheckBattleOutcome());
        // �������� ���� �ð� ����
        Invoke("TimeOut", playDuration);
    }

    // ���� �ð� ������ �� ���� ����
    private IEnumerator StartCombatAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // ���� ����
        catInstance.SetEnemy(enemyInstance);
        enemyInstance.SetEnemy(catInstance);
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
            ShowDungeonResultText("TIME OUT...", 2);
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
                ShowDungeonResultText("CLEAR!!", 2);
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
            ShowDungeonResultText("FAIL...", 2);
        }

        StartCoroutine(DestroyObjectsWithDelay(3.0f));
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

    // DungeonResultText
    public void ShowDungeonResultText(string message, float displayDuration)
    {
        DungeonResultText.text = message;
        DungeonResultText.gameObject.SetActive(true);
        StartCoroutine(DisableDungeonResultTextAfterDelay(displayDuration));
    }

    private IEnumerator DisableDungeonResultTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DungeonResultText.gameObject.SetActive(false);
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


