using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeStageUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private StageManager stageManager;
    [SerializeField] private Button backImage;

    [SerializeField] private GameObject themeButtonPrefab;
    [SerializeField] private GameObject ScrollViewContentObject;

    private void Awake()
    {
        closeButton.onClick.AddListener(CloseChangeStageUI);
        backImage.onClick.AddListener(CloseChangeStageUI);
        
        OnAwakeThemeButtonCreate();

    }

    void SelectThemeButton(int themeButtonNumber)
    {
        int themeNumber = themeButtonNumber * 5 + 1;
        Debug.Log(themeNumber + "스테이지에 대한 정보 로드");
    }

    void CloseChangeStageUI()
    {
        gameObject.SetActive(false);
    }



    void OnAwakeThemeButtonCreate()
    {
        if (ScrollViewContentObject.transform.childCount < 10)
        {
            // Theme 버튼 생성
            for (int i = 0; i < 20; ++i)
            {
                GameObject themeButtonObj = Instantiate(themeButtonPrefab, ScrollViewContentObject.transform);
                themeButtonObj.GetComponent<ThemeButton>().ChangeButtonStartThemeNum(i * 5 + 1);
            }
            
            // Theme 버튼 OnClick 이벤트 연결
            int themeButtonCount = ScrollViewContentObject.transform.childCount;
            for (int i = 0; i < themeButtonCount; ++i)
            {
                Button themeButton = ScrollViewContentObject.transform.GetChild(i).gameObject.GetComponent<Button>();
                int themeButtonNum = i;
                themeButton.onClick.AddListener(() => SelectThemeButton(themeButtonNum));
            }

        }
    }
}
