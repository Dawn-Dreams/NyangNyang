using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UI;


public class ThemeButton : MonoBehaviour
{
    // sprite resource
    private Dictionary<int, Sprite> _stageSprites = null;
    private AsyncOperationHandle<IList<Sprite>> _spriteHandle;


    public int startThemeNum = 1;
    private int stageBundle = 5;
    public AssetLabelReference spriteLabel;
    [SerializeField] private Image themeButtonImage;
    [SerializeField] private TextMeshProUGUI themeNameText;
    [SerializeField] private TextMeshProUGUI themeNumberBundleText;

    void Awake()
    {
        AssetLoad();
        stageBundle = GameManager.GetInstance().stageManager.maxStageCount;
        //stageBundle = 5;
        ChangeButtonStartThemeNum(startThemeNum);
    }

    private void OnDestroy()
    {
        Addressables.Release(_spriteHandle);
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
        
        Sprite themeBGSprite = _stageSprites[themeStep % stageBundle + 1];
        themeButtonImage.sprite = themeBGSprite;
        
        themeNameText.text = "지역 이름";
        themeNumberBundleText.text = "(" + startThemeNum + " ~ " + (startThemeNum + stageBundle - 1) + ")";
    }

    void AssetLoad()
    {
        _stageSprites = new Dictionary<int, Sprite>();

        _spriteHandle = Addressables.LoadAssetsAsync<Sprite>(spriteLabel, (result) =>
        {
            int bgNumber = int.Parse(result.name.Substring(result.name.Length - 1, 1));
            _stageSprites.Add(bgNumber,result);
        });
        _spriteHandle.WaitForCompletion();
        
    }
}
