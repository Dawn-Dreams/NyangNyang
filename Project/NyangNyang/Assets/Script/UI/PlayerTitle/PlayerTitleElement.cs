using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;


public class PlayerTitleElement : MonoBehaviour
{
    private AddressableHandle<Sprite> _notOwningSprite;
    private AddressableHandle<Sprite> _owningSprite;
    private AddressableHandle<Sprite> _selectedSprite;

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
        titleNameText.color = TitleDataManager.titleGradeColors[(TitleGrade)titleInfo.grade];

        

        //TODO: 보유 효과 작성하기
    }

    public void SetIsOwning(bool newOwning)
    {
        selectTitleButton.interactable = false;
        if (newOwning)
        {
            if (titleInfo.id == Player.PlayerCurrentTitleID)
            {
                selectTitleButtonImage.sprite = _selectedSprite.obj;
                selectTitleText.text = "착용중";
            }
            else
            {
                selectTitleButtonImage.sprite = _owningSprite.obj;
                selectTitleText.text = "보유중";
                selectTitleButton.interactable = true;
            }

        }
        else
        {
            selectTitleButtonImage.sprite = _notOwningSprite.obj;
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
            _notOwningSprite = new AddressableHandle<Sprite>();
            _notOwningSprite.Load("TitleButton/NotOwning");
        }

        if (_owningSprite == null)
        {
            _owningSprite = new AddressableHandle<Sprite>();
            _owningSprite.Load("TitleButton/Owning");
        }

        if (_selectedSprite == null)
        {
            _selectedSprite = new AddressableHandle<Sprite>();
            _selectedSprite.Load("TitleButton/CurrentSelect");
        }
    }
}
