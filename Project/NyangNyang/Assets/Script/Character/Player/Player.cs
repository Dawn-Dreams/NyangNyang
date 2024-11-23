
using System.Numerics;
using System.Security.Cryptography;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static int userID = -1;
    public static string PlayerName;

    public static Status playerStatus;

    // ===== 플레이어 재화 =====
    public static CurrencyData playerCurrency;
    public static BigInteger Gold
    {
        get => playerCurrency.gold;
        set => playerCurrency.SetGold(value);
    }

    public static int Diamond
    {
        get => playerCurrency.diamond;
        set => playerCurrency.SetDiamond(value);
    }
    
    public static int Cheese
    {
        get => playerCurrency.cheese;
        set => playerCurrency.SetCheese(value);
    }

    // 인덱스를 자주 업데이트하거나 읽을 일이 많다면 별도 프로퍼티로 관리하는 방식이 나음
    public static int Ticket1
    {
        get => playerCurrency.ticket[0];
        set => playerCurrency.ticket[0] = value;
    }

    public static int Ticket2
    {
        get => playerCurrency.ticket[1];
        set => playerCurrency.ticket[1] = value;
    }

    public static int Ticket3
    {
        get => playerCurrency.ticket[2];
        set => playerCurrency.ticket[2] = value;
    }


    // ========================
    // ===== 플레이어 경험치 =====
    private static UserLevelData playerLevelData;
    public static UserLevelData UserLevel
    {
        get => playerLevelData;
        set => playerLevelData.SetUserLevelData(value.currentLevel, value.currentExp);
    }

    // ==========================


    // 티켓 변화 델리게이트 이벤트
    public delegate void OnTicketChangeDelegate(int[] newTicketVal);
    public static event OnTicketChangeDelegate OnTicketChange;

    // 한 스테이지 내에서 반복 전투를 진행하는 것에 대한 변수
    public static bool continuousCombat = false;
    // 최대 클리어 스테이지 정보
    public static int[] playerHighestClearStageData = new int[2];

   

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

    // 게임 매니저 내에서 실행
    public static void OnAwakeGetInitialDataFromServer()
    {


        // 서버로부터 user id 받기
        userID = 0;
        if (playerStatus == null)
        {
            playerStatus = new Status(DummyServerData.GetUserStatusLevelData(userID));
            // 플레이어는 디폴트 스탯 버프
            playerStatus.BuffPlayerStatusDefaultValue(5);
            playerStatus.isPlayerStatus = true;
        }
        GetCurrencyDataFromServer();
        GetExpDataFromServer();
        
        // 서버로부터 받기
        PlayerName = "냥냥이";

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

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // 다이아 지급
            Diamond += 100;
        }
    }

    public static void SetUserLoginData(ResponseLogin res)
    {
        //로그인 성공 시 받아오는 데이터
        // status, stauslv, goods(골드,다이아,치즈),메일함
        

    }
    public static int GetUserID()
    {
        return userID;
    }
    public static void SetUserID(int uid)
    {
        //게스트로그인 - 기기저장 uid
        PlayerPrefs.SetInt("UserID", uid);
        userID = uid;
    }
    public static void GetCurrencyDataFromServer()
    {
        if (playerCurrency == null)
        {
            playerCurrency = ScriptableObject.CreateInstance<CurrencyData>();
        }
        CurrencyData data = DummyServerData.GetUserGoldData(userID);
        Gold = data.gold;
        Diamond = data.diamond;
    }

    public static void GetExpDataFromServer()
    {
        if (UserLevel == null)
        {
            playerLevelData = ScriptableObject.CreateInstance<UserLevelData>();
        }
        UserLevelData data = UserLevelData.GetNewDataFromSource(DummyServerData.GetUserLevelData(userID));
        UserLevel.SetUserLevelData(data.currentLevel, data.currentExp);

    }

    public static void AddExp(BigInteger addExpValue, bool applyExpBuff = false)
    {
        if (applyExpBuff)
        {
            addExpValue = MyBigIntegerMath.MultiplyWithFloat(addExpValue, playerStatus.expAcquisitionPercent);
        }
        playerLevelData.AddExp(addExpValue);
    }

    public static void AddGold(BigInteger addGoldValue, bool applyGoldBuff = false)
    {
        if (applyGoldBuff)
        {
            addGoldValue = MyBigIntegerMath.MultiplyWithFloat(addGoldValue, playerStatus.goldAcquisitionPercent);
        }
        Player.Gold += addGoldValue;
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

    public static void UpdatePlayerStatusLevelByType(StatusLevelType type, int newValue)
    {
        playerStatus.UpdateStatusLevelByType(type, newValue);

    }

    public static void GetPlayerHighestClearStageData(out int themeData, out int stageData)
    {
        if (playerHighestClearStageData[0] == 0)
        {
            DummyServerData.GetUserClearStageData(Player.GetUserID(), out playerHighestClearStageData[0], out playerHighestClearStageData[1]);
        }

        themeData = playerHighestClearStageData[0];
        stageData = playerHighestClearStageData[1];
    }

    // ================
    // ===========


    
    // =====================

}
