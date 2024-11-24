using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolTimeDownSkill : PassiveSkill
{
    public CoolTimeDownSkill(int _id, string _name, int _possession, int _level, int _levelUpCost, float _effect)
    : base(_id, _name, _possession, _level, _levelUpCost, _effect)
    {
    }

    public override void ApplyEffect()
    {
        // 플레이어(고양이) 쿨타임 감소
    }

    public override void DetachEffect()
    {
    }
}
