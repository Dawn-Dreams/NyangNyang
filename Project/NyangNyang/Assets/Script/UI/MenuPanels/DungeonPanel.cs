using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DungeonPanel : MenuPanel
{
    [SerializeField]
    private ScrollRect scrollView;
    private Button[] stageButtons;

    [SerializeField]
    private GameObject[] stageTabs;
    private Button[] startButtons, sweepButtons;
    private TextMeshProUGUI[] ticketTexts, titleTexts;
    private ScrollRect[] levelScrollViews;
    private Button[][] levelSelectButtons;

    private DungeonManager dungeonManager;

    private readonly List<string> dungeonNames = new List<string> { "황야의 대지", "눈꽃 동굴", "독거미 숲" };
    private readonly List<string> ticketNames = new List<string> { "노랑", "파랑", "빨강" };

    private int[] highestClearedStage = new int[3] { 1, 1, 1 };
    public int TempDungeonStageLevel { get; private set; }
    private int currentActiveTabIndex = 0;

    private void Start()
    {
        InitializeManagers();
        InitializeUIComponents();
        OnClickStageButton(0); // 기본 탭 선택
    }

    private void InitializeManagers()
    {
        dungeonManager = FindObjectOfType<DungeonManager>() ?? throw new NullReferenceException("DungeonManager가 존재하지 않습니다.");
    }

    private void InitializeUIComponents()
    {
        InitializeStageButtons();
        InitializeTabs();
        InitializeLevelSelectButtons();
    }

    private void InitializeStageButtons()
    {
        stageButtons = scrollView.content.GetComponentsInChildren<Button>();
        for (int i = 0; i < stageButtons.Length; i++)
        {
            int index = i;
            stageButtons[i].onClick.AddListener(() => OnClickStageButton(index));
        }
    }

    private void InitializeTabs()
    {
        int tabCount = stageTabs.Length;
        startButtons = new Button[tabCount];
        sweepButtons = new Button[tabCount];
        ticketTexts = new TextMeshProUGUI[tabCount];
        titleTexts = new TextMeshProUGUI[tabCount];
        levelScrollViews = new ScrollRect[tabCount];

        for (int i = 0; i < tabCount; i++)
        {
            var tab = stageTabs[i].transform;
            
            startButtons[i] = tab.Find("DungeonStartButton").GetComponent<Button>();
            sweepButtons[i] = tab.Find("DungeonSweepButton").GetComponent<Button>();
            ticketTexts[i] = tab.Find("TicketText").GetComponent<TextMeshProUGUI>();
            titleTexts[i] = tab.Find("GameTitleText").GetComponent<TextMeshProUGUI>();

            int index = i;
            
            startButtons[i].onClick.AddListener(() => OnClickStartButton(index));
            sweepButtons[i].onClick.AddListener(() => OnClickSweepButton(index));

            levelScrollViews[i] = stageTabs[i].GetComponentInChildren<ScrollRect>();
        }
    }

    private void InitializeLevelSelectButtons()
    {
        levelSelectButtons = new Button[levelScrollViews.Length][];
        for (int i = 0; i < levelScrollViews.Length; i++)
        {
            var buttons = levelScrollViews[i].content.GetComponentsInChildren<Button>();
            levelSelectButtons[i] = buttons;
            InitializeLevelButtonInteractions(i, buttons);
        }
    }

    private void InitializeLevelButtonInteractions(int tabIndex, Button[] buttons)
    {
        for (int j = 0; j < buttons.Length; j++)
        {
            int level = j + 1; // 레벨은 1부터 시작하므로 j + 1로 설정
            buttons[j].onClick.RemoveAllListeners(); // 이전 리스너 제거
            buttons[j].onClick.AddListener(() => OnClickStageLevelButton(tabIndex, level - 1)); // levelIndex는 0부터 시작하므로 level - 1
            buttons[j].interactable = level <= highestClearedStage[tabIndex]; // 최고 클리어된 레벨까지만 활성화
            buttons[j].GetComponentInChildren<TextMeshProUGUI>().text = $"던전 LEVEL {level}"; // 레벨 번호 표시
        }
    }


    public void OnStageCleared(int tabIndex, int clearedStageLevel)
    {
        if (clearedStageLevel >= highestClearedStage[tabIndex])
        {
            highestClearedStage[tabIndex] = clearedStageLevel;
            UpdateLevelSelectButtons(tabIndex);
        }
        UpdateStageButtons(tabIndex);
    }

    private void UpdateLevelSelectButtons(int tabIndex)
    {
        for (int i = 0; i < levelSelectButtons[tabIndex].Length; i++)
        {
            int level = i + 1; // 레벨은 1부터 시작
            UpdateStageButtons(tabIndex);
            levelSelectButtons[tabIndex][i].GetComponentInChildren<TextMeshProUGUI>().text = $"던전 LEVEL {level}"; // 레벨 번호 표시
        }
    }

    private void OnClickStageButton(int index)
    {
        SetActiveTab(index);
        titleTexts[index].text = $"{dungeonNames[index]}";
        UpdateTicketText(index);
    }

    private void SetActiveTab(int index)
    {
        foreach (var tab in stageTabs) tab.SetActive(false);
        stageTabs[index].SetActive(true);
        currentActiveTabIndex = index;
    }

    private void OnClickStageLevelButton(int tabIndex, int levelIndex)
    {
        TempDungeonStageLevel = levelIndex + 1;
        UpdateLevelSelectButtons(tabIndex);
        titleTexts[tabIndex].text = $"{dungeonNames[tabIndex]} - LEVEL {TempDungeonStageLevel}";
    }

    private void OnClickStartButton(int index)
    {
        if (!DummyServerData.HasTicket(Player.GetUserID(), index))
        {
            Debug.Log("입장권이 부족합니다.");
            return;
        }
        dungeonManager.StartDungeon(index, TempDungeonStageLevel);
        highestClearedStage[index] = dungeonManager.DungeonLevels[index];
        UpdateTicketText(index);
    }

    private void OnClickSweepButton(int index)
    {
        if (!DummyServerData.HasTicket(Player.GetUserID(), index))
        {
            Debug.Log("입장권이 부족합니다.");
            return;
        }
        Debug.Log("소탕");
        highestClearedStage[index] = dungeonManager.DungeonLevels[index];
        UpdateTicketText(index);
    }

    private void UpdateTicketText(int index)
    {
        string ticketName = ticketNames[index];
        int sweepTicketCount = DummyServerData.GetTicketCount(Player.GetUserID(), index);

        ticketTexts[index].text = $"{ticketName} 조개패 {sweepTicketCount}개";
    }

    private void UpdateStageButtons(int tabIndex)
    {
        for (int i = 0; i < levelSelectButtons[tabIndex].Length; i++)
        {
            levelSelectButtons[tabIndex][i].interactable = (i + 1) <= highestClearedStage[tabIndex];
        }
    }
}
