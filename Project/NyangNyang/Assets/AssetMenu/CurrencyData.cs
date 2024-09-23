using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyData", menuName = "ScriptableObjects/CurrencyData", order = 1)]
public class CurrencyData : ScriptableObject
{
    public BigInteger gold;
    public BigInteger diamond;

    public CurrencyData SetCurrencyData(BigInteger getGold, BigInteger getDiamond)
    {
        gold = getGold;
        diamond = getDiamond;
        return this;
    }

    public CurrencyData SetCurrencyData(CurrencyData otherData)
    {
        gold = otherData.gold;
        diamond = otherData.diamond;
        return this;
    }

    public void RequestAddGold(BigInteger addGoldValue)
    {
        DummyServerData.AddGoldOnServer(Player.GetUserID(), addGoldValue);
    }
}
