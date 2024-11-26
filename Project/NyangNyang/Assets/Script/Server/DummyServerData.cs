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

    //11.25 윤석 - json으로 이동
    #region 제거한 데이터

    //// 각 유저 스탯 레벨 데이터 
    //protected static StatusLevelData[] usersStatusLevelData = new StatusLevelData[]
    //{
    //    new StatusLevelData(0,0,0,0,0),
    //    new StatusLevelData(10,0,5,0),
    //};

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

    //// 유저 레벨+경험치 데이터
    //protected static UserLevelData[] usersLevelData = new UserLevelData[]
    //{
    //    ScriptableObject.CreateInstance<UserLevelData>().SetUserLevelData(1, 0),
    //    ScriptableObject.CreateInstance<UserLevelData>(),
    //    ScriptableObject.CreateInstance<UserLevelData>(),
    //    ScriptableObject.CreateInstance<UserLevelData>(),
    //    ScriptableObject.CreateInstance<UserLevelData>(),
    //};
    #endregion


    //11.25 윤석 - json으로 이동 // 치즈 및 조개패 데이터만 옮기시면 될것같습니다 승희님.
    // 각 유저 재화(골드+다이아+치즈+조개패) 데이터 
    protected static CurrencyData[] usersCurrencyData = new CurrencyData[]
    {
        ScriptableObject.CreateInstance<CurrencyData>().SetCurrencyData(1_000_000_000,3,1000,new int[] {5,5,5}),
        ScriptableObject.CreateInstance<CurrencyData>(),
        ScriptableObject.CreateInstance<CurrencyData>(),
        ScriptableObject.CreateInstance<CurrencyData>(),
        ScriptableObject.CreateInstance<CurrencyData>(),
    };


   


    // 각 유저의 클리어 스테이지 정보 // 스테이지 테마와 스테이지 정보만 관리
    protected static int[,] playerClearStageData = new int[,]
    {
        { 2,1 },        // 0번 유저
        {20,3}
    };

    // 데이터 종료
    // ==================
    // 함수 시작



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


    // 조개패가 있는지 확인하는 함수 (유저 ID, 조개패 종류)
    public static bool HasShell(int userID, int index)
    {

        if (!IsValidUser(userID) || !IsValidShellIndex(index))
        {
            Debug.Log("INVALID USERID OR INDEX");
            return false;
        }

        return usersCurrencyData[userID].shell[index] > 0;
    }

    // 조개패를 사용하는 함수
    public static bool UseShell(int userID, int index)
    {
        if (HasShell(userID, index))
        {
            usersCurrencyData[userID].shell[index]--;
            Debug.Log($"조개패 사용: 남은 조개패 수량 {usersCurrencyData[userID].shell[index]}");
            return true;
        }

        Debug.Log("조개패가 부족합니다.");
        return false;
    }

    // 조개패 수량을 가져오는 함수
    public static int GetShellCount(int userID, int index)
    {
        if (!IsValidUser(userID) || !IsValidShellIndex(index))
        {
            Debug.Log("INVALID USERID OR INDEX");
            return 0;
        }

        return usersCurrencyData[userID].shell[index];
    }

    // 조개패를 추가하는 함수
    public static void AddShell(int userID, int index, int amount)
    {
        if (!IsValidUser(userID) || !IsValidShellIndex(index))
        {
            Debug.Log("INVALID USERID OR INDEX");
            return;
        }

        usersCurrencyData[userID].shell[index] += amount;
        Debug.Log($"유저 {userID}에게 조개패 {index + 1}번을 {amount}개 추가했습니다. 현재 조개패 수량: {usersCurrencyData[userID].shell[index]}개");
    }

    // 유저 ID의 유효성을 확인하는 함수
    private static bool IsValidUser(int userID)
    {
        return userID >= 0 && userID < usersCurrencyData.Length;
    }

    // 조개패 인덱스의 유효성을 확인하는 함수
    private static bool IsValidShellIndex(int index)
    {
        return index >= 0 && index < 3; // 조개패 배열 크기와 동일
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
