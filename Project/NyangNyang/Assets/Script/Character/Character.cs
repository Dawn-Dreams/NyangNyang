using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public Status status;
    protected bool IsEnemy = false;
    public Character enemyObject;
    public int characterID;

    protected BigInteger currentHP;
    protected BigInteger maxHP;
    protected BigInteger currentMP;

    [SerializeField]
    protected Slider healthBarSlider;
    [SerializeField]
    protected TextMeshProUGUI textMeshPro;

    // 체력 변화 델리게이트 이벤트
    public delegate void OnHealthChangeDelegate();
    public event OnHealthChangeDelegate OnHealthChange;

    // 공격 코루틴
    private Coroutine attackCoroutine;

    // 카메라 흔들림 참조
    [SerializeField]
    private CameraShake cameraShake;

    public BigInteger CurrentHP
    {
        get { return currentHP; }
        set
        {
            if (value == currentHP)
            {
                return;
            }

            currentHP = value;

            if (OnHealthChange != null)
            {
                OnHealthChange();
            }
        }
    }


    protected virtual void Awake()
    {
        InitialSettings();

        if (!cameraShake)
        {
            cameraShake = FindObjectOfType<CameraShake>();
        }
    }

    public virtual void InitialSettings()
    {
        // Status 정보 Enemy/Cat 각자에서 정보를 받은 후 Character.Awake() 실행
        // 델리게이트 연결
        OnHealthChange += ChangeHealthBar;

        // 초기화
        maxHP = status.hp;
        CurrentHP = maxHP;
        currentMP = status.mp;
        if (healthBarSlider)
        {
            healthBarSlider.value = 1;
        }
        

    }

    protected IEnumerator AttackEnemy()
    {
        while (true)
        {
            if (CombatManager.GetInstance().canFight)
            {
                //Attack();
            }
            // TODO: 공격속도 기반 전투 딜레이 적용
            yield return new WaitForSeconds(1f);
        }
    }

    protected virtual void Attack()
    {
        if (enemyObject && enemyObject.gameObject.activeSelf)
        {
            enemyObject.TakeDamage(CalculateDamage());

            if (cameraShake)
            {
                cameraShake.TriggerShake();  // 기본 흔들림
                // cameraShake.TriggerShake(0.5f, 0.8f);  // 커스텀 흔들림

            }
        }
    }

    protected virtual BigInteger CalculateDamage()
    {
        return status.CalculateDamage();
    }

    // 스킬 등 외부 클래스에서 대미지 적용 시킬 가능성이 존재하여 public 으로 변경
    public virtual BigInteger TakeDamage(BigInteger damage, bool isAOESkill = false)
    {
        if (CurrentHP <= 0) return -1;

        // TODO: 이 식도 추후 status 에서 적용
        BigInteger applyDamage = BigInteger.Max(0, damage - status.defence);
        DecreaseHp(applyDamage);
       
        return applyDamage;
    }

    protected void DecreaseHp(BigInteger applyDamage)
    {
        CurrentHP = BigInteger.Min(maxHP, BigInteger.Max(0, currentHP - applyDamage));
        
        if (IsDead())
        {
            Death();
        }
    }

    protected void ChangeHealthBar()
    {
        if (healthBarSlider)
        {
            float healthPercent = MyBigIntegerMath.DivideToFloat(currentHP,maxHP,5);
            healthBarSlider.value = healthPercent;
        }
        if (textMeshPro)
        {
            textMeshPro.SetText(MyBigIntegerMath.GetAbbreviationFromBigInteger(currentHP) + " / " + MyBigIntegerMath.GetAbbreviationFromBigInteger(maxHP));
        }
    }

    public virtual bool IsDead()
    {
        return currentHP <= 0;
    }

    public void SetEnemy(Character targetObject)
    {
        if (targetObject == null || !targetObject.gameObject.activeSelf)
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
            
            return;
        }
        enemyObject = targetObject;
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(AttackEnemy());
        }
        
    }

    protected virtual void Death()
    {
        if (enemyObject)
        {
            enemyObject.SetEnemy(null);
        }
    }

}

//Character -> Cat / Enemy
//    Cat 내부 -> StatusManager::GetPlayerStatus;
//                StatusManager 내에 계정 스탯도 같이 보유하게 ,,
//Player




