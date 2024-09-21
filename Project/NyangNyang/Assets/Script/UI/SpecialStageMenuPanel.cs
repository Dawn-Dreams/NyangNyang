using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialStageMenuPanel : MenuPanel
{
    [SerializeField]
    private ScrollRect scrollView; // 가로 스크롤을 위한 ScrollRect
    [SerializeField]
    private Button[] stageButtons; // 스크롤되는 스테이지 버튼들
    [SerializeField]
    private GameObject[] stageTabs; // 스테이지별 탭들 (각 탭에 두 개의 버튼 존재)

    private GameObject currentActiveTab; // 현재 활성화된 탭

    void Start()
    {
        for (int i = 0; i < stageButtons.Length; i++)
        {
            int index = i; // 클로저 문제 해결을 위해 인덱스를 로컬 변수로 저장
            stageButtons[i].onClick.AddListener(() => OnClickStageButton(index));
        }

        // 첫 번째 탭 활성화
        OnClickStageButton(0);
    }

    void OnClickStageButton(int index)
    {
        // 모든 탭을 비활성화
        foreach (var tab in stageTabs)
        {
            tab.SetActive(false);
        }

        // 선택된 탭 활성화
        currentActiveTab = stageTabs[index];
        currentActiveTab.SetActive(true);

        SetButtonActiveColor(stageButtons[index]);
    }

    // 선택된 버튼 색상만 하이라이트 기능 ... 메뉴 패널클래스로 옮겨서 상속받아 사용할 수 있게 추후 변경
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
