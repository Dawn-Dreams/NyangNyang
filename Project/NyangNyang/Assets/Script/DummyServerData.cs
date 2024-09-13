using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DummyServerData : MonoBehaviour
{
    private static StatusLevelData[] usersStatusLevelData = new StatusLevelData[]
    {
        new StatusLevelData(),
        new StatusLevelData(),
        new StatusLevelData(),
        new StatusLevelData(),
        new StatusLevelData(),
    };
    

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
