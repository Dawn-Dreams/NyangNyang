using System;
using System.ComponentModel;

[Serializable]
public enum StatusLevelType
{
    HP = 0,
    MP,
    STR,
    DEF,
    HEAL_HP,
    HEAL_MP,
    CRIT,
    ATTACK_SPEED,
    GOLD,
    EXP, 

    COUNT
}