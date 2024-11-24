using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public enum StatusLevelType
{
    HP = 0, MP, STR, DEF, HEAL_HP, HEAL_MP, CRIT, ATTACK_SPEED, GOLD, EXP, COUNT
}
[Serializable]
public class StatusLevelData
{
    public int[] statusLevels = new int[(int)StatusLevelType.COUNT];


    private int HP_DEFAULT_VALUE = 50;
    private int MP_DEFAULT_VALUE = 10;
    private int STR_DEFAULT_VALUE = 5;
    private float ATTACK_SPEED_DEFAULT_VALUE = 0.75f;
    //private static int MAX_ATTACK_SPEED = 10000;

    public StatusLevelData(int hpLevel, int mpLevel, int strLevel, int defenceLevel = 0, int healHpLevel = 0, int healMpLevel = 0, int critLevel = 0, int attackSpeedLevel = 0, int goldAcquisition = 0, int expAcquisition = 0)
    {
        statusLevels[(int)StatusLevelType.HP] = hpLevel;
        statusLevels[(int)StatusLevelType.MP] = mpLevel;
        statusLevels[(int)StatusLevelType.STR] = strLevel;
        statusLevels[(int)StatusLevelType.DEF] = defenceLevel;
        statusLevels[(int)StatusLevelType.HEAL_HP] = healHpLevel;
        statusLevels[(int)StatusLevelType.HEAL_MP] = healMpLevel;
        statusLevels[(int)StatusLevelType.CRIT] = critLevel;
        statusLevels[(int)StatusLevelType.ATTACK_SPEED] = attackSpeedLevel;
        statusLevels[(int)StatusLevelType.GOLD] = goldAcquisition;
        statusLevels[(int)StatusLevelType.EXP] = expAcquisition;
    }

    public StatusLevelData(StatusLevelData otherData)
    {
        statusLevels[(int)StatusLevelType.HP] = otherData.statusLevels[(int)StatusLevelType.HP];
        statusLevels[(int)StatusLevelType.MP] = otherData.statusLevels[(int)StatusLevelType.MP];
        statusLevels[(int)StatusLevelType.STR] = otherData.statusLevels[(int)StatusLevelType.STR];
        statusLevels[(int)StatusLevelType.DEF] = otherData.statusLevels[(int)StatusLevelType.DEF];
        statusLevels[(int)StatusLevelType.HEAL_HP] = otherData.statusLevels[(int)StatusLevelType.HEAL_HP];
        statusLevels[(int)StatusLevelType.HEAL_MP] = otherData.statusLevels[(int)StatusLevelType.HEAL_MP];
        statusLevels[(int)StatusLevelType.CRIT] = otherData.statusLevels[(int)StatusLevelType.CRIT];
        statusLevels[(int)StatusLevelType.ATTACK_SPEED] = otherData.statusLevels[(int)StatusLevelType.ATTACK_SPEED];
        statusLevels[(int)StatusLevelType.GOLD] = otherData.statusLevels[(int)StatusLevelType.GOLD];
        statusLevels[(int)StatusLevelType.EXP] = otherData.statusLevels[(int)StatusLevelType.EXP];
    }

    public float CalculateValueFromLevel(StatusLevelType type)
    {
        BigInteger value = 0;

        // 각 스탯의 레벨 -> 값 을 계산하여 반환(계산식)
        switch (type)
        {
            case StatusLevelType.HP:
                // 기본 10, 레벨당 1
                value = HP_DEFAULT_VALUE + statusLevels[(int)StatusLevelType.HP];
                break;
            case StatusLevelType.MP:
                // 기본 10, 레벨당 1
                value = MP_DEFAULT_VALUE + statusLevels[(int)StatusLevelType.MP];
                break;
            case StatusLevelType.STR:
                value = STR_DEFAULT_VALUE + statusLevels[(int)StatusLevelType.STR];
                break;
            case StatusLevelType.DEF:
                value = statusLevels[(int)StatusLevelType.DEF];
                break;
            case StatusLevelType.HEAL_HP:
                value = statusLevels[(int)StatusLevelType.HEAL_HP];
                break;
            case StatusLevelType.HEAL_MP:
                value = statusLevels[(int)StatusLevelType.HEAL_MP];
                break;
            case StatusLevelType.CRIT:
                //value = (float)statusLevels[(int)StatusLevelType.CRIT] / 100;      // 1 레벨 당 0.1%
                return (float)statusLevels[(int)StatusLevelType.CRIT] / 100;
                //break;
            case StatusLevelType.ATTACK_SPEED:
                // TODO <- 회의 필요 // 0 ~ 10000 레벨을 마스터로 1 ~ 0.25
                //value = 0.25f + Mathf.Lerp(1.0f, MAX_ATTACK_SPEED, MAX_ATTACK_SPEED - statusLevels[(int)StatusLevelType.ATTACK_SPEED]) * 0.75f;
                return ATTACK_SPEED_DEFAULT_VALUE;
                //return 1f;
                //break;
            case StatusLevelType.GOLD:
                // 기본 1%, 레벨당 0.1% 추가
                //value = 1.0f + 0.1f * statusLevels[(int)StatusLevelType.GOLD];
                return 1f;
                //break;
            case StatusLevelType.EXP:
                // 기본 1%, 레벨당 0.1% 추가
                //value = 1.0f + 0.1f * statusLevels[(int)StatusLevelType.EXP];
                return 1f;
                //break;
            default:
                Debug.Log("ERROR TYPE INPUT");
                break;
        }
        return (float)value;
    }

