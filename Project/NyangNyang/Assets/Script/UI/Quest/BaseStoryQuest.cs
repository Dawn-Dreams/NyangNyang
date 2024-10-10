using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStoryQuest : BaseQuest
{
    public GameObject storyQuestClearPanel;

    protected override void Start()
    {
        questData = DummyStroyQuestServer.SendQuestInfoDataToUser(Player.GetUserID());

        base.Start();
    }

    public override void SetRewardButtonInteractable(bool newActive, string newText)
    {
        // 퀘스트가 클리어 됨을 표시하는 함수
        storyQuestClearPanel.SetActive(newActive);
    }
}
