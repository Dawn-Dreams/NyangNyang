using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyData", menuName = "ScriptableObjects/CurrencyData", order = 1)]
public class CurrencyData : ScriptableObject
{
    public System.Numerics.BigInteger gold;
    public System.Numerics.BigInteger exp;

    public CurrencyData()
    {
        gold = new System.Numerics.BigInteger(0);
        exp = new System.Numerics.BigInteger(0);
    }
    public CurrencyData(System.Numerics.BigInteger getGold, System.Numerics.BigInteger getExp)
    {
        gold = getGold;
        exp = getExp;   
    }
    
}
