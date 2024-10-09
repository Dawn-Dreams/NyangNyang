using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public enum QuestType
{
    GoldSpending,
}
public class DummyQuestServer : DummyServerData
{
    private static BigInteger[] userGoldSpendingData = new BigInteger[]
    {
        150_000,
        0,
    };
    private const int RequireGoldSpending = 500_000;

    public static void ExecuteDummyQuestServer()
    {
        OnUserGoldSpending += UserGoldSpending;
    }

    public static void SendQuestDataToPlayer(int userID, QuestType questType)
    {
        // 범위 체크 생략 // value return
        //return userGoldSpendingData[userID];
        // 더미 서버이므로 현재는 강제로 플레이어에게 주입
        switch (questType)
        {
            case QuestType.GoldSpending:
                Player.RecvGoldSpendingDataFromServer(userGoldSpendingData[userID]);
                break;
            default:
                break;
        }
        
    }
    public static void UserGoldSpending(int userID, BigInteger spendingAmount)
    {
        userGoldSpendingData[userID] += spendingAmount;

        SendQuestDataToPlayer(userID,QuestType.GoldSpending);
    }

    public static void UserRequestReward(int userID, QuestType questType)
    {
        switch (questType)
        {
            // TODO: 함수화
            case QuestType.GoldSpending:
                if (userGoldSpendingData[userID] < RequireGoldSpending)
                {
                    // 유저에게 잘못된 정보를 받았다는 정보 패킷 송신
                    return;
                }

                BigInteger clearCount = BigInteger.Divide(userGoldSpendingData[userID], RequireGoldSpending);
                userGoldSpendingData[userID] -= RequireGoldSpending * clearCount;
                
                // 해당하는 재화 추가 후 정보 전송
                DummyServerData.GiveUserDiamondAndSendData(userID, clearCount);
                SendQuestDataToPlayer(userID,QuestType.GoldSpending);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(questType), questType, null);
        }
       
    }

    
}
