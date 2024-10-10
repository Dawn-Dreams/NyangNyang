using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class DummyStroyQuestServer : DummyQuestServer
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

    public static QuestDataBase SendQuestInfoDataToUser(int userID)
    {
        int currentUserQuestID = usersQuestID[userID];
        
        QuestDataBase questInfoData = QuestData[currentUserQuestID];
        
        DummyServerData.OnUserStatusLevelUp += SendLevelUpStatusQuestDataToUser;
        return questInfoData;
    }

    public static void SendLevelUpStatusQuestDataToUser(int userID, StatusLevelType type)
    {
        // 특정 userID인지 체크하는 부분 추가
        // static 델리게이트 특성 상 델리게이트에 추가된 이벤트 만큼 반복 실행하므로
        // 해당 플레이어에게 정보를 전송했는지 같은 값을 추가하거나... 등을 진행하면 가능할 것으로 보임
        BigInteger userCurrentStatusLevel = GetUserStatusLevelData(userID).GetLevelFromType(type);
        Player.RecvStatusDataFromServer(type,userCurrentStatusLevel);
    }
    public static void SendQuestDataToUser(int userID, QuestType storyQuestType)
    {
        switch (storyQuestType)
        {
            case QuestType.LevelUpStatus:
                //Player.RecvStatusDataFromServer();
                break;
            default:
                break;
        }
    }
}