    public BigInteger GetLevelFromType(StatusLevelType type)
    {
        return statusLevels[(int)type];
    }

    public void AddLevel(StatusLevelType type, int value)
    {
        statusLevels[(int)type] += value;
    }

    // 적군의 스탯은 스테이지에 따라 배수로 적용 되는 상황에서 사용
    public void MultipleLevel(float mulValue)
    {
        for (int i = 0; i < (int)StatusLevelType.COUNT; ++i)
        {
            statusLevels[i] = (int)(statusLevels[i] * mulValue);
        }
    }

    public void MultipleStatusLevelByType(float mulValue, StatusLevelType type)
    {
        statusLevels[(int)type] = (int)(statusLevels[(int)type] * mulValue);
    }

    // 플레이어의 스탯의 디폴트 효과를 배로 적용시켜주는 함수
    public void BuffDefaultValue(int buffValue)
    {
        HP_DEFAULT_VALUE *= buffValue;
        MP_DEFAULT_VALUE *= buffValue;
        STR_DEFAULT_VALUE *= buffValue;
        ATTACK_SPEED_DEFAULT_VALUE = 0.25f;
    }
}

public class Status
{
    private StatusLevelData levelData;

    public bool isPlayerStatus = false;

    // 스테이터스 레벨 변화 델리게이트
    public delegate void OnStatusLevelChangeDelegate(StatusLevelType type);

    public event OnStatusLevelChangeDelegate OnStatusLevelChange;

    // 개인 스탯 (유저 / 적 적용)
    public int hp;             // 체력
    public int mp;             // 마나
    public int attackPower;    // 공격력
    public int defence;        // 방어력
    public int healHPPerSec;   // 초당 체력 회복량   
    public int healMPPerSec;   // 초당 마나 회복량
    public float critPercent; // 치명타 확률
    public float attackSpeed;    // 공격 속도(초기 1, 0.25 상한선 스탯) <- TODO: 회의 필요
    public float weaponEffect;
    public float skillAttackEffect;
    public float skillDefenceEffect;

    public Dictionary<SnackType, float> snackBuffValue = new Dictionary<SnackType, float>
    {
        { SnackType.Atk, 1.0f },
        { SnackType.Hp, 1.0f },
        { SnackType.Gold, 1.0f }
    };

    public Dictionary<StatusLevelType, int> titleOwningEffectValue = new Dictionary<StatusLevelType, int>();

    // 계정 스탯 (유저 (고양이) 적용) --> 게임메니저 관리 보류
    // 적군을 잡을 때에 받는 경우에만 해당
    public float goldAcquisitionPercent = 1.0f;    // 골드 획득량(가중치) (초기 1, value%로 적용)
    public float expAcquisitionPercent = 1.0f;     // 경험치 획득량(가중치) (초기 1, value%로 적용)
    

    public Status(StatusLevelData data)
    {
        levelData = new StatusLevelData(data);
        UpdateStatus();
    }

    public int CalculateDamage()
    {
        int damage = 0;
        // TODO: 추후 식 수정
        damage = attackPower;

        // TODO: 추후 치명타 적용

        return damage;
    }

    public StatusLevelData GetStatusLevelData()
    {
        return levelData;
    }

    public void BuffPlayerStatusDefaultValue(int buffMulValue = 5)
    {
        levelData.BuffDefaultValue(buffMulValue);
        UpdateStatus();
    }

    public void UpdateStatusLevelByType(StatusLevelType type, int newLevel)
    {
        levelData.statusLevels[(int)type] = newLevel;
        UpdateSpecificStatus(type);
    }

