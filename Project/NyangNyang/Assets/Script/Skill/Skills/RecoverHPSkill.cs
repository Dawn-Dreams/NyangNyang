using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverHPSkill : PassiveSkill
{
    public RecoverHPSkill(int _id, string _name, int _possession, int _level, int _levelUpCost, float _effect)
    : base(_id, _name, _possession, _level, _levelUpCost, _effect)
    {
    }

    public override void ApplyEffect()
    {
        // 플레이어(고양이) 체력 회복 로직 작성
        // 코루틴 사용하기
    }

    public override void DetachEffect()
    {
    }
}
