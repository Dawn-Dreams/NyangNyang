using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RepeatQuestDataBase : QuestDataBase
{
    public override void RequestQuestReward()
    {
        DummyQuestServer.UserRequestReward(Player.GetUserID(), QuestCategory, QuestType, this);
    }
}
