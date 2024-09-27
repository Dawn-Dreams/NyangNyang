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

        Player.OnHPLevelChange += HPLevelChanged;
        base.Awake();
    }

    public override void InitialSettings()
    {
        base.InitialSettings();


    }


    void HPLevelChanged()
    {
        BigInteger hpDifference = Player.playerStatus.hp - maxHP;
        maxHP = Player.playerStatus.hp;
        CurrentHP += hpDifference;
        
    }
    
}
