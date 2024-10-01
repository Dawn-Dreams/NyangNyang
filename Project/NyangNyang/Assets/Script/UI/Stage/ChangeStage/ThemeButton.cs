using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeButton : MonoBehaviour
{
    public int startThemeNum = 1;
    private int stageBundle = 5;
    [SerializeField] private Sprite[] stageSprites;
    [SerializeField] private Image themeButtonImage;
    [SerializeField] private TextMeshProUGUI themeNameText;
    [SerializeField] private TextMeshProUGUI themeNumberBundleText;

    private void OnValidate()
    {
        if (startThemeNum % 5 != 1 || startThemeNum <= 0)
        {
            Debug.LogError("startThemeNum % 5 must 1 and PositiveNumber");
            return;
        }

        ChangeButtonStartThemeNum(startThemeNum);
    }

    public void ChangeButtonStartThemeNum(int newStartThemeNum)
    {
        if (startThemeNum % 5 != 1 || startThemeNum <= 0)
        {
            Debug.LogError("startThemeNum % 5 must 1 and PositiveNumber");
            return;
        }
        startThemeNum = newStartThemeNum;

        int themeStep = ((startThemeNum - 1) / stageBundle);
        Sprite themeBGSprite = stageSprites[themeStep % stageSprites.Length];
        themeButtonImage.sprite = themeBGSprite;
        themeNameText.text = "지역 이름";
        themeNumberBundleText.text = "(" + startThemeNum + " ~ " + (startThemeNum + stageBundle - 1) + ")";
    }
}
