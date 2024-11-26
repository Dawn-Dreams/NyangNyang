using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseUpSkill : PassiveSkill
{
    public DefenseUpSkill(int _id, string _name, int _possession, int _level, int _levelUpCost, float _effect, string _type)
    : base(_id, _name, _possession, _level, _levelUpCost, _effect, _type)
    {
    }

    public override void ApplyEffect()
    {
        Player.playerStatus.SetSKillDefenceEffect(effect);
    }

    public override void DetachEffect()
    {
        Player.playerStatus.SetSKillDefenceEffect(-effect);
    }
}
