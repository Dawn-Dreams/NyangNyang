using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public enum QuestType
{
    GoldSpending, KillMonster,
    FirstTime,
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
    // 서버 내 DB 구조상 Dict<Tuple, Data> 로 하려고 했으나, 접근의 편의성을 위해 더미 서버에서는 Dict<enum, Dict<enum, Data>> 로 관리
    // 추후 서버 개발자가 코드를 리팩토링 한다면
    // int를 키로 AAABBBCCC (A - 퀘스트 카테고리 / B - 퀘스트 타입 / C - ...) 등으로 int내 파싱해서 관리하면 좋을 것으로 예상됨
    // ex) 1002003 -> 001/ 002 / 003


    // 퀘스트의 요구 수치 데이터
    private static Dictionary<QuestCategory, Dictionary<QuestType,int>> _questRequireValueData = new Dictionary<QuestCategory, Dictionary<QuestType, int>>
    {
        // 반복 퀘스트
        { QuestCategory.Repeat , new Dictionary<QuestType, int>
        {
            { QuestType.GoldSpending , 500_000},
            { QuestType.KillMonster , 3}
        }},
        
        // 일일 퀘스트
        { QuestCategory.Daily , new Dictionary<QuestType, int>
        {
            { QuestType.GoldSpending , 1_000_000},
        }}
    };

    // 퀘스트 데이터 변수
    private static Dictionary<QuestCategory, Dictionary<QuestType, QuestDataBase>> questDataBases =
        new Dictionary<QuestCategory, Dictionary<QuestType, QuestDataBase>>
        {
            // 반복 퀘스트
            {
                QuestCategory.Repeat, new Dictionary<QuestType, QuestDataBase>
                {
                    {
                        QuestType.GoldSpending,
                        ScriptableObject.CreateInstance<GoldSpendingQuestData>().QuestInitialize(QuestCategory.Repeat,
                            _questRequireValueData[QuestCategory.Repeat][QuestType.GoldSpending])
                    },
                    {
                        QuestType.KillMonster,
                        ScriptableObject.CreateInstance<KillMonsterQuestData>().QuestInitialize(QuestCategory.Repeat,
                            _questRequireValueData[QuestCategory.Repeat][QuestType.KillMonster])
                    },
                }
            },
            // 일일 퀘스트 ====================================
            {
                QuestCategory.Daily, new Dictionary<QuestType, QuestDataBase>
                {
                    {
                        QuestType.GoldSpending, 
                        ScriptableObject.CreateInstance<GoldSpendingQuestData>().QuestInitialize(QuestCategory.Daily,
                            _questRequireValueData[QuestCategory.Daily][QuestType.GoldSpending], 200)
                    },
                }
            }
        };
    // 유저가 퀘스트 보상을 받았는지에 대한 변수
    private static Dictionary<QuestCategory, Dictionary<QuestType, List<int>>> _getRewardUsersID =
        new Dictionary<QuestCategory, Dictionary<QuestType, List<int>>>
        {
            // 일일 퀘스트
            {
                QuestCategory.Daily, new Dictionary<QuestType, List<int>>
                {
                    { QuestType.GoldSpending, new List<int>() }
                }
            },
        };


    // 유저의 퀘스트 진행 데이터 저장
    // TODO: 급히 만드느라 3중 dirtinary에 저장하고 int 로 저장해도 되는 데이터들을 BigInteger로 저장함
    // 추후 개선 예정
    // 해당 변수 내 데이터 중 일부는 초기화 진행(일일 / 주간 퀘스트는 특정 시간 마다 초기화)
    // 24.10.14) 유저 퀘스트 진행 데이터는 타입 - 카테고리로 하여 델리게이트 이벤트 내에서 사용하기 편하게 수정
    private static Dictionary<QuestType, Dictionary<QuestCategory, Dictionary<int, BigInteger>>> userQuestProgressData =
        new Dictionary<QuestType, Dictionary<QuestCategory, Dictionary<int, BigInteger>>>
        {
            // GoldSpending 퀘스트
            {
                QuestType.GoldSpending, new Dictionary<QuestCategory, Dictionary<int, BigInteger>>
                {
                    //  반복 퀘스트
                    {
                        QuestCategory.Repeat, new Dictionary<int, BigInteger>
                        {
                            { 0, 150_000 },
                            { 1, 0 }
                        }
                    },
                    // 일일 퀘스트
                    {
                        QuestCategory.Daily, new Dictionary<int, BigInteger>
                        {
                            { 0, 500 },
                            { 1, 0 }
                        }
                    }
                }
            },

            // KillMonster 퀘스트
            {
                QuestType.KillMonster, new Dictionary<QuestCategory, Dictionary<int, BigInteger>>
                {
                    // 반복 퀘스트
                    {
                        QuestCategory.Repeat, new Dictionary<int, BigInteger>
                        {
                            {0, 98},
                            {1, 0}
                        }
                    },
                }
            }
        };
    // =========================

    // DummyServer라서 시작 시 초기화를 해당 함수에서 진행 (GameManager내에서 호출)
    // <- 해당 방식 사용하지 않으므로 삭제 해도 됨.

    public static QuestDataBase SendQuestInfoToUser(int userID, QuestCategory questCategory, QuestType questType)
    {
        // TODO: 해당 플레이어에게 보낼 수 있도록
        return GetQuestInfo(questCategory, questType);
    }

    // 유저가 일일/주간/업적 퀘스트 등에서 보상을 받은 적 있는지의 정보를 전송해주는 함수
    public static bool SendRewardInfoToUser(int userID, QuestCategory questCategory, QuestType questType)
    {
        bool isUserGetReward = false;
        if (_getRewardUsersID.ContainsKey(questCategory))
        {
            // 카테고리로 보상 반복 유무가 정해지므로 카테고리만 확인
            isUserGetReward = _getRewardUsersID[questCategory][questType].Contains(userID);
        }
        return isUserGetReward;
    }

    // 서버 내 저장된 BaseQuestData 정보에 대한 Get함수
    public static QuestDataBase GetQuestInfo(QuestCategory questCategory, QuestType questType)
    {
        return questDataBases[questCategory][questType];
    }

    // userID 유저가 요구한 QuestCategory,QuestType의 퀘스트에서 사용되는 데이터 전송
    public static void SendQuestDataToPlayer(int userID, QuestCategory questCategory,QuestType questType)
    {
        // 범위 체크 생략 

        // 더미 서버이므로 현재는 강제로 플레이어에게 주입
        switch (questType)
        {
            // 반복 퀘스트
            case QuestType.GoldSpending:
                Player.RecvGoldSpendingDataFromServer(userQuestProgressData[questType][questCategory][userID], questCategory);
                break;
            case QuestType.KillMonster:
                Player.RecvMonsterKillDataFromServer((long)userQuestProgressData[questType][questCategory][userID]);
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

    // 유저가 보상 흭득을 요구하는 함수
    public static void UserRequestReward(int userID,QuestCategory questCategory,  QuestType questType, QuestDataBase  questInfo)
    {
        // 서버의 퀘스트 정보를 쓰려했으나 스토리 퀘스트 서버가 분리되어 적용 불가, 추후 개선 예정
        //QuestDataBase questInfo = GetQuestInfo(questCategory, questType);


        Action<int, BigInteger> giveUserCurrencyAction = null;
        BigInteger rewardCount;
        if (questInfo)
        {
            rewardCount = questInfo.rewardCount;
            switch (questInfo.rewardType)
            {
                case RewardType.Gold:
                    // TODO: DummyServerData 내 제공하는 함수 제작 후 연결 진행
                    break;
                case RewardType.Diamond:
                     giveUserCurrencyAction += DummyServerData.GiveUserDiamondAndSendData;
                    break;
            }

            
        }
        
        {
            // TODO: 유저의 데이터가 퀘스트 클리어 가능한지 체크
            // 안될 시 잘못된 정보라는 패킷 전송
            //유저가 daily/weekly/achievement 퀘스트 등 일회성 퀘스트를 달성했었는지에 대한 체크
            if (!questInfo.IsRewardRepeatable() && _getRewardUsersID.ContainsKey(questInfo.GetQuestCategory()) && _getRewardUsersID[questCategory][questType].Contains(userID))
            {
                Debug.Log($"{userID} 유저가 이미 클리어한 퀘스트를 클리어하겠다고 요청함 확인 필요");
                return;
            }
        }

        
        // 보상 갯수 계산
        int clearCount = 1;
        if (questInfo.IsRewardRepeatable())
        {
            clearCount = (int)BigInteger.Divide(userQuestProgressData[questType][questCategory][userID],
                _questRequireValueData[questCategory][questType]);
            
        }

        // 필요 시 퀘스트의 데이터를 감소시키는 코드 진행
        if (userQuestProgressData.ContainsKey(questType) && userQuestProgressData[questType].ContainsKey(questCategory))
        {
            userQuestProgressData[questType][questCategory][userID] -=
                clearCount * _questRequireValueData[questCategory][questType];
        }
        

        // 보상 지급
        if (questInfo && giveUserCurrencyAction != null)
        {
            giveUserCurrencyAction(userID, rewardCount * clearCount);

            SetUserGetReward(userID, questInfo);
            
        }

        // 퀘스트 정보 다시 전송하여 퀘스트 정보 초기화
        SendQuestDataToPlayer(userID,questCategory, questType);

    }

    private static void SetUserGetReward(int userID, QuestDataBase questInfo)
    {
        if (!questInfo.IsRewardRepeatable() && _getRewardUsersID.ContainsKey(questInfo.GetQuestCategory()))
        {
            _getRewardUsersID[questInfo.GetQuestCategory()][questInfo.GetQuestType()].Add(userID);
        }
    }


    public static void UserGoldSpending(int userID, BigInteger spendingAmount)
    {
        // 범위 체크 생략

        RenewalUserQuestProgressData(userID, QuestType.GoldSpending, spendingAmount);
    }

    public static void UserMonsterKill(int userID, int monsterKillCount)
    {
        // 범위 체크 생략


        RenewalUserQuestProgressData(userID,QuestType.KillMonster,monsterKillCount);
    }

    protected static void RenewalUserQuestProgressData(int userID, QuestType questType, BigInteger newAddVal)
    {
        // 반복,일일,주간,업적 등에서 userID 유저에 해당하는 퀘스트들 정보 전부 갱신
        foreach (var progressData in userQuestProgressData[questType])
        {
            if (progressData.Value.ContainsKey(userID))
            {
                progressData.Value[userID] += newAddVal;
                SendQuestDataToPlayer(userID, progressData.Key, questType);
            }
        }
    }

    
    
}
