using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public enum QuestType
{
    GoldSpending, KillMonster,

    LevelUpStatus, StageClear,
}

public enum QuestCategory
{
    Repeat,
    Daily,
    Weekly,
    Achievement,
    Story
}

public enum RewardType
{
    Gold, Diamond
}

public class DummyQuestServer : DummyServerData
{

    // === 반복 퀘스트 데이터 ===
    private const int RequireGoldSpending = 500_000;
    private const int RequireMonsterKill = 3;
    private static Dictionary<QuestType, QuestDataBase> _repeatQuestDataBases = new Dictionary<QuestType, QuestDataBase>
    {
        { QuestType.GoldSpending , 
            ScriptableObject.CreateInstance<GoldSpendingQuestData>().QuestInitialize(QuestCategory.Repeat,RequireGoldSpending)},
        { QuestType.KillMonster,
            ScriptableObject.CreateInstance<KillMonsterQuestData>().QuestInitialize(QuestCategory.Repeat,RequireMonsterKill)},

    };
    private static Dictionary<int, BigInteger> _repeatQuest_userGoldSpendingData = new Dictionary<int, BigInteger>
    {
        {0, 150_000},
        {1, 0}
    };
    

    private static long[] userMonsterKillData = new long[]
    {
        98, 0,
    };
    
    // ========================
    // === 일일 퀘스트 데이터 ====
    // 실제 서버에서는 {일일 초기화 시간} 마다 초기화 해주어야함.
    protected static int Daily_RequireGoldSpending = 1_000_000;
    private static Dictionary<QuestType, QuestDataBase> _dailyQuestDataBases = new Dictionary<QuestType, QuestDataBase>
    {
        { QuestType.GoldSpending, ScriptableObject.CreateInstance<GoldSpendingQuestData>().QuestInitialize(QuestCategory.Daily, Daily_RequireGoldSpending, 200) },

    };

    private static Dictionary<QuestType, List<int>> _dailyQuestGetRewardUsersID = new Dictionary<QuestType, List<int>>
    {
        { QuestType.GoldSpending, new List<int>() }
    };
    private static Dictionary<int, BigInteger> _dailyQuest_userGoldSpendingData = new Dictionary<int, BigInteger>
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

    public static QuestDataBase SendQuestInfoToUser(int userID, QuestCategory questCategory, QuestType questType)
    {
        // TODO: 해당 플레이어에게 보낼 수 있도록
        return GetQuestInfo(questCategory, questType);
    }

    public static bool SendRewardInfoToUser(int userID, QuestCategory questCategory, QuestType questType)
    {
        bool isUserGetReward = false;
        switch (questCategory)
        {
            case QuestCategory.Daily:
                isUserGetReward = _dailyQuestGetRewardUsersID[questType].Contains(userID);
                
                break;
        }

        return isUserGetReward;

    }

    public static QuestDataBase GetQuestInfo(QuestCategory questCategory, QuestType questType)
    {
        switch (questCategory)
        {
            case QuestCategory.Repeat:
                return _repeatQuestDataBases[questType];
            case QuestCategory.Daily:
                return _dailyQuestDataBases[questType];
            default:
                return null;
        }
    }

    public static void SendQuestDataToPlayer(int userID, QuestCategory questCategory,QuestType questType)
    {
        // 범위 체크 생략 

        // 더미 서버이므로 현재는 강제로 플레이어에게 주입
        switch (questType)
        {
            // 반복 퀘스트
            case QuestType.GoldSpending:
                Player.RecvGoldSpendingDataFromServer(GetUserGoldSpendingData(userID,questCategory), questCategory);
                break;
            case QuestType.KillMonster:
                Player.RecvMonsterKillDataFromServer(userMonsterKillData[userID]);
                break;

            // 스토리 퀘스트
            case QuestType.LevelUpStatus:
            // Flow
            case QuestType.StageClear:
                DummyStoryQuestServer.SendNewQuestDataToUser(userID);
                break;

            default:
                break;
        }
        
    }
    public static void UserRequestReward(int userID,QuestCategory questCategory,  QuestType questType, QuestDataBase questInfo)
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
            case QuestType.GoldSpending:
                {
                    if (GetUserGoldSpendingData(userID, questCategory) < GetRequireSpendingGoldValue(questCategory))
                    {
                        // 유저에게 잘못된 정보를 받았다는 정보 패킷 송신
                        return;
                    }

                    BigInteger rewardValue = questInfo.rewardCount;
                    BigInteger clearCount = 1;
                    if (questInfo.IsRewardRepeatable())
                    {
                        clearCount = BigInteger.Divide(GetUserGoldSpendingData(userID, questCategory), RequireGoldSpending);
                        rewardValue *= clearCount;
                    }
                    BigInteger newValue = GetUserGoldSpendingData(userID, questCategory) - GetRequireSpendingGoldValue(questCategory) * clearCount;
                    SetUserGoldSpendingQuestDataAfterReward(userID, questCategory, newValue);
                    // 해당하는 재화 추가 후 정보 전송
                    DummyServerData.GiveUserDiamondAndSendData(userID, rewardValue);
                }
                break;

