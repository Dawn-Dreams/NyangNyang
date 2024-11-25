using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
//using UnityEditor.TerrainTools;
using UnityEngine;
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
    [SerializeField] 
    private TextMeshProUGUI goldCostText;

    private int startGoldCost = 100;

    // 레벨업 배수 버튼
    public int levelUpMultiplyValue = 1;

    public void Start()
    {
        Player.playerCurrency.OnGoldChange += SetGoldCostText;
        Player.playerStatus.OnStatusLevelChange += Initialize; // += Initialize;
        Initialize(statusLevelType);

        levelUpButton.onClick.AddListener(OnClickLevelUpButton);
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
    }

    // currentStatusLevel로부터 결과 값 적용시키는 함수
    private void SetStatusValueText()
    {
        BigInteger value = Player.playerStatus.GetStatusLevelData().GetLevelFromType(statusLevelType);
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
        BigInteger currentStatusLevel = Player.playerStatus.GetStatusLevelData().statusLevels[(int)statusLevelType];
        BigInteger goldCost = CalculateGoldCost(startGoldCost, _statusLevelUpCostDict[statusLevelType], currentStatusLevel);
        if (playerGold >= goldCost)
        {
            goldCostText.color = new Color(0, 0, 255);
        }
        else
        {
            goldCostText.color = new Color(255, 0, 0);
        }
        goldCostText.text = goldCost.ToString();
    }

    void OnClickLevelUpButton()
    {
        LevelUpStatus();
    }

    void LevelUpStatus()
    {
        int currentStatusLevel = Player.playerStatus.GetStatusLevelData().statusLevels[(int)statusLevelType];
        BigInteger goldCost = CalculateGoldCost(statusLevelType, currentStatusLevel, levelUpMultiplyValue);
        if (Player.Gold >= goldCost)
        {
            Player.playerStatus.GetStatusLevelData().statusLevels[(int)statusLevelType] += levelUpMultiplyValue;

            Player.Gold -= goldCost;

            //DummyServerData.UserStatusLevelUp(Player.GetUserID(), statusLevelType, currentStatusLevel,  levelUpMultiplyValue);
            //  TODO 11.25 정보 json에 저장하는 코드 만들기
            SaveLoadManager.GetInstance().SavePlayerStatusLevel(Player.playerStatus.GetStatusLevelData(), 5.0f);

            Player.UpdatePlayerStatusLevelByType(statusLevelType, Player.playerStatus.GetStatusLevelData().statusLevels[(int)statusLevelType]);
        }
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
        statusText.text = statusData.GetStringFromType(statusLevelType);

        currentLevelText.text = 5.ToString();

        goldCostText.text = "100";
    }
}
