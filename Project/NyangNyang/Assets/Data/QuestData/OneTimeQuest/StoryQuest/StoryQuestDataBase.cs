using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoryQuestDataBase : OneTimeQuestDataBase
{
    public int storyQuestID;

    public override void QuestActing(BaseQuest quest, QuestType type)
    {
        questCategory = QuestCategory.Story;

        base.QuestActing(quest, type);
    }

    public override void RequestQuestReward()
    {
        base.RequestQuestReward();

        // 서버에서 퀘스트 클리어 및 보상 적용
        DummyStoryQuestServer.UserSendStoryQuestClear(Player.GetUserID(), this);
        
        // 클라에서도 동일하게 지급되도록
        Player.Diamond += rewardCount;

        BaseStoryQuest.GetInstance().ClearStoryQuest();
    }


}
