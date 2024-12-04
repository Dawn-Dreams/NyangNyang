using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
//using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatusLevelupPanel : MonoBehaviour
{
    // 스탯 별 레벨에 따른 추가되는 골드에 대한 dictionary
    private static Dictionary<StatusLevelType, int> _statusLevelUpCostDict = new Dictionary<StatusLevelType, int>
    {
        { StatusLevelType.HP, 100 }, { StatusLevelType.MP, 100 },
        { StatusLevelType.STR, 100 }, { StatusLevelType.DEF, 100 },
        { StatusLevelType.HEAL_HP, 300 }, { StatusLevelType.HEAL_MP, 300 },
        { StatusLevelType.CRIT, 50000 }, { StatusLevelType.ATTACK_SPEED, 10000 },
        { StatusLevelType.GOLD, 100000 }, { StatusLevelType.EXP, 100000 }
    };

    private static int _attackSpeedMaxLevel = 50;

[SerializeField]
    private StatusLevelType statusLevelType;
    [SerializeField] 
    private StatusLevelupData statusData;
    [SerializeField]
    private TextMeshProUGUI currentLevelText;

    [SerializeField] 
    private TextMeshProUGUI statusValueText;

    [SerializeField] 
    private Button levelUpButton;
    private MyHoldButton _holdButton;
    private Coroutine _levelUpCoroutine = null;

    [SerializeField] 
    private TextMeshProUGUI goldCostText;

    private int startGoldCost = 100;

    // 레벨업 배수 버튼
    public int levelUpMultiplyValue = 1;

    public void Start()
    {
        Player.playerCurrency.OnGoldChange += SetGoldCostText;
        Player.playerStatus.OnStatusLevelChange += Initialize; // += Initialize;
        _holdButton = levelUpButton.gameObject.GetComponent<MyHoldButton>();
        _holdButton.onPressStartEvent = OnClickLevelUpButton;
        Initialize(statusLevelType);

        //levelUpButton.onClick.AddListener(OnClickLevelUpButton);
    }



    void Initialize(StatusLevelType type)
    {
        if (type != statusLevelType)
        {
            return;
        }

        SetGoldCostText(Player.Gold);
        SetStatusLevelText();
        SetStatusValueText();
        CheckIsMaxLevel();
    }

    private bool CheckIsMaxLevel()
    {
        if (statusLevelType == StatusLevelType.ATTACK_SPEED && Player.playerStatus.GetStatusLevelData().statusLevels[(int)statusLevelType] >= StatusLevelData.MAX_ATTACK_SPEED_LEVEL)
        {
            _holdButton.isPressed = false;
            levelUpButton.interactable = false;
            return true;
        }

        return false;
    }

    // currentStatusLevel로부터 결과 값 적용시키는 함수
    private void SetStatusValueText()
    {
        //BigInteger value = ;//.GetLevelFromType(statusLevelType);
        float value = Player.playerStatus.GetStatusLevelData().CalculateValueFromLevel(statusLevelType);
        if (statusLevelType == StatusLevelType.ATTACK_SPEED)
        {
            value = (float)Math.Truncate(value * 100)/100;
        }
        
        
        statusValueText.text = value.ToString();
        
    }

    BigInteger CalculateGoldCost(int startCost, float multiplyValue, BigInteger currentLevel)
    {
        // n ~ m 레벨 계산 ((n부터 m까지의 갯수) * (n+m) / 2 )
        BigInteger levelUpValue = (levelUpMultiplyValue) * (currentLevel + (currentLevel + levelUpMultiplyValue)) / 2;
        BigInteger goldCost = _statusLevelUpCostDict[statusLevelType] * (levelUpValue);

        return goldCost;
    }

    void SetStatusLevelText()
    {
        currentLevelText.text = Player.playerStatus.GetStatusLevelData().statusLevels[(int)statusLevelType].ToString();
    }

    void SetGoldCostText(BigInteger playerGold)
    {
        if (CheckIsMaxLevel())
        {
            goldCostText.text = "최대 레벨";
            goldCostText.color = Color.black;
            return;
        }

        BigInteger currentStatusLevel = Player.playerStatus.GetStatusLevelData().statusLevels[(int)statusLevelType];
        BigInteger goldCost = CalculateGoldCost(startGoldCost, _statusLevelUpCostDict[statusLevelType], currentStatusLevel);
        string goldCostString = MyBigIntegerMath.GetAbbreviationFromBigInteger(goldCost);
        if (playerGold >= goldCost)
        {
            goldCostText.color = new Color(0, 0, 255);
        }
        else
        {
            goldCostText.color = new Color(255, 0, 0);
        }
        goldCostText.text = goldCostString;
    }

    void OnClickLevelUpButton()
    {
        if (_levelUpCoroutine == null && !CheckIsMaxLevel())
        {
            _levelUpCoroutine = StartCoroutine(LevelUpStatus());
        }
    }
    IEnumerator LevelUpStatus()
    {
        int pressTime = 0;
        while (true)
        {
            int currentStatusLevel = Player.playerStatus.GetStatusLevelData().statusLevels[(int)statusLevelType] + pressTime;
            int canLevelUpMultiplyValue = CalcCanLevelUpMultiplyValue(levelUpMultiplyValue);
            BigInteger goldCost = CalculateGoldCost(statusLevelType, currentStatusLevel, canLevelUpMultiplyValue);
            if (Player.Gold >= goldCost && _holdButton.isPressed && !IsStatusMaxLevel())
            {
                Player.playerStatus.GetStatusLevelData().statusLevels[(int)statusLevelType] += canLevelUpMultiplyValue;
                Player.Gold -= goldCost;
                pressTime += 1;
                int statusLevel = (Player.playerStatus.GetStatusLevelData().statusLevels[(int)statusLevelType]);
                SetStatusValueText();
                currentLevelText.text = statusLevel.ToString();
            }
            else
            {
                // 유저가 버튼을 뗐을 경우 or 골드 부족 할 경우 발생

                if (pressTime > 0)
                {
                    SaveLoadManager.GetInstance().SavePlayerStatusLevel(Player.playerStatus.GetStatusLevelData());
                    Player.UpdatePlayerStatusLevelByType(statusLevelType, Player.playerStatus.GetStatusLevelData().statusLevels[(int)statusLevelType]);
                }
                _holdButton.isPressed = false;
                _levelUpCoroutine = null;
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }

    }

    private int CalcCanLevelUpMultiplyValue(int initLevelUpMulValue)
    {
        // 공격속도
        if (statusLevelType == StatusLevelType.ATTACK_SPEED &&
            Player.playerStatus.GetStatusLevelData().statusLevels[(int)statusLevelType] + initLevelUpMulValue >=
            StatusLevelData.MAX_ATTACK_SPEED_LEVEL)
        {
            return StatusLevelData.MAX_ATTACK_SPEED_LEVEL -
                   Player.playerStatus.GetStatusLevelData().statusLevels[(int)statusLevelType];
        }

        return initLevelUpMulValue;
    }

    bool IsStatusMaxLevel()
    {
        // 현재 스탯이 최고 레벨인지 파악하는 함수

        // 공격속도
        if (statusLevelType == StatusLevelType.ATTACK_SPEED &&
            Player.playerStatus.GetStatusLevelData().statusLevels[(int)statusLevelType] >=
            StatusLevelData.MAX_ATTACK_SPEED_LEVEL)
        {
            return true;
        }

        return false;
    }
    public static BigInteger CalculateGoldCost(StatusLevelType type, BigInteger currentLevel, int levelUpMultiplyValue)
    {
        int goldAddValue = _statusLevelUpCostDict[type];
        // n ~ m 레벨 계산 ((n부터 m까지의 갯수) * (n+m) / 2 )
        BigInteger levelUpValue = (levelUpMultiplyValue) * (currentLevel + (currentLevel + levelUpMultiplyValue)) / 2;
        BigInteger goldCost = goldAddValue * (levelUpValue);

        return goldCost;
    }

    public void ChangeMultiplyValue(int newValue)
    {
        levelUpMultiplyValue = newValue;
        SetGoldCostText(Player.Gold);
    }

    void OnValidate()
    {
        Image statusImage = transform.Find("StatusImage").GetComponent<Image>();
        statusImage.sprite = statusData.GetSpriteFromType(statusLevelType);

        TextMeshProUGUI statusText = transform.Find("StatusTypeText").GetComponent<TextMeshProUGUI>();
        statusText.text = //statusData.GetStringFromType(statusLevelType);
            EnumTranslator.GetStatusTypeText(statusLevelType);

        currentLevelText.text = 5.ToString();

        goldCostText.text = "100";
    }
}
