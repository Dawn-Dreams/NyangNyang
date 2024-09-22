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
    private static StatusLevelData[] usersStatusLevelData = new StatusLevelData[]
    {
        new StatusLevelData(50,0,5),
        new StatusLevelData(10,0,5,2),
        new StatusLevelData(),
        new StatusLevelData(),
        new StatusLevelData(),
    };

    private static StatusLevelData[] enemyStatusLevelData = new StatusLevelData[]
    {
        new StatusLevelData(10, 0, 5, 2),
        new StatusLevelData(10, 0, 5, 2),
    };

    // 유저 재화(골드+보석) 데이터
    private static CurrencyData[] usersCurrencyData = new CurrencyData[]
    {
        ScriptableObject.CreateInstance<CurrencyData>().SetCurrencyData(150_000,3),
        ScriptableObject.CreateInstance<CurrencyData>(),
        ScriptableObject.CreateInstance<CurrencyData>(),
        ScriptableObject.CreateInstance<CurrencyData>(),
        ScriptableObject.CreateInstance<CurrencyData>(),
    };

    // 유저 레벨+경험치 데이터
    private static UserLevelData[] usersLevelData = new UserLevelData[]
    {
        ScriptableObject.CreateInstance<UserLevelData>().SetUserLevelData(5, 100),
        ScriptableObject.CreateInstance<UserLevelData>(),
        ScriptableObject.CreateInstance<UserLevelData>(),
        ScriptableObject.CreateInstance<UserLevelData>(),
        ScriptableObject.CreateInstance<UserLevelData>(),
    };

    // 소탕권 개수를 저장하는 배열 (각 유저의 소탕권 수량 관리)
    private static int[] sweepTickets = new int[] { 5, 2, 0, 3, 1 };

    // 스텟 레벨업 계산식 데이터
    private static int statusStartGoldCost = 100;
    private static int[] statusGoldCostAddValue = new int[]
    {
        // StatusLevelType enum
        // HP, MP, STR, DEF, HEAL_HP, HEAL_MP, CRIT, ATTACK_SPEED, GOLD, EXP
        100, 100, 100, 100, 300, 300, 50000, 10000, 100000,100000
    };
    // 경험치 레벨업 계산식 데이터
    private static int addExpPerLevel = 500;



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

    public static StatusLevelData GetEnemyStatusLevelData(int characterID)
    {
        if (!(0 <= characterID && characterID < enemyStatusLevelData.Length))
        {
            Debug.Log("INVALID CHARACTER_ID");
            return null;
        }

        return enemyStatusLevelData[characterID];
    }

    public static int GetUserStatusLevelFromType(int userID, StatusLevelType type)
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
    // 소탕권이 있는지 확인하는 함수
    public static bool HasSweepTicket(int userID)
    {
        if (userID < 0 || userID >= sweepTickets.Length)
        {
            Debug.Log("INVALID USERID");
            return false;
        }

        return sweepTickets[userID] > 0;
    }

    // 소탕권을 사용하는 함수
    public static bool UseSweepTicket(int userID)
    {
        if (HasSweepTicket(userID))
        {
            sweepTickets[userID]--;
            Debug.Log($"소탕권 사용: 남은 소탕권 수량 {sweepTickets[userID]}");
            return true;
        }

        Debug.Log("소탕권이 부족합니다.");
        return false;
    }

    // 소탕권 수량을 가져오는 함수
    public static int GetSweepTicketCount(int userID)
    {
        if (userID < 0 || userID >= sweepTickets.Length)
        {
            Debug.Log("INVALID USERID");
            return 0;
        }

        return sweepTickets[userID];
    }
    // 함수 종료
    // ================
}
