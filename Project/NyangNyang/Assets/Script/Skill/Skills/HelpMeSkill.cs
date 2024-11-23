using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMeSkill : ActiveSkill
{
    public HelpMeSkill(int _id, string _name, int _possession, int _level, int _levelUpCost)
    : base(_id, _name, _possession, _level, _levelUpCost)
    {
    }

    public override void ApplyEffect()
    {
        // 플레이어(고양이) 체력 회복 로직 작성
        // 코루틴 사용하기
    }
}
