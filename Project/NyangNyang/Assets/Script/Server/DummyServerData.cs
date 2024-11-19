using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using UnityEngine;


public class DummyServerData : MonoBehaviour
{
    // 임시 더미 데이터, 서버(+DB)라 가정하고 제작 진행

    // ===============
    // 데이터 시작

    // 각 유저 스탯 레벨 데이터
    protected static StatusLevelData[] usersStatusLevelData = new StatusLevelData[]
    {
        new StatusLevelData(0,0,0,0,0),
        new StatusLevelData(10,0,5,0),
    };

    // MonsterData 에서 관리
    //private static StatusLevelData[] enemyStatusLevelData = new StatusLevelData[]
    //{
    //    new StatusLevelData(1, 1, 1, 2),
    //    new StatusLevelData(10, 0, 5, 2),
    //};

    // 클라에서 관리
    //protected static MonsterData[] enemyDatas = new MonsterData[]
    //{
    //    // 임시 일반 몬스터 데이터
    //    };

    // 각 유저 재화(골드+다이아+치즈+조개패) 데이터
    protected static CurrencyData[] usersCurrencyData = new CurrencyData[]
    {
        ScriptableObject.CreateInstance<CurrencyData>().SetCurrencyData(1_000_000_000,3,1000,new int[] {5,5,5}),
        ScriptableObject.CreateInstance<CurrencyData>(),
        ScriptableObject.CreateInstance<CurrencyData>(),
        ScriptableObject.CreateInstance<CurrencyData>(),
        ScriptableObject.CreateInstance<CurrencyData>(),
    };

    // 유저 레벨+경험치 데이터
    protected static UserLevelData[] usersLevelData = new UserLevelData[]
    {
        ScriptableObject.CreateInstance<UserLevelData>().SetUserLevelData(1, 0),
        ScriptableObject.CreateInstance<UserLevelData>(),
        ScriptableObject.CreateInstance<UserLevelData>(),
        ScriptableObject.CreateInstance<UserLevelData>(),
        ScriptableObject.CreateInstance<UserLevelData>(),
    };

   


    // 각 유저의 클리어 스테이지 정보 // 스테이지 테마와 스테이지 정보만 관리
    protected static int[,] playerClearStageData = new int[,]
    {
        { 5,5 },        // 0번 유저
        {20,3}
    };

    // 데이터 종료
    // ==================
    // 함수 시작


    public static StatusLevelData GetUserStatusLevelData(int userID)
    {
        if (!(0 <= userID && userID < usersStatusLevelData.Length))
        {
            Debug.Log("INVALID USERID");
            return null;
        }

        return usersStatusLevelData[userID];
    }

    // MonsterData에서 관리
    //public static StatusLevelData GetEnemyStatusLevelData(int characterID)
    //{
    //    if (!(0 <= characterID && characterID < enemyStatusLevelData.Length))
    //    {
    //        Debug.Log("INVALID CHARACTER_ID");
    //        return null;
    //    }
    //
    //    
    //    return enemyStatusLevelData[characterID];
    //}

    public static BigInteger GetUserStatusLevelFromType(int userID, StatusLevelType type)
    {
        StatusLevelData statusLevelData = GetUserStatusLevelData(userID);
        if (statusLevelData != null)
        {
            return statusLevelData.GetLevelFromType(type);
        }

        return -1;
    }

    public static bool UserStatusLevelUp(int userID,StatusLevelType type, int newLevel, BigInteger currentGold)
    {
        // 골드 정보 및 스탯 레벨 정보 갱신
        // 클라의 정보는 클라에서 갱신
        GetUserCurrencyData(userID).gold = currentGold;
        GetUserStatusLevelData(userID).statusLevels[(int)type] = newLevel;

        // TODO: 클라의 패킷이 정상적이지 않은 데이터를 담을 경우 false 리턴 or false 되는 패킷 전송
        return true;
    }

    public static CurrencyData GetUserCurrencyData(int userID)
    {
        if (!(0 <= userID && userID < usersStatusLevelData.Length))
        {
            Debug.Log("INVALID USERID");
            return null;
        }

        return usersCurrencyData[userID];
    }

    public static UserLevelData GetUserLevelData(int userID)
    {
        if (!(0 <= userID && userID < usersLevelData.Length))
        {
            Debug.Log("INVALID USERID");
            return null;
        }

        return usersLevelData[userID];
    }



    public static CurrencyData GetUserGoldData(int userId)
    {
        CurrencyData userData = GetUserCurrencyData(userId);
        if (userData == null)
        {
            Debug.Log("Error - DummyServerData.GetUserGoldData");
        }
        return userData;
    }

