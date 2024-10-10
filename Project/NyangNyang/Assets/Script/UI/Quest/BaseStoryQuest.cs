using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseStoryQuest : BaseQuest
{
    public GameObject storyQuestClearPanel;

    protected override void Start()
    {
        RecvQuestDataFromServer();
        //base.Start();
    }

    public override void SetRewardButtonInteractable(bool newActive, string newText)
    {
        base.SetRewardButtonInteractable(newActive, newText);

        // 퀘스트가 클리어 됨을 표시하는 함수
        storyQuestClearPanel.SetActive(newActive);
    }

    public void RecvQuestDataFromServer()
    {
        questData = DummyStoryQuestServer.SendQuestInfoDataToUser(Player.GetUserID());
        if (questData == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            LoadQuest();
        }

        
        
    }
}
