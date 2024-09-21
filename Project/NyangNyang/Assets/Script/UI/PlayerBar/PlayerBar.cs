using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI playerLevelText;

    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private Slider expSlider;

    void Start()
    {
        GoldTextChangeHandler(Player.Gold);
        Player.OnGoldChange += GoldTextChangeHandler;

        ExpChangeHandler(Player.UserLevel);
        Player.OnExpChange += ExpChangeHandler;
    }

    private void GoldTextChangeHandler(BigInteger goldValue)
    {
        string text = CurrencyData.GetAbbreviationFromBigInteger(Player.Gold);
        if (goldText != null)
        {
            goldText.text = text;
        }
    }

    private void ExpChangeHandler(UserLevelData userLevelData)
    {
        if (currentLevelText)
        {
            currentLevelText.text = userLevelData.currentLevel.ToString();
        }

        if (expSlider)
        {
            BigInteger currentRequireExp = UserLevelData.CalculateExp(userLevelData.currentLevel);
            
            // ※ BigInteger는 divide 시 정수형 divide 만 지원
            //   따라서 두 값의 앞 4자리수만 비교하는 방식으로 진행

            
            // TODO : 새로운 계산식으로 변경 예정(임시코드)
            int approxCurrentExp = (int)userLevelData.currentExp;
            int approxRequireExp = (int)currentRequireExp;

            expSlider.value = (float)approxCurrentExp / approxRequireExp;
        }
        
    }
}
