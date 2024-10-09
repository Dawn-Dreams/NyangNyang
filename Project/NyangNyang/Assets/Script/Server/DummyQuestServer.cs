using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class DummyQuestServer : DummyServerData
{
    public static void ExecuteDummyQuestServer()
    {
        OnUserGoldSpending += UserGoldSpending;
        // 처음에 유저에게 정보 전송해주기
    }

    private static BigInteger[] userGoldSpendingData = new BigInteger[]
    {
        150_000,
        0,
    };

    public static void SendGoldSpendingDataToPlayer(int userID)
    {
        // 범위 체크 생략 // value return
        //return userGoldSpendingData[userID];
        // 더미 서버이므로 현재는 강제로 플레이어에게 주입
        Player.RecvGoldSpendingDataFromServer(userGoldSpendingData[userID]);
    }
    public static void UserGoldSpending(int userID, BigInteger spendingAmount)
    {
        userGoldSpendingData[userID] += spendingAmount;
        
        SendGoldSpendingDataToPlayer(userID);
    }

    

    
}
