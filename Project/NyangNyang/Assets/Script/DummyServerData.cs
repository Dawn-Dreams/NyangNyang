using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private static int statusStartGoldCost = 100;

    private static float[] statusGoldCostMultiplyValue = new float[]
    {
        // StatusLevelType enum
        // HP, MP, STR, DEF, HEAL_HP, HEAL_MP, CRIT, ATTACK_SPEED, GOLD, EXP
        1.05f, 1.05f, 1.05f, 1.05f, 1.05f, 1.05f, 5.0f, 3.0f, 1.1f, 1.1f
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

    public static float GetGoldCostMultipleValueFromType(StatusLevelType type)
    {
        return statusGoldCostMultiplyValue[(int)type];
    }

    public static bool UserStatusLevelUp(int userID,StatusLevelType type, int value)
    {
        // TODO : 서버 내에서라면 검증하는 시스템 추가
        GetUserStatusLevelData(userID).AddLevel(type,value);

        return true;

        // TODO: 클라의 패킷이 정상적이지 않은 데이터를 담을 경우 false 리턴 or false 되는 패킷 전송
    }

    
}
