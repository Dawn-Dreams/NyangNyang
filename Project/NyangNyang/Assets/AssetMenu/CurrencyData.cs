using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyData", menuName = "ScriptableObjects/CurrencyData", order = 1)]
public class CurrencyData : ScriptableObject
{
    public BigInteger gold;
    public BigInteger diamond;
    public int[] ticket = {0,0,0};

    public CurrencyData SetCurrencyData(BigInteger getGold, BigInteger getDiamond, int[] getTicket)
    {
        gold = getGold;
        diamond = getDiamond;
        ticket = (int[])getTicket.Clone();
        return this;
    }

    public CurrencyData SetCurrencyData(CurrencyData otherData)
    {
        gold = otherData.gold;
        diamond = otherData.diamond;
        ticket = otherData.ticket; 
        return this;
    }

    public void RequestAddGold(BigInteger addGoldValue)
    {
        DummyServerData.AddGoldOnServer(Player.GetUserID(), addGoldValue);
    }
}
