using UnityEngine;
using UnityEngine.UI;

public class SpecialStageManager : MonoBehaviour
{
    
    [SerializeField]
    private GameObject specialStageUI;  // ����� ������������ ��Ÿ�� UI

    private bool isSpecialStageActive = false;  // ����� �������� Ȱ��ȭ ���� üũ
    private StageManager stageManager;          // StageManager ����
    private int originalBackground;             // ���� ��� �׸� ����

    public float playSec = 10.0f;

    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();  // StageManager ã��

        if (stageManager == null)
        {
            Debug.LogError("StageManager�� ã�� �� �����ϴ�.");
        }
    }

    // ����� �������� ����
    public void StartSpecialStage(int index)
    {
        //Debug.Log("����� �������� ����");
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
            stageManager.ChangeBackgroundToSpecialStage(index+5); // ����� ����� �������� ���(6)���� ����
        }

        // playSec �� ����� �������� ����
        Invoke("EndSpecialStage", playSec);
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
            stageManager.StopSpecialStage();    
        }

        //Debug.Log("����� �������� ����");
    }
}