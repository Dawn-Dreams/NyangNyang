using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cat : Character
{
    protected override void Awake()
    {
        //characterID = 0;
        characterID = Player.GetUserID();
        status = Player.playerStatus;

        Player.OnHPLevelChange += HPLevelChanged;
        base.Awake();
    }

    public override void InitialSettings()
    {
        base.InitialSettings();


    }

    protected override bool TakeDamage(int damage)
    {
        bool getDamaged = base.TakeDamage(damage);


        return getDamaged;
    }

    void HPLevelChanged()
    {
        BigInteger hpDifference = Player.playerStatus.hp - maxHP;
        maxHP = Player.playerStatus.hp;
        CurrentHP += hpDifference;
        
    }
    
}
