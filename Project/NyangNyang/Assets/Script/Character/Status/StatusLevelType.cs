using System;
using System.ComponentModel;

[Serializable]
public enum StatusLevelType
{
    [Description("최대 생명력")]
    HP = 0,
    [Description("최대 마나")]
    MP,
    [Description("공격력")]
    STR,
    [Description("체력")]
    DEF,
    [Description("체력")]
    HEAL_HP,
    [Description("체력")]
    HEAL_MP,
    [Description("체력")]
    CRIT,
    [Description("체력")]
    ATTACK_SPEED,
    [Description("체력")]
    GOLD,
    [Description("")]
    EXP, 

    COUNT
}