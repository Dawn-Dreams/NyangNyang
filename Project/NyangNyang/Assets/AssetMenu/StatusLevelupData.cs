using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusTypeData", menuName = "ScriptableObjects/StatusTypeData", order = 1)]
public class StatusLevelupData : ScriptableObject
{
    //HP = 0, MP, STR, DEF, HEAL_HP, HEAL_MP, CRIT, ATTACK_SPEED, GOLD, EXP, COUNT
    public Sprite hp_sprite;
    public Sprite mp_sprite;
    public Sprite str_sprite;
    public Sprite def_sprite;
    public Sprite heal_hp_sprite;
    public Sprite heal_mp_sprite;
    public Sprite crit_sprite;
    public Sprite attack_speed_sprite;
    public Sprite gold_sprite;
    public Sprite exp_sprite;

    public string hp_string;
    public string mp_string;
    public string str_string;
    public string def_string;
    public string heal_hp_string;
    public string heal_mp_string;
    public string crit_string;
    public string attack_speed_string;
    public string gold_string;
    public string exp_string;
    
    public Sprite GetSpriteFromType(StatusLevelType type)
    {
        switch (type)
        {
           case StatusLevelType.HP:
               return hp_sprite;
           case StatusLevelType.MP:
               return mp_sprite;
           case StatusLevelType.STR:
               return str_sprite;
           case StatusLevelType.DEF:
               return def_sprite;
           case StatusLevelType.HEAL_HP:
               return heal_hp_sprite;
           case StatusLevelType.HEAL_MP:
               return heal_mp_sprite;
           case StatusLevelType.CRIT:
               return crit_sprite;
           case StatusLevelType.ATTACK_SPEED:
               return attack_speed_sprite;
           case StatusLevelType.GOLD:
               return gold_sprite;
           case StatusLevelType.EXP:
                return exp_sprite;
        }

        return null;
    }

    public string GetStringFromType(StatusLevelType type)
    {
        switch (type)
        {
            case StatusLevelType.HP:
                return hp_string;
            case StatusLevelType.MP:
                return mp_string;
            case StatusLevelType.STR:
                return str_string;
            case StatusLevelType.DEF:
                return def_string;
            case StatusLevelType.HEAL_HP:
                return heal_hp_string;
            case StatusLevelType.HEAL_MP:
                return heal_mp_string;
            case StatusLevelType.CRIT:
                return heal_mp_string;
            case StatusLevelType.ATTACK_SPEED:
                return attack_speed_string;
            case StatusLevelType.GOLD:
                return gold_string;
            case StatusLevelType.EXP:
                return exp_string;
        }

        return null;
    }
}
