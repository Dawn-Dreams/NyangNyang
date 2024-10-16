using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDropData", menuName = "ScriptableObjects/EnemyDropData", order = 1)]
public class EnemyDropData : ScriptableObject
{
    private BigInteger _gold;
    private BigInteger _exp;

    //TODO: 장비 등 기타 드랍 아이템이 있을 경우 해당 클래스에 추가

    public EnemyDropData SetEnemyDropData(BigInteger getGold, BigInteger getExp)
    {
        _gold = getGold;
        _exp = getExp;

        return this;
    }

    public EnemyDropData SetEnemyDropData(EnemyDropData otherData)
    {
        _gold = otherData._gold;
        _exp = otherData._exp;

        return this;
    }

    void GetDataFromServer(int enemyID)
    {
        
    }

    public void GiveItemToPlayer()
    {
        Player.AddGold(_gold);
        Player.AddExp(_exp);

        _gold = 0;
        _exp = 0;
    }

    public void MulDropData(float mulValue)
    {
        _gold = MyBigIntegerMath.MultiplyWithFloat(_gold, mulValue, 5);
        _exp = MyBigIntegerMath.MultiplyWithFloat(_exp, mulValue, 5);
    }
}
