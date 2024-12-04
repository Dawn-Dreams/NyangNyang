using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpSkill : PassiveSkill
{
    public HealthUpSkill(int _id, string _name, int _count, string _type, bool _isLock, float _effect, int _level, int _coin, string _ment)
        : base(_id, _name, _count, _type, _isLock, _effect, _level, _coin, _ment)
    {

    }

    public override void ApplyEffect()
    {
        Player.playerStatus.SetSKillHPEffect(effect);
    }

    public override void DetachEffect()
    {
        Player.playerStatus.SetSKillHPEffect(-effect);
    }
}
