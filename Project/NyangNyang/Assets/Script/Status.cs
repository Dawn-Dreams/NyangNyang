using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StatusLevelType
{
    HP = 0, MP, STR, DEF, HEAL_HP, HEAL_MP, CRIT, ATTACK_SPEED, GOLD, EXP
}

public class StatusLevelData
{
    public int hp = 1;
    public int mp = 1;
    public int strength = 1;
    public int defence = 1;
    public int healHP = 1;
    public int healMP = 1;
    public int crit = 1;
    public int attackSpeed = 1;

    private static int HP_DEFAULT_VALUE = 10;
    private static int MP_DEFAULT_VALUE = 10;
    private static int MAX_ATTACK_SPEED = 10000;
    
    public int goldAcquisition = 1;
    public int expAcquisition = 1;


    public float CalculateValueFromLevel(StatusLevelType type)
    {
        float value = 0;

        // 각 스탯의 레벨 -> 값 을 계산하여 반환(계산식)
        switch (type)
        {
            case StatusLevelType.HP:
                // 기본 10, 레벨당 1
                value = HP_DEFAULT_VALUE + hp;
                break;
            case StatusLevelType.MP:
                // 기본 10, 레벨당 1
                value = MP_DEFAULT_VALUE + mp;
                break;
            case StatusLevelType.STR:
                value = strength;
                break;
            case StatusLevelType.DEF:
                value = defence;
                break;
            case StatusLevelType.HEAL_HP:
                value = healHP;
                break;
            case StatusLevelType.HEAL_MP:
                value = healMP;
                break;
            case StatusLevelType.CRIT:
                value = (float)crit / 100;      // 1 레벨 당 0.1%
                break;
            case StatusLevelType.ATTACK_SPEED:
                // TODO <- 회의 필요 // 0 ~ 10000 레벨을 마스터로 1 ~ 0.25
                value = 0.25f + Mathf.Lerp(1.0f, MAX_ATTACK_SPEED, MAX_ATTACK_SPEED - attackSpeed) * 0.75f; 
                break;
            case StatusLevelType.GOLD:
                // 기본 1%, 레벨당 0.1% 추가
                value = 1.0f + 0.1f * goldAcquisition;     
                break;

            case StatusLevelType.EXP:
                // 기본 1%, 레벨당 0.1% 추가
                value = 1.0f + 0.1f * expAcquisition;
                break;

            default:
                Debug.Log("ERROR TYPE INPUT");
                break;
        }

        return value;
    }


    
}
public class Status
{
    private StatusLevelData levelData;

    // 개인 스탯 (유저 / 적 적용)
    public int hp;             // 체력
    public int mp;             // 마나
    public int attackPower;    // 공격력
    public int defence;        // 방어력
    public int healHPPerSec;   // 초당 체력 회복량   
    public int healMPPerSec;   // 초당 마나 회복량
    public float critPercent; // 치명타 확률
    public float attackSpeed;    // 공격 속도(초기 1, 0.25 상한선 스탯) <- TODO: 회의 필요
    
    // 계정 스탯 (유저 (고양이) 적용) --> 게임메니저 관리 보류
    float goldAcquisitionPercent;    // 골드 획득량(가중치) (초기 1, value%로 적용)
    float expAcquisitionPercent;     // 경험치 획득량(가중치) (초기 1, value%로 적용)
    int userTouchDamage;    // 터치 당 공격력 <- TODO: 터치 말고 다른 좋은 아이디어 있는지 회의

    public Status()
    {
        // TODO : 서버에서 StatusLevelData 받아오기 // UserID 추후 더미서버에 추가
        levelData = DummyServerData.GetUserStatusLevelData(0);

        hp = (int)levelData.CalculateValueFromLevel(StatusLevelType.HP);
        mp = (int)levelData.CalculateValueFromLevel(StatusLevelType.MP);
        attackPower = (int)levelData.CalculateValueFromLevel(StatusLevelType.STR); 
        defence = (int)levelData.CalculateValueFromLevel(StatusLevelType.DEF); 
        healHPPerSec = (int)levelData.CalculateValueFromLevel(StatusLevelType.HEAL_HP); 
        healMPPerSec = (int)levelData.CalculateValueFromLevel(StatusLevelType.HEAL_MP); 
        critPercent = levelData.CalculateValueFromLevel(StatusLevelType.CRIT); 
        attackSpeed = levelData.CalculateValueFromLevel(StatusLevelType.ATTACK_SPEED);
    }

    public int CalculateDamage()
    {
        int damage = 0;
        // TODO: 추후 식 수정
        damage = attackPower;

        // TODO: 추후 치명타 적용
        
        return damage;
    }

}
