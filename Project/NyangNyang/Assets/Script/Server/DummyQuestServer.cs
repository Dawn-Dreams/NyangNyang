using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public enum QuestType
{
    // 일반 퀘스트
    GoldSpending, KillMonster, ObtainWeapon, CombineWeapon, ObtainSkill, SkillLevelUp, 
    // 스토리
    LevelUpStatus, StageClear,
    // 업적
    FirstTime,
    KillStarfish, KillOctopus,KillPuffe, KillShellfish, KillKrake,
}
[Serializable]
public enum QuestCategory
{
    Repeat,
    Daily,
    Weekly,
    Achievement,
    Story,
    Count
}
[Serializable]
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




    // 유저가 퀘스트 보상을 받았는지에 대한 변수
    private static Dictionary<QuestCategory, Dictionary<QuestType, List<int>>> _getRewardUsersID =
        new Dictionary<QuestCategory, Dictionary<QuestType, List<int>>>
        {
            // 일일 퀘스트
            {
                QuestCategory.Daily, new Dictionary<QuestType, List<int>>
                {
                    { QuestType.GoldSpending, new List<int>() },
                    {QuestType.KillMonster, new List<int>()},
                    {QuestType.ObtainWeapon, new List<int>()},
                    {QuestType.ObtainSkill, new List<int>()},
                    {QuestType.SkillLevelUp, new List<int>()},
                    {QuestType.CombineWeapon, new List<int>()},
                }
            },
            // 주간 퀘스트
            {
                QuestCategory.Weekly, new Dictionary<QuestType, List<int>>
                {
                    {QuestType.KillMonster, new List<int>()}
                }
            }
            // 업적 퀘스트는 클라 내에서 해당 칭호를 가지고 있는지로 파악 하여 진행 예정
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
                    // 일일 퀘스트
                    {
                        QuestCategory.Daily, new Dictionary<int, BigInteger>
                        {
                            {0, 30},
                            {1, 0}
                        }
                    },
                    // 주간 퀘스트
                    {
                        QuestCategory.Weekly, new Dictionary<int, BigInteger>
                        {
                            {0, 70},
                            {1, 0}
                        }
                    }
                }
            },

            // ObtainWeapon 퀘스트
            {
                QuestType.ObtainWeapon, new Dictionary<QuestCategory, Dictionary<int, BigInteger>>
                {
                    // 일일퀘스트
                    {
                        QuestCategory.Daily, new Dictionary<int, BigInteger>
                        {
                            {0, 35}
                        }
                    }
                }
             
            },

            // CombineWeapon 퀘스트
            {
                QuestType.CombineWeapon, new Dictionary<QuestCategory, Dictionary<int, BigInteger>>
                {
                    // 일일퀘스트
                    {
                        QuestCategory.Daily, new Dictionary<int, BigInteger>
                        {
                            {0, 3}
                        }
                    }
                }

            },

            // ObtainSkill 퀘스트
            {
                QuestType.ObtainSkill, new Dictionary<QuestCategory, Dictionary<int, BigInteger>>
                {
                    // 일일퀘스트
                    {
                        QuestCategory.Daily, new Dictionary<int, BigInteger>
                        {
                            {0, 10}
                        }
                    }
                }

            },

            // SkillLevelUp 퀘스트
            {
                QuestType.SkillLevelUp, new Dictionary<QuestCategory, Dictionary<int, BigInteger>>
                {
                    // 반복퀘스트
                    {
                        QuestCategory.Repeat, new Dictionary<int, BigInteger>
                        {
                            {0, 5}
                        }
                    }
                }

            },

            // KillStarfish 퀘스트
            {
                QuestType.KillStarfish, new Dictionary<QuestCategory, Dictionary<int, BigInteger>>
                {
                    // 업적 퀘스트
                    {
                        QuestCategory.Achievement, new Dictionary<int, BigInteger>
                        {
                            {0, 990}
                        }
                    }
                }
            },
            // KillOctopus 퀘스트
            {
                QuestType.KillOctopus, new Dictionary<QuestCategory, Dictionary<int, BigInteger>>
                {
                    // 업적 퀘스트
                    {
                        QuestCategory.Achievement, new Dictionary<int, BigInteger>
                        {
                            {0, 990}
                        }
                    }
                }
            },
            // KillPuffe 퀘스트
            {
                QuestType.KillPuffe, new Dictionary<QuestCategory, Dictionary<int, BigInteger>>
                {
                    // 업적 퀘스트
                    {
                        QuestCategory.Achievement, new Dictionary<int, BigInteger>
                        {
                            {0, 990}
                        }
                    }
                }
            },
            // KillKrake 퀘스트
            {
                QuestType.KillKrake, new Dictionary<QuestCategory, Dictionary<int, BigInteger>>
                {
                    // 업적 퀘스트
                    {
                        QuestCategory.Achievement, new Dictionary<int, BigInteger>
                        {
                            {0, 990}
                        }
                    }
                }
            },
            // KillShellfish 퀘스트
            {
                QuestType.KillShellfish, new Dictionary<QuestCategory, Dictionary<int, BigInteger>>
                {
                    // 업적 퀘스트
                    {
                        QuestCategory.Achievement, new Dictionary<int, BigInteger>
                        {
                            {0, 990}
                        }
                    }
                }
            }

        };
    // =========================

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

    public static void GetQuestDataFromClient(int userID, QuestCategory questCategory, QuestType questType,
        BigInteger newValue)
    {
        if (userQuestProgressData.ContainsKey(questType) && userQuestProgressData[questType].ContainsKey(questCategory))
        {
            userQuestProgressData[questType][questCategory][userID] = newValue;
        }
    }

    // TODO : SendQuestDataToPlayer와 비슷한 맥락을 하는 함수지만 바로 접근하는 함수
    public static BigInteger SendQuestProgressDataToClient(int userID, QuestCategory questCategory, QuestType questType)
    {
        return userQuestProgressData[questType][questCategory][userID];
    }

    // 유저가 보상 흭득을 요구하는 함수
    public static void UserRequestReward(int userID, NormalQuestDataBase questInfo)
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
                    //giveUserCurrencyAction += DummyServerData.GiveUserDiamondAndSendData;
                    break;
            }
        }
        
        {
            // TODO: 유저의 데이터가 퀘스트 클리어 가능한지 체크
            // 안될 시 잘못된 정보라는 패킷 전송
            //유저가 daily/weekly/achievement 퀘스트 등 일회성 퀘스트를 달성했었는지에 대한 체크
            if (!questInfo.IsRewardRepeatable() && _getRewardUsersID.ContainsKey(questInfo.GetQuestCategory()) && _getRewardUsersID[questInfo.questCategory][questInfo.questType].Contains(userID))
            {
                Debug.Log($"{userID} 유저가 이미 클리어한 퀘스트를 클리어하겠다고 요청함 확인 필요");
                return;
            }
        }

        // 10.31 추가 서버내에서 따로 값을 관리하는 것이 아닌 클라이언트의 값을 그대로 덮는, 저장방식으로 진행하도록 수정
        if (userQuestProgressData.ContainsKey(questInfo.questType) && userQuestProgressData[questInfo.questType].ContainsKey(questInfo.questCategory))
        {
            userQuestProgressData[questInfo.questType][questInfo.questCategory][userID] = questInfo.GetCurrentQuestCount();
        }
        
        
        // 보상 갯수 계산
        int clearCount = 1;
        if (questInfo.IsRewardRepeatable())
        {
            clearCount = (int)BigInteger.Divide(userQuestProgressData[questInfo.questType][questInfo.questCategory][userID],
                questInfo.GetRequireCount());
        }

        // 필요 시 퀘스트의 데이터를 감소시키는 코드 진행
        if (userQuestProgressData.ContainsKey(questInfo.questType) && userQuestProgressData[questInfo.questType].ContainsKey(questInfo.questCategory))
        {
            userQuestProgressData[questInfo.questType][questInfo.questCategory][userID] -=
                clearCount * questInfo.GetRequireCount();
        }
        

        // 보상 지급
        if (questInfo && giveUserCurrencyAction != null)
        {
            giveUserCurrencyAction(userID, rewardCount * clearCount);

            SetUserGetReward(userID, questInfo);
            
        }

        // 퀘스트 정보 다시 전송하여 퀘스트 정보 초기화
        // 11.12 클라내에서도 동일하게 횟수를 깎아서 진행하기로 설정
        //SendQuestDataToPlayer(userID, questInfo.questCategory, questInfo.questType);

    }

    private static void SetUserGetReward(int userID, QuestDataBase questInfo)
    {
        if (!questInfo.IsRewardRepeatable() && _getRewardUsersID.ContainsKey(questInfo.GetQuestCategory()))
        {
            _getRewardUsersID[questInfo.GetQuestCategory()][questInfo.GetQuestType()].Add(userID);
        }
    }

    
    
}
