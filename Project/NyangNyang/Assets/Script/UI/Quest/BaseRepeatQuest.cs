using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRepeatQuest : BaseQuest
{
    public QuestType questType;

    protected override void Start()
    {
        QuestData = DummyQuestServer.SendRepeatQuestInfoToUser(Player.GetUserID(), questType);

        base.Start();
    }
}
