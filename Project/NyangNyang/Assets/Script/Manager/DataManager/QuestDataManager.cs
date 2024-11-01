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
    public List<QuestDataBase> storyQuestData;

    public List<List<QuestDataBase>> questDataList;

    void Awake()
    {
        questDataList = new List<List<QuestDataBase>>
        {
            repeatQuestData,
            dailyQuestData,
            weeklyQuestData,
            achievementQuestData,
            storyQuestData,
        };


        if (_instance == null)
        {
            _instance = this;
        }    
    }


}
