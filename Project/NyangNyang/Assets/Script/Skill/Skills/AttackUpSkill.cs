using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUpSkill : PassiveSkill
{
    public AttackUpSkill(int _id, string _name, int _possession, int _level, int _levelUpCost, float _effect)
    : base(_id, _name, _possession, _level, _levelUpCost, _effect)
    {
    }

    public override void ApplyEffect()
    {
        Player.playerStatus.SetSKillAttackEffect(effect);
    }

    public override void DetachEffect()
    {
        Player.playerStatus.SetSKillAttackEffect(-effect);
    }
}
