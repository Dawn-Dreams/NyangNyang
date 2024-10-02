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
    private ScrollRect scrollView; // ���� ��ũ���� ���� ScrollRect
    private Button[] stageButtons; // ��ũ�ѵǴ� �������� ��ư��

    [SerializeField]
    private GameObject[] stageTabs; // ���������� �ǵ� (�� �ǿ� �� ���� ��ư ����)

    private GameObject currentActiveTab; // ���� Ȱ��ȭ�� ��
    private int currentActiveTabIndex = 0; // ���� Ȱ��ȭ�� �� �ε���

    // �� ���� ��ư��
    private Button[] startButtons; // �̴ϰ��� ���� ��ư
    private Button[] sweepButtons; // ���� ��ư
    private TextMeshProUGUI[] ticketTexts; // Ƽ�� ���� ǥ�� �ؽ�Ʈ
    private TextMeshProUGUI[] titleTexts;

    private SpecialStageManager specialStageManager;

    private MiniGame1 miniGame1;

    [SerializeField]
    private ScrollRect[] levelScrollView;
    private Button[] levelSelectButtons;

    public int tempSpecialStageLevel { get; private set; } // ���� ���õ� ����� �������� ����
    private int highestClearedStage = 1;            // ���� Ŭ����� �ְ� �������� (1���� ����)
    private int miniGameIndex = 1;
    void Start()
    {
        miniGame1 = FindObjectOfType<MiniGame1>();

        if (miniGame1 == null) Debug.LogError("MiniGame1 ��ü�� ã�� �� �����ϴ�.");

        specialStageManager = FindObjectOfType<SpecialStageManager>();
        if (specialStageManager == null)
        {
            Debug.LogError("SpecialStageManager�� ã�� �� �����ϴ�. SpecialStageManager�� ���� �߰��Ǿ����� Ȯ���ϼ���.");
            return;
        }

        stageButtons = scrollView.content.GetComponentsInChildren<Button>();
        for (int i = 0; i < stageButtons.Length; i++)
        {
            int index = i; // Ŭ���� ���� �ذ��� ���� �ε����� ���� ������ ����
            stageButtons[i].onClick.AddListener(() => OnClickStageButton(index));
        }

        // �� ���� ������ �ִ� ��ư�� �ؽ�Ʈ�� ã�Ƽ� �ڵ����� �Ҵ�
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

            // Scroll View�� ã�Ƽ� levelScrollView�� �Ҵ�
            //levelScrollView[i] = stageTabs[i].transform.Find("Scroll View").GetComponent<ScrollRect>();
            int index = i;
            startButtons[i].onClick.AddListener(() => OnClickStartButton(index));
            sweepButtons[i].onClick.AddListener(() => OnClickSweepButton(index));
            
            // Scroll View�� Content ������ ��� ��ư���� ������
            levelSelectButtons = levelScrollView[i].content.GetComponentsInChildren<Button>();

            // ���콺 ���� �̺�Ʈ �߰�
            AddEventTrigger(sweepButtons[i], EventTriggerType.PointerEnter, () => OnHoverSweepButton(index));
            AddEventTrigger(sweepButtons[i], EventTriggerType.PointerExit, () => OnExitSweepButton(index));
        }

        for (int i = 0; i < levelSelectButtons.Length; i++)
        {
            int level = i;
            levelSelectButtons[i].onClick.AddListener(() => OnClickStageLevelButton(level));

            // ��ư �Ʒ��� �ִ� �ؽ�Ʈ�� ã�� �ε����� ǥ��
            TextMeshProUGUI buttonText = levelSelectButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = "���� LEVEL " + (level + 1); // 1���� �����ϴ� �ε��� ��ȣ�� ǥ��
            }
            // �ְ� Ŭ���� ���������� �������� ��ư Ȱ��ȭ/��Ȱ��ȭ
            levelSelectButtons[i].interactable = (level + 1) <= highestClearedStage;
        }
        // ù ��° �� Ȱ��ȭ
        OnClickStageButton(0);
    }
    // Ư�� �������� Ŭ���� �� ȣ���� �޼���
    public void OnStageCleared(int clearedStageLevel)
    {
        if (clearedStageLevel > highestClearedStage)
        {
            highestClearedStage = clearedStageLevel;
        }

        // ��ư���� Ȱ��ȭ ���¸� �ٽ� üũ
        UpdateStageButtons();
    }

    // �������� ��ư ���� ������Ʈ
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
        tempSpecialStageLevel = index + 1; // ��ư �ε����� ����� �������� ������ ����
        UpdateStageButtons();
        if (titleTexts[index] != null)
        {
            titleTexts[index].text = "�̴ϰ��� ���� " + (index + 1); // 1���� �����ϴ� �ε��� ��ȣ�� ǥ��
        }
        //Debug.Log("���õ� ����� �������� ����: " + tempSpecialStageLevel);
    }

    // EventTrigger�� �̺�Ʈ �߰� �Լ�
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

    // ���콺�� ���� ��ư ���� ���� �� ����
    void OnHoverSweepButton(int index)
    {
        UpdateTicketText(index);
    }

    // ���콺�� ���� ��ư���� ������ �� ����
    void OnExitSweepButton(int index)
    {
        ticketTexts[index].text = "";
    }

    void OnClickStageButton(int index)
    {
        // ��� ���� ��Ȱ��ȭ
        foreach (var tab in stageTabs)
        {
            tab.SetActive(false);
        }

        // ���õ� �� Ȱ��ȭ
        currentActiveTab = stageTabs[index];
        currentActiveTab.SetActive(true);
        currentActiveTabIndex = index;

        SetButtonActiveColor(stageButtons[index]);

        if (titleTexts[index] != null)
        {
            titleTexts[index].text = "�̴ϰ��� ���� " + (index + 1); // 1���� �����ϴ� �ε��� ��ȣ�� ǥ��
        }
        // ���õ� �ǿ� �´� ������ ���� ������Ʈ
        UpdateTicketText(index);
    }

    // ���õ� ��ư ���� ���̶���Ʈ ��� ... �޴� �г�Ŭ������ �Űܼ� ��ӹ޾� ����� �� �ְ� ���� ����
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

    // �̴ϰ��� ���� ��ư Ŭ�� �� ����
    void OnClickStartButton(int index)
    {
        // �̴ϰ��� ����
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
                Debug.LogWarning("�ùٸ��� ���� �ε����Դϴ�.");
                break;
        }

    }

    // ���� ��ư Ŭ�� �� ����
    void OnClickSweepButton(int index)
    {
        // ���� ���������� �´� ������ ���
        if (DummyServerData.HasTicket(Player.GetUserID(), index))
        {
            if (index < 0 || index >= 3)
            {
                Debug.LogError($"�߸��� ����� �������� �ε���: {index}");
                return;
            }
            DummyServerData.UseTicket(Player.GetUserID(), index);
            specialStageManager.StartSpecialStage(index, tempSpecialStageLevel);
            highestClearedStage = specialStageManager.specialStageLevels[index];
            //Debug.Log("�ְ� ���� ��������" + highestClearedStage);
        }
        else
        {
            Debug.Log("�������� �����մϴ�.");
        }
    }

    // ������ ���� �ؽ�Ʈ ������Ʈ
    void UpdateTicketText(int index)
    {
        int sweepTicketCount = DummyServerData.GetTicketCount(Player.GetUserID(), index);
        ticketTexts[index].text = $"{index + 1}�� ������ ����: {sweepTicketCount}";
    }
}
