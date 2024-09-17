using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum LevelUpButtonType
{
    X1 = 0, X10, X100
}


public class StatusMenuPanel : MenuPanel
{
    [SerializeField]
    private Button[] levelUpMultipleButtons;
    private Button currentActiveLevelUpMultipleButton;

    private StatusLevelupPanel[] levelUpPanels;

    //private StatusLevelupPanel[] statusLevelUpPanels;
    
    void Start()
    {
        levelUpPanels = GetComponentsInChildren<StatusLevelupPanel>();

        levelUpMultipleButtons[(int)LevelUpButtonType.X1].onClick.AddListener(
            () => OnClickEventLevelUpButton(1, levelUpMultipleButtons[(int)LevelUpButtonType.X1])
            );
        levelUpMultipleButtons[(int)LevelUpButtonType.X10].onClick.AddListener(
            () => OnClickEventLevelUpButton(10, levelUpMultipleButtons[(int)LevelUpButtonType.X10])
            );
        levelUpMultipleButtons[(int)LevelUpButtonType.X100].onClick.AddListener(
            () => OnClickEventLevelUpButton(100, levelUpMultipleButtons[(int)LevelUpButtonType.X100])
            );

        //SetButtonActiveColor(levelUpMultipleButtons[(int)LevelUpButtonType.X1]);
        OnClickEventLevelUpButton(1, levelUpMultipleButtons[(int)LevelUpButtonType.X1]);
    }

    void OnClickEventLevelUpButton(int buttonValue, Button targetButton)
    {
        foreach (StatusLevelupPanel panel in levelUpPanels)
        {
            panel.ChangeMultiplyValue(buttonValue);
        }

        SetButtonActiveColor(targetButton);
    }

    // 선택된 버튼 색상만 하이라이트 기능
    void SetButtonActiveColor(Button targetButton)
    {
        foreach (Button button in levelUpMultipleButtons)
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
        tempCB.normalColor = new Color(0.25f,0.25f,0.25f);
        tempCB.highlightedColor = new Color(0.25f, 0.25f, 0.25f);
        tempCB.selectedColor = new Color(0.25f, 0.25f, 0.25f);
        tempCB.pressedColor = new Color(0.25f, 0.25f, 0.25f);
        targetButton.colors = tempCB;
    }
}
