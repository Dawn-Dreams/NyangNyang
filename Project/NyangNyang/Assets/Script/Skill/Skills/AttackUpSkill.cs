using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUpSkill : PassiveSkill
{
    public AttackUpSkill(int _id, string _name, int _possession, int _level, int _levelUpCost)
    : base(_id, _name, _possession, _level, _levelUpCost)
    {
    }

    public override void ApplyEffect()
    {
        // �÷��̾�(�����) ���ݷ� �߰�
    }

    public override void DetachEffect()
    {
    }
}
