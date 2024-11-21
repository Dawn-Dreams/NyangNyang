using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseUpSkill : PassiveSkill
{
    public DefenseUpSkill(int _id, string _name, int _possession, int _level, int _levelUpCost)
    : base(_id, _name, _possession, _level, _levelUpCost)
    {
    }

    public override void ApplyEffect()
    {
        // 플레이어(고양이) 방어력 추가
    }

    public override void DetachEffect()
    {
    }
}
