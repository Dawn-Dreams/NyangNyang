using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyData", menuName = "ScriptableObjects/CurrencyData", order = 1)]
public class CurrencyData : ScriptableObject
{
    public BigInteger gold;
    public BigInteger exp;

    public static string GetAbbreviationFromBigInteger(BigInteger value)
    {
        string retStr = "";

        // 단위 (a, b, c, ...)
        char unit = ' ';
        int logValue = (int)BigInteger.Log(value, 1000);
        if (logValue > 0)
        {
            unit = (char)((int)'a' - 1 + logValue);
        }

        // 수의 자리 (1, 10, 100, ...)
        BigInteger placeValue = BigInteger.Divide(value, BigInteger.Pow(1000, logValue));
        
        retStr = placeValue.ToString() + unit.ToString();
        
        return retStr;
    }

    public CurrencyData SetCurrencyData(BigInteger getGold, BigInteger getExp)
    {
        gold = getGold;
        exp = getExp;
        return this;
    }
}
