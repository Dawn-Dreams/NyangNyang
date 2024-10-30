using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyData", menuName = "ScriptableObjects/CurrencyData", order = 1)]
public class CurrencyData : ScriptableObject
{
    public BigInteger gold;
    public int diamond;
    public int[] ticket = {0,0,0};

    // 골드 변화 델리게이트 이벤트
    public delegate void OnGoldChangeDelegate(BigInteger newGoldVal);
    public event OnGoldChangeDelegate OnGoldChange;

    // 다이아 변화 델리게이트 이벤트
    public delegate void OnDiamondChangeDelegate(int newDiamondValue);
    public event OnDiamondChangeDelegate OnDiamondChange;

    public CurrencyData SetCurrencyData(BigInteger getGold, int getDiamond, int[] getTicket)
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

    public void SetGold(BigInteger newGold)
    {
        gold = newGold;
        if (OnGoldChange != null)
        {
            OnGoldChange(newGold);
        }
    }

    public void SetDiamond(int newDiamond)
    {
        diamond = newDiamond;
        if (OnDiamondChange != null)
        {
            OnDiamondChange(newDiamond);
        }
    }


    public void RequestAddGold(BigInteger addGoldValue)
    {
        DummyServerData.AddGoldOnServer(Player.GetUserID(), addGoldValue);
    }
}
