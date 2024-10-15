using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnackBuff : MonoBehaviour
{
    public RectTransform snackBuffUIPanelRectTransform;

    public SnackPanel[] snackPanels;
    private SnackPanel _currentPickSnackPanel;

    

    void Start()
    {
        // 서버로부터 정보 받기
        {

        }

        foreach (var snackPanel in snackPanels)
        {
            snackPanel.showAdButton.onClick.AddListener(() => OnClickShowAdButton(snackPanel));
        }

        // UI 위치 갱신 및 SetActive(false)
        {
            snackBuffUIPanelRectTransform.offsetMin = snackBuffUIPanelRectTransform.offsetMax = new Vector2(0, 0);
            snackBuffUIPanelRectTransform.gameObject.SetActive(false);
        }
    }



    void OnClickShowAdButton(SnackPanel snackPanel)
    {
        _currentPickSnackPanel = snackPanel;
        GoogleMobileAdsManager.GetInstance().ShowRewardedAd(SnackBuffAdReward);
    }


    private void SnackBuffAdReward(Reward reward)
    {
        Debug.Log($"플레이어가 {_currentPickSnackPanel.snackType} 광고 영상을 봤습니다.");
    }
}
