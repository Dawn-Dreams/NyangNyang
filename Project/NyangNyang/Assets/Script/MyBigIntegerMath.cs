using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class MyBigIntegerMath : MonoBehaviour
{
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
        // 수치가 10 이하면 2자리까지, 100이하면 1자리까지 소수점 표현
        BigInteger digitBase = BigInteger.Pow(1000, logValue);
        if (logValue > 0) digitBase /= 100;

        float placeValue = (int)BigInteger.Divide(value, digitBase);
        if (logValue > 0)
        {
            placeValue /= 100;
            if (placeValue >= 100)
            {
                placeValue = Mathf.Floor(placeValue);
            }
            else if (placeValue >= 10)
            {
                placeValue = Mathf.Floor(placeValue * 10.0f) / 10.0f;
            }
        }
        retStr = placeValue.ToString() + unit.ToString();

        return retStr;
    }

}