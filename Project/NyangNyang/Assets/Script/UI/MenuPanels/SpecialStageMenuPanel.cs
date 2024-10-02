using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SpecialStageMenuPanel : MenuPanel
{
    [SerializeField]
    private ScrollRect scrollView; // 가로 스크롤을 위한 ScrollRect
    private Button[] stageButtons; // 스크롤되는 스테이지 버튼들

    [SerializeField]
    private GameObject[] stageTabs; // 스테이지별 탭들 (각 탭에 두 개의 버튼 존재)

    private GameObject currentActiveTab; // 현재 활성화된 탭
    private int currentActiveTabIndex = 0; // 현재 활성화된 탭 인덱스

    // 각 탭의 버튼들
    private Button[] startButtons; // 미니게임 시작 버튼
    private Button[] sweepButtons; // 소탕 버튼
    private TextMeshProUGUI[] ticketTexts; // 티켓 수량 표시 텍스트
    private TextMeshProUGUI[] titleTexts;

    private SpecialStageManager specialStageManager;

    private MiniGame1 miniGame1;

    [SerializeField]
    private ScrollRect[] levelScrollView;
    private Button[] levelSelectButtons;

    public int tempSpecialStageLevel { get; private set; } // 현재 선택된 스페셜 스테이지 레벨
    private int highestClearedStage = 1;            // 현재 클리어된 최고 스테이지 (1부터 시작)
    private int miniGameIndex = 1;
    void Start()
    {
        miniGame1 = FindObjectOfType<MiniGame1>();

        if (miniGame1 == null) Debug.LogError("MiniGame1 객체를 찾을 수 없습니다.");

        specialStageManager = FindObjectOfType<SpecialStageManager>();
        if (specialStageManager == null)
        {
            Debug.LogError("SpecialStageManager를 찾을 수 없습니다. SpecialStageManager가 씬에 추가되었는지 확인하세요.");
            return;
        }

        stageButtons = scrollView.content.GetComponentsInChildren<Button>();
        for (int i = 0; i < stageButtons.Length; i++)
        {
            int index = i; // 클로저 문제 해결을 위해 인덱스를 로컬 변수로 저장
            stageButtons[i].onClick.AddListener(() => OnClickStageButton(index));
        }

        // 각 탭의 하위에 있는 버튼과 텍스트를 찾아서 자동으로 할당
        startButtons = new Button[stageTabs.Length];
        sweepButtons = new Button[stageTabs.Length];
        ticketTexts = new TextMeshProUGUI[stageTabs.Length];
        titleTexts = new TextMeshProUGUI[stageTabs.Length];


        for (int i = 0; i < stageTabs.Length; i++)
        {
            startButtons[i] = stageTabs[i].transform.Find("MiniGameStartButton").GetComponent<Button>();
            sweepButtons[i] = stageTabs[i].transform.Find("SpecialStageStartButton").GetComponent<Button>();
            ticketTexts[i] = stageTabs[i].transform.Find("TicketText").GetComponent<TextMeshProUGUI>();
            titleTexts[i] = stageTabs[i].transform.Find("GameTitleText").GetComponent<TextMeshProUGUI>();

            // Scroll View를 찾아서 levelScrollView에 할당
            //levelScrollView[i] = stageTabs[i].transform.Find("Scroll View").GetComponent<ScrollRect>();
            int index = i;
            startButtons[i].onClick.AddListener(() => OnClickStartButton(index));
            sweepButtons[i].onClick.AddListener(() => OnClickSweepButton(index));
            
            // Scroll View의 Content 내에서 모든 버튼들을 가져옴
            levelSelectButtons = levelScrollView[i].content.GetComponentsInChildren<Button>();

            // 마우스 오버 이벤트 추가
            AddEventTrigger(sweepButtons[i], EventTriggerType.PointerEnter, () => OnHoverSweepButton(index));
            AddEventTrigger(sweepButtons[i], EventTriggerType.PointerExit, () => OnExitSweepButton(index));
        }

        for (int i = 0; i < levelSelectButtons.Length; i++)
        {
            int level = i;
            levelSelectButtons[i].onClick.AddListener(() => OnClickStageLevelButton(level));

            // 버튼 아래에 있는 텍스트를 찾아 인덱스를 표시
            TextMeshProUGUI buttonText = levelSelectButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = "던전 LEVEL " + (level + 1); // 1부터 시작하는 인덱스 번호로 표시
            }
            // 최고 클리어 스테이지를 기준으로 버튼 활성화/비활성화
            levelSelectButtons[i].interactable = (level + 1) <= highestClearedStage;
        }
        // 첫 번째 탭 활성화
        OnClickStageButton(0);
    }
    // 특정 스테이지 클리어 후 호출할 메서드
    public void OnStageCleared(int clearedStageLevel)
    {
        if (clearedStageLevel > highestClearedStage)
        {
            highestClearedStage = clearedStageLevel;
        }

        // 버튼들의 활성화 상태를 다시 체크
        UpdateStageButtons();
    }

    // 스테이지 버튼 상태 업데이트
    private void UpdateStageButtons()
    {
        for (int i = 0; i < levelSelectButtons.Length; i++)
        {
            levelSelectButtons[i].interactable = (i + 1) <= highestClearedStage;
        }
    }
    private void OnClickStageLevelButton(int index)
    {
        //SetButtonActiveColor(levelSelectButtons[index]);
        tempSpecialStageLevel = index + 1; // 버튼 인덱스를 스페셜 스테이지 레벨로 설정
        UpdateStageButtons();
        if (titleTexts[index] != null)
        {
            titleTexts[index].text = "미니게임 던전 " + (index + 1); // 1부터 시작하는 인덱스 번호로 표시
        }
        //Debug.Log("선택된 스페셜 스테이지 레벨: " + tempSpecialStageLevel);
    }

    // EventTrigger에 이벤트 추가 함수
    private void AddEventTrigger(Button button, EventTriggerType eventType, UnityEngine.Events.UnityAction action)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = eventType
        };
        entry.callback.AddListener((eventData) => action());
        trigger.triggers.Add(entry);
    }

    // 마우스가 소탕 버튼 위에 있을 때 실행
    void OnHoverSweepButton(int index)
    {
        UpdateTicketText(index);
    }

    // 마우스가 소탕 버튼에서 나갔을 때 실행
    void OnExitSweepButton(int index)
    {
        ticketTexts[index].text = "";
    }

    void OnClickStageButton(int index)
    {
        // 모든 탭을 비활성화
        foreach (var tab in stageTabs)
        {
            tab.SetActive(false);
        }

        // 선택된 탭 활성화
        currentActiveTab = stageTabs[index];
        currentActiveTab.SetActive(true);
        currentActiveTabIndex = index;

        SetButtonActiveColor(stageButtons[index]);

        if (titleTexts[index] != null)
        {
            titleTexts[index].text = "미니게임 던전 " + (index + 1); // 1부터 시작하는 인덱스 번호로 표시
        }
        // 선택된 탭에 맞는 소탕권 수량 업데이트
        UpdateTicketText(index);
    }

    // 선택된 버튼 색상만 하이라이트 기능 ... 메뉴 패널클래스로 옮겨서 상속받아 사용할 수 있게 추후 변경
    void SetButtonActiveColor(Button targetButton)
    {
        foreach (Button button in stageButtons)
        {
            if (button == targetButton) continue;

            ColorBlock tempColorBlock = button.colors;
            tempColorBlock.normalColor = new Color(1, 1, 1);
            tempColorBlock.selectedColor = new Color(1, 1, 1);
            tempColorBlock.highlightedColor = new Color(1, 1, 1);
            tempColorBlock.pressedColor = new Color(1, 1, 1);
            button.colors = tempColorBlock;
        }

        ColorBlock tempCB = targetButton.colors;
        tempCB.normalColor = new Color(0.25f, 0.25f, 0.25f);
        tempCB.highlightedColor = new Color(0.25f, 0.25f, 0.25f);
        tempCB.selectedColor = new Color(0.25f, 0.25f, 0.25f);
        tempCB.pressedColor = new Color(0.25f, 0.25f, 0.25f);
        targetButton.colors = tempCB;
    }

    // 미니게임 시작 버튼 클릭 시 실행
    void OnClickStartButton(int index)
    {
        // 미니게임 시작
        switch (index)
        {
            case 0:
                FindObjectOfType<MiniGame1>().StartGame();
                break;
            case 1:
                //FindObjectOfType<MiniGame2>().StartGame();
                break;
            case 2:
                //FindObjectOfType<MiniGame3>().StartGame();
                break;
            default:
                Debug.LogWarning("올바르지 않은 인덱스입니다.");
                break;
        }

    }

    // 소탕 버튼 클릭 시 실행
    void OnClickSweepButton(int index)
    {
        // 현재 스테이지에 맞는 소탕권 사용
        if (DummyServerData.HasTicket(Player.GetUserID(), index))
        {
            if (index < 0 || index >= 3)
            {
                Debug.LogError($"잘못된 스페셜 스테이지 인덱스: {index}");
                return;
            }
            DummyServerData.UseTicket(Player.GetUserID(), index);
            specialStageManager.StartSpecialStage(index, tempSpecialStageLevel);
            highestClearedStage = specialStageManager.specialStageLevels[index];
            //Debug.Log("최고 해제 스테이지" + highestClearedStage);
        }
        else
        {
            Debug.Log("소탕권이 부족합니다.");
        }
    }

    // 소탕권 수량 텍스트 업데이트
    void UpdateTicketText(int index)
    {
        int sweepTicketCount = DummyServerData.GetTicketCount(Player.GetUserID(), index);
        ticketTexts[index].text = $"{index + 1}번 소탕권 개수: {sweepTicketCount}";
    }
}
