using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseStoryQuest : BaseQuest
{
    private static BaseStoryQuest _instance;

    public static BaseStoryQuest GetInstance()
    {
        return _instance;
    }

    public GameObject storyQuestClearPanel;
    private int _currentQuestID = 0;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    protected override void Start()
    {
        GetCurrentQuestIDFromServer();

        base.Start();
    }

    public void GetCurrentQuestIDFromServer()
    {
        _currentQuestID = DummyStoryQuestServer.SendCurrentStoryQuestIDToUser(Player.GetUserID());

        LoadCurrentQuest();
    }

    public void LoadCurrentQuest()
    {
        List<QuestDataBase> storyQuestData = QuestDataManager.GetInstance().questDataList[(int)QuestCategory.Story].objs;
        if (_currentQuestID >= storyQuestData.Count)
        {
            gameObject.SetActive(false);
        }
        else
        {
            questData = storyQuestData[_currentQuestID];
            LoadQuest(questData);
        }
    }

    public void ClearStoryQuest()
    {
        if (questData != null)
        {
            questData.RemoveQuest();
        }

        questData = null;
        _currentQuestID += 1;
        LoadCurrentQuest();
    }

    public override void SetRewardButtonInteractable(bool newActive, string newText)
    {
        base.SetRewardButtonInteractable(newActive, newText);

        // 퀘스트가 클리어 됨을 표시하는 함수
        storyQuestClearPanel.SetActive(newActive);
    }
}
