using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public enum QuestType
{
    // Normal Quest(Repeat)
    Repeat_GoldSpending, Repeat_KillMonster,

    // Story Quest
    LevelUpStatus, StageClear,
}

public enum RewardType
{
    Gold, Diamond
}

public class DummyQuestServer : DummyServerData
{
    // === 반복 퀘스트 데이터 ===
    private static Dictionary<int, BigInteger> _repeatQuest_userGoldSpendingData = new Dictionary<int, BigInteger>
    {
        {0, 150_000},
        {1, 0}
    };
    private const int RequireGoldSpending = 500_000;

    private static long[] userMonsterKillData = new long[]
    {
        98, 0,
    };
    private const int RequireMonsterKill = 50;
    // ========================
    // === 일일 퀘스트 데이터 ====
    // 실제 서버에서는 {일일 초기화 시간} 마다 초기화 해주어야함.
    private static Dictionary<int, BigInteger> dailyQuest_userGoldSpendingData = new Dictionary<int, BigInteger>
    {
        {0, 300},
        {1, 0}
    };
    // =========================

    // DummyServer라서 시작 시 초기화를 해당 함수에서 진행 (GameManager내에서 호출)
    public static void ExecuteDummyQuestServer()
    {
        OnUserGoldSpending += UserGoldSpending;
    }

    public static void SendQuestDataToPlayer(int userID, QuestType questType)
    {
        // 범위 체크 생략 

        // 더미 서버이므로 현재는 강제로 플레이어에게 주입
        switch (questType)
        {
            case QuestType.Repeat_GoldSpending:
                Player.RecvGoldSpendingDataFromServer(_repeatQuest_userGoldSpendingData[userID]);
                break;
            case QuestType.Repeat_KillMonster:
                Player.RecvMonsterKillDataFromServer(userMonsterKillData[userID]);
                break;
            case QuestType.LevelUpStatus:
            // Flow
            case QuestType.StageClear:
                DummyStoryQuestServer.SendNewQuestDataToUser(userID);
                break;
            default:
                break;
        }
        
    }
    public static void UserRequestReward(int userID, QuestType questType, QuestDataBase questInfo)
    {
        Action<int, BigInteger> giveUserCurrency = null;
        BigInteger rewardCount;
        if (questInfo)
        {
            rewardCount = questInfo.rewardCount;
            switch (questInfo.rewardType)
            {
                case RewardType.Gold:
                    
                    break;
                case RewardType.Diamond:
                     giveUserCurrency += DummyServerData.GiveUserDiamondAndSendData;
                    break;
            }

            
        }

        switch (questType)
        {
            // TODO: 함수화
            case QuestType.Repeat_GoldSpending:
                {
                    if (_repeatQuest_userGoldSpendingData[userID] < RequireGoldSpending)
                    {
                        // 유저에게 잘못된 정보를 받았다는 정보 패킷 송신
                        return;
                    }

                    BigInteger clearCount = BigInteger.Divide(_repeatQuest_userGoldSpendingData[userID], RequireGoldSpending);
                    _repeatQuest_userGoldSpendingData[userID] -= RequireGoldSpending * clearCount;

                    // 해당하는 재화 추가 후 정보 전송
                    DummyServerData.GiveUserDiamondAndSendData(userID, clearCount);
                }
                break;

            case QuestType.Repeat_KillMonster:
                {
                    // 갯수 체크 생략
                    int clearCount = (int)(userMonsterKillData[userID] / RequireMonsterKill);
                    userMonsterKillData[userID] -= RequireMonsterKill * clearCount;

                    DummyServerData.GiveUserDiamondAndSendData(userID,clearCount);
                }

                break;
            case QuestType.LevelUpStatus:
                //Flow
            case QuestType.StageClear:
            {
                // 체크 생략
                if (questInfo)
                {
                    if (giveUserCurrency != null)
                    {
                        giveUserCurrency(userID, rewardCount);
                    }
                }
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(questType), questType, null);
        }
        SendQuestDataToPlayer(userID, questType);

    }


    public static void UserGoldSpending(int userID, BigInteger spendingAmount)
    {
        // 범위 체크 생략

        // 반복 퀘스트 데이터 계산
        if (_repeatQuest_userGoldSpendingData.ContainsKey(userID))
        {
            _repeatQuest_userGoldSpendingData[userID] += spendingAmount;
            SendQuestDataToPlayer(userID, QuestType.Repeat_GoldSpending);
        }
        
        
        //
        
    }

    public static void UserMonsterKill(int userID, int monsterKillCount)
    {
        userMonsterKillData[userID] += monsterKillCount;

        SendQuestDataToPlayer(userID,QuestType.Repeat_KillMonster);
    }

    
}