            case QuestType.KillMonster:
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
        SendQuestDataToPlayer(userID,questCategory, questType);

    }


    public static void UserGoldSpending(int userID, BigInteger spendingAmount)
    {
        // 범위 체크 생략

        // 반복 퀘스트 데이터 계산
        if (_repeatQuest_userGoldSpendingData.ContainsKey(userID))
        {
            _repeatQuest_userGoldSpendingData[userID] += spendingAmount;
            SendQuestDataToPlayer(userID, QuestCategory.Repeat, QuestType.GoldSpending);
        }
        
        
        // TODO: 일일 퀘스트 데이터 계산
        if (_dailyQuest_userGoldSpendingData.ContainsKey(userID))
        {
            _dailyQuest_userGoldSpendingData[userID] += spendingAmount;
            SendQuestDataToPlayer(userID,QuestCategory.Daily, QuestType.GoldSpending);
        }
    }

    public static BigInteger GetUserGoldSpendingData(int userID, QuestCategory questCategory)
    {
        switch (questCategory)
        {
            case QuestCategory.Repeat:
                return _repeatQuest_userGoldSpendingData[userID];
            case QuestCategory.Daily:
                return _dailyQuest_userGoldSpendingData[userID];
            default:
                Debug.Log($"Error - {nameof(DummyQuestServer)} - {nameof(DummyQuestServer.GetUserGoldSpendingData)}" );
                return 0;
        }
    }

    public static void SetUserGoldSpendingQuestDataAfterReward(int userID, QuestCategory questCategory,  BigInteger newValue)
    {
        if (!GetQuestInfo(questCategory, QuestType.GoldSpending).IsRewardRepeatable())
        {
            // TODO: 보상 받은 유저들 정보 추가
            switch (questCategory)
            {
                case QuestCategory.Daily:
                    _dailyQuestGetRewardUsersID[QuestType.GoldSpending].Add(userID);
                    break;
                default:
                    Debug.Log($"Error - {nameof(DummyQuestServer)} - {nameof(SetUserGoldSpendingQuestDataAfterReward)}");
                    break;
            }
        }
        else
        {
            // 퀘스트 데이터 값 갱신
            switch (questCategory)
            {
                case QuestCategory.Repeat:
                    _repeatQuest_userGoldSpendingData[userID] = newValue;
                    break;
                case QuestCategory.Daily:
                    //_dailyQuest_userGoldSpendingData[userID] = newValue;
                    break;
                default:
                    Debug.Log($"Error - {nameof(DummyQuestServer)} - {nameof(SetUserGoldSpendingQuestDataAfterReward)}");
                    break;
            }
        }

        
    }
    public static int GetRequireSpendingGoldValue(QuestCategory questCategory)
    {
        switch (questCategory)
        {
            case QuestCategory.Repeat:
                return RequireGoldSpending;
            case QuestCategory.Daily:
                return Daily_RequireGoldSpending;
            default:
                Debug.Log($"Error - {nameof(DummyQuestServer)} - {nameof(DummyQuestServer.GetRequireSpendingGoldValue)}");
                return 0;
        }
    }

    public static void UserMonsterKill(int userID, int monsterKillCount)
    {
        userMonsterKillData[userID] += monsterKillCount;

        SendQuestDataToPlayer(userID,QuestCategory.Repeat, QuestType.KillMonster);
    }

    
    
}
