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

    void Awake()
    {
        // 서버로부터 user id 받기
        userID = 0;
        
        if (playerStatus == null)
            playerStatus = new Status(userID);
        if (playerCurrency == null)
            playerCurrency = DummyServerData.GetUserCurrencyData(userID);
    }

    public static int GetUserID()
    {
        return userID;
    }

    public static void GetGoldDataFromServer()
    {
        Gold = DummyServerData.GetUserGoldData(userID);
        OnGoldChange(Gold);
    }
}
