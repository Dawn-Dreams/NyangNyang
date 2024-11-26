using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class ChangeStageUI : MonoBehaviour
{
    // sprite resource
    private Dictionary<int, Sprite> _stageSprites = null;
    private AsyncOperationHandle<IList<Sprite>> _spriteHandle;
    public AssetLabelReference spriteLabel;
    private string[] _themeNames;

    // Hide UI
    private HideUi _hideUI;

    // Buttons
    [SerializeField] private StageManager stageManager;

    // Theme Button
    [SerializeField] private GameObject themeButtonPrefab;
    [SerializeField] private GameObject themeButtonScrollViewContentObject;
    // Stage Button
    [SerializeField] private GameObject stageButtonPrefab;
    [SerializeField] private GameObject stageButtonScrollViewContentObject;

    [SerializeField] private GameObject[] themeNumberObject;
    private Image[] themeNumberImages;
    private TextMeshProUGUI[] themeNumberTexts;
    [SerializeField] private Sprite themeNumberNormalSprite; 
    [SerializeField] private Sprite themeNumberSelectSprite;

    private StageButton[] stageButtons;

    [SerializeField] private Image themeImage;
    [SerializeField] private TextMeshProUGUI themeNameText;
    [SerializeField] private TextMeshProUGUI stageLevelText;

    private int _curStartThemeNum = 0;
    private int _curSelectThemeNum = 0;
    private int _curSelectStageNum = 0;

    [SerializeField] private Button changeStageButton;

    private void Start()
    {
        _hideUI = GetComponent<HideUi>();
        //_hideUI.OnShowAction = OnEnable;
        changeStageButton.onClick.AddListener(ChangeStage);

        OnStartThemeStepButtonCreate();
        OnStartStageButtonCreate();
        OnStartThemeNumberButtonAddListener();

        SetInitialData();
    }

    public void MapOpen()
    {
        SetInitialData();
        AssetLoad();
    }

    void OnDisable()
    {
        //Addressables.Release(_spriteHandle);    
    }

    void SetInitialData()
    {
        _curStartThemeNum =
            (stageManager.GetCurrentTheme() - 1) / stageManager.maxStageCount * stageManager.maxStageCount + 1;
        SelectThemeNumberButton((stageManager.GetCurrentTheme() - 1) % stageManager.maxStageCount);
        SelectStageNumberButton(stageManager.GetCurrentStage());

        RenewalStageButtonTypeInCurrentTheme();
    }


    private void OnStartThemeNumberButtonAddListener()
    {
        themeNumberImages = new Image[themeNumberObject.Length];
        themeNumberTexts = new TextMeshProUGUI[themeNumberObject.Length];

        for (int i = 0; i < themeNumberObject.Length; ++i)
        {
            themeNumberImages[i] = themeNumberObject[i].GetComponent<Image>();
            themeNumberTexts[i] = themeNumberObject[i].GetComponentInChildren<TextMeshProUGUI>();
            int buttonID = i;
            themeNumberObject[i].GetComponent<Button>().onClick.AddListener(()=>SelectThemeNumberButton(buttonID));
        }
    }

    void SelectThemeStepButton(int themeStepButtonNumber)
    {
        _curStartThemeNum = themeStepButtonNumber * stageButtons.Length + 1;

        // 테마 선택 버튼 해당 스텝에 맞게 조정
        for (int i = 0; i < themeNumberTexts.Length; ++i)
        {
            themeNumberTexts[i].text = (_curStartThemeNum + i).ToString();
        }

        // 테마 이미지 및 이름 변경
        themeNameText.text = _themeNames[themeStepButtonNumber % themeNumberObject.Length];
        themeImage.sprite = _stageSprites[themeStepButtonNumber % themeNumberObject.Length + 1];

        

        SelectThemeNumberButton(0);
    }

    void SelectThemeNumberButton(int buttonNumberID)
    {
        if (themeNumberImages == null || themeNumberImages.Length == 0)
        {
            return;
        }

        if (0 <= _curSelectThemeNum && _curSelectThemeNum < themeNumberImages.Length)
        {
            themeNumberImages[_curSelectThemeNum].sprite = themeNumberNormalSprite;
        }

        for (int i = 0; i < themeNumberTexts.Length; ++i)
        {
            themeNumberTexts[i].text = (_curStartThemeNum + i).ToString();
        }

        _curSelectThemeNum = buttonNumberID;
        themeNumberImages[_curSelectThemeNum].sprite = themeNumberSelectSprite;

        RenewalStageButtonTypeInCurrentTheme();

        SelectStageNumberButton(1);
    }

    void SelectStageNumberButton(int buttonNumberID)
    {
        if (stageButtons == null || stageButtons.Length == 0)
        {
            return;
        }

        if (1 <= _curSelectStageNum && _curSelectStageNum <= stageButtons.Length)
        {
            stageButtons[_curSelectStageNum - 1].UnSelect();
        }

        _curSelectStageNum = buttonNumberID;
        //stageButtons[_curSelectStageNum - 1].SetButtonType(StageButtonType.Select);
        stageButtons[_curSelectStageNum - 1].Select();


        // 스테이지 레벨 텍스트 출력
        string currentStageText = _curStartThemeNum + _curSelectThemeNum + " - " + _curSelectStageNum;
        stageLevelText.text = currentStageText;

        SetStageButtonSelect();
    }

    void SetStageButtonSelect()
    {
        if (_curSelectStageNum == 0)
        {
            _curSelectStageNum = 1;
        }

        int currentSelectTheme = _curStartThemeNum + _curSelectThemeNum;
        int highestTheme = 0;
        int highestStage = 0;
        Player.GetPlayerHighestClearStageData(out highestTheme, out highestStage);
        
        
        if (stageButtons[_curSelectStageNum - 1].GetButtonType() == StageButtonType.Close ||
            stageButtons[_curSelectStageNum - 1].GetButtonType() == StageButtonType.Current)
        {
            changeStageButton.interactable = false;
        }
        else
        {
            changeStageButton.interactable = true;
        }
    }

    void ChangeStage()
    {
        stageManager.GoToSpecificStage(_curStartThemeNum + _curSelectThemeNum, _curSelectStageNum);
        CloseChangeStageUI();
        
    }

    void CloseChangeStageUI()
    {
        //gameObject.SetActive(false);
        _hideUI.HideUIInVisible();
    }

    void OnStartThemeStepButtonCreate()
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

    private void OnStartStageButtonCreate()
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

    void AssetLoad()
    {
        // Load Sprite
        _stageSprites = new Dictionary<int, Sprite>();

        _spriteHandle = Addressables.LoadAssetsAsync<Sprite>(spriteLabel, (result) =>
        {
            int bgNumber = int.Parse(result.name.Substring(result.name.Length - 1, 1));
            _stageSprites.Add(bgNumber, result);
        });
        _spriteHandle.WaitForCompletion();


        // Load Text
        var textAssetHandle = Addressables.LoadAssetAsync<TextAsset>("ThemeNameText");
        textAssetHandle.WaitForCompletion();
        TextAsset themeNameTextAsset = textAssetHandle.Result;
        _themeNames = themeNameTextAsset.text.Split('\n');
    }

    public void RenewalStageButtonTypeInCurrentTheme()
    {
        if (stageButtons == null || stageButtons.Length == 0)
        {
            return;
        }
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
            if (curTheme < highestTheme || (curTheme == highestTheme && curStage <= highestStage + 1))
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
            }
        }

        SetStageButtonSelect();
    }

    public void SetChangeStageButtonInteractable(bool newActive)
    {
        changeStageButton.interactable = newActive;
    }
}
