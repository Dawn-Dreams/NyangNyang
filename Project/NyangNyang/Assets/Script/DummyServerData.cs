using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DummyServerData : MonoBehaviour
{
    // 임시 더미 데이터, 서버라 가정하고 제작 진행중
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
}
