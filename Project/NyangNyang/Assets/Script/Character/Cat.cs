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

        Player.playerStatus.OnStatusLevelChange += HPLevelChanged;
        
        base.Awake();
    }

    public void HPLevelChanged(StatusLevelType type)
    {
        if (type != StatusLevelType.HP)
        {
            return;
        }
        BigInteger hpDifference = Player.playerStatus.hp - maxHP;
        maxHP = Player.playerStatus.hp;
        CurrentHP += hpDifference;
    }

    public void CatRespawn()
    {
        CurrentHP = maxHP;
        gameObject.SetActive(true);
    }

    protected override void Death()
    {
        base.Death();

        GameManager.GetInstance().stageManager.PlayerDie();
    }
}
