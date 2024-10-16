using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using Unity.VisualScripting;
//using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.EventSystems;

public class AdMobObject : MonoBehaviour, IPointerClickHandler
{
    private EventTrigger eventTrigger;
    BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        
    }

    void Update()
    {
    }

    private void OnMouseUp()
    {
        
    }

    public void Click()
    {

        Debug.Log(name + "Game Object Click in Progress");
        //GoogleMobileAdsManager.GetInstance().ShowRewardedAd(RewardAction);  
    }

    void RewardAction(Reward reward)
    {
        Debug.Log(" 다이아 지급 ");
        DummyServerData.GiveUserDiamondAndSendData(Player.GetUserID(), (int)reward.Amount);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("클릭");
    }
}
