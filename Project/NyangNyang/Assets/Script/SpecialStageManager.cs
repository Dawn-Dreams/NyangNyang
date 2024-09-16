using UnityEngine;
using UnityEngine.UI;

public class SpecialStageManager : MonoBehaviour
{
    [SerializeField]
    private Button specialStageButton;  // ����� ���������� �̵��ϴ� ��ư

    [SerializeField]
    private GameObject specialStageUI;  // ����� ������������ ��Ÿ�� UI

    private bool isSpecialStageActive = false;  // ����� �������� Ȱ��ȭ ���� üũ
    private StageManager stageManager;          // StageManager ����
    private int originalBackground;             // ���� ��� �׸� ����

    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();  // StageManager ã��

        if (stageManager == null)
        {
            Debug.LogError("StageManager�� ã�� �� �����ϴ�.");
        }

        if (specialStageButton != null)
        {
            specialStageButton.onClick.AddListener(StartSpecialStage); // ��ư Ŭ�� �� ����� �������� ����
        }
    }

    // ����� �������� ����
    public void StartSpecialStage()
    {
        if (isSpecialStageActive)
        {
            Debug.LogWarning("����� ���������� �̹� Ȱ��ȭ�Ǿ� �ֽ��ϴ�.");
            return;
        }

        isSpecialStageActive = true;
        stageManager.isSpecial = true;  // ����� �������� ���·� ��ȯ

        specialStageUI.SetActive(true);  // ����� �������� UI Ȱ��ȭ

        if (stageManager != null)
        {
            originalBackground = stageManager.GetCurrentTheme(); // ���� ��� �׸� ����
            stageManager.ChangeBackgroundToSpecialStage(6); // ����� ����� �������� ���(6)���� ����
        }

        // 10�� �ڿ� ����� �������� ����
        Invoke("EndSpecialStage", 10f);
    }

    // ����� �������� ����
    public void EndSpecialStage()
    {
        isSpecialStageActive = false;
        stageManager.isSpecial = false;  // �Ϲ� ���������� ���� ��ȯ

        specialStageUI.SetActive(false);  // ����� �������� UI ��Ȱ��ȭ

        if (stageManager != null)
        {
            stageManager.ChangeBackgroundToSpecialStage(originalBackground); // ���� ������� ����
        }

        Debug.Log("����� �������� ����");
    }
}
