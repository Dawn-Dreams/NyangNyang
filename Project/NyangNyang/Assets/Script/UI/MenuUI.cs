using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    private MenuButton[] menuButtons;
    private MenuPanel[] menuPanels;

    // 디테일 패널에서의 액티브 인덱스
    public int startActiveIndex = -1;
    // 실제 코드 상 액티브 인덱스
    private int currentActiveIndex = -1;

    void Start() 
    {
        menuButtons = GetComponentsInChildren<MenuButton>();
        menuPanels = GetComponentsInChildren<MenuPanel>(true);

        for (int i = 0; i < menuButtons.Length; ++i)
        {
            int index = i;
            menuButtons[i].gameObject.GetComponent<Button>().onClick.AddListener(()=>OnClickMenuButtons(index));
        }
    }

    void OnClickMenuButtons(int index)
    {
        if (menuButtons[index] == null) return;
        // 선택중인 패널일 경우 리턴
        if (currentActiveIndex == index) return;

        int lastActiveIndex = currentActiveIndex;
        currentActiveIndex = index;

        if(lastActiveIndex >= 0)
            SetActiveMenu(lastActiveIndex,false);

        SetActiveMenu(currentActiveIndex, true);
    }

    private void SetActiveMenu(int index, bool newActive)
    {
        
        menuButtons[index].SetButtonActivity(newActive);
        menuPanels[index].gameObject.SetActive(newActive);
    }

    private void OnValidate()
    {
        MenuButton[] menuButtonsForDebug = GetComponentsInChildren<MenuButton>();
        MenuPanel[] menuPanelsForDebug = GetComponentsInChildren<MenuPanel>(true);

        foreach (MenuButton menuButton in menuButtonsForDebug)
        {
            menuButton.SetButtonActivityOnDebug(false);
        }

        foreach (MenuPanel menuPanel in menuPanelsForDebug)
        {
            menuPanel.gameObject.SetActive(false);
        }

        currentActiveIndex = startActiveIndex;

        if (startActiveIndex < 0 || startActiveIndex >= menuButtonsForDebug.Length) return;

        if (!menuButtonsForDebug[startActiveIndex].isActive || !menuPanelsForDebug[startActiveIndex].gameObject.activeInHierarchy)
        {
            menuButtonsForDebug[startActiveIndex].SetButtonActivityOnDebug(true);
            menuPanelsForDebug[startActiveIndex].gameObject.SetActive(true);   
        }
    }
}
