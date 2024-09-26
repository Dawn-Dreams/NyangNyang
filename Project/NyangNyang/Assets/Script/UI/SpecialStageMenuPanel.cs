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
    [SerializeField]
    private Button[] stageButtons; // 스크롤되는 스테이지 버튼들
    [SerializeField]
    private GameObject[] stageTabs; // 스테이지별 탭들 (각 탭에 두 개의 버튼 존재)

    private GameObject currentActiveTab; // 현재 활성화된 탭
    private int currentActiveTabIndex = 0; // 현재 활성화된 탭 인덱스

    // 각 탭의 버튼들
    [SerializeField]
    private Button[] startButtons; // 미니게임 시작 버튼
    [SerializeField]
    private Button[] sweepButtons; // 소탕 버튼
    [SerializeField]
    private TextMeshProUGUI[] sweepTicketTexts; // 소탕권 수량 표시 텍스트

    private SpecialStageManager specialStageManager;

    private MiniGame1 miniGame1;

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

        for (int i = 0; i < stageButtons.Length; i++)
        {
            int index = i; // 클로저 문제 해결을 위해 인덱스를 로컬 변수로 저장
            stageButtons[i].onClick.AddListener(() => OnClickStageButton(index));
        }

        for (int i = 0; i < startButtons.Length; i++)
        {
            int index = i;
            startButtons[i].onClick.AddListener(() => OnClickStartButton(index));
            sweepButtons[i].onClick.AddListener(() => OnClickSweepButton(index));

            // 마우스 오버 이벤트 추가
            AddEventTrigger(sweepButtons[i], EventTriggerType.PointerEnter, () => OnHoverSweepButton(index));
            AddEventTrigger(sweepButtons[i], EventTriggerType.PointerExit, () => OnExitSweepButton(index));
        }

        // 첫 번째 탭 활성화
        OnClickStageButton(0);
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
        sweepTicketTexts[index].text = "";
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
            specialStageManager.StartSpecialStage(index);
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
        sweepTicketTexts[index].text = $"{index + 1}번 소탕권 개수: {sweepTicketCount}";
    }
}
