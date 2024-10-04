using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class SpecialStageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject specialStageUI;

    public int[] specialStageLevels = new int[3] { 1, 1, 1 };
    private int currentSpecialStageIndex;

    public float playSec = 13.0f;               // ����� �������� ���� �ð�
    public float playDuration = 10.0f;
    public float goldInterval = 0.5f;
    public int baseGoldAmount = 100000;
    private Coroutine goldCoroutine;
    private bool isSuccess;
    private int gainGold = 100000;               // �⺻ ��� ȹ�淮
    // �ӽ� ��ü�� ����� cat�� enemy ������
    public Character catPrefab;
    public Enemy enemyPrefab;
    private Character catInstance;
    private Enemy enemyInstance;

    // �̱��� �ν��Ͻ�
    public static SpecialStageManager Instance { get; private set; }

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
    public void StartSpecialStage(int index, int level)
    {
        // ����� ���������� �̹� ���� ������ Ȯ��
        if (GameManager.isSpecialStageActive)
        {
            Debug.LogWarning("����� ���������� �̹� Ȱ��ȭ�Ǿ� �ֽ��ϴ�.");
            return;
        }

        // �̴ϰ����� ���� ���̸� ����� ���������� ������ �� ����
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
        GameManager.isSpecialStageActive = true;

        // ������ �ν��Ͻ� ����
        catInstance = Instantiate(catPrefab, new Vector3(-2, 0, 0), Quaternion.identity).GetComponent<Character>();
        enemyInstance = Instantiate(enemyPrefab, new Vector3(2, 0, 0), Quaternion.identity).GetComponent<Enemy>();

        // ������ ��
        enemyInstance.SetNumberOfEnemyInGroup(1);

        //enemyInstance.SetEnemyStatus(1);  // ������ ���� ������ ������ ������Ŵ
        // ����̿� ������ ���� ���·� ����
        catInstance.SetEnemy(enemyInstance);
        enemyInstance.SetEnemy(catInstance);

        currentSpecialStageIndex = index;
        specialStageUI.SetActive(true);  // UI Ȱ��ȭ

        // ���� �ð� ���� ��带 ȹ���ϴ� �ڷ�ƾ
        // goldCoroutine = StartCoroutine(GainGoldOverTime(level));
        DummyServerData.UseTicket(Player.GetUserID(), index); // Ƽ�� ����

        // ���� ��� üũ ����
        StartCoroutine(CheckBattleOutcome());
        // �������� ���� �ð� ����
        Invoke("TimeOut", playSec);
    }

    private IEnumerator GainGoldOverTime(int level)
    {
        while (GameManager.isSpecialStageActive)
        {
            Player.AddGold(baseGoldAmount * level);
            yield return new WaitForSeconds(goldInterval);
        }
    }

    // ���� ��� üũ
    private IEnumerator CheckBattleOutcome()
    {
        while (GameManager.isSpecialStageActive)
        {
            // ������ ������ Ŭ����
            if (enemyInstance != null && enemyInstance.IsDead())
            {
                isSuccess = true;
                EndSpecialStage();
                yield break;
            }

            // �÷��̾ ������ ����
            if (catInstance != null && catInstance.IsDead())
            {
                isSuccess = false;
                EndSpecialStage();
                yield break;
            }

            yield return null;
        }
    }

    // ���� �ð� �ʰ� �� ���� ó��
    private void TimeOut()
    {
        if (GameManager.isSpecialStageActive)
        {
            Debug.Log("���� �ð��� �ʰ��Ǿ����ϴ�.");
            isSuccess = false;
            EndSpecialStage(); // ���� ó��
        }
    }

    public void EndSpecialStage()
    {
        GameManager.isSpecialStageActive = false;
        specialStageUI.SetActive(false);

        if (goldCoroutine != null)
            StopCoroutine(goldCoroutine);

        // ���� ó��
        if (isSuccess)
        {
            if (currentSpecialStageIndex >= 0 && currentSpecialStageIndex < specialStageLevels.Length)
            {
                specialStageLevels[currentSpecialStageIndex]++;
                Debug.Log($"����� �������� {currentSpecialStageIndex + 1} Ŭ����! ���� ������ Ȱ��ȭ�˴ϴ�.");
                Player.AddGold(specialStageLevels[currentSpecialStageIndex]*gainGold);
                // SpecialStageMenuPanel�� Ŭ����� �������� ������Ʈ
                var specialStageMenuPanel = FindObjectOfType<SpecialStageMenuPanel>();
                if (specialStageMenuPanel != null)
                {
                    specialStageMenuPanel.OnStageCleared(currentSpecialStageIndex, specialStageLevels[currentSpecialStageIndex]);
                }
            }
        }
        else
        {
            Debug.Log($"����� �������� {currentSpecialStageIndex + 1} ����.");
        }

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
    }
}
