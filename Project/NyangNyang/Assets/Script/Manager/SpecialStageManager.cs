using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpecialStageManager : MonoBehaviour
{

    [SerializeField]
    private GameObject specialStageUI;  // ����� ������������ ��Ÿ�� UI

    private bool isSpecialStageActive = false;  // ����� �������� Ȱ��ȭ ���� üũ
    private StageManager stageManager;          // StageManager ����
    private int originalBackground;             // ���� ��� �׸� ����

    public float playSec = 10.0f;               // ����� �������� ���� �ð�
    public float goldInterval = 0.5f;           // ��� ȹ�� �ֱ� (��)
    public int baseGoldAmount = 100000;             // �⺻ ��� ȹ�淮

    private Coroutine goldCoroutine;            // ��� ȹ�� �ڷ�ƾ

    // ����� ���������� ������ ������ �迭
    public int[] specialStageLevels = new int[3] { 1, 1, 1 };  // �� ����� ���������� ���� (3�� ��������)
    private int currentSpecialStageIndex;  // ���� Ȱ��ȭ�� ����� �������� �ε���

    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();  // StageManager ã��

        if (stageManager == null)
        {
            Debug.LogError("StageManager�� ã�� �� �����ϴ�.");
        }
    }

    // ����� �������� ����
    public void StartSpecialStage(int index, int level)
    {
        if (GameManager.isSpecialStageActive)
        {
            Debug.LogWarning("����� ���������� �̹� Ȱ��ȭ�Ǿ� �ֽ��ϴ�.");
            return;
        }
        if (GameManager.isMiniGameActive)
        {
            Debug.Log("MiniGame�� ���� ���̹Ƿ� Special Stage�� ������ �� �����ϴ�.");
            return;
        }

        GameManager.isSpecialStageActive = true;
        stageManager.isSpecial = true;  // ����� �������� ���·� ��ȯ
        currentSpecialStageIndex = index;  // ���� ����� �������� �ε��� ����

        specialStageUI.SetActive(true);  // ����� �������� UI Ȱ��ȭ

        if (stageManager != null)
        {
            originalBackground = stageManager.GetCurrentTheme(); // ���� ��� �׸� ����
            stageManager.ChangeBackgroundToSpecialStage(index + 6); // ����� ����� �������� ���(6)���� ����
        }

        // ��� ȹ�� �ڷ�ƾ ����
        goldCoroutine = StartCoroutine(GainGoldOverTime(level));

        // playSec �� ����� �������� ����
        Invoke("EndSpecialStage", playSec);
    }

    // ����� �������� ����
    public void EndSpecialStage()
    {
        GameManager.isSpecialStageActive = false;
        stageManager.isSpecial = false;  // �Ϲ� ���������� ���� ��ȯ

        specialStageUI.SetActive(false);  // ����� �������� UI ��Ȱ��ȭ

        if (goldCoroutine != null)
        {
            StopCoroutine(goldCoroutine);
        }

        if (stageManager != null)
        {
            //stageManager.ChangeBackgroundToSpecialStage(originalBackground); // ���� ������� ����
            stageManager.StopSpecialStage();
        }

        // ���� ����� ���������� ���� ���
        if (currentSpecialStageIndex >= 0 && currentSpecialStageIndex < specialStageLevels.Length)
        {
            specialStageLevels[currentSpecialStageIndex]++;
        }

    }

    // ���� �ð����� ��带 ��� �ڷ�ƾ
    private IEnumerator GainGoldOverTime(int level)
    {
        while (GameManager.isSpecialStageActive)
        {
            int goldEarned = baseGoldAmount * level;
            Player.AddGold(goldEarned);
            // Debug.Log("goldEarned : " + goldEarned + " level:" + level);
            yield return new WaitForSeconds(goldInterval);
        }
    }
}
