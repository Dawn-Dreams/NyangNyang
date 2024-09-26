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
    [SerializeField]
    private Button[] stageButtons; // ��ũ�ѵǴ� �������� ��ư��
    [SerializeField]
    private GameObject[] stageTabs; // ���������� �ǵ� (�� �ǿ� �� ���� ��ư ����)

    private GameObject currentActiveTab; // ���� Ȱ��ȭ�� ��
    private int currentActiveTabIndex = 0; // ���� Ȱ��ȭ�� �� �ε���

    // �� ���� ��ư��
    [SerializeField]
    private Button[] startButtons; // �̴ϰ��� ���� ��ư
    [SerializeField]
    private Button[] sweepButtons; // ���� ��ư
    [SerializeField]
    private TextMeshProUGUI[] sweepTicketTexts; // ������ ���� ǥ�� �ؽ�Ʈ

    private SpecialStageManager specialStageManager;

    private MiniGame1 miniGame1;

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

        for (int i = 0; i < stageButtons.Length; i++)
        {
            int index = i; // Ŭ���� ���� �ذ��� ���� �ε����� ���� ������ ����
            stageButtons[i].onClick.AddListener(() => OnClickStageButton(index));
        }

        for (int i = 0; i < startButtons.Length; i++)
        {
            int index = i;
            startButtons[i].onClick.AddListener(() => OnClickStartButton(index));
            sweepButtons[i].onClick.AddListener(() => OnClickSweepButton(index));

            // ���콺 ���� �̺�Ʈ �߰�
            AddEventTrigger(sweepButtons[i], EventTriggerType.PointerEnter, () => OnHoverSweepButton(index));
            AddEventTrigger(sweepButtons[i], EventTriggerType.PointerExit, () => OnExitSweepButton(index));
        }

        // ù ��° �� Ȱ��ȭ
        OnClickStageButton(0);
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
        sweepTicketTexts[index].text = "";
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
            specialStageManager.StartSpecialStage(index);
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
        sweepTicketTexts[index].text = $"{index + 1}�� ������ ����: {sweepTicketCount}";
    }
}
