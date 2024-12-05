using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuUI : MonoBehaviour
{
    public GameObject toggleParentObject;   // Toggle들을 담고 있는 부모 오브젝트
    public GameObject panelParentObject;    // 패널들을 담고 있는 부모 오브젝트

    public GameObject noticeTextPrefab;     // 공지 텍스트 프리팹
    public GameObject boardButtonPrefab;    // 게시판 버튼 프리팹
    public GameObject mailButtonPrefab;     // 우편 버튼 프리팹
    public GameObject friendButtonPrefab;   // 친구 버튼 프리팹

    private Toggle[] toggles;               // 동적으로 찾은 Toggle들을 저장할 배열
    private GameObject[] panels;            // 동적으로 찾은 패널들을 저장할 배열

    public GameObject friendProfilePopupPrefab; // 친구 프로필 팝업 프리팹
    public GameObject boardPopupPrefab; // 게시판 팝업 프리팹
    public GameObject mailPopupPrefab; // 우편 팝업 프리팹
    private GameObject currentPopup; // 현재 활성화된 팝업

    private void OnEnable()
    {
        InitializeMenuUI();
        InitializeDefaultData();
    }

    private void OnDisable()
    {
        // 기존의 이벤트 리스너 제거 (중복 방지)
        foreach (var toggle in toggles)
        {
            toggle.onValueChanged.RemoveAllListeners();
        }
        // 데이터를 저장합니다.
        SaveLoadManager._instance.SaveNotices(SaveLoadManager._instance.LoadNotices());
        SaveLoadManager._instance.SaveMails(SaveLoadManager._instance.LoadMails());
        SaveLoadManager._instance.SaveFriends(SaveLoadManager._instance.LoadDungeonData());
        SaveLoadManager._instance.SaveRankings(SaveLoadManager._instance.LoadRankings());
        SaveLoadManager._instance.SaveBoards(SaveLoadManager._instance.LoadBoards());
    }

    private void InitializeMenuUI()
    {

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
            CallPanelFunction(0);         // 0번 패널의 데이터 초기화
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
                OpenSettingsPanel();
                break;
            case 1:
                OpenNoticePanel();
                break;
            case 2:
                OpenBulletinBoardPanel();
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
        // 공지 데이터 로드
        List<NoticeData> noticeList = SaveLoadManager._instance.LoadNotices();

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

            // 모든 공지 내용을 하나의 문자열로 정리
            string allNotices = "";

            foreach (NoticeData notice in noticeList)
            {
                allNotices += $"공지 ID {notice.noticeID}: {notice.title}\n내용: {notice.content}\n날짜: {notice.date}\n\n";
            }

            // 공지 텍스트 UI에 적용
            noticeTextComponent.text = allNotices;
        }
        else
        {
            Debug.LogWarning("공지 데이터가 없습니다.");
        }
    }

    // 우편
    void OpenMessagePanel()
    {
        // 데이터 불러오기 (유저 ID는 0으로 설정, 필요에 따라 변경)
        List<MailData> mailList = SaveLoadManager._instance.LoadMails();
        GameObject contentObj = GameObject.Find("MessageUI/Viewport/Content");

        // 기존 버튼 제거
        foreach (Transform child in contentObj.transform)
        {
            Destroy(child.gameObject);
        }

        // 데이터가 있는 경우 처리
        if (mailList.Count > 0)
        {
            foreach (MailData mailData in mailList)
            {
                GameObject mailButton = Instantiate(mailButtonPrefab, contentObj.transform);
                Button mailButtonComponent = mailButton.GetComponent<Button>();

                // 버튼 클릭 시 상세 보기 팝업
                mailButtonComponent.onClick.AddListener(() => ShowMailPopup(mailData));

                // UI 텍스트 설정
                TMP_Text mailNumberText = mailButton.transform.Find("MessageNumber").GetComponent<TMP_Text>();
                TMP_Text mailTitleText = mailButton.transform.Find("MessageTitle").GetComponent<TMP_Text>();
                TMP_Text mailDateText = mailButton.transform.Find("MessageDate").GetComponent<TMP_Text>();
                TMP_Text mailReceivedText = mailButton.transform.Find("MessageIsReceived").GetComponent<TMP_Text>();

                mailNumberText.text = mailData.mailID.ToString();
                mailTitleText.text = mailData.title;
                mailDateText.text = mailData.date;
                mailReceivedText.text = mailData.isReceived ? "수령 완료" : "미수령";
            }
        }
        else
        {
            Debug.LogWarning("우편 데이터가 없습니다.");
        }
    }


    // 게시판
    void OpenBulletinBoardPanel()
    {
        // 게시판 데이터 로드
        List<BoardData> boardList = SaveLoadManager._instance.LoadBoards();
        GameObject contentObj = GameObject.Find("BulletinBoardUI/Viewport/Content");

        // 기존 UI 요소 제거
        foreach (Transform child in contentObj.transform)
        {
            Destroy(child.gameObject);
        }

        if (boardList.Count > 0)
        {
            foreach (BoardData boardData in boardList)
            {
                GameObject boardButton = Instantiate(boardButtonPrefab, contentObj.transform);

                // 텍스트 설정
                TMP_Text titleText = boardButton.transform.Find("Title").GetComponent<TMP_Text>();
                titleText.text = boardData.title;

                // 버튼 이벤트 추가
                Button boardButtonComponent = boardButton.GetComponent<Button>();
                boardButtonComponent.onClick.AddListener(() => ShowBoardPopup(boardData));
            }
        }
        else
        {
            Debug.LogWarning("게시판 데이터가 없습니다.");
        }
    }

    // 친구
    void OpenFriendsPanel()
    {
        // 친구 데이터 로드
        List<DungeonData> friendList = SaveLoadManager._instance.LoadDungeonData();
        GameObject contentObj = GameObject.Find("FriendUI/Viewport/Content");

        // 기존 UI 요소 제거
        foreach (Transform child in contentObj.transform)
        {
            Destroy(child.gameObject);
        }

        if (friendList.Count > 0)
        {
            foreach (DungeonData friendData in friendList)
            {
                GameObject friendButton = Instantiate(friendButtonPrefab, contentObj.transform);

                // 텍스트 설정
                TMP_Text friendUserIDText = friendButton.transform.Find("FriendUserID").GetComponent<TMP_Text>();
                TMP_Text friendUserNameText = friendButton.transform.Find("FriendUserName").GetComponent<TMP_Text>();
                TMP_Text friendUserLevelText = friendButton.transform.Find("FriendUserLevel").GetComponent<TMP_Text>();

                friendUserIDText.text = friendData.UID.ToString();
                friendUserNameText.text = friendData.Name;
                friendUserLevelText.text = friendData.dungeonLevel.ToString();

                // 버튼 이벤트 추가
                Button buttonComponent = friendButton.GetComponent<Button>();
                buttonComponent.onClick.AddListener(() => ShowFriendProfilePopup(friendData));
            }
        }
        else
        {
            Debug.LogWarning("친구 데이터가 없습니다.");
        }
    }


    // 커뮤니티
    public void OpenCommunityPanel()
    {
        Application.OpenURL("https://cafe.naver.com/nyangnyangexpedition"); // 게시판에 커뮤니티 버튼 추가
    }

    // 설정
    void OpenSettingsPanel()
    {

    }

    // 계정
    void OpenAccountPanel()
    {

    }


    // 친구 버튼 팝업 메서드
    void ShowFriendProfilePopup(DungeonData friendData)
    {
        // 기존 팝업 닫기
        if (currentPopup != null)
        {
            Destroy(currentPopup);
        }

        // 팝업 생성
        currentPopup = Instantiate(friendProfilePopupPrefab, transform);

        // 프로필 팝업 데이터 설정
        TMP_Text nameText = currentPopup.transform.Find("NameText").GetComponent<TMP_Text>();
        TMP_Text idText = currentPopup.transform.Find("IDText").GetComponent<TMP_Text>();
        TMP_Text levelText = currentPopup.transform.Find("LevelText").GetComponent<TMP_Text>();
        Button closeButton = currentPopup.transform.Find("CloseButton").GetComponent<Button>();

        nameText.text = $"{friendData.Name}";
        idText.text = $"ID : {friendData.UID}";
        levelText.text = $"{friendData.dungeonLevel} Level";

        // 닫기 버튼 이벤트 설정
        closeButton.onClick.AddListener(() =>
        {
            Destroy(currentPopup);
            currentPopup = null;
        });
    }

    // 우편 팝업 열기 메서드
    void ShowMailPopup(MailData mailData)
    {
        // 기존 팝업 닫기
        if (currentPopup != null)
        {
            Destroy(currentPopup);
        }

        // 팝업 생성
        currentPopup = Instantiate(mailPopupPrefab, transform);

        // 팝업 데이터 설정
        TMP_Text titleText = currentPopup.transform.Find("TitleText").GetComponent<TMP_Text>();
        TMP_Text contentText = currentPopup.transform.Find("ContentText").GetComponent<TMP_Text>();
        TMP_Text dateText = currentPopup.transform.Find("DateText").GetComponent<TMP_Text>();
        Button closeButton = currentPopup.transform.Find("CloseButton").GetComponent<Button>();

        titleText.text = mailData.title;
        contentText.text = mailData.content;
        dateText.text = $"Date: {mailData.date}";

        // 닫기 버튼 이벤트 설정
        closeButton.onClick.AddListener(() =>
        {
            Destroy(currentPopup);
            currentPopup = null;
        });
    }


    // 게시판 팝업 열기 메서드
    void ShowBoardPopup(BoardData boardData)
    {
        // 기존 팝업 닫기
        if (currentPopup != null)
        {
            Destroy(currentPopup);
        }

        // 팝업 생성
        currentPopup = Instantiate(boardPopupPrefab, transform);

        // 팝업 데이터 설정
        TMP_Text titleText = currentPopup.transform.Find("TitleText").GetComponent<TMP_Text>();
        TMP_Text contentText = currentPopup.transform.Find("ContentText").GetComponent<TMP_Text>();
        TMP_Text dateText = currentPopup.transform.Find("DateText").GetComponent<TMP_Text>();
        Button closeButton = currentPopup.transform.Find("CloseButton").GetComponent<Button>();

        titleText.text = boardData.title;
        contentText.text = boardData.content;
        dateText.text = $"Date: {boardData.date}";

        // 닫기 버튼 이벤트 설정
        closeButton.onClick.AddListener(() =>
        {
            Destroy(currentPopup);
            currentPopup = null;
        });
    }
    [System.Serializable]
    public class BoardDataList
    {
        public List<BoardData> items;
    }
    [System.Serializable]
    public class NoticeDataList
    {
        public List<NoticeData> items;
    }
    private void InitializeDefaultData()
    {
        // 공지 데이터
        string noticeJson = @"
        {
            ""items"": [
                {
                    ""noticeID"": 1,
                    ""title"": ""냥냥원정대 11월 27일 베타테스트 시작!"",
                    ""content"": ""안녕하세요, 던 드림즈 팀입니다!\n\n냥냥원정대의 우주 탐험을 함께 해주시는 모든 집사님들께 감사드립니다! \n\n 11월 27일, 베타 테스트 출시가 진행되고 있습니다~\n아래에서 이번 베타 테스트 버전의 내용과 앞으로의 계획을 확인해 보세요! \n\n\n 1. 11월 27일 베타 테스트 주요 내용\n\n현재 냥냥원정대의 주요 컨텐츠는 다음과 같습니다!\n\n방치형으로 키우는 냥냥대원 : 다양한 행성에서 자원을 모으고, 치즈를 찾아 모험하세요!\n\n스킬과 무기 수집 : 탐험과 전투를 통해 다양한 무기를 수집하고, 특별한 스킬 조합으로 최강의 팀을 만들어 보세요.\n\n코스튬 시스템 : 테마의 의상을 입혀 나만의 고양이를 꾸며보세요!\n\n펫 시스템 : 모험을 도와줄 귀여운 펫 친구를 만나보세요.\n\n간식 시스템 : 특정 조건을 달성하면 버프가 팡팡!\n\n우주냥 던전 : 각기 다른 스타일의 던전 격파!\n\n미니게임 : 치즈조각을 얻을 수 있는 새로운 재미 요소!\n\n\n 2. 개선할 주요 내용\n\n스토리 확장 : 고양이들의 여정이 더 풍성해질 수 있도록 퀘스트 및 컨텐츠 추가 개발 예정입니다.\n\n다양한 보상 : 더 많은 보상을 제공할 방법을 모색 중입니다.\n\n컨텐츠 반복성 개선: 집사님들이 오래 즐기실 수 있도록 게임 흐름을 다각화할 예정입니다.\n\n광고제거 기능 추가 : 광고 없는 쾌적한 게임 환경을 즐기세요!\n\n\n여러분의 소중한 의견을 언제나 환영합니다! 댓글이나 피드백을 통해 자유롭게 알려주세요. \n\n\n앞으로도 집사님들과 함께 냥냥원정대를 더욱 멋진 게임으로 만들어 나가겠습니다.\n\n\n많은 관심과 응원 부탁드립니다!\n\n감사합니다.\n던 드림즈 팀 드림\n\n\n\n\n\n\n\n\n"",
                    ""date"": ""2024-11-27""
                }
            ]
        }";

        // 게시판 데이터
        string boardJson = @"
{
    ""items"": [
        {
            ""postID"": 1,
            ""title"": ""초보자를 위한 팁 - 냥냥 관리"",
            ""content"": ""초반에는 탐험을 성공적으로 수행하기 위해, 대원을 우선적으로 키워보세요. 대원의 냥냥스탯과 스킬/장비 조합이 승패를 좌우할 수 있어요."",
            ""date"": ""2024-11-27""
        },
        {
            ""postID"": 2,
            ""title"": ""초보자를 위한 팁 - 장비 수집"",
            ""content"": ""탐험 중 뽑기를 통해 얻는 무기와 스킬은 최대한 많이 모아서, 조합해 보세요.\n조합에 따라 강력한 시너지를 낼 수 있습니다!"",
            ""date"": ""2024-11-27""
        },
        {
            ""postID"": 3,
            ""title"": ""초보자를 위한 팁 - 코스튬"",
            ""content"": ""귀여운 냥냥대원 꾸미기! 다양한 의상을 획득해 대원을 마음껏 꾸며보세요!"",
            ""date"": ""2024-11-27""
        },
        {
            ""postID"": 4,
            ""title"": ""초보자를 위한 팁 - 펫"",
            ""content"": ""펫은 탐험을 계속하다보면 확률적으로 동료가 되어 함께할 수 있어요! 다양한 맵을 돌며 다양한 펫을 수집해보세요!"",
            ""date"": ""2024-11-27""
        },
        {
            ""postID"": 5,
            ""title"": ""초보자를 위한 팁 - 간식"",
            ""content"": ""초기 자원을 빠르게 모으고 싶다면, 광고를 활용해 간식 버프 효과를 받아보세요!"",
            ""date"": ""2024-11-27""
        }
    ]
}";


        // JSON 데이터를 파싱하고 저장
        NoticeDataList noticeDataList = JsonUtility.FromJson<NoticeDataList>(noticeJson);
        SaveLoadManager._instance.SaveNotices(noticeDataList.items);

        // JSON 데이터를 BoardDataList로 파싱
        BoardDataList boardDataList = JsonUtility.FromJson<BoardDataList>(boardJson);
        List<BoardData> boardData = boardDataList.items; // BoardDataList에서 items를 추출하여 List<BoardData>로 저장

        // 저장
        SaveLoadManager._instance.SaveBoards(boardData); // List<BoardData>를 저장
    }
}

