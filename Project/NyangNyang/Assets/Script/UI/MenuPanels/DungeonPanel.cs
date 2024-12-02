using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class DungeonPanel : MenuPanel
{
    [SerializeField]
    private GameObject stageButtonsParent;
    private Button[] stageButtons;

    [SerializeField]
    private GameObject[] stageTabs;
    private Button[] startButtons, sweepButtons;
    private TextMeshProUGUI[] shellTexts, titleTexts, warningText;
    private ScrollRect[] levelScrollViews;
    private Button[][] levelSelectButtons;

    [SerializeField] private GameObject rewardPopup;
    [SerializeField] private TextMeshProUGUI goldText;    // 팝업의 골드 텍스트
    [SerializeField] private TextMeshProUGUI expText;     // 팝업의 경험치 텍스트
    [SerializeField] private int gainGold = 10000;               // 기본 골드 획득량
    [SerializeField] private int gainEXP = 1000;               // 기본 경험치 획득량

    private DungeonManager dungeonManager;

    private readonly List<string> dungeonNames = new List<string> { "황야의 대지", "눈꽃 동굴", "독거미 숲" };
    private readonly List<string> shellNames = new List<string> { "노랑", "파랑", "빨강" };

    private int[] dungeonHighestClearLevel = new int[3] { 1, 1, 1 };
    public int TempDungeonStageLevel { get; private set; }
    private int currentActiveTabIndex = 0;

    private void OnEnable()
    {
        InitializeManagers();
        InitializeUIComponents();
        SetActiveTab(0); // 기본 탭 선택
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
        HideRewardPopup();
    }

    private void InitializeStageButtons()
    {
        List<Button> buttons = new List<Button>();
        foreach (Transform child in stageButtonsParent.transform)
        {
            Button button = child.GetComponent<Button>();
            if (button != null)
            {
                buttons.Add(button);
            }
        }

        stageButtons = buttons.ToArray();

        for (int i = 0; i < stageButtons.Length; i++)
        {
            int index = i; // 클로저 문제 해결
            stageButtons[i].onClick.AddListener(() => OnClickStageButton(index));
        }

        // 정식출시 전
        //Player.SetShell(0, Player.GetShell(0) + 1);
    }

    private void InitializeTabs()
    {
        int tabCount = stageTabs.Length;
        startButtons = new Button[tabCount];
        sweepButtons = new Button[tabCount];
        shellTexts = new TextMeshProUGUI[tabCount];
        titleTexts = new TextMeshProUGUI[tabCount];
        levelScrollViews = new ScrollRect[tabCount];

        for (int i = 0; i < tabCount; i++)
        {
            var tab = stageTabs[i].transform;

            startButtons[i] = tab.Find("DungeonStartButton").GetComponent<Button>();
            sweepButtons[i] = tab.Find("DungeonSweepButton").GetComponent<Button>();
            shellTexts[i] = tab.Find("ShellText").GetComponent<TextMeshProUGUI>();
            titleTexts[i] = tab.Find("GameTitleText").GetComponent<TextMeshProUGUI>();

            int index = i;

            // 기존 리스너 제거 후 리스너 등록
            startButtons[i].onClick.RemoveAllListeners();
            sweepButtons[i].onClick.RemoveAllListeners();

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
        dungeonHighestClearLevel[tabIndex]= dungeonManager.dungeonHighestClearLevel[tabIndex];
        for (int j = 0; j < buttons.Length; j++)
        {
            int level = j + 1; // 레벨은 1부터 시작하므로 j + 1로 설정
            buttons[j].onClick.RemoveAllListeners(); // 이전 리스너 제거
            buttons[j].onClick.AddListener(() => OnClickStageLevelButton(tabIndex, level - 1)); // levelIndex는 0부터 시작하므로 level - 1
            buttons[j].interactable = level <= dungeonHighestClearLevel[tabIndex]; // 최고 클리어된 레벨까지만 활성화
            buttons[j].GetComponentInChildren<TextMeshProUGUI>().text = $"레벨 {level}"; // 레벨 번호 표시
        }
        // 조개패 개수 init
        UpdateShellText(tabIndex);
    }


    public void OnStageCleared(int tabIndex, int clearedStageLevel)
    {
        dungeonHighestClearLevel[tabIndex] = dungeonManager.dungeonHighestClearLevel[tabIndex];
        if (clearedStageLevel >= dungeonHighestClearLevel[tabIndex])
        {
            dungeonHighestClearLevel[tabIndex] = clearedStageLevel;
            UpdateLevelSelectButtons(tabIndex);
        }
        GetReward(tabIndex);
        UpdateStageButtons(tabIndex);
    }

    private void UpdateLevelSelectButtons(int tabIndex)
    {
        for (int i = 0; i < levelSelectButtons[tabIndex].Length; i++)
        {
            int level = i + 1; // 레벨은 1부터 시작
            UpdateStageButtons(tabIndex);
            levelSelectButtons[tabIndex][i].GetComponentInChildren<TextMeshProUGUI>().text = $"던전 레벨 {level}"; // 레벨 번호 표시
        }
    }

    private void OnClickStageButton(int index)
    {
        SetActiveTab(index);
        titleTexts[index].text = $"{dungeonNames[index]}";
        UpdateShellText(index);
    }

    private void SetActiveTab(int index)
    {
        // 배열의 범위 내인지 확인
        if (index < 0 || index >= stageTabs.Length)
        {
            return;
        }

        foreach (var tab in stageTabs)
            tab.SetActive(false);

        stageTabs[index].SetActive(true);
        currentActiveTabIndex = index;
    }

    private void OnClickStageLevelButton(int tabIndex, int levelIndex)
    {
        TempDungeonStageLevel = levelIndex + 1;
        UpdateLevelSelectButtons(tabIndex);
        dungeonManager.currentDungeonLevel = TempDungeonStageLevel;
        titleTexts[tabIndex].text = $"{dungeonNames[tabIndex]} - 레벨 {TempDungeonStageLevel}";
    }

    private void OnClickStartButton(int index)
    {
        if (Player.GetShell(index) <= 0)
        {
            WarningText.Instance.Set("조개패가 부족합니다");
            return;
        }
        dungeonManager.StartDungeon(index, TempDungeonStageLevel);

        dungeonHighestClearLevel[index] = dungeonManager.dungeonHighestClearLevel[index];
        UpdateShellText(index);
    }

    private void OnClickSweepButton(int index)
    {
        if (Player.GetShell(index)<=0)
        {
            WarningText.Instance.Set("조개패가 부족합니다");
            return;
        }
        if (TempDungeonStageLevel < dungeonHighestClearLevel[index])
        {
            WarningText.Instance.Set($"<color=#2AEFB7>Level{TempDungeonStageLevel} 소탕 완료!</color>");
            Player.SetShell(index, Player.GetShell(index) - 1); // 티켓 차감
            dungeonManager.currentDungeonLevel = TempDungeonStageLevel;
            UpdateShellText(index);
            GetReward(index);
        }
        else
            WarningText.Instance.Set("아직 클리어 되지 않았습니다");
    }

    private void UpdateShellText(int index)
    {
        string shellName = shellNames[index];
        int sweepShellCount = Player.GetShell(index);

        shellTexts[index].text = $"{shellName} 조개패 {sweepShellCount}개";
    }

    private void UpdateStageButtons(int tabIndex)
    {
        for (int i = 0; i < levelSelectButtons[tabIndex].Length; i++)
        {
            levelSelectButtons[tabIndex][i].interactable = (i + 1) <= dungeonHighestClearLevel[tabIndex];
        }
    }
    private void GetReward(int tabIndex)
    {
        // 보상 계산
        int rewardGold = Mathf.CeilToInt(gainGold * Mathf.Pow(dungeonManager.currentDungeonLevel, 1.2f));
        int rewardEXP = Mathf.CeilToInt(gainEXP * Mathf.Pow(dungeonManager.currentDungeonLevel, 1.2f));

        // 플레이어에게 골드와 경험치 지급
        Player.AddGold(rewardGold);
        Player.AddExp(rewardEXP);

        // 보상 팝업 업데이트
        ShowRewardPopup(rewardGold, rewardEXP);
    }

    private void ShowRewardPopup(int rewardGold, int rewardEXP)
    {
        if (rewardPopup == null)
        {
            return;
        }

        // 팝업 텍스트 업데이트
        goldText.text = $"{rewardGold:N0} 골드 획득";
        expText.text = $"{rewardEXP:N0} 경험치 획득";

        // 팝업 활성화
        rewardPopup.SetActive(true);
    }

    public void HideRewardPopup()
    {
        if (rewardPopup != null)
        {
            rewardPopup.SetActive(false);
        }
    }
}
