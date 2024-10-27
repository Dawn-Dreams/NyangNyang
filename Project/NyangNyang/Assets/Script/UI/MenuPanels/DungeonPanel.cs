using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DungeonPanel : MenuPanel
{
    [SerializeField]
    private ScrollRect scrollView;
    private Button[] stageButtons;

    [SerializeField]
    private GameObject[] stageTabs;
    private Button[] minigameButtons, startButtons, sweepButtons;
    private TextMeshProUGUI[] ticketTexts, titleTexts;
    private ScrollRect[] levelScrollViews;
    private Button[][] levelSelectButtons;

    private DungeonManager DungeonManager;
    //private MiniGame1 miniGame1;

    // 현재 클리어한 최고 레벨을 Player가 저장하도록 수정 필요
    private int[] highestClearedStage = new int[3] { 1, 1, 1 };
    public int TempDungeonStageLevel { get; private set; }
    private int currentActiveTabIndex = 0;

    private void Start()
    {
        InitializeManagers();
        InitializeUIComponents();
        OnClickStageButton(0); // Default tab selection
    }

    private void InitializeManagers()
    {
        //miniGame1 = FindObjectOfType<MiniGame1>() ?? throw new NullReferenceException("MiniGame1이 NULL입니다.");
        DungeonManager = FindObjectOfType<DungeonManager>() ?? throw new NullReferenceException("DungeonManager가 존재하지 않습니다.");
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
        minigameButtons = new Button[tabCount];
        startButtons = new Button[tabCount];
        sweepButtons = new Button[tabCount];
        ticketTexts = new TextMeshProUGUI[tabCount];
        titleTexts = new TextMeshProUGUI[tabCount];
        levelScrollViews = new ScrollRect[tabCount];

        for (int i = 0; i < tabCount; i++)
        {
            var tab = stageTabs[i].transform;
            minigameButtons[i] = tab.Find("MiniGameStartButton").GetComponent<Button>();
            startButtons[i] = tab.Find("DungeonStartButton").GetComponent<Button>();
            sweepButtons[i] = tab.Find("DungeonSweepButton").GetComponent<Button>();
            ticketTexts[i] = tab.Find("TicketText").GetComponent<TextMeshProUGUI>();
            titleTexts[i] = tab.Find("GameTitleText").GetComponent<TextMeshProUGUI>();

            int index = i;
            minigameButtons[i].onClick.AddListener(() => OnClickMinigameButton(index));
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
        titleTexts[index].text = $"미니게임 던전 {index + 1}";
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
        titleTexts[tabIndex].text = $"미니게임 던전 {tabIndex + 1}-{TempDungeonStageLevel}";
    }

    // 미니게임 시작 버튼 클릭 시 실행
    void OnClickMinigameButton(int index)
    {
        // 스페셜 스테이지가 진행 중이면 미니게임을 시작하지 않음
        if (GameManager.isDungeonActive)
        {
            Debug.Log("스페셜 스테이지가 실행 중이므로 미니게임을 시작할 수 없습니다.");
            return;
        }

        // 미니게임이 이미 진행 중인지 확인
        if (GameManager.isMiniGameActive)
        {
            Debug.Log("다른 미니게임이 이미 실행 중입니다.");
            return;
        }


        switch (index)
        {
            case 0:
                SceneManager.LoadScene("MiniGame1", LoadSceneMode.Additive);
                //FindObjectOfType<MiniGame1>().StartGame();
                GameManager.isMiniGameActive = true;
                break;
            case 1:
                // FindObjectOfType<MiniGame2>().StartGame();
                Debug.Log("미니게임 2 시작버튼클릭");
                break;
            case 2:
                // FindObjectOfType<MiniGame3>().StartGame();
                Debug.Log("미니게임 3 시작버튼클릭");
                break;
            default:
                Debug.LogWarning("올바르지 않은 인덱스입니다.");
                break;
        }
        UpdateTicketText(index);
    }

    private void OnClickStartButton(int index)
    {
        if (!DummyServerData.HasTicket(Player.GetUserID(), index))
        {
            Debug.Log("입장권이 부족합니다.");
            return;
        }
        DungeonManager.StartDungeon(index, TempDungeonStageLevel);
        highestClearedStage[index] = DungeonManager.DungeonLevels[index];
        UpdateTicketText(index);
    }

    private void OnClickSweepButton(int index)
    {
        if (!DummyServerData.HasTicket(Player.GetUserID(), index))
        {
            Debug.Log("입장권이 부족합니다.");
            return;
        }
        //DungeonManager.StartDungeon(index, TempDungeonStageLevel);

        // 아이템 획득 로직 추가
        Debug.Log("소탕");
        highestClearedStage[index] = DungeonManager.DungeonLevels[index];
        UpdateTicketText(index);
    }

    private void UpdateTicketText(int index)
    {
        int sweepTicketCount = DummyServerData.GetTicketCount(Player.GetUserID(), index);
        ticketTexts[index].text = $"{index + 1}번 소탕권 개수: {sweepTicketCount}";
    }
    private void UpdateStageButtons(int tabIndex)
    {
        for (int i = 0; i < levelSelectButtons[tabIndex].Length; i++)
        {
            // 최고 클리어된 스테이지 이하의 버튼만 활성화
            levelSelectButtons[tabIndex][i].interactable = (i + 1) <= highestClearedStage[tabIndex];
        }
    }
}
