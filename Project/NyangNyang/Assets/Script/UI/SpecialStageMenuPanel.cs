using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    void Start()
    {
        for (int i = 0; i < stageButtons.Length; i++)
        {
            int index = i; // Ŭ���� ���� �ذ��� ���� �ε����� ���� ������ ����
            stageButtons[i].onClick.AddListener(() => OnClickStageButton(index));
        }

        // �� ���� ��ư�� Ŭ�� �̺�Ʈ ���
        for (int i = 0; i < startButtons.Length; i++)
        {
            int index = i;
            startButtons[i].onClick.AddListener(() => OnClickStartButton(index));
            sweepButtons[i].onClick.AddListener(() => OnClickSweepButton(index));
        }

        // ù ��° �� Ȱ��ȭ
        OnClickStageButton(0);
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
        // Special Stage ���� ����
        Debug.Log($"Special Stage {index + 1} ����!");

        // �������� ������ Ȯ�� �� �������� ����
        if (DummyServerData.HasSweepTicket(Player.GetUserID()))
        {
            DummyServerData.UseSweepTicket(Player.GetUserID());
            UpdateSweepTicketText(index);
            Debug.Log("����� ���������� �����մϴ�. �������� �ϳ� �����߽��ϴ�.");
        }
        else
        {
            Debug.Log("�������� �����մϴ�.");
        }
    }

    // ���� ��ư Ŭ�� �� ����
    void OnClickSweepButton(int index)
    {
        // ���� ��� ���� ���� (������ ���� �߰�)
        Debug.Log($"���� ��ư {index + 1} Ŭ����.");
    }

    // ������ ���� ������Ʈ
    void UpdateSweepTicketText(int index)
    {
        int sweepTicketCount = DummyServerData.GetSweepTicketCount(Player.GetUserID());
        sweepTicketTexts[index].text = $"������: {sweepTicketCount}";
    }
}
