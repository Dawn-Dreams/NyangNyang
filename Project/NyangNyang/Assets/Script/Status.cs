using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Status
{
    // 개인 스탯 (유저 / 적 적용)
    public int id;             // 고유 아이디
    public int hp;             // 체력
    public int mp;             // 마나
    public int attackPower;    // 공격력
    public int defence;        // 방어력
    public int healHPPerSec;   // 초당 체력 회복량   
    public int healMPPerSec;   // 초당 마나 회복량
    public double critPercent; // 치명타 확률
    public int attackSpeed;    // 공격 속도(1 sec)
    
    // 계정 스탯 (유저 (고양이) 적용) --> 게임메니저 관리 보류
    int goldAcquisition;    // 골드 획득량(가중치)
    int expAcquisition;     // 경험치 획득량(가중치)
    int userTouchDamage;    // 터치 당 공격력

    public int CalculateDamage()
    {
        int damage = 0;
        // TODO: 추후 식 수정
        damage = attackPower;

        // TODO: 추후 치명타 적용
        
        return damage;
    }

}
