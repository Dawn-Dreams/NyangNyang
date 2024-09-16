using UnityEngine;
using UnityEngine.SceneManagement;  // �� �̵��� ���� ���ӽ����̽�
using UnityEngine.UI;

public class SpecialStageManager : MonoBehaviour
{
    [SerializeField]
    private Button specialStageButton;  // ����� ���������� �̵��ϴ� ��ư

    [SerializeField]
    private GameObject specialStageUI;  // ����� ������������ ��Ÿ�� UI

    private bool isSpecialStageActive = false; // ����� �������� Ȱ��ȭ ���� üũ

    void Start()
    {
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

        // ���� ������������ UI�� ��Ȱ��ȭ�ϰ�, ����� �������� UI Ȱ��ȭ
        specialStageUI.SetActive(true);
        Debug.Log("����� �������� ����");

        // ����� �������������� ���Ͱ� ����, �����۸� ������
        DisableMonsters();
        EnableSpecialItems();
    }

    // ����� �������� ����
    public void EndSpecialStage()
    {
        isSpecialStageActive = false;

        // ����� �������� UI�� ��Ȱ��ȭ�ϰ�, ���� �������� UI ����
        specialStageUI.SetActive(false);
        Debug.Log("����� �������� ����");
    }

    // ���͸� ��Ȱ��ȭ�ϴ� �Լ�
    private void DisableMonsters()
    {
        // ���͸� ��Ȱ��ȭ�ϱ� ���� ����. ���÷� ��� Enemy ������Ʈ�� ��Ȱ��ȭ.
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(false);  // ��� ���� ��Ȱ��ȭ
        }

        Debug.Log("����� �������������� ���Ͱ� �������� ����.");
    }

    // ����� �������� �����Ű�� �Լ�
    private void EnableSpecialItems()
    {
        // ����� ���������� ������ �������� Ȱ��ȭ�ϴ� ����
        // �ʿ��� �����۵��� Ȱ��ȭ
        Debug.Log("����� �������� �����Ŵ.");
    }

    // ���� �̵��ϴ� ����� ����� �� ���� (�ʿ�� �ٸ� ������ �̵�)
    public void LoadSpecialStageScene()
    {
        SceneManager.LoadScene("SpecialStageScene"); // �� �̸��� ���� ����
    }
}
