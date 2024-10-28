using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PassCategoryType
{
    Free, Gold, Count
}

public enum PassButtonState
{
    Close,Open,Reward,Count
}

public class PassRewardItemButton : MonoBehaviour
{
    private static Dictionary<PassCategoryType, Color> countTextImageColors = new Dictionary<PassCategoryType, Color>
    {
        { PassCategoryType.Free, new Color(208f / 255, 235f / 255, 255f / 255) },
        { PassCategoryType.Gold, new Color(244f / 255, 218f / 255, 250f / 255) }
    };

    private AddressableHandle<GameObject> _openBackgroundPrefab;
    private AddressableHandle<GameObject> _closeBackgroundPrefab;
    private GameObject _currentBackgroundGameObject = null;
    private AddressableHandle<Sprite> _rewardSprite;
    private AddressableHandle<Sprite> _rewardStateSprite;

    public RewardType rewardType;
    public PassCategoryType passCategoryType;
    public PassButtonState passButtonState;

    public Button requestRewardButton;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image countImage;
    [SerializeField] private Image rewardStateIconImage;
    [SerializeField] private TextMeshProUGUI countText;

    public void SetTypes(RewardType setRewardType, PassCategoryType setPassCategoryType, PassButtonState buttonState)
    {
        rewardType = setRewardType;
        passCategoryType = setPassCategoryType;
        passButtonState = buttonState;

        AssetLoad();
    }

    void AssetLoad()
    {
        // 보상 스프라이트 로드 및 연결
        _rewardSprite = new AddressableHandle<Sprite>();
        _rewardSprite.Load("Icon/"+rewardType);
        iconImage.sprite = _rewardSprite.obj;

        // 패스 카테고리에 따른 색상 변경 및 배경 프리펩 로드
        countImage.color = countTextImageColors[passCategoryType];
        _openBackgroundPrefab = new AddressableHandle<GameObject>();
        _openBackgroundPrefab.Load("Pass/Background/Open");
        if (passButtonState == PassButtonState.Close)
        {
            _closeBackgroundPrefab = new AddressableHandle<GameObject>();
            _closeBackgroundPrefab.Load("Pass/Background/"+passButtonState + "/"+passCategoryType);
            _currentBackgroundGameObject = Instantiate(_closeBackgroundPrefab.obj, gameObject.transform);
        }
        else
        {
            _currentBackgroundGameObject = Instantiate(_openBackgroundPrefab.obj, gameObject.transform);
        }
        _currentBackgroundGameObject.transform.SetSiblingIndex(0);

        // 리워드 상태를 나타내는 아이콘
        LoadAndApplyRewardStateSprite();
    }

    void LoadAndApplyRewardStateSprite()
    {
        // Release 및 할당
        if (_rewardStateSprite != null)
        {
            _rewardStateSprite.Release();
        }
        else
        {
            _rewardStateSprite = new AddressableHandle<Sprite>();
        }

        // 아이콘 로드
        switch (passButtonState)
        {
            case PassButtonState.Close:
                _rewardStateSprite.Load("Icon/Lock");
                break;
            case PassButtonState.Open:
                // null
                break;
            case PassButtonState.Reward:
                _rewardStateSprite.Load("Icon/Checkmark");
                break;
            default:
                break;
        }

        
        if (_rewardStateSprite.obj != null)
        {
            rewardStateIconImage.gameObject.SetActive(true);
            rewardStateIconImage.sprite = _rewardStateSprite.obj;
        }
        else
        {
            rewardStateIconImage.gameObject.SetActive(false);
        }
    }

    void SetCountText(string newString)
    {
        if (countText)
        {
            countText.text = newString;
        }
    }

    void OnDestroy()
    {
        if (_openBackgroundPrefab != null)
        {
            _openBackgroundPrefab.Release();
            _openBackgroundPrefab = null;
        }
        if (_closeBackgroundPrefab != null)
        {
            _closeBackgroundPrefab.Release();
            _closeBackgroundPrefab = null;
        }
        if (_rewardSprite != null)
        {
            _rewardSprite.Release();
            _rewardSprite = null;
        }
    }
}
