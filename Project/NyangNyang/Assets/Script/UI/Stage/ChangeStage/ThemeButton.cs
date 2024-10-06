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
    private AsyncOperationHandle<TextAsset> _nameTextAssetHandle;
    public AssetLabelReference spriteLabel;
    private string[] _themeNames;

    public int startThemeNum = 1;
    private int _stageBundle = 5;
    [SerializeField] private Image themeButtonImage;
    [SerializeField] private TextMeshProUGUI themeNameText;
    [SerializeField] private TextMeshProUGUI themeNumberBundleText;

    void OnEnable()
    {
        AssetLoad();
        _stageBundle = GameManager.GetInstance().stageManager.maxStageCount;
        //stageBundle = 5;
        ChangeButtonStartThemeNum(startThemeNum);
    }

    void OnDisable()
    {
        Addressables.Release(_spriteHandle);
        Addressables.Release(_nameTextAssetHandle);
    }


    public void ChangeButtonStartThemeNum(int newStartThemeNum)
    {
        if (startThemeNum % 5 != 1 || startThemeNum <= 0)
        {
            Debug.LogError("startThemeNum % 5 must 1 and PositiveNumber");
            return;
        }
        startThemeNum = newStartThemeNum;

        int themeStep = ((startThemeNum - 1) / _stageBundle);

        Sprite themeBGSprite = _stageSprites[themeStep % _stageBundle + 1];
        themeButtonImage.sprite = themeBGSprite;

        themeNameText.text = _themeNames[themeStep % _stageBundle];

        themeNumberBundleText.text = "(" + startThemeNum + " ~ " + (startThemeNum + _stageBundle - 1) + ")";
    }

    void AssetLoad()
    {
        // Load Sprite
        _stageSprites = new Dictionary<int, Sprite>();

        _spriteHandle = Addressables.LoadAssetsAsync<Sprite>(spriteLabel, (result) =>
        {
            
            int bgNumber = int.Parse(result.name.Substring(result.name.Length - 1, 1));
            _stageSprites.Add(bgNumber,result);
        });
        _spriteHandle.WaitForCompletion();


        // Load Text
        _nameTextAssetHandle = Addressables.LoadAssetAsync<TextAsset>("ThemeNameText");
        _nameTextAssetHandle.WaitForCompletion();
        TextAsset themeNameTextAsset = _nameTextAssetHandle.Result;
        _themeNames = themeNameTextAsset.text.Split('\n');
    }
}
