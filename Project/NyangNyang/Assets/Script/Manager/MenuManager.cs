using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Parent Object for Buttons")]
    public GameObject buttonParentObject; // ��ư���� ��� �ִ� �θ� ������Ʈ

    [Header("Panels")]
    public GameObject settingsPanel; // ����â �г�
    public GameObject noticePanel;   // ���� �г�
    public GameObject rankingPanel;  // ��ŷ �г�
    public GameObject boardPanel;    // �Խ��� �г�

    private Button noticeButton;
    private Button rankingButton;
    private Button boardButton;
    private Button cafeButton;
    private Button settingsButton;

    private void Start()
    {
        // �������� ��ư���� ã��
        FindButtons();

        // �� ��ư�� �Լ� ����
        noticeButton.onClick.AddListener(OpenNotice);
        rankingButton.onClick.AddListener(OpenRanking);
        boardButton.onClick.AddListener(OpenBoard);
        cafeButton.onClick.AddListener(OpenNaverCafe);
        settingsButton.onClick.AddListener(OpenSettings);

        // ���� �� �гε� ��Ȱ��ȭ
        settingsPanel.SetActive(false);
        noticePanel.SetActive(false);
        rankingPanel.SetActive(false);
        boardPanel.SetActive(false);
    }

    // �θ� ������Ʈ�κ��� ��ư ã��
    void FindButtons()
    {
        Button[] buttons = buttonParentObject.GetComponentsInChildren<Button>();

        foreach (Button button in buttons)
        {
            switch (button.gameObject.name)
            {
                case "NoticeButton":
                    noticeButton = button;
                    break;
                case "RankingButton":
                    rankingButton = button;
                    break;
                case "BoardButton":
                    boardButton = button;
                    break;
                case "CafeButton":
                    cafeButton = button;
                    break;
                case "SettingsButton":
                    settingsButton = button;
                    break;
                default:
                    Debug.LogWarning($"{button.gameObject.name}�� ��ϵ��� ���� ��ư�Դϴ�.");
                    break;
            }
        }
    }

    // ������ â ���� �Լ�
    void OpenNotice()
    {
        Debug.Log("������ â ����");
        // ���� �г��� Ȱ��ȭ
        noticePanel.SetActive(true);
    }

    // ��ŷ â ���� �Լ�
    void OpenRanking()
    {
        Debug.Log("��ŷ â ����");
        // ��ŷ �г��� Ȱ��ȭ
        rankingPanel.SetActive(true);
    }

    // �Խ��� ���� �Լ�
    void OpenBoard()
    {
        Debug.Log("�Խ��� â ����");
        // �Խ��� �г��� Ȱ��ȭ
        boardPanel.SetActive(true);
    }

    // ���̹� ī�� ���� �Լ�
    void OpenNaverCafe()
    {
        Debug.Log("���̹� ī�� ����");
        // ���̹� ī�� URL�� �̵�
        Application.OpenURL("https://cafe.naver.com/yourcafeurl");
    }

    // ����â ���� �Լ�
    void OpenSettings()
    {
        Debug.Log("���� â ����");
        // ���� �г��� Ȱ��ȭ
        settingsPanel.SetActive(true);
    }

    // ����â �ݱ� �Լ�
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}
