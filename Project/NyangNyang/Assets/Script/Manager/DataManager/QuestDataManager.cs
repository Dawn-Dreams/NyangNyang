using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDataManager : MonoBehaviour
{
    private static QuestDataManager _instance;

    public static QuestDataManager GetInstance()
    {
        return _instance;
    }

    public List<QuestDataBase> repeatQuestData;
    public List<QuestDataBase> dailyQuestData;
    public List<QuestDataBase> weeklyQuestData;
    public List<QuestDataBase> achievementQuestData;

    public List<List<QuestDataBase>> questDataList;

    void Awake()
    {
        questDataList = new List<List<QuestDataBase>>
        {
            repeatQuestData,
            dailyQuestData,
            weeklyQuestData,
            achievementQuestData,
        };


        if (_instance == null)
        {
            _instance = this;
        }    
    }


    /*
     *
     *
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
     */
}
