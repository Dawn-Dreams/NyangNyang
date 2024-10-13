using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuManager : MonoBehaviour
{
    public GameObject buttonParentObject;   // 버튼들을 담고 있는 부모 오브젝트
    public GameObject panelParentObject;    // 패널들을 담고 있는 부모 오브젝트

    // 버튼과 패널의 순서(index)가 대응되어야 작동
    private Button[] buttons;               // 동적으로 찾은 버튼들을 저장할 배열
    private GameObject[] panels;            // 동적으로 찾은 패널들을 저장할 배열

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

    // 고유 함수 호출 및 패널 열기
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

    // 각 패널마다 고유한 함수 호출
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
                Debug.LogWarning("해당 인덱스에 대한 고유 함수가 없습니다.");
                break;
        }
    }

    // ----------------------------- 패널 고유 함수 -------------------------------------
    // 공지
    void OpenNoticePanel()
    {
        List<NoticeData> noticeList = DummyOptionsServer.GetNoticeData();

        if (noticeList.Count > 0)
        {
            GameObject noticeTextObj = GameObject.Find("NoticeText");  // NoticeUI > Contents > Scroll View > Viewport > Content > NoticeText
            TMP_Text noticeTextComponent = noticeTextObj.GetComponent<TMP_Text>();

            // 공지 내용 초기화
            string allNotices = "";  // 모든 공지를 저장할 문자열

            foreach (NoticeData notice in noticeList)
            {
                allNotices += $"데이터 {notice.noticeID}: {notice.title} - {notice.content} ({notice.date})\n";  // 형식에 맞춰 추가
            }

            noticeTextComponent.text = allNotices;  // TMP_Text에 모든 공지를 적용
        }
        else
        {
            Debug.LogWarning("공지 데이터가 없습니다.");
        }
    }


    // 랭킹
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

                rankNumberText.text = (i + 1).ToString();  // 순위를 1부터 시작하도록 수정
                rankUserNameText.text = rankData.userName;
                rankScoreText.text = rankData.score.ToString();
            }
        }
        else
        {
            Debug.LogWarning("랭킹 데이터가 없습니다.");
        }
    }


    // 게시판
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
            Debug.LogWarning("게시판 데이터가 없습니다.");
        }
    }

    // 우편
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
                mailReceivedText.text = mailData.isReceived ? "수령 완료" : "미수령";  // 우편 수령 상태
            }
        }
        else
        {
            Debug.LogWarning("우편 데이터가 없습니다.");
        }
    }


    // 친구
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
            Debug.LogWarning("친구 데이터가 없습니다.");
        }
    }


    // 커뮤니티
    void OpenCommunityPanel()
    {
        Application.OpenURL("https://cafe.naver.com/nyangnyangcafeurl");
    }

    // 설정
    void OpenSettingsPanel()
    {
        Debug.Log("설정 패널이 열렸습니다.");
    }
}
