using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassUI : MonoBehaviour
{
    public PassRewardItemButton passRewardButtonPrefab;

    public GridLayoutGroup passButtonGridLayoutGroup;
    public RectTransform contentRectTransform;
    public RectTransform sliderRectTransform;

    public RectTransform goldContentRectTransform;
    public RectTransform freeContentRectTransform;
    private List<PassRewardItemButton> _goldRewardButtons = new List<PassRewardItemButton>();
    private List<PassRewardItemButton> _freeRewardButtons = new List<PassRewardItemButton>();


    public int levelCount = 10;
    private int userLevel = 1;

    void Awake()
    {
        CreateButtons();

        SetContentSizeFitter();
    }

    private void SetContentSizeFitter()
    {
        float cellSizeY = passButtonGridLayoutGroup.cellSize.y;
        float stepY = passButtonGridLayoutGroup.spacing.y;
        float length = (cellSizeY + stepY) * levelCount;
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x,length);
        float sliderLength = (cellSizeY + stepY) * (levelCount-1);
        sliderRectTransform.sizeDelta = new Vector2(sliderRectTransform.sizeDelta.x, sliderLength);
    }

    private void CreateButtons()
    {
        for (int i = 0; i < levelCount; ++i)
        {
            // 유료 패스 버튼 생성
            PassRewardItemButton goldButton = Instantiate(passRewardButtonPrefab, goldContentRectTransform.gameObject.transform);
            PassButtonState clearState;
            if (userLevel > i)
            {
                clearState = PassButtonState.Open;
                // 이미 받은건지에 대해서도 체크해야함
            }
            else
            {
                clearState = PassButtonState.Close;
            }
            goldButton.SetTypes(RewardType.Diamond, PassCategoryType.Gold, clearState);
            goldButton.gameObject.transform.SetSiblingIndex(i);
            _goldRewardButtons.Add(goldButton);

            PassRewardItemButton freeButton = Instantiate(passRewardButtonPrefab, freeContentRectTransform.gameObject.transform);
            clearState = PassButtonState.Count;
            if (userLevel > i)
            {
                clearState = PassButtonState.Open;
                // 이미 받은건지에 대해서도 체크해야함
            }
            else
            {
                clearState = PassButtonState.Close;
            }
            freeButton.SetTypes(RewardType.Gold, PassCategoryType.Free, clearState);
            freeButton.gameObject.transform.SetSiblingIndex(i);
            _goldRewardButtons.Add(freeButton);
        }
    }
}
