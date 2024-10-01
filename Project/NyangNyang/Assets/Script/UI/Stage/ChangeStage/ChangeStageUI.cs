using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeStageUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private StageManager stageManager;
    [SerializeField] private Button backImage;

    [SerializeField] private GameObject themeButtonPrefab;
    [SerializeField] private GameObject themeButtonScrollViewContentObject;

    [SerializeField] private GameObject stageButtonPrefab;
    [SerializeField] private GameObject stageButtonScrollViewContentObject;

    [SerializeField] private GameObject[] themeNumberObject;
    private Image[] themeNumberImages;
    private TextMeshProUGUI[] themeNumberTexts;
    [SerializeField] private Sprite themeNumberNormalSprite; 
    [SerializeField] private Sprite themeNumberSelectSprite;

    private StageButton[] stageButtons;

    private int _curStartThemeNum = 0;
    private int _curSelectThemeNum = 0;
    private int _curSelectStageNum = 0;

    [SerializeField] private Button changeStageButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(CloseChangeStageUI);
        backImage.onClick.AddListener(CloseChangeStageUI);
        changeStageButton.onClick.AddListener(ChangeStage);

        OnAwakeThemeStepButtonCreate();
        OnAwakestageButtonCreate();
        OnAwakeThemeNumberButtonAddListener();

        SetInitialData();
    }

    void OnEnable()
    {
        SetInitialData();
    }

    void SetInitialData()
    {
        _curStartThemeNum =
            (stageManager.GetCurrentTheme() - 1) / stageManager.maxStageCount * stageManager.maxStageCount + 1;
        SelectStageThemeNumberButton((stageManager.GetCurrentTheme() - 1) % stageManager.maxStageCount);
        SelectStageNumberButton(stageManager.GetCurrentStage());
    }


    private void OnAwakeThemeNumberButtonAddListener()
    {
        themeNumberImages = new Image[themeNumberObject.Length];
        themeNumberTexts = new TextMeshProUGUI[themeNumberObject.Length];

        for (int i = 0; i < themeNumberObject.Length; ++i)
        {
            themeNumberImages[i] = themeNumberObject[i].GetComponent<Image>();
            themeNumberTexts[i] = themeNumberObject[i].GetComponentInChildren<TextMeshProUGUI>();
            int buttonID = i;
            themeNumberObject[i].GetComponent<Button>().onClick.AddListener(()=>SelectStageThemeNumberButton(buttonID));
        }
    }

    void SelectThemeStepButton(int themeStepButtonNumber)
    {
        _curStartThemeNum = themeStepButtonNumber * stageButtons.Length + 1;

        for (int i = 0; i < themeNumberTexts.Length; ++i)
        {
            themeNumberTexts[i].text = (_curStartThemeNum + i).ToString();
        }

        SelectStageThemeNumberButton(0);
    }

    void SelectStageThemeNumberButton(int buttonNumberID)
    {
        if (0 <= _curSelectThemeNum && _curSelectThemeNum < themeNumberImages.Length)
        {
            themeNumberImages[_curSelectThemeNum].sprite = themeNumberNormalSprite;
        }

        for (int i = 0; i < themeNumberTexts.Length; ++i)
        {
            themeNumberTexts[i].text = (_curStartThemeNum + i).ToString();
        }

        _curSelectThemeNum = buttonNumberID;
        Debug.Log(_curStartThemeNum + _curSelectThemeNum + "번째 스테이지 정보 출력");
        themeNumberImages[_curSelectThemeNum].sprite = themeNumberSelectSprite;

        // 스테이지 버튼은 Theme가 변경될 때 마다 갱신 해줘야함
        int highestTheme = 0;
        int highestStage = 0;
        Player.GetPlayerHighestClearStageData(out highestTheme, out highestStage);
        
        for (int i = 0; i < stageButtons.Length; ++i)
        {
            int curTheme = _curStartThemeNum + _curSelectThemeNum;
            int curStage = i + 1;
            stageButtons[i].GetButton().interactable = true;

            // 플레이어가 이동 가능한 스테이지 버튼 
            if (curTheme <= highestTheme && curStage <= highestStage + 1)
            {
                // 현재 있는 버튼
                if (curTheme == stageManager.GetCurrentTheme() && curStage == stageManager.GetCurrentStage())
                {
                    stageButtons[i].SetButtonType(StageButtonType.Current);
                    continue;
                }
                stageButtons[i].SetButtonType(StageButtonType.Normal);
            }
            // 예외로 curTheme의 마지막 스테이지를 클리어 했을 경우 highestTheme + 1 테마의 1스테이지는 오픈해야함
            else if (curTheme == highestTheme + 1 && highestStage == stageButtons.Length && curStage == 1)
            {
                stageButtons[i].SetButtonType(StageButtonType.Normal);
            }
            else
            {
                stageButtons[i].SetButtonType(StageButtonType.Close);
                stageButtons[i].GetButton().interactable = false;
            }
        }
    }

    void SelectStageNumberButton(int buttonNumberID)
    {
        if (1 <= _curSelectStageNum && _curSelectStageNum <= stageButtons.Length)
        {
            stageButtons[_curSelectStageNum - 1].UnSelect();
        }
        Debug.Log((_curStartThemeNum + _curSelectThemeNum) + " - " + buttonNumberID + "스테이지 정보 출력");
        _curSelectStageNum = buttonNumberID;
        stageButtons[_curSelectStageNum - 1].SetButtonType(StageButtonType.Select);
    }

    void ChangeStage()
    {
        stageManager.GoToSpecificStage(_curStartThemeNum+_curSelectThemeNum,_curSelectStageNum);
        CloseChangeStageUI();
    }

    void CloseChangeStageUI()
    {
        gameObject.SetActive(false);
    }

    void OnAwakeThemeStepButtonCreate()
    {
        if (themeButtonScrollViewContentObject.transform.childCount < 10)
        {
            // Theme 버튼 생성
            for (int i = 0; i < 20; ++i)
            {
                GameObject themeButtonObj = Instantiate(themeButtonPrefab, themeButtonScrollViewContentObject.transform);
                themeButtonObj.GetComponent<ThemeButton>().ChangeButtonStartThemeNum(i * stageManager.maxStageCount + 1);
            }
            
            // Theme 버튼 OnClick 이벤트 연결
            int themeButtonCount = themeButtonScrollViewContentObject.transform.childCount;
            for (int i = 0; i < themeButtonCount; ++i)
            {
                Button themeButton = themeButtonScrollViewContentObject.transform.GetChild(i).gameObject.GetComponent<Button>();
                int themeButtonNum = i;
                themeButton.onClick.AddListener(() => SelectThemeStepButton(themeButtonNum));
            }

        }
    }

    private void OnAwakestageButtonCreate()
    {
        if (stageButtonScrollViewContentObject.transform.childCount < 10 && stageButtons == null)
        {
            int maxStageCount = stageManager.maxStageCount;

            stageButtons = new StageButton[maxStageCount];

            for (int i = 0; i < maxStageCount; ++i)
            {
                GameObject stageButtonObj = Instantiate(stageButtonPrefab, stageButtonScrollViewContentObject.transform);
                StageButton stageButton = stageButtonObj.GetComponent<StageButton>();

                int stageNumber = i + 1;
                stageButton.Initialize(stageNumber);

                stageButton.GetButton().onClick.AddListener(() => SelectStageNumberButton(stageNumber));

                stageButtons[i] = stageButton;
            }
        }
    }
}
