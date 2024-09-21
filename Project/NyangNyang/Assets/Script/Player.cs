using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static int userID = 0;
    public static Status playerStatus;
    private static CurrencyData playerCurrency;

    public delegate void OnGoldChangeDelegate(BigInteger newGoldVal);
    public static event OnGoldChangeDelegate OnGoldChange;

    private static UserLevelData playerLevelData;

    public delegate void OnExpChangeDelegate(UserLevelData newLevelData);
    public static event OnExpChangeDelegate OnExpChange;

    
    public static BigInteger Gold
    {
        get { return playerCurrency.gold; }
        set
        {
            if (playerCurrency.gold == value) return;
            playerCurrency.gold = value;

            if(OnGoldChange != null)
                OnGoldChange(playerCurrency.gold);

        }
    }
    public static UserLevelData UserLevel
    {
        get
        {
            return playerLevelData;
        }
        set
        {
            if (playerLevelData.currentExp == value.currentExp 
                && playerLevelData.currentLevel == value.currentLevel) return;
            playerLevelData = value;

            if (OnExpChange != null)
                OnExpChange(playerLevelData);
        }
    }

    void Awake()
    {
        // 서버로부터 user id 받기
        userID = 0;
        
        if (playerStatus == null)
            playerStatus = new Status(userID);
        if (playerCurrency == null)
            playerCurrency = DummyServerData.GetUserCurrencyData(userID);
        if (playerLevelData == null)
            GetExpDataFromServer();
    }

    void Update()
    {
        //TODO: Test, Delete Later
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Player.AddExp(1000);
        }
    }

    public static int GetUserID()
    {
        return userID;
    }

    public static void GetGoldDataFromServer()
    {
        Gold = DummyServerData.GetUserGoldData(userID);
        OnGoldChange(Gold);
        if(OnGoldChange != null)
            OnGoldChange(Gold);
    }

    public static void GetExpDataFromServer()
    {
        // 해당 방식으로 저장될 경우 래퍼런스 타입을 가짐
        //playerLevelData = DummyServerData.GetUserLevelData(userID);
        playerLevelData = UserLevelData.GetNewDataFromSource(DummyServerData.GetUserLevelData(userID));
        

        if (OnExpChange != null)
            OnExpChange(playerLevelData);
    }

    public static void AddExp(BigInteger addExpValue)
    {
        playerLevelData.AddExp(addExpValue);
    }
}
