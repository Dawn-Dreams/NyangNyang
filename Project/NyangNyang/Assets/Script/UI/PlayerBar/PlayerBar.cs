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
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI playerLevelText;

    void Start()
    {
        GoldTextChangeHandler(Player.Gold);

        Player.OnGoldChange += GoldTextChangeHandler;

    }

    private void GoldTextChangeHandler(BigInteger goldValue)
    {
        string text = CurrencyData.GetAbbreviationFromBigInteger(Player.Gold);
        if (goldText != null)
        {
            goldText.text = text;
        }
    }
}
