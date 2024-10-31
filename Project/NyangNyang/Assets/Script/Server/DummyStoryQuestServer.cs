using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class DummyStoryQuestServer : DummyQuestServer
{

    protected static int[] usersStoryQuestID = new int[]
    {
        0,
        0,
    };

    //// TODO: 유저 접속 종료 시 해당 델리게이트 종료되도록 
    public static int SendCurrentStoryQuestIDToUser(int userID)
    {
        int currentUserQuestID = usersStoryQuestID[userID];
        return currentUserQuestID;
    }

    public static void UserSendStoryQuestClear(int userID, QuestDataBase questData)
    {
        // 보상지급
        {
            usersCurrencyData[userID].diamond += questData.rewardCount;
        }

        usersStoryQuestID[userID] += 1;
    }
}
