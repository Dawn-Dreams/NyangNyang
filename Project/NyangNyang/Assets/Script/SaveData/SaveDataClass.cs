using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponInfo
{
    public int ID;
    public string name;
    public int count;
    public string grade;
    public string subGrade;
    public bool isLock;

    public float effect;
    public int level;
    public int coin;

    public string ment;
}

[Serializable]
public class SkillInfo 
{
    public int ID;
    public string name;
    public int count;
    public string type;
    public bool isLock;

    public float effect;
    public int level;
    public int coin;

    public string ment;
}

[Serializable]
public class PlayInfo
{
    public int weaponGachaCount;
    public int weaponGachaLevel;

    public int skillGachaCount;
    public int skillGachaLevel;

    public int nyangnyangCount;
    public int nyangnyangLevel;

    public int currentWeaponID;
    public int currentSkillID;
}

[Serializable]
public class WeaponData
{
    public WeaponInfo[] weapons;
}

[Serializable]
public class SkillData
{
    public SkillInfo[] skills;
}

[Serializable]
public class PlayData
{
    public PlayInfo info;
}