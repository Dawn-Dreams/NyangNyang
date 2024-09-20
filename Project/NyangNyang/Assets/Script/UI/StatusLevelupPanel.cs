using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.TerrainTools;
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
    private Button levelUpButton;
    [SerializeField] 
    private TextMeshProUGUI goldCostText;

    private int currentStatusLevel = 1;
    private int startGoldCost;
    private float goldCostMultiplyValue;

    public void Awake()
    {
        // 서버로부터 비용 정보 받기
        startGoldCost = DummyServerData.GetStartGoldCost();
        goldCostMultiplyValue = DummyServerData.GetGoldCostMultipleValueFromType(statusLevelType);

        currentStatusLevel = DummyServerData.GetUserStatusLevelFromType(Player.GetUserID(), statusLevelType);

        SetStatusLevelText();
        SetGoldCostText();


        levelUpButton.onClick.AddListener(OnClickLevelUpButton);
    }

    int CalculateGoldCost(int startCost, float multiplyValue, int currentLevel)
    {
        float goldCost =  startCost * Mathf.Pow(multiplyValue, currentLevel - 1);
        
        return (int)goldCost;
    }

    void SetStatusLevelText()
    {
        currentLevelText.text = currentStatusLevel.ToString();
    }

    void SetGoldCostText()
    {
        int goldCost = CalculateGoldCost(startGoldCost,goldCostMultiplyValue,currentStatusLevel);
        goldCostText.text = goldCost.ToString();
    }

    void OnClickLevelUpButton()
    {
        LevelUpStatus();
    }

    void LevelUpStatus()
    {
        // TODO: 서버에서 작동되도록 구현하기
        if (DummyServerData.UserStatusLevelUp(Player.GetUserID(), statusLevelType, 1))
        {
            // TODO: 서버에서 성공 패킷을 받을 경우 실행하기
            LevelUpSuccess();
        }
    }

    // TODO: 서버에서 레벨업 성공 했을 때 받은 패킷에서 실행시킬 함수
    void LevelUpSuccess()
    {
        currentStatusLevel = DummyServerData.GetUserStatusLevelFromType(Player.GetUserID(), statusLevelType);
        SetStatusLevelText();
        SetGoldCostText();
    }


    void OnValidate()
    {
        Image statusImage = transform.Find("StatusImage").GetComponent<Image>();
        statusImage.sprite = statusData.GetSpriteFromType(statusLevelType);

        TextMeshProUGUI statusText = transform.Find("StatusTypeText").GetComponent<TextMeshProUGUI>();
        statusText.text = statusData.GetStringFromType(statusLevelType);

        currentLevelText.text = currentStatusLevel.ToString();

        goldCostText.text = DummyServerData.GetStartGoldCost().ToString();
    }
}
