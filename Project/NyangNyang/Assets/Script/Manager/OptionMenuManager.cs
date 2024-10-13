using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        List<NoticeData> noticeList = DummyOptionsServer.GetNoticeData();

        if (noticeList.Count > 0)
        {
            GameObject noticeTextObj = GameObject.Find("NoticeText");  // NoticeUI > Contents > Scroll View > Viewport > Content > NoticeText
            TMP_Text noticeTextComponent = noticeTextObj.GetComponent<TMP_Text>();

            // ���� ���� �ʱ�ȭ
            string allNotices = "";  // ��� ������ ������ ���ڿ�

            foreach (NoticeData notice in noticeList)
            {
                allNotices += $"������ {notice.noticeID}: {notice.title} - {notice.content} ({notice.date})\n";  // ���Ŀ� ���� �߰�
            }

            noticeTextComponent.text = allNotices;  // TMP_Text�� ��� ������ ����
        }
        else
        {
            Debug.LogWarning("���� �����Ͱ� �����ϴ�.");
        }
    }


    // ��ŷ
    void OpenRankingPanel()
    {
        List<RankingData> rankList = DummyOptionsServer.GetRankingData();

        if (rankList.Count > 0)
        {
            GameObject contentObj = GameObject.Find("RankUI/Contents/Scroll View/Viewport/Content");

            for (int i = 0; i < rankList.Count; i++)
            {
                RankingData rankData = rankList[i];
                GameObject rankUserButton = contentObj.transform.GetChild(i).gameObject;

                TMP_Text rankNumberText = rankUserButton.transform.Find("RankNumber").GetComponent<TMP_Text>();
                TMP_Text rankUserNameText = rankUserButton.transform.Find("RankUserName").GetComponent<TMP_Text>();
                TMP_Text rankScoreText = rankUserButton.transform.Find("RankScore").GetComponent<TMP_Text>();

                rankNumberText.text = (i + 1).ToString();  // ������ 1���� �����ϵ��� ����
                rankUserNameText.text = rankData.userName;
                rankScoreText.text = rankData.score.ToString();
            }
        }
        else
        {
            Debug.LogWarning("��ŷ �����Ͱ� �����ϴ�.");
        }
    }


    // �Խ���
    void OpenBulletinBoardPanel()
    {
        List<BoardData> boardList = DummyOptionsServer.GetBoardData();

        if (boardList.Count > 0)
        {
            GameObject contentObj = GameObject.Find("BulletinBoardUI/Contents/Scroll View/Viewport/Content");

            for (int i = 0; i < boardList.Count; i++)
            {
                BoardData boardData = boardList[i];
                GameObject boardButton = contentObj.transform.GetChild(i).gameObject;

                TMP_Text titleText = boardButton.transform.Find("Title").GetComponent<TMP_Text>();
                titleText.text = boardData.title;
            }
        }
        else
        {
            Debug.LogWarning("�Խ��� �����Ͱ� �����ϴ�.");
        }
    }

    // ����
    void OpenMessagePanel()
    {
        List<MailData> mailList = DummyOptionsServer.GetMailData();

        if (mailList.Count > 0)
        {
            GameObject contentObj = GameObject.Find("MessegeUI/Contents/Scroll View/Viewport/Content");

            for (int i = 0; i < mailList.Count; i++)
            {
                MailData mailData = mailList[i];
                GameObject mailButton = contentObj.transform.GetChild(i).gameObject;

                TMP_Text mailNumberText = mailButton.transform.Find("MessegeNumber").GetComponent<TMP_Text>();
                TMP_Text mailTitleText = mailButton.transform.Find("MessegeTitle").GetComponent<TMP_Text>();
                TMP_Text mailDateText = mailButton.transform.Find("MessegeDate").GetComponent<TMP_Text>();
                TMP_Text mailReceivedText = mailButton.transform.Find("MessegeIsReceived").GetComponent<TMP_Text>();

                mailNumberText.text = mailData.mailID.ToString();
                mailTitleText.text = mailData.title;
                mailDateText.text = mailData.date;
                mailReceivedText.text = mailData.isReceived ? "���� �Ϸ�" : "�̼���";  // ���� ���� ����
            }
        }
        else
        {
            Debug.LogWarning("���� �����Ͱ� �����ϴ�.");
        }
    }


    // ģ��
    void OpenFriendsPanel()
    { 
        List<FriendData> friendList = DummyOptionsServer.GetFriendData();

        if (friendList.Count > 0)
        {
            GameObject contentObj = GameObject.Find("FriendsUI/Contents/Scroll View/Viewport/Content");

            for (int i = 0; i < friendList.Count; i++)
            {
                FriendData friendData = friendList[i];
                GameObject friendButton = contentObj.transform.GetChild(i).gameObject;

                TMP_Text friendUserIDText = friendButton.transform.Find("FriendUserID").GetComponent<TMP_Text>();
                TMP_Text friendUserNameText = friendButton.transform.Find("FriendUserName").GetComponent<TMP_Text>();
                TMP_Text friendUserScoreText = friendButton.transform.Find("FriendUserScore").GetComponent<TMP_Text>();

                friendUserIDText.text = friendData.friendUID.ToString();
                friendUserNameText.text = friendData.friendName; 
                friendUserScoreText.text = friendData.friendLevel.ToString();
            }
        }
        else
        {
            Debug.LogWarning("ģ�� �����Ͱ� �����ϴ�.");
        }
    }


    // Ŀ�´�Ƽ
    void OpenCommunityPanel()
    {
        Application.OpenURL("https://cafe.naver.com/nyangnyangcafeurl");
    }

    // ����
    void OpenSettingsPanel()
    {
        Debug.Log("���� �г��� ���Ƚ��ϴ�.");
    }
}
