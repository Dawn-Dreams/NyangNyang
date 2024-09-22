using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Status status;
    protected bool IsEnemy = false;
    public Character enemyObject;
    public int characterID;

    protected int currentHP;
    protected int currentMP;
    public Character()
    {
    }

    protected virtual void Awake()
    {
        InitialSettings();
        StartCoroutine(AttackEnemy());
    }

    public virtual void InitialSettings()
    {
        if (status == null)
            status = new Status(characterID, IsEnemy);

        // 초기화
        currentHP = status.hp;
        currentMP = status.mp;
    }

    IEnumerator AttackEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (enemyObject)
            {
                enemyObject.TakeDamage(status.CalculateDamage());
            }
        }
    }

    protected virtual bool TakeDamage(int damage)
    {
        if (currentHP <= 0) return false;

        // TODO: 이 식도 추후 status 에서 적용
        int applyDamage = damage - status.defence;
        currentHP = Math.Max(0, currentHP - applyDamage);

        if (currentHP <= 0)
        {
            // 사망 처리
            gameObject.SetActive(false);
        }

        return true;
    }

    public void SetEnemy(Character targetObject)
    {
        if (targetObject)
        {
            enemyObject = targetObject;
        }
    }
}

//Character -> Cat / Enemy
//    Cat 내부 -> StatusManager::GetPlayerStatus;
//                StatusManager 내에 계정 스탯도 같이 보유하게 ,,
//Player




