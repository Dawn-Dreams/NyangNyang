using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using Unity.VisualScripting;
//using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.EventSystems;

public class AdMobObject : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
    }

    private void OnMouseUp()
    {
        
    }

    public void OnMouseDown()
    {
        Debug.Log("????");
        GoogleMobileAdsManager.GetInstance().ShowRewardedAd(RewardAction);
    }

    void RewardAction(Reward reward)
    {
        Debug.Log(" 다이아 지급 ");
        DummyServerData.GiveUserDiamondAndSendData(Player.GetUserID(), (int)reward.Amount);
    }
}
