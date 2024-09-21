using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using UnityEngine;



public class DummyServerData : MonoBehaviour
{
    // 임시 더미 데이터, 서버(+DB)라 가정하고 제작 진행중
    // 09.13 임시 ID
    // ID 0 -> Cat
    // ID 1 -> Enemy

    private static StatusLevelData[] usersStatusLevelData = new StatusLevelData[]
    {
        new StatusLevelData(50,0,5),
        new StatusLevelData(10,0,5,2),
        new StatusLevelData(),
        new StatusLevelData(),
        new StatusLevelData(),
    };

    private static CurrencyData[] usersCurrencyData = new CurrencyData[]
    {
        ScriptableObject.CreateInstance<CurrencyData>().SetCurrencyData(150_000,3),
        ScriptableObject.CreateInstance<CurrencyData>(),
        ScriptableObject.CreateInstance<CurrencyData>(),
        ScriptableObject.CreateInstance<CurrencyData>(),
        ScriptableObject.CreateInstance<CurrencyData>(),
    };

    private static UserLevelData[] usersLevelData = new UserLevelData[]
    {
        ScriptableObject.CreateInstance<UserLevelData>().SetUserLevelData(5, 100),
        ScriptableObject.CreateInstance<UserLevelData>(),
        ScriptableObject.CreateInstance<UserLevelData>(),
        ScriptableObject.CreateInstance<UserLevelData>(),
        ScriptableObject.CreateInstance<UserLevelData>(),
    };

    private static int statusStartGoldCost = 100;

    private static int[] statusGoldCostAddValue = new int[]
    {
        // StatusLevelType enum
        // HP, MP, STR, DEF, HEAL_HP, HEAL_MP, CRIT, ATTACK_SPEED, GOLD, EXP
        100, 100, 100, 100, 300, 300, 50000, 10000, 100000,100000
    };

    void Start()
    {
        
    }

    public static StatusLevelData GetUserStatusLevelData(int userID)
    {
        if (!(0 <= userID && userID < usersStatusLevelData.Length))
        {
            Debug.Log("INVALID USERID");
            return null;
        }

        return usersStatusLevelData[userID];
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
        if (!(0 <= userID && userID < usersStatusLevelData.Length))
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



}
