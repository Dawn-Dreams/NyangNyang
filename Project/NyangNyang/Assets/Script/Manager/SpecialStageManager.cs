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

    public float playSec = 3.0f;               // ����� �������� ���� �ð�
    public float playDuration = 10.0f;
    public float goldInterval = 0.5f;
    public int baseGoldAmount = 100000;
    private Coroutine goldCoroutine;
    private bool isSuccess;
    // �߰�: �� ���������� ���̵� (��: 10, 15, 20 ��)
    private int[] stageDifficulties = new int[3] { 10, 15, 20 };

    private void Start()
    {
        stageManager = FindObjectOfType<StageManager>() ?? throw new NullReferenceException("StageManager is missing.");
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
        // ����� �������� ����
        GameManager.isSpecialStageActive = true;
        currentSpecialStageIndex = index;  // ���� ����� �������� �ε��� ����

        specialStageUI.SetActive(true);  // ����� �������� UI Ȱ��ȭ

        if (stageManager != null)
        {
            //originalBackground = stageManager.GetCurrentTheme(); // ���� ��� �׸� ����
            //stageManager.ChangeBackgroundToSpecialStage(index + 6); // ����� ����� �������� ���(6)���� ����
        }

        // ��� ȹ�� �ڷ�ƾ ����
        goldCoroutine = StartCoroutine(GainGoldOverTime(level));
        isSuccess = true;
        // playSec �� ����� �������� ����
        Invoke("EndSpecialStage", playSec);

        // Ƽ�� ���� - ���������� ���������� ���۵Ǿ��� ���� Ƽ���� ������
        DummyServerData.UseTicket(Player.GetUserID(), index);
        //Debug.Log($"����� �������� {index + 1}�� Ƽ���� �����Ǿ����ϴ�.");
    }


    private IEnumerator GainGoldOverTime(int level)
    {
        while (GameManager.isSpecialStageActive)
        {
            Player.AddGold(baseGoldAmount * level);
            yield return new WaitForSeconds(goldInterval);
        }
    }

    public void EndSpecialStage()
    {
        GameManager.isSpecialStageActive = false;
        specialStageUI.SetActive(false);

        if (goldCoroutine != null) StopCoroutine(goldCoroutine);

        // ���� ó��
        if (isSuccess)
        {
            // ���� ����� ���������� ���� ���
            if (currentSpecialStageIndex >= 0 && currentSpecialStageIndex < specialStageLevels.Length)
            {
                // ���� ���
                specialStageLevels[currentSpecialStageIndex]++;
                stageDifficulties[currentSpecialStageIndex] *= 2;
                Debug.Log($"����� �������� {currentSpecialStageIndex + 1} Ŭ����! ���� ������ Ȱ��ȭ�˴ϴ�.");
                Debug.Log($"({Player.playerStatus.attackPower} , {stageDifficulties[currentSpecialStageIndex]})");
                // SpecialStageMenuPanel�� OnStageCleared ȣ��
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
    }

}
