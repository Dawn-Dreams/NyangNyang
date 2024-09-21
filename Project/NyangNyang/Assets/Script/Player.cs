using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static int userID = 0;
    public static Status playerStatus;
    private static CurrencyData playerCurrency;
    private static UserLevelData playerLevelData;

    // 골드 변화 델리게이트 이벤트
    public delegate void OnGoldChangeDelegate(BigInteger newGoldVal);
    public static event OnGoldChangeDelegate OnGoldChange;

    public delegate void OnExpChangeDelegate(UserLevelData newExpVal);
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
        get { return playerLevelData; }
        set
        {
            if(playerLevelData == value) return;
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
            playerLevelData = DummyServerData.GetUserLevelData(userID);
    }

    public static int GetUserID()
    {
        return userID;
    }

    public static void GetGoldDataFromServer()
    {
        Gold = DummyServerData.GetUserGoldData(userID);
        if(OnGoldChange != null)
            OnGoldChange(Gold);
    }

    public static void GetExpDataFromServer()
    {
        UserLevel = DummyServerData.GetUserLevelData(userID);

        if(OnExpChange != null)
            OnExpChange(UserLevel);
    }
}