    public static void UserLevelUp(int userID, int levelUpCount, BigInteger addExp)
    {
        GetUserLevelData(userID).currentExp += addExp;
        GetUserLevelData(userID).currentLevel += levelUpCount;

        // 10.31 클라에서 서버에 정보를 보내면서 클라에서도 레벨업을 처리하도록 진행
        // 서버로부터 정보를 받도록 패킷 전송
        //Player.GetExpDataFromServer();
    }

    // MonsterData 내에 추가
    //public static EnemyDropData GetEnemyDropData(int characterID)
    //{
    //    if (characterID < 0 || characterID >= enemyDropData.Length)
    //    {
    //        Debug.Log("INVALID CHARACTER_ID");
    //        return null;
    //    }
    //
    //    return enemyDropData[characterID];
    //}


    // 티켓이 있는지 확인하는 함수 (유저 ID, 티켓 종류)
    public static bool HasTicket(int userID, int index)
    {
        if (!IsValidUser(userID) || !IsValidTicketIndex(index))
        {
            Debug.Log("INVALID USERID OR INDEX");
            return false;
        }

        return usersCurrencyData[userID].ticket[index] > 0;
    }

    // 티켓을 사용하는 함수
    public static bool UseTicket(int userID, int index)
    {
        if (HasTicket(userID, index))
        {
            usersCurrencyData[userID].ticket[index]--;
            Debug.Log($"티켓 사용: 남은 티켓 수량 {usersCurrencyData[userID].ticket[index]}");
            return true;
        }

        Debug.Log("티켓이 부족합니다.");
        return false;
    }

    // 티켓 수량을 가져오는 함수
    public static int GetTicketCount(int userID, int index)
    {
        if (!IsValidUser(userID) || !IsValidTicketIndex(index))
        {
            Debug.Log("INVALID USERID OR INDEX");
            return 0;
        }

        return usersCurrencyData[userID].ticket[index];
    }

    // 티켓을 추가하는 함수
    public static void AddTicket(int userID, int index, int amount)
    {
        if (!IsValidUser(userID) || !IsValidTicketIndex(index))
        {
            Debug.Log("INVALID USERID OR INDEX");
            return;
        }

        usersCurrencyData[userID].ticket[index] += amount;
        Debug.Log($"유저 {userID}에게 티켓 {index + 1}번을 {amount}개 추가했습니다. 현재 티켓 수량: {usersCurrencyData[userID].ticket[index]}개");
    }

    // 유저 ID의 유효성을 확인하는 함수
    private static bool IsValidUser(int userID)
    {
        return userID >= 0 && userID < usersCurrencyData.Length;
    }

    // 티켓 인덱스의 유효성을 확인하는 함수
    private static bool IsValidTicketIndex(int index)
    {
        return index >= 0 && index < 3; // 티켓 배열 크기와 동일
    }

    public static void GetUserClearStageData(int userID, out int clearStageTheme, out int clearStage)
    {
        if (!(0 <= userID && userID < playerClearStageData.Length))
        {
            Debug.Log("INVALID USERID");
            clearStageTheme = 1;
            clearStage = 1;
            return;
        }

        clearStageTheme = playerClearStageData[userID, 0];
        clearStage = playerClearStageData[userID, 1];
    }


    public static void PlayerClearStage(int userID, int clearTheme, int clearStage)
    {
        if (!(0 <= userID && userID < playerClearStageData.Length))
        {
            Debug.Log("INVALID USERID");
            return;
        }

        // 잘못된 데이터인지 체크
        int playerHighestTheme = playerClearStageData[userID, 0];
        int playerHighestStage = playerClearStageData[userID, 1];
        if (playerHighestTheme > clearTheme || (playerHighestTheme == clearTheme && playerHighestStage >= clearStage))
        {
            Debug.Log(userID + "이전 최고 스테이지보다 낮은 스테이지를 클리어했다고 정보 전달받음");
            return;
        } 
        
        // *스테이지가 하나 차이인지도 추후 확인해야함
        playerClearStageData[userID, 0] = clearTheme;
        playerClearStageData[userID, 1] = clearStage;
    }

    public static void GiveUserDiamondAndSendData(int userID, BigInteger addDiamond)
    {
        // 다이아몬드는 int 범위 내에서 해결 가능하지만, Gold(BigInteger)와 Action으로 한번에 묶여서 관리할 일이 있어
        // 해당 함수는 BigInteger로 인자를 받도록 진행

        //범위 체크 생략
        usersCurrencyData[userID].diamond += (int)addDiamond;
        
        // 강제로 플레이어에게 주입
        Player.Diamond = usersCurrencyData[userID].diamond;
    }

     public static void GiveUserCheeseAndSendData(int userID, BigInteger addCheese)
    {
        
        //범위 체크 생략
        usersCurrencyData[userID].cheese += (int)addCheese;
        
        // 강제로 플레이어에게 주입
        Player.Cheese = usersCurrencyData[userID].cheese;
    }

    // 함수 종료
    // ================
}
