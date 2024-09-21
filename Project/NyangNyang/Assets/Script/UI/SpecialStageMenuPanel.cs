using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialStageMenuPanel : MenuPanel
{
    [SerializeField]
    private ScrollRect scrollView; // ���� ��ũ���� ���� ScrollRect
    [SerializeField]
    private Button[] stageButtons; // ��ũ�ѵǴ� �������� ��ư��
    [SerializeField]
    private GameObject[] stageTabs; // ���������� �ǵ� (�� �ǿ� �� ���� ��ư ����)

    private GameObject currentActiveTab; // ���� Ȱ��ȭ�� ��

    void Start()
    {
        for (int i = 0; i < stageButtons.Length; i++)
        {
            int index = i; // Ŭ���� ���� �ذ��� ���� �ε����� ���� ������ ����
            stageButtons[i].onClick.AddListener(() => OnClickStageButton(index));
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
}
