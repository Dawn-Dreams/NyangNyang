using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuManager : MonoBehaviour
{
    private static OptionMenuManager instance;
    public GameObject toggleParentObject;   // Toggle들을 담고 있는 부모 오브젝트
    public GameObject panelParentObject;    // 패널들을 담고 있는 부모 오브젝트

    public GameObject noticeTextPrefab;     // 공지 텍스트 프리팹
    public GameObject rankUserButtonPrefab; // 랭킹 버튼 프리팹
    public GameObject boardButtonPrefab;    // 게시판 버튼 프리팹
    public GameObject mailButtonPrefab;     // 우편 버튼 프리팹
    public GameObject friendButtonPrefab;   // 친구 버튼 프리팹

    private Toggle[] toggles;               // 동적으로 찾은 Toggle들을 저장할 배열
    private GameObject[] panels;            // 동적으로 찾은 패널들을 저장할 배열
    
    private List<RankingData> rankList;

    public static OptionMenuManager GetOptionManager()
    {
        return instance;
    }
    private void Start()
    {

        if (instance == null) instance = this;
        FindTogglesAndPanels();

        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i;
            toggles[i].onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                {
                    OpenPanel(index);
                }
            });
        }

        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        
        // 0번 패널 활성화
        if (toggles.Length > 0 && panels.Length > 0)
        {
            toggles[0].isOn = true;
            panels[0].SetActive(true);
        }
    }

    void FindTogglesAndPanels()
    {

        toggles = toggleParentObject.GetComponentsInChildren<Toggle>();
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
        else
        {
        }
    }

    void CallPanelFunction(int index)
    {
        switch (index)
        {
            case 0:
                OpenMessagePanel();
                break;
            case 1:
                OpenFriendsPanel();
                break;
            case 2:
                OpenRankingPanel();
                break;
            case 3:
                OpenSettingsPanel();
                break;
            case 4:
                OpenNoticePanel();
                break;
            case 5:
                OpenBulletinBoardPanel();
                break;
            case 6:
                OpenCommunityPanel();
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
            GameObject noticeTextObj = GameObject.Find("NoticeText");

            if (noticeTextObj == null)
            {
                Debug.LogError("NoticeText 오브젝트를 찾을 수 없습니다. 이름이 정확한지 확인하세요.");
                return;
            }

            TMP_Text noticeTextComponent = noticeTextObj.GetComponent<TMP_Text>();

            if (noticeTextComponent == null)
            {
                Debug.LogError("TMP_Text 컴포넌트를 찾을 수 없습니다. 해당 오브젝트에 TMP_Text가 있는지 확인하세요.");
                return;
            }

            string allNotices = "";

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
    public void SetRankList(List<RankingData> ranks)
    {
        rankList = ranks;
        GameObject contentObj = GameObject.Find("RankUI/Viewport/Content");
        if (rankList.Count > 0)
        {
            foreach (RankingData rankData in rankList)
            {
                GameObject rankUserButton = Instantiate(rankUserButtonPrefab, contentObj.transform);

                TMP_Text rankNumberText = rankUserButton.transform.Find("RankNumber").GetComponent<TMP_Text>();
                TMP_Text rankUserNameText = rankUserButton.transform.Find("RankUserName").GetComponent<TMP_Text>();
                TMP_Text rankScoreText = rankUserButton.transform.Find("RankScore").GetComponent<TMP_Text>();

                rankNumberText.text = (rankList.IndexOf(rankData) + 1).ToString();
                rankUserNameText.text = rankData.nickname;
                rankScoreText.text = rankData.score.ToString();
            }
        }
        else
        {
            Debug.LogWarning("랭킹 데이터가 없습니다.");
        }

    }
    void OpenRankingPanel()
    {

        GameObject contentObj = GameObject.Find("RankUI/Viewport/Content");
        // 기존에 생성된 요소들을 모두 제거
        foreach (Transform child in contentObj.transform)
        {
            Destroy(child.gameObject);
        }
        NetworkManager.GetStatusManager().UpdatePlayersRanking(rankList);

    }

    // 게시판
    void OpenBulletinBoardPanel()
    {
        List<BoardData> boardList = DummyOptionsServer.GetBoardData();
        GameObject contentObj = GameObject.Find("BulletinBoardUI/Viewport/Content");
        // Application.OpenURL("https://cafe.naver.com/nyangnyangcafeurl"); // 게시판에 커뮤니티 버튼 추가
        // 기존에 생성된 요소들을 모두 제거
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
            Debug.LogWarning("게시판 데이터가 없습니다.");
        }
    }

    // 우편
    void OpenMessagePanel()
    {
        List<MailData> mailList = DummyOptionsServer.GetMailData();
        GameObject contentObj = GameObject.Find("MessageUI/Viewport/Content");

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

                mailNumberText.text = mailData.mailID.ToString();  // 우편 ID 표시
                mailTitleText.text = mailData.title;  // 우편 제목 표시
                mailDateText.text = mailData.date;  // 우편 날짜 표시
                mailReceivedText.text = mailData.isReceived ? "수령 완료" : "미수령";  // 수령 상태 표시
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
        GameObject contentObj = GameObject.Find("FriendUI/Viewport/Content");

        // 기존에 생성된 요소들을 모두 제거
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
            Debug.LogWarning("친구 데이터가 없습니다.");
        }
    }

    // 커뮤니티
    void OpenCommunityPanel()
    {
        
    }

    // 설정
    void OpenSettingsPanel()
    {
    }
}
