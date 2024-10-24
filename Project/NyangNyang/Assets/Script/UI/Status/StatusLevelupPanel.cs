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

    private int startGoldCost;
    private int goldCostAddValue;

    // 레벨업 배수 버튼
    public int levelUpMultiplyValue = 1;

    public void Start()
    {
        // 서버로부터 비용 정보 받기
        startGoldCost = DummyServerData.GetStartGoldCost();
        goldCostAddValue = DummyServerData.GetGoldCostAddValueFromType(statusLevelType);

        Player.OnGoldChange += SetGoldCostText;
        Player.OnStatusLevelChange += Initialize;
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
        BigInteger goldCost = goldCostAddValue * (levelUpValue);

        return goldCost;
    }

    void SetStatusLevelText()
    {
        currentLevelText.text = Player.playerStatus.GetStatusLevelData().statusLevels[(int)statusLevelType].ToString();
    }

    void SetGoldCostText(BigInteger playerGold)
    {
        BigInteger currentStatusLevel = Player.playerStatus.GetStatusLevelData().statusLevels[(int)statusLevelType];
        BigInteger goldCost = CalculateGoldCost(startGoldCost,goldCostAddValue,currentStatusLevel);
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
        BigInteger currentStatusLevel = Player.playerStatus.GetStatusLevelData().statusLevels[(int)statusLevelType];
        // TODO: 서버에서 작동되도록 구현하기
        if (DummyServerData.UserStatusLevelUp(Player.GetUserID(), statusLevelType, currentStatusLevel, levelUpMultiplyValue))
        {
            // TODO: 서버에서 성공 패킷을 받을 경우 실행하기
            LevelUpSuccess();
        }
    }

    // TODO: 서버에서 레벨업 성공 했을 때 받은 패킷에서 실행시킬 함수
    void LevelUpSuccess()
    {
        Player.GetGoldDataFromServer();

        BigInteger newLevelValue = DummyServerData.GetUserStatusLevelFromType(Player.GetUserID(), statusLevelType);
        Player.UpdatePlayerStatusLevelByType(statusLevelType, newLevelValue);
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

        goldCostText.text = DummyServerData.GetStartGoldCost().ToString();
    }
}
