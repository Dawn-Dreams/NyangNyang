using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using static CurrencyData;

[CreateAssetMenu(fileName = "CurrencyData", menuName = "ScriptableObjects/CurrencyData", order = 1)]
public class CurrencyData : ScriptableObject
{
    public BigInteger gold;
    public int diamond;
    public int cheese;
    public int[] shell = {0,0,0};

    // 골드 변화 델리게이트 이벤트
    public delegate void OnGoldChangeDelegate(BigInteger newGoldVal);
    public event OnGoldChangeDelegate OnGoldChange;

    public delegate void OnSpendingGoldDelegate(BigInteger newSpendingGoldVal);
    public event OnSpendingGoldDelegate OnSpendingGold;

    // 다이아 변화 델리게이트 이벤트
    public delegate void OnDiamondChangeDelegate(int newDiamondValue);
    public event OnDiamondChangeDelegate OnDiamondChange;
    
    // 치즈 변화 델리게이트 이벤트
    public delegate void OnCheeseChangeDelegate(int newCheeseValue);
    public event OnCheeseChangeDelegate OnCheeseChange;

    public CurrencyData SetCurrencyData(BigInteger getGold, int getDiamond, int getCheese, int[] getShell)
    {
        gold = getGold;
        diamond = getDiamond;
        cheese = getCheese;
        shell = (int[])getShell.Clone();
        return this;
    }

    public CurrencyData SetCurrencyData(CurrencyData otherData)
    {
        gold = otherData.gold;
        diamond = otherData.diamond;
        cheese = otherData.cheese;
        shell = otherData.shell; 
        return this;
    }

    public void SetGold(BigInteger newGold)
    {
        // 골드를 사용한 상황
        if (gold >= newGold)
        {
            BigInteger spendingGoldVal = gold - newGold;
            if (OnSpendingGold != null)
            {
                OnSpendingGold(spendingGoldVal);
            }
        }
        
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

     public void SetCheese(int newCheese)
    {
        cheese = newCheese;
        if (OnCheeseChange != null)
        {
            OnCheeseChange(newCheese);
        }
    }

}
