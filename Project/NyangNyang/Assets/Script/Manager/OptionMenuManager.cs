using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuManager : MonoBehaviour
{
    public GameObject buttonParentObject;   // ��ư���� ��� �ִ� �θ� ������Ʈ
    public GameObject panelParentObject;    // �гε��� ��� �ִ� �θ� ������Ʈ

    public GameObject noticeTextPrefab;     // ���� �ؽ�Ʈ ������
    public GameObject rankUserButtonPrefab; // ��ŷ ��ư ������
    public GameObject boardButtonPrefab;    // �Խ��� ��ư ������
    public GameObject mailButtonPrefab;     // ���� ��ư ������
    public GameObject friendButtonPrefab;   // ģ�� ��ư ������

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
                panels[3].gameObject.SetActive(false);
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

            // Null üũ �߰�
            if (noticeTextObj == null)
            {
                Debug.LogError("NoticeText ������Ʈ�� ã�� �� �����ϴ�. �̸��� ��Ȯ���� Ȯ���ϼ���.");
                return;
            }

            TMP_Text noticeTextComponent = noticeTextObj.GetComponent<TMP_Text>();

            // TMP_Text ������Ʈ�� null���� üũ
            if (noticeTextComponent == null)
            {
                Debug.LogError("TMP_Text ������Ʈ�� ã�� �� �����ϴ�. �ش� ������Ʈ�� TMP_Text�� �ִ��� Ȯ���ϼ���.");
                return;
            }

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
        GameObject contentObj = GameObject.Find("RankUI/Contents/Scroll View/Viewport/Content");

        // ������ ������ ��ҵ��� ��� ����
        foreach (Transform child in contentObj.transform)
        {
            Destroy(child.gameObject);
        }

        if (rankList.Count > 0)
        {
            foreach (RankingData rankData in rankList)
            {
                GameObject rankUserButton = Instantiate(rankUserButtonPrefab, contentObj.transform);

                TMP_Text rankNumberText = rankUserButton.transform.Find("RankNumber").GetComponent<TMP_Text>();
                TMP_Text rankUserNameText = rankUserButton.transform.Find("RankUserName").GetComponent<TMP_Text>();
                TMP_Text rankScoreText = rankUserButton.transform.Find("RankScore").GetComponent<TMP_Text>();

                rankNumberText.text = (rankList.IndexOf(rankData) + 1).ToString();  // ������ 1���� ����
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
        GameObject contentObj = GameObject.Find("BulletinBoardUI/Contents/Scroll View/Viewport/Content");

        // ������ ������ ��ҵ��� ��� ����
        foreach (Transform child in contentObj.transform)
        {
            Destroy(child.gameObject);
        }

        if (boardList.Count > 0)
        {
            foreach (BoardData boardData in boardList)
            {
                GameObject boardButton = Instantiate(boardButtonPrefab, contentObj.transform);
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
        GameObject contentObj = GameObject.Find("MessageUI/Contents/Scroll View/Viewport/Content");

        foreach (Transform child in contentObj.transform)
        {
            Destroy(child.gameObject);
        }

        if (mailList.Count > 0)
        {
            foreach (MailData mailData in mailList)
            {
                GameObject mailButton = Instantiate(mailButtonPrefab, contentObj.transform);

                TMP_Text mailNumberText = mailButton.transform.Find("MessageNumber").GetComponent<TMP_Text>();
                TMP_Text mailTitleText = mailButton.transform.Find("MessageTitle").GetComponent<TMP_Text>();
                TMP_Text mailDateText = mailButton.transform.Find("MessageDate").GetComponent<TMP_Text>();
                TMP_Text mailReceivedText = mailButton.transform.Find("MessageIsReceived").GetComponent<TMP_Text>();

                mailNumberText.text = mailData.mailID.ToString();  // ���� ID ǥ��
                mailTitleText.text = mailData.title;  // ���� ���� ǥ��
                mailDateText.text = mailData.date;  // ���� ��¥ ǥ��
                mailReceivedText.text = mailData.isReceived ? "���� �Ϸ�" : "�̼���";  // ���� ���� ǥ��
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
        GameObject contentObj = GameObject.Find("FriendsUI/Contents/Scroll View/Viewport/Content");

        // ������ ������ ��ҵ��� ��� ����
        foreach (Transform child in contentObj.transform)
        {
            Destroy(child.gameObject);
        }

        if (friendList.Count > 0)
        {
            foreach (FriendData friendData in friendList)
            {
                GameObject friendButton = Instantiate(friendButtonPrefab, contentObj.transform);

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
    }
}
