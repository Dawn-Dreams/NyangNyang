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

    // 티켓 변화 델리게이트 이벤트
    public delegate void OnTicketChangeDelegate(int[] newTicketVal);
    public static event OnTicketChangeDelegate OnTicketChange;

    [SerializeField] private GameObject levelUpIconObject;

    // 스테이터스 레벨 변화 델리게이트
    public delegate void OnStatusLevelChangeDelegate(StatusLevelType type);
    public static event OnStatusLevelChangeDelegate OnStatusLevelChange;

    // 스테이터스중 체력 변화 시 실행될 델리게이트 (캐릭터 체력 변경, 체력바 UI 기능 등)
    public delegate void OnHPStatusLevelChangeDelegate();
    public static event OnHPStatusLevelChangeDelegate OnHPLevelChange;

    // 한 스테이지 내에서 반복 전투를 진행하는 것에 대한 변수
    public static bool continuousCombat = true;

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

    public static int[] Ticket
    {
        get { return playerCurrency.ticket; }
        set
        {
            // 배열 비교를 위해 참조 대신 값 비교를 해야 함
            if (playerCurrency.ticket.Length == value.Length)
            {
                bool isEqual = true;
                for (int i = 0; i < playerCurrency.ticket.Length; i++)
                {
                    if (playerCurrency.ticket[i] != value[i])
                    {
                        isEqual = false;
                        break;
                    }
                }

                if (isEqual) return;
            }

            playerCurrency.ticket = (int[])value.Clone();

            if (OnTicketChange != null)
                OnTicketChange(playerCurrency.ticket);
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
            playerCurrency = ScriptableObject.CreateInstance<CurrencyData>().SetCurrencyData(DummyServerData.GetUserCurrencyData(userID));
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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 임시 모든 적군 공격 
            Character enemy = GameManager.GetInstance().catObject.enemyObject;
            if (enemy)
            {
                enemy.TakeDamage(100000, true);
            }
            
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

    public static void AddGold(BigInteger addGoldValue)
    {
        playerCurrency.RequestAddGold(addGoldValue);
        // Gold += addGoldValue;
    }

    public static void AddTickets(int[] addTicketValues)
    {
        // 서버에 티켓 추가 요청이 있을 경우 처리
        // DummyServerData.AddTicketsOnServer(userID, addTicketValues);

        // 티켓 값 업데이트 (예시로 티켓의 각 값을 더하는 로직 사용)
        for (int i = 0; i < playerCurrency.ticket.Length && i < addTicketValues.Length; i++)
        {
            playerCurrency.ticket[i] += addTicketValues[i];
        }

        if (OnTicketChange != null)
            OnTicketChange(playerCurrency.ticket);
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

    public static void UpdatePlayerStatusLevelByType(StatusLevelType type, BigInteger newValue)
    {
        playerStatus.UpdateStatusLevelByType(type, newValue);
        if (OnStatusLevelChange != null)
        {
            OnStatusLevelChange(type);
        }

        if (type == StatusLevelType.HP && OnHPLevelChange != null)
        {
            OnHPLevelChange();
        }
    }
}
