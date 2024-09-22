using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class Player : MonoBehaviour
{
    private static int userID = 0;
    public static Status playerStatus;
    private static CurrencyData playerCurrency;
    private static UserLevelData playerLevelData;

    // 골드 변화 델리게이트 이벤트
    public delegate void OnGoldChangeDelegate(BigInteger newGoldVal);
    public static event OnGoldChangeDelegate OnGoldChange;


    // 경험치 변화 델리게이트 이벤트
    public delegate void OnExpChangeDelegate(UserLevelData newLevelData);
    public static event OnExpChangeDelegate OnExpChange;

    [SerializeField] private GameObject levelUpIconObject;

    public static BigInteger Gold
    {
        get { return playerCurrency.gold; }
        set
        {
            if (playerCurrency.gold == value) return;
            playerCurrency.gold = value;

            if(OnGoldChange != null)
                OnGoldChange(playerCurrency.gold);

        }
    }
    public static UserLevelData UserLevel
    {
        get
        {
            return playerLevelData;
        }
        set
        {
            if (playerLevelData.currentExp == value.currentExp 
                && playerLevelData.currentLevel == value.currentLevel) return;
            playerLevelData = value;

            if (OnExpChange != null)
                OnExpChange(playerLevelData);
        }
    }


    void Awake()
    {
        // 서버로부터 user id 받기
        userID = 0;
        
        if (playerStatus == null)
            playerStatus = new Status(userID);
        if (playerCurrency == null)
            playerCurrency = DummyServerData.GetUserCurrencyData(userID);
        if (playerLevelData == null)
            GetExpDataFromServer();
    }

    void Update()
    {
        //TODO: Test, Delete Later
        if (Input.GetKey(KeyCode.Alpha0))
        {
            Player.AddExp(0_333_333);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Player.AddExp(0_888_888);
        }
    }

    public static int GetUserID()
    {
        return userID;
    }

    public static void GetGoldDataFromServer()
    {
        Gold = DummyServerData.GetUserGoldData(userID);
        OnGoldChange(Gold);
        if(OnGoldChange != null)
            OnGoldChange(Gold);
    }

    public static void GetExpDataFromServer()
    {
        // 해당 방식으로 저장될 경우 래퍼런스 타입을 가짐
        //playerLevelData = DummyServerData.GetUserLevelData(userID);
        playerLevelData = UserLevelData.GetNewDataFromSource(DummyServerData.GetUserLevelData(userID));
        

        if (OnExpChange != null)
            OnExpChange(playerLevelData);
    }

    public static void AddExp(BigInteger addExpValue)
    {
        playerLevelData.AddExp(addExpValue);
    }


    // TODO: 임시 함수
    public void ShowLevelUpIcon()
    {
        // 대미지 출력
        UnityEngine.Vector3 spawnTransform = GameManager.GetInstance().catObject.transform.position;
        spawnTransform.z = -5;
        GameObject textObject = Instantiate(levelUpIconObject, spawnTransform ,UnityEngine.Quaternion.identity);
        Destroy(textObject, 3.0f);
    }
}
