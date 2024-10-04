using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class SpecialStageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject specialStageUI;
    private StageManager stageManager;

    public int[] specialStageLevels = new int[3] { 1, 1, 1 };
    private int currentSpecialStageIndex;
    private int originalBackground;             // ���� ��� �׸� ����

    public float playSec = 13.0f;               // ����� �������� ���� �ð�
    public float playDuration = 10.0f;
    public float goldInterval = 0.5f;
    public int baseGoldAmount = 100000;
    private Coroutine goldCoroutine;
    private bool isSuccess;

    // ���̵� �迭
    private int[] stageDifficulties = new int[3] { 10, 15, 20 };

    // �ӽ� ��ü�� ����� cat�� enemy ������
    public Character playerPrefab;
    public Enemy enemyPrefab;
    private Character playerInstance;
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

    private void Start()
    {
        stageManager = FindObjectOfType<StageManager>() ?? throw new NullReferenceException("StageManager is missing.");
    }

    public void EndSpecialStage(bool playerWon)
    {
        if (playerWon)
        {
            Debug.Log("Ŭ����!");
        }
        else
        {
            Debug.Log("����");
        }

        // ��ü ���� (cat�� enemy ����)
        if (playerInstance != null) Destroy(playerInstance);
        if (enemyInstance != null) Destroy(enemyInstance);
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

        // ���̵� üũ: �÷��̾��� ���ݷ��� ���̵����� ������ ����
        if (Player.playerStatus.attackPower < stageDifficulties[index])
        {
            Debug.Log("�÷��̾��� ���ݷ��� �����Ͽ� ����� �������� ����.");
            isSuccess = false;
            EndSpecialStage(); // ���� ó��
            return;
        }

        // ���� Ȱ��ȭ
        GameManager.isSpecialStageActive = true;

        // ����� ������ �ν��Ͻ� ����
        playerInstance = Instantiate(playerPrefab, new Vector3(-2, 0, 0), Quaternion.identity).GetComponent<Character>();
        Debug.Log("����� ���� �Ϸ�.");

        // ���� ������ �ν��Ͻ� ����
        enemyInstance = Instantiate(enemyPrefab, new Vector3(2, 0, 0), Quaternion.identity).GetComponent<Enemy>();
        Debug.Log("���� ���� �Ϸ�.");

        // ������ ���� ���� (1���� �Ǵ� ���� ���� ����)
        enemyInstance.SetNumberOfEnemyInGroup(3); // ���⼭ enemyInstance�� ���� �޼��带 ȣ��

        // ����̿� ������ ���� ���·� ����
        playerInstance.SetEnemy(enemyInstance);
        enemyInstance.SetEnemy(playerInstance);

        currentSpecialStageIndex = index;
        specialStageUI.SetActive(true);  // UI Ȱ��ȭ

        if (stageManager != null)
        {
            // originalBackground = stageManager.GetCurrentTheme();
            // stageManager.ChangeBackgroundToSpecialStage(index + 6);
        }

        goldCoroutine = StartCoroutine(GainGoldOverTime(level));
        DummyServerData.UseTicket(Player.GetUserID(), index); // Ƽ�� ����

        // ���� ��� ���� ����
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
                EndSpecialStage(true); // Ŭ���� ó��
                yield break;
            }

            // �÷��̾ ������ ����
            if (playerInstance != null && playerInstance.IsDead())
            {
                EndSpecialStage(false); // ���� ó��
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
            EndSpecialStage(false); // ���� ó��
        }
    }
    // ����� �������� ���� �޼ҵ�
    public void EndSpecialStage()
    {
        GameManager.isSpecialStageActive = false;
        specialStageUI.SetActive(false);

        if (goldCoroutine != null) StopCoroutine(goldCoroutine);

        // ���� ó��
        if (isSuccess)
        {
            if (currentSpecialStageIndex >= 0 && currentSpecialStageIndex < specialStageLevels.Length)
            {
                specialStageLevels[currentSpecialStageIndex]++;
                stageDifficulties[currentSpecialStageIndex] *= 2;
                Debug.Log($"����� �������� {currentSpecialStageIndex + 1} Ŭ����! ���� ������ Ȱ��ȭ�˴ϴ�.");
                if (playerInstance != null) Destroy(playerInstance);
                if (enemyInstance != null) Destroy(enemyInstance);
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
            if (playerInstance != null) Destroy(playerInstance);
            if (enemyInstance != null) Destroy(enemyInstance);
        }

        // cat�� enemy ��ü ����
        if (playerInstance != null) Destroy(playerInstance);
        if (enemyInstance != null) Destroy(enemyInstance);
    }
}
