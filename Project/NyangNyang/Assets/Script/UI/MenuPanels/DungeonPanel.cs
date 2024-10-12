using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private DungeonManager DungeonManager;
    private MiniGame1 miniGame1;

    // ���� Ŭ������ �ְ� ������ Player�� �����ϵ��� ���� �ʿ�
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
        miniGame1 = FindObjectOfType<MiniGame1>() ?? throw new NullReferenceException("MiniGame1�� NULL�Դϴ�.");
        DungeonManager = FindObjectOfType<DungeonManager>() ?? throw new NullReferenceException("DungeonManager�� �������� �ʽ��ϴ�.");
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
            startButtons[i] = tab.Find("MiniGameStartButton").GetComponent<Button>();
            sweepButtons[i] = tab.Find("DungeonStartButton").GetComponent<Button>();
            //sweepButtons[i] = tab.Find("DungeonSweepButton").GetComponent<Button>();
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
            int level = j + 1; // ������ 1���� �����ϹǷ� j + 1�� ����
            buttons[j].onClick.RemoveAllListeners(); // ���� ������ ����
            buttons[j].onClick.AddListener(() => OnClickStageLevelButton(tabIndex, level - 1)); // levelIndex�� 0���� �����ϹǷ� level - 1
            buttons[j].interactable = level <= highestClearedStage[tabIndex]; // �ְ� Ŭ����� ���������� Ȱ��ȭ
            buttons[j].GetComponentInChildren<TextMeshProUGUI>().text = $"���� LEVEL {level}"; // ���� ��ȣ ǥ��
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
            int level = i + 1; // ������ 1���� ����
            UpdateStageButtons(tabIndex);
            levelSelectButtons[tabIndex][i].GetComponentInChildren<TextMeshProUGUI>().text = $"���� LEVEL {level}"; // ���� ��ȣ ǥ��
        }
    }

    private void OnClickStageButton(int index)
    {
        SetActiveTab(index);
        titleTexts[index].text = $"�̴ϰ��� ���� {index + 1}";
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
        titleTexts[tabIndex].text = $"�̴ϰ��� ���� {tabIndex + 1}-{TempDungeonStageLevel}";
    }

    // �̴ϰ��� ���� ��ư Ŭ�� �� ����
    void OnClickStartButton(int index)
    {
        // ����� ���������� ���� ���̸� �̴ϰ����� �������� ����
        if (GameManager.isDungeonActive)
        {
            Debug.Log("����� ���������� ���� ���̹Ƿ� �̴ϰ����� ������ �� �����ϴ�.");
            return;
        }

        // �̴ϰ����� �̹� ���� ������ Ȯ��
        if (GameManager.isMiniGameActive)
        {
            Debug.Log("�ٸ� �̴ϰ����� �̹� ���� ���Դϴ�.");
            return;
        }


        switch (index)
        {
            case 0:
                FindObjectOfType<MiniGame1>().StartGame();
                GameManager.isMiniGameActive = true;
                break;
            case 1:
                // FindObjectOfType<MiniGame2>().StartGame();
                Debug.Log("�̴ϰ��� 2 ���۹�ưŬ��");
                break;
            case 2:
                // FindObjectOfType<MiniGame3>().StartGame();
                Debug.Log("�̴ϰ��� 3 ���۹�ưŬ��");
                break;
            default:
                Debug.LogWarning("�ùٸ��� ���� �ε����Դϴ�.");
                break;
        }
        UpdateTicketText(index);
    }

    private void OnClickSweepButton(int index)
    {
        if (!DummyServerData.HasTicket(Player.GetUserID(), index))
        {
            Debug.Log("�������� �����մϴ�.");
            return;
        }
        DungeonManager.StartDungeon(index, TempDungeonStageLevel);
        highestClearedStage[index] = DungeonManager.DungeonLevels[index];
        UpdateTicketText(index);
    }

    private void UpdateTicketText(int index)
    {
        int sweepTicketCount = DummyServerData.GetTicketCount(Player.GetUserID(), index);
        ticketTexts[index].text = $"{index + 1}�� ������ ����: {sweepTicketCount}";
    }
    private void UpdateStageButtons(int tabIndex)
    {
        for (int i = 0; i < levelSelectButtons[tabIndex].Length; i++)
        {
            // �ְ� Ŭ����� �������� ������ ��ư�� Ȱ��ȭ
            levelSelectButtons[tabIndex][i].interactable = (i + 1) <= highestClearedStage[tabIndex];
        }
    }
}
