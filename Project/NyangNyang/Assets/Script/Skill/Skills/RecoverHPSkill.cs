using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverHPSkill : PassiveSkill
{
    public RecoverHPSkill(int _id, string _name, int _possession, int _level, int _levelUpCost)
    : base(_id, _name, _possession, _level, _levelUpCost)
    {
    }

    public override void ApplyEffect()
    {
        // �÷��̾�(�����) ü�� ȸ�� ���� �ۼ�
        // �ڷ�ƾ ����ϱ�
    }

    public override void DetachEffect()
    {
    }
}
