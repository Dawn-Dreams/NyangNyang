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
    [SerializeField] private TextMeshProUGUI diamondText;

    [SerializeField] private TextMeshProUGUI playerLevelText;

    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI expText;

    

    //[SerializeField] private Button menuButton;

    void Start()
    {
        // 골드, 다이아 변화 핸들러
        GoldTextChangeHandler(Player.Gold);
        Player.playerCurrency.OnGoldChange += GoldTextChangeHandler;
        DiamondTextChangeHandler(Player.Diamond);
        Player.playerCurrency.OnDiamondChange += DiamondTextChangeHandler;

        // 레벨(경험치) 변화 핸들러
        ExpChangeHandler(Player.UserLevel);
        Player.UserLevel.OnExpChange += ExpChangeHandler;

        

        //menuButton.onClick.AddListener(OnClickMenuButton);
    }

    private void GoldTextChangeHandler(BigInteger goldValue)
    {
        string text = MyBigIntegerMath.GetAbbreviationFromBigInteger(Player.Gold);
        if (goldText != null)
        {
            goldText.text = text;
        }
    }

    private void DiamondTextChangeHandler(int newDiamondValue)
    {
        string text = newDiamondValue.ToString();
        if (diamondText != null)
        {
            diamondText.text = text;
        }
    }

    private void ExpChangeHandler(UserLevelData userLevelData)
    {
        if (currentLevelText)
        {
            currentLevelText.text = userLevelData.currentLevel.ToString();
        }

        BigInteger currentRequireExp = UserLevelData.CalculateExp(userLevelData.currentLevel);
        if (expSlider)
        {
            // ※ BigInteger는 divide 시 정수형 divide 만 지원
            //   따라서 두 값의 앞 4자리수만 비교하는 방식으로 진행
            expSlider.value = MyBigIntegerMath.DivideToFloat(userLevelData.currentExp, currentRequireExp, 4);
        }

        if (expText)
        {
            string curExpString = MyBigIntegerMath.GetAbbreviationFromBigInteger(userLevelData.currentExp);
            string curReqString = MyBigIntegerMath.GetAbbreviationFromBigInteger(currentRequireExp);

            expText.text = curExpString + " / " + curReqString;
        }
    }

    void OnClickMenuButton()
    {
        Debug.Log("Menu Button Click");
        //NetworkManager.GetInstance().TestFunc();

        /*
         * * GetInstance는 NetworkManager 내에
         public static NetworkManager GetInstnace(){
            return instance;
        }
        입니다!
         */
    }
}
