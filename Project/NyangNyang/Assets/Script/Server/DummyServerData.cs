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

    // 유저 스탯 레벨 데이터
    protected static StatusLevelData[] usersStatusLevelData = new StatusLevelData[]
    {
        new StatusLevelData(0,0,0,0,0),
        new StatusLevelData(10,0,5,0),
        new StatusLevelData(),
        new StatusLevelData(),
        new StatusLevelData(),
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

    // 유저 재화(골드+보석+티켓) 데이터
    protected static CurrencyData[] usersCurrencyData = new CurrencyData[]
    {
        ScriptableObject.CreateInstance<CurrencyData>().SetCurrencyData(1_000_000_000,3,new int[] {5,5,5}),
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

    // 스텟 레벨업 계산식 데이터
    protected static int statusStartGoldCost = 100;
    protected static int[] statusGoldCostAddValue = new int[]
    {
        // StatusLevelType enum
        // HP, MP, STR, DEF, HEAL_HP, HEAL_MP, CRIT, ATTACK_SPEED, GOLD, EXP
        100, 100, 100, 100, 300, 300, 50000, 10000, 100000,100000
    };
    // 경험치 레벨업 계산식 데이터
    protected static int addExpPerLevel = 500;

    protected static EnemyDropData[] enemyDropData = new EnemyDropData[]
    {
        ScriptableObject.CreateInstance<EnemyDropData>().SetEnemyDropData(1_000_000, 777_777),
        ScriptableObject.CreateInstance<EnemyDropData>(),
        ScriptableObject.CreateInstance<EnemyDropData>()
    };

    // 스테이지 테마와 스테이지 정보만 관리
    protected static int[,] playerClearStageData = new int[,]
    {
        { 1,0 },        // 0번 유저
        {20,3}
    };

    // 데이터 종료
    // ================== 
    // 델리게이트 시작

    public delegate void OnUserGoldSpendingDelegate(int userID,  BigInteger spendingAmount);
    public static OnUserGoldSpendingDelegate OnUserGoldSpending;
    public delegate void OnUserStatusLevelUpDelegate(int userID, StatusLevelType type);
    public static OnUserStatusLevelUpDelegate OnUserStatusLevelUp;
    public delegate void OnUserStageClearDelegate(int userID);
    public static OnUserStageClearDelegate OnUserStageClear;

    // 델리게이트 종료
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

    public static int GetStartGoldCost()
    {
        return statusStartGoldCost;
    }

    public static int GetGoldCostAddValueFromType(StatusLevelType type)
    {
        return statusGoldCostAddValue[(int)type];
    }

    public static bool UserStatusLevelUp(int userID,StatusLevelType type, BigInteger currentLevel,  int value)
    {
        // 소지한 골드가 정상적인지 체크
        BigInteger goldCost = CalculateGoldCost(type, currentLevel, value);
        if (GetUserCurrencyData(userID).gold >= goldCost)
        {
            GetUserStatusLevelData(userID).AddLevel(type, value);
            GetUserCurrencyData(userID).gold -= goldCost;

            if (OnUserGoldSpending != null)
            {
                OnUserGoldSpending(userID, goldCost);
            }

            if (OnUserStatusLevelUp != null)
            {
                OnUserStatusLevelUp(userID, type);
            }
            

            return true;
        }

        // TODO: 클라의 패킷이 정상적이지 않은 데이터를 담을 경우 false 리턴 or false 되는 패킷 전송
        return false;

        
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



    public static BigInteger GetUserGoldData(int userId)
    {
        CurrencyData userData = GetUserCurrencyData(userId);
        if (userData == null)
        {
            Debug.Log("Error - DummyServerData.GetUserGoldData");
        }
        return userData.gold;
    }


    // 서버 내 골드 계산 검증 함수
    public static BigInteger CalculateGoldCost(StatusLevelType type, BigInteger currentLevel, int levelUpMultiplyValue)
    {
        int goldAddValue = statusGoldCostAddValue[(int)type];
        // n ~ m 레벨 계산 ((n부터 m까지의 갯수) * (n+m) / 2 )
        BigInteger levelUpValue = (levelUpMultiplyValue) * (currentLevel + (currentLevel + levelUpMultiplyValue)) / 2;
        BigInteger goldCost = goldAddValue * (levelUpValue);

        return goldCost;
    }

    public static int GetAddExpPerLevelValue()
    {
        return addExpPerLevel;

    }

    public static void UserLevelUp(int userID, int levelUpCount, BigInteger addExp)
    {
        GetUserLevelData(userID).currentExp += addExp;
        GetUserLevelData(userID).currentLevel += levelUpCount;

        // 서버로부터 정보를 받도록 패킷 전송
        Player.GetExpDataFromServer();
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

    public static bool AddGoldOnServer(int userID, BigInteger addGoldValue)
    {
        CurrencyData userCurrencyData = GetUserCurrencyData(userID);
        if (userCurrencyData)
        {
            userCurrencyData.gold += addGoldValue;
            Player.GetGoldDataFromServer();
            return true;
        }

        return false;
    }

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

        if (OnUserStageClear != null)
        {
            OnUserStageClear(userID);
        }
    }

    public static void GiveUserDiamondAndSendData(int userID, BigInteger addDiamond)
    {
        //범위 체크 생략
        usersCurrencyData[userID].diamond += addDiamond;
        
        // 강제로 플레이어에게 주입
        Player.Diamond = usersCurrencyData[userID].diamond;
    }

    // 함수 종료
    // ================
}
