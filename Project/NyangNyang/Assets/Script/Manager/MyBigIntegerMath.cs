using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class MyBigIntegerMath : MonoBehaviour
{
    public static string GetAbbreviationFromBigInteger(BigInteger value)
    {
        if (value == 0)
        {
            return "0";
        }
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

    public static float DivideToFloat(BigInteger dividend, BigInteger divisor, int precisionDigit)
    {
        // BigInteger.Divide 는 BigInteger만 반환하므로,
        // slider 등에서 사용하기 위해 float를 반환하는 함수 제작
        // dividend와 divisor 의 앞 {precisionDigit} 자리 만큼의 숫자들 끼리만 연산을 진행
        if (precisionDigit <= 0) precisionDigit = 1;
        if (precisionDigit >= 6) precisionDigit = 6;

        string dividendStr = dividend.ToString();
        string divisorStr = divisor.ToString();

        // 자릿수 맞추기
        int lengthMax = Mathf.Max(dividendStr.Length, divisorStr.Length);
        precisionDigit = Mathf.Min(lengthMax, precisionDigit);
        if (dividendStr.Length < lengthMax)
        {
            int count = lengthMax - dividendStr.Length;
            for (int index = 0; index < count; ++index)
            {
                dividendStr = "0" + dividendStr;
            }
        }

        if (divisorStr.Length < lengthMax)
        {
            int count = lengthMax - divisorStr.Length;
            for (int index = 0; index < count; ++index)
            {
                divisorStr = "0" + divisorStr;
            }
        }

        int dividendInt = int.Parse(dividendStr.Substring(0, precisionDigit));
        int divisorInt = int.Parse(divisorStr.Substring(0, precisionDigit));

        if (divisorInt == 0)
        {
            return 0.0f;
        }

        return (float)dividendInt / divisorInt;
    }

    public static BigInteger MultiplyWithFloat(BigInteger left, float right, int precisionDigit = 5)
    {
        // BigInteger.Multiply는 큰 수 * float로 인해 float에 대한 곱셈은 존재하지않음
        // 따라서 근삿값을 구할 수 있도록 left 의 앞 {precisionDigit} 자리 만큼의 숫자를 float와 곱셈 후
        // 다시 자릿수를 맞추어 반환하여 float 연산이 가능하도록 진행

        precisionDigit = Mathf.Clamp(precisionDigit, 1, 10);

        string leftStr = left.ToString();
        int initialBigIntegerDigit = leftStr.Length;

        if (precisionDigit > leftStr.Length)
        {
            precisionDigit = leftStr.Length;
            initialBigIntegerDigit = 0;
        }
        else
        {
            initialBigIntegerDigit -= precisionDigit;
        }

        int leftInt = int.Parse(leftStr.Substring(0, precisionDigit));

        leftInt = (int)(leftInt * right);

        BigInteger returnValue = leftInt * BigInteger.Pow(10, initialBigIntegerDigit);

        return returnValue;
    }
}