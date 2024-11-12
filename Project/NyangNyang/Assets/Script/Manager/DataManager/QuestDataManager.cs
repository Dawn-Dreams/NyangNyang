using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestDataManager : MonoBehaviour
{
    private static QuestDataManager _instance;

    public static QuestDataManager GetInstance()
    {
        return _instance;
    }

    public List<AddressableHandleAssets<QuestDataBase>> questDataList;

    void Awake()
    {
        AssetLoad();

        if (_instance == null)
        {
            _instance = this;
        }    
    }

    public void AssetLoad()
    {
        questDataList = new List<AddressableHandleAssets<QuestDataBase>>();
        for (int i = 0; i < (int)QuestCategory.Story; ++i)
        {
            AddressableHandleAssets<QuestDataBase> questData = new AddressableHandleAssets<QuestDataBase>();
            questData.LoadAssets("QuestData/" + (QuestCategory)i);
            questDataList.Add(questData);
        }


        // 스토리는 정렬방식을 다르게 진행
        AddressableHandleAssets<QuestDataBase> storyQuestData = new AddressableHandleAssets<QuestDataBase>();
        storyQuestData.LoadAssets("QuestData/Story");
        storyQuestData.objs.Sort(( a, b) =>
        {
            return ((StoryQuestDataBase)a).storyQuestID < ((StoryQuestDataBase)b).storyQuestID ? -1 : 1;
        });
        questDataList.Add(storyQuestData);
    }
}