    // 모든 스테이터스 정보 업데이트
    private void UpdateStatus()
    {
        for (int i = 0; i < (int)StatusLevelType.COUNT; ++i)
        {
            UpdateSpecificStatus((StatusLevelType)i);
        }
    }

    public void SetActiveSnackBuff(SnackType type, bool newActive, float mulValue = 1.0f)
    {
        // 간식 버프 활성화
        if (newActive)
        {
            snackBuffValue[type] = mulValue;
        }
        // 이전 버프 적용 값 해제
        else
        {
            snackBuffValue[type] = 1.0f;
        }

        UpdateStatus();
    }

    public void SetWeaponEffect(float _attack)
    {
        weaponEffect = _attack;

        UpdateStatus();
    }

    public void SetSKillAttackEffect(float _attack)
    {
        skillAttackEffect = _attack;

        UpdateStatus();
    }

    public void SetSKillDefenceEffect(float _attack)
    {
        skillDefenceEffect = _attack;

        UpdateStatus();
    }


    public void SetTitleOwningEffect(List<TitleOwningEffect> titleOwningEffects)
    {
        titleOwningEffectValue = new Dictionary<StatusLevelType, int>();
        foreach (TitleOwningEffect effect in titleOwningEffects)
        {
            StatusLevelType levelType = Enum.Parse<StatusLevelType>(effect.type);
            if (titleOwningEffectValue.ContainsKey(levelType))
            {
                titleOwningEffectValue[levelType] += effect.value;
            }
            else
            {
                titleOwningEffectValue.Add(levelType,effect.value);
            }
        }

        UpdateStatus();
    }

    public void UpdateSpecificStatus(StatusLevelType type)
    {
        if (OnStatusLevelChange != null)
        {
            OnStatusLevelChange(type);
        }

        // 임시 업데이트
        switch (type)
        {
            case StatusLevelType.HP:
                float hpMulValue = snackBuffValue[SnackType.Hp];
                int hpAddValue = 0;
                if (titleOwningEffectValue.ContainsKey(StatusLevelType.HP))
                {
                    hpAddValue += titleOwningEffectValue[StatusLevelType.HP];
                }
                hp = (int)((levelData.CalculateValueFromLevel(StatusLevelType.HP) + hpAddValue) * hpMulValue);
                break;

            case StatusLevelType.MP:
                mp = (int)levelData.CalculateValueFromLevel(StatusLevelType.MP);
                break;
            case StatusLevelType.STR:
                float attackMulValue = snackBuffValue[SnackType.Atk] + weaponEffect + skillAttackEffect;
                int strAddValue = 0;
                if (titleOwningEffectValue.ContainsKey(StatusLevelType.STR))
                {
                    strAddValue += titleOwningEffectValue[StatusLevelType.STR];
                }
                attackPower = (int)((levelData.CalculateValueFromLevel(StatusLevelType.STR) + strAddValue) * attackMulValue);
                break;
            case StatusLevelType.DEF:
                defence = (int)levelData.CalculateValueFromLevel(StatusLevelType.DEF);
                break;
            case StatusLevelType.HEAL_HP:
                healHPPerSec = (int)levelData.CalculateValueFromLevel(StatusLevelType.HEAL_HP);
                break;
            case StatusLevelType.HEAL_MP:
                healMPPerSec = (int)levelData.CalculateValueFromLevel(StatusLevelType.HEAL_MP);
                break;
            case StatusLevelType.CRIT:
                critPercent = levelData.CalculateValueFromLevel(StatusLevelType.CRIT);
                break;
            case StatusLevelType.ATTACK_SPEED:
                attackSpeed = levelData.CalculateValueFromLevel(StatusLevelType.ATTACK_SPEED);
                break;
            case StatusLevelType.GOLD:
                float goldAddValue = 0;
                if (titleOwningEffectValue.ContainsKey(StatusLevelType.GOLD))
                {
                    goldAddValue += (float)titleOwningEffectValue[StatusLevelType.GOLD] / 100;
                }
                goldAcquisitionPercent = levelData.CalculateValueFromLevel(StatusLevelType.GOLD) + goldAddValue;
                break;
            case StatusLevelType.EXP:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        if (isPlayerStatus)
        {
            CombatPowerManager.GetInstance()
                .ChangeCurrentCombatPower(GetCurrentAttackPower(), GetCurrentDefencePower());
        }

    }

    public BigInteger GetCurrentAttackPower()
    {
        return attackPower + (int)(critPercent * 100) + (int)(10000/attackSpeed);
    }
    public BigInteger GetCurrentDefencePower()
    {
        return defence + healHPPerSec + healMPPerSec + hp;
    }
}
