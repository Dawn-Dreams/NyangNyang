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




    //// 유저가 퀘스트 보상을 받았는지에 대한 변수
    //private static Dictionary<QuestCategory, Dictionary<QuestType, List<int>>> _getRewardUsersID =
    //    new Dictionary<QuestCategory, Dictionary<QuestType, List<int>>>
    //    {
    //        // 일일 퀘스트
    //        {
    //            QuestCategory.Daily, new Dictionary<QuestType, List<int>>
    //            {
    //                { QuestType.GoldSpending, new List<int>() },
    //                {QuestType.KillMonster, new List<int>()},
    //                {QuestType.ObtainWeapon, new List<int>()},
    //                {QuestType.ObtainSkill, new List<int>()},
    //                {QuestType.SkillLevelUp, new List<int>()},
    //                {QuestType.CombineWeapon, new List<int>()},
    //            }
    //        },
    //        // 주간 퀘스트
    //        {
    //            QuestCategory.Weekly, new Dictionary<QuestType, List<int>>
    //            {
    //                {QuestType.KillMonster, new List<int>()}
    //            }
    //        }
    //        // 업적 퀘스트는 클라 내에서 해당 칭호를 가지고 있는지로 파악 하여 진행 예정
    //    };


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
                            { 0, 98 },
                            { 1, 0 }
                        }
                    },
                    // 일일 퀘스트
                    {
                        QuestCategory.Daily, new Dictionary<int, BigInteger>
                        {
                            { 0, 30 },
                            { 1, 0 }
                        }
                    },
                    // 주간 퀘스트
                    {
                        QuestCategory.Weekly, new Dictionary<int, BigInteger>
                        {
                            { 0, 70 },
                            { 1, 0 }
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
                            { 0, 35 }
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
                            { 0, 3 }
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
                            { 0, 10 }
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
                            { 0, 5 }
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
                            { 0, 990 }
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
                            { 0, 990 }
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
                            { 0, 990 }
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
                            { 0, 990 }
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
                            { 0, 990 }
                        }
                    }
                }
            }

        };
    // =========================
}
