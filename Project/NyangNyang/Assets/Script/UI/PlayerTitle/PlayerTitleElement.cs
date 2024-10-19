using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AddressableHandleSpritePair
{
    public AsyncOperationHandle<Sprite> handle;
    public Sprite sprite;

    public void Load(string key)
    {
        handle = Addressables.LoadAssetAsync<Sprite>(key);
        handle.WaitForCompletion();
        sprite = handle.Result;
    }

    public void Release()
    {
        handle.Release();
    }
}

public class PlayerTitleElement : MonoBehaviour
{
    private static Dictionary<TitleGrade, Color> _titleGradeColors = new Dictionary<TitleGrade, Color>
    {
        { TitleGrade.Normal, Color.white },
        { TitleGrade.Uncommon, Color.green },
        { TitleGrade.Rare, Color.blue },
        { TitleGrade.Epic, new Color(185.0f / 255, 80.0f / 255, 250.0f / 255) },
        { TitleGrade.Legendary, new Color(1.0f, 100.0f / 255, 0.0f) }
    };

    private AddressableHandleSpritePair _notOwningSprite;
    private AddressableHandleSpritePair _owningSprite;
    private AddressableHandleSpritePair _selectedSprite;

    public TitleInfo titleInfo;

    [SerializeField] private TextMeshProUGUI titleNameText;
    [SerializeField] private TextMeshProUGUI titleEffectText;

    [SerializeField] private Image selectTitleButtonImage;
    public Button selectTitleButton;
    [SerializeField] private TextMeshProUGUI selectTitleText;

    public void SetTitle(TitleInfo newTitleInfo)
    {
        AssetLoad();

        titleInfo = newTitleInfo;
        titleNameText.text = titleInfo.name;
        titleNameText.color = _titleGradeColors[(TitleGrade)titleInfo.grade];

        

        //TODO: 보유 효과 작성하기
    }

    public void SetIsOwning(bool newOwning)
    {
        if (newOwning)
        {
            if (titleInfo.id == Player.PlayerCurrentTitleID)
            {
                selectTitleButtonImage.sprite = _selectedSprite.sprite;
                selectTitleText.text = "착용중";
            }
            else
            {
                selectTitleButtonImage.sprite = _owningSprite.sprite;
                selectTitleText.text = "보유중";
            }
        }
        else
        {
            selectTitleButtonImage.sprite = _notOwningSprite.sprite;
            selectTitleText.text = "미보유";
        }
    }

    public void OnEnable()
    {
        AssetLoad();
    }

    public void OnDisable()
    {
        _notOwningSprite?.Release();
        _owningSprite?.Release();
        _selectedSprite?.Release();

        _notOwningSprite = _owningSprite = _selectedSprite = null;
    }


    void AssetLoad()
    {
        // Load Sprite
        if (_notOwningSprite == null)
        {
            _notOwningSprite = new AddressableHandleSpritePair();
            _notOwningSprite.Load("TitleButton/NotOwning");
        }

        if (_owningSprite == null)
        {
            _owningSprite = new AddressableHandleSpritePair();
            _owningSprite.Load("TitleButton/Owning");
        }

        if (_selectedSprite == null)
        {
            _selectedSprite = new AddressableHandleSpritePair();
            _selectedSprite.Load("TitleButton/CurrentSelect");
        }
    }
}
