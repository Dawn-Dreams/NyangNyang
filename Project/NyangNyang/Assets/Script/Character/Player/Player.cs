
using System.Numerics;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static int userID = 0;
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

    // Shell 배열을 프로퍼티로 처리
    public static int GetShell(int index)
    {
        if (index < 0 || index >= playerCurrency.shell.Length)
        {
            Debug.LogError($"잘못된 Shell 인덱스: {index}");
            return 0;
        }
        return playerCurrency.shell[index];
    }

    public static void SetShell(int index, int value)
    {
        if (index < 0 || index >= playerCurrency.shell.Length)
        {
            Debug.LogError($"잘못된 Shell 인덱스: {index}");
            return;
        }
        playerCurrency.SetShell(index, value);
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
    public delegate void OnShellChangeDelegate(int[] newShellVal);
    public static event OnShellChangeDelegate OnShellChange;

    // 한 스테이지 내에서 반복 전투를 진행하는 것에 대한 변수
    public static bool continuousCombat = false;
    // 최대 클리어 스테이지 정보
    public static int[] playerHighestClearStageData = new int[2];

   

    public static int[] Shell
    {
        get { return playerCurrency.shell; }
        set
        {
            // 배열 비교를 위해 참조 대신 값 비교를 해야 함
            if (playerCurrency.shell.Length == value.Length)
            {
                bool isEqual = true;
                for (int i = 0; i < playerCurrency.shell.Length; i++)
                {
                    if (playerCurrency.shell[i] != value[i])
                    {
                        isEqual = false;
                        break;
                    }
                }

                if (isEqual) return;
            }

            playerCurrency.shell = (int[])value.Clone();

            if (OnShellChange != null)
                OnShellChange(playerCurrency.shell);
        }
    }

    //처음 플레이이 하는 유저일때 uid 발급
    public static void SetUserId(int uid)
    {
        //기기 내 userid저장
        PlayerPrefs.SetInt("uid", uid);
        userID = uid;
        PlayerName = "냥냥" + uid;
        //player의 초기값 채우면될듯

    }

    


    // 게임 매니저 내에서 실행
    public static void OnAwakeGetInitialDataFromServer()
    {
        userID = 0;

        GetPlayerStatusData();
        GetCurrencyData();
        GetExpData();
        
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

    public static int GetUserID()
    {
        return userID;
    }

    public static void GetPlayerStatusData()
    {
        if (playerStatus == null)
        {
            //new Status(DummyServerData.GetUserStatusLevelData(userID));
            StatusLevelData playerStatusLevelData = new StatusLevelData(0, 0, 0);
            SaveLoadManager.GetInstance().LoadPlayerStatusLevel(playerStatusLevelData);
            playerStatus = new Status(playerStatusLevelData);


            //// 플레이어는 디폴트 스탯 버프
            //playerStatus.BuffPlayerStatusDefaultValue(5);
            playerStatus.isPlayerStatus = true;
        }
    }
    public static void GetCurrencyData()
    {
        if (playerCurrency == null)
        {
            playerCurrency = ScriptableObject.CreateInstance<CurrencyData>();
        }

        CurrencyData data = ScriptableObject.CreateInstance<CurrencyData>();
        SaveLoadManager.GetInstance().LoadPlayerCurrencyData(data);
        //DummyServerData.GetUserGoldData(userID);
        playerCurrency.SetCurrencyData(data);
    }

    public static void GetExpData()
    {
        if (UserLevel == null)
        {
            playerLevelData = ScriptableObject.CreateInstance<UserLevelData>();
        }
        SaveLoadManager.GetInstance().LoadPlayerLevelData(UserLevel);
        UserLevel.ExecuteExpUpDelegate();

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

    public static void AddShells(int[] addShellValues)
    {
        
        // 티켓 값 업데이트 (예시로 티켓의 각 값을 더하는 로직 사용)
        for (int i = 0; i < playerCurrency.shell.Length && i < addShellValues.Length; i++)
        {
            playerCurrency.shell[i] += addShellValues[i];
        }

        if (OnShellChange != null)
            OnShellChange(playerCurrency.shell);
    }

    public static void UpdatePlayerStatusLevelByType(StatusLevelType type, int newValue)
    {
        playerStatus.UpdateStatusLevelByType(type, newValue);

    }

    public static void GetPlayerHighestClearStageData(out int themeData, out int stageData)
    {
        themeData = playerHighestClearStageData[0];
        stageData = playerHighestClearStageData[1];
    }

    // ================
    // ===========


    
    // =====================

}
