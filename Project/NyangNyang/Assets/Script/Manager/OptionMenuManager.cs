using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuManager : MonoBehaviour
{
    public GameObject buttonParentObject;   // ��ư���� ��� �ִ� �θ� ������Ʈ
    public GameObject panelParentObject;    // �гε��� ��� �ִ� �θ� ������Ʈ

    // ��ư�� �г��� ����(index)�� �����Ǿ�� �۵�
    private Button[] buttons;               // �������� ã�� ��ư���� ������ �迭
    private GameObject[] panels;            // �������� ã�� �гε��� ������ �迭

    private void Start()
    {
        FindButtonsAndPanels();

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => OpenPanel(index));
        }

        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    void FindButtonsAndPanels()
    {
        buttons = buttonParentObject.GetComponentsInChildren<Button>();
        panels = new GameObject[panelParentObject.transform.childCount];

        for (int i = 0; i < panels.Length; i++)
        {
            panels[i] = panelParentObject.transform.GetChild(i).gameObject;
        }
    }

    // ���� �Լ� ȣ�� �� �г� ����
    void OpenPanel(int index)
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        if (index >= 0 && index < panels.Length)
        {
            panels[index].SetActive(true);
            CallPanelFunction(index);
        }
    }

    // �� �гθ��� ������ �Լ� ȣ��
    void CallPanelFunction(int index)
    {
        switch (index)
        {
            case 0:
                OpenNoticePanel();
                break;
            case 1:
                OpenRankingPanel();
                break;
            case 2:
                OpenBulletinBoardPanel();
                break;
            case 3:
                OpenCommunityPanel();
                panels[index].SetActive(false);
                break;
            case 4:
                OpenSettingsPanel();
                break;
            case 5:
                OpenFriendsPanel();
                break;
            case 6:
                OpenMessagePanel();
                break;
            default:
                Debug.LogWarning("�ش� �ε����� ���� ���� �Լ��� �����ϴ�.");
                break;
        }
    }

    // ----------------------------- �г� ���� �Լ� -------------------------------------
    // ����
    void OpenNoticePanel()
    {
        List<NoticeData> notices = DummyOptionsServer.SendNoticeDataToUser(0);
        foreach (NoticeData notice in notices)
        {
            Debug.Log($"����: {notice.title}, ��¥: {notice.date}");
        }
    }

    // ��ŷ
    void OpenRankingPanel()
    {
        List<RankingData> rankings = DummyOptionsServer.SendRankingDataToUser(0);
        foreach (RankingData ranking in rankings)
        {
            Debug.Log($"��ŷ ����: {ranking.userName}, ����: {ranking.score}");
        }
    }

    // �Խ���
    void OpenBulletinBoardPanel()
    {
        List<BoardData> boardPosts = DummyOptionsServer.SendBoardDataToUser(0);
        foreach (BoardData post in boardPosts)
        {
            Debug.Log($"�Խñ�: {post.title}, �ۼ���: {post.date}");
        }
    }

    // ģ��
    void OpenFriendsPanel()
    {
        List<FriendData> friends = DummyOptionsServer.SendFriendDataToUser(0);
        foreach (FriendData friend in friends)
        {
            Debug.Log($"ģ��: {friend.friendName}, ����: {friend.friendLevel}");
        }
    }

    // ����
    void OpenMessagePanel()
    {
        List<MailData> mails = DummyOptionsServer.SendMailDataToUser(0);
        foreach (MailData mail in mails)
        {
            Debug.Log($"����: {mail.title}, ��¥: {mail.date}, ���� ����: {mail.isReceived}");
        }
    }

    // Ŀ�´�Ƽ
    void OpenCommunityPanel()
    {
        Application.OpenURL("https://cafe.naver.com/yourcafeurl");
    }

    // ����
    void OpenSettingsPanel()
    {
        Debug.Log("���� �г��� ���Ƚ��ϴ�.");
    }
}
