using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static int userID = 0;
    public static Status playerStatus;

    void Awake()
    {
        // 서버로부터 user id 받기
        userID = 0;

        if (playerStatus == null)
            playerStatus = new Status(userID);
    }

    public static int GetUserID()
    {
        return userID;
    }
}
