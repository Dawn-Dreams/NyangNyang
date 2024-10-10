using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class DummyStoryQuestServer : DummyQuestServer
{
    protected static QuestDataBase[] QuestData = new QuestDataBase[]
    {
        ScriptableObject.CreateInstance<LevelUpStatusQuestData>().Initialize(StatusLevelType.STR, 100),
        ScriptableObject.CreateInstance<LevelUpStatusQuestData>().Initialize(StatusLevelType.HP, 100),
        ScriptableObject.CreateInstance<LevelUpStatusQuestData>().Initialize(StatusLevelType.STR, 200),
        ScriptableObject.CreateInstance<LevelUpStatusQuestData>().Initialize(StatusLevelType.HP, 200),
        ScriptableObject.CreateInstance<LevelUpStatusQuestData>().Initialize(StatusLevelType.STR, 300),
        ScriptableObject.CreateInstance<LevelUpStatusQuestData>().Initialize(StatusLevelType.HP, 300),
        ScriptableObject.CreateInstance<LevelUpStatusQuestData>().Initialize(StatusLevelType.STR, 400),
        ScriptableObject.CreateInstance<LevelUpStatusQuestData>().Initialize(StatusLevelType.HP, 400),
        // 추후 생성...
    };

    protected static int[] usersQuestID = new int[]
    {
        0,
        0,
    };

    // 유저가 접속한 초기 한번만 실행되도록
    // TODO: 유저 접속 종료 시 해당 델리게이트 종료되도록 
    public static QuestDataBase SendQuestInfoDataToUser(int userID)
    {
        int currentUserQuestID = usersQuestID[userID];

        if (currentUserQuestID < 0 || currentUserQuestID >= QuestData.Length)
        {
            return null;
        }

        QuestDataBase questInfoData = QuestData[currentUserQuestID];
        
        questInfoData.BindDelegateOnServer();

        return questInfoData;
    }

    public static void SendLevelUpStatusQuestDataToUser(int userID, StatusLevelType type)
    {
        // 특정 userID인지 체크하는 부분 추가
        // 델리게이트 방식으로 함수를 추가하여 진행하지 말고 별도의 변수로 관리하면 편할 것으로 예상

        BigInteger userCurrentStatusLevel = GetUserStatusLevelData(userID).GetLevelFromType(type);
        Player.RecvStatusDataFromServer(type,userCurrentStatusLevel);
    }

    public static void SendNewQuestDataToUser(int userID)
    {
        int currentUserQuestID = usersQuestID[userID];
        QuestDataBase questInfoData = QuestData[currentUserQuestID];
        questInfoData.UnBindDelegateOnServer();

        usersQuestID[userID] += 1;
        int newQuestID = usersQuestID[userID];
        QuestDataBase newQuestInfo = QuestData[currentUserQuestID];
        newQuestInfo.BindDelegateOnServer();
        GameManager.GetInstance().storyQuestObject.RecvQuestDataFromServer();
    }
}
