using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UI;
public class DungeonBossEnemy : Enemy
{
    public enum BossType
    {
        Scarecrow,       // 허수아비
        SkillOnly,       // 스킬로만 데미지
        Normal     // N초 간격 포효 스킬
    }

    [SerializeField] private BossType bossType;
    [SerializeField] private int roarInterval;
    private int bossLevel=1;
    private Coroutine roarSkillCoroutine;     // 포효 스킬 관리 코루틴

    // 보스 전용 스킬 또는 패턴을 위한 변수들
    static private float specialAttackCooldown = 5f; // 보스의 특수 공격 쿨다운
    private bool isSpecialAttackReady = true; // 특수 공격이 준비되었는지 여부

    protected override void Awake()
    {
        base.Awake();

        healthBarSlider = FindObjectOfType<Slider>();
        textMeshPro = FindObjectOfType<TextMeshProUGUI>();
        isIndependent = true;
    }

    public void InitializeBossForDungeon(int dungeonIndex, int dungeonLevel)
    {
        // 보스 유형 설정
        switch (dungeonIndex)
        {
            case 0: // Scarecrow - 기존 체력 증가폭 유지
                bossType = BossType.Scarecrow;
                maxHP = new BigInteger(1000 * (dungeonLevel + 1) * (dungeonLevel + 1));
                currentHP = maxHP;
                break;

            case 1: // SkillOnly - 체력 증가폭 작게 설정
                bossType = BossType.SkillOnly;
                maxHP = new BigInteger(500 * (dungeonLevel + 1) * (dungeonLevel + 1));
                currentHP = maxHP;
                break;

            case 2: // Normal - 중간 체력 증가폭 설정
                bossType = BossType.Normal;
                maxHP = new BigInteger(800 * (dungeonLevel + 1) * (dungeonLevel + 1));
                currentHP = maxHP;
                break;

            default:
                Debug.LogError("Invalid index for BossType");
                return;
        }


        bossLevel = dungeonLevel;
        foreach (var dummyEnemy in _dummyEnemies)
        {
            dummyEnemy.EnemyPlayAnimation(AnimationManager.AnimationState.IdleA);
        }

        Debug.Log($"보스 체력 설정 level:{bossLevel} maxHP:{maxHP}, currentHP:{currentHP}, _dummyEnemyMonsterTypes:{_dummyEnemyMonsterTypes[0]}");
    }

    // 보스 전용 특수 공격 메서드
    private void SpecialAttack()
    {
        if (isSpecialAttackReady)
        {
            BigInteger damage = CalculateDamage(bossLevel); // 던전 레벨 사용

            if (enemyObject && enemyObject.gameObject.activeSelf)
            {
                Debug.Log($"던전보스 공격 데미지:{damage}");
                enemyObject.TakeDamage(damage);
                StartCoroutine(AnimationBoss());
            }

            isSpecialAttackReady = false;
            StartCoroutine(SpecialAttackCooldown());
        }
    }

    private IEnumerator AnimationBoss()
    {
        foreach (var dummyEnemy in _dummyEnemies)
        {
            // ATK1 애니메이션 실행
            dummyEnemy.EnemyPlayAnimation(AnimationManager.AnimationState.ATK1);
        }
        // 0.5초 동안 대기
        yield return new WaitForSeconds(0.5f);

        foreach (var dummyEnemy in _dummyEnemies)
        {
            // Idle 애니메이션 실행
            dummyEnemy.EnemyPlayAnimation(AnimationManager.AnimationState.IdleA);
        }
    }


    // 특수 공격 쿨다운을 관리하는 코루틴
    private IEnumerator SpecialAttackCooldown()
    {
        yield return new WaitForSeconds(specialAttackCooldown);
        
        isSpecialAttackReady = true;
    }

    // 공격 메서드 오버라이딩
    protected override void Attack()
    {
        // 허수아비는 공격하지 않음
        if (bossType == BossType.Scarecrow)
        {
            return;
        }

        // 스킬로만 데미지를 주는 보스는 일반 공격 대신 특수 공격
        else if (bossType == BossType.SkillOnly)
        {
            SpecialAttack();
        }
        else
            base.Attack();
    }

    BigInteger CalculateDamage(int level)
    {
        BigInteger damage = base.CalculateDamage();
        BigInteger initialDamage = damage;

        // 제곱근 기반 공격력 상승 (sqrt(level))
        double multiplier = Math.Sqrt(level*0.2f);
        multiplier = Math.Round(multiplier, 5);
        damage = MyBigIntegerMath.MultiplyWithFloat(initialDamage, (float)multiplier, 5);

        
        return damage;
    }

    public override BigInteger TakeDamage(BigInteger damage, bool isAOESkill = false)
    {
        BigInteger reducedDamage = damage;  //대미지 90%만 받음 -> 수정

        currentHP -= reducedDamage;
        if (currentHP < 0)
            currentHP = 0;

        healthBarSlider.value = (float)(currentHP) / (float)(maxHP);
        textMeshPro.text = $"{currentHP} / {maxHP}";

        // 사망 처리
        if (currentHP <= 0)
        {
            //Debug.Log($"보스 사망: {gameObject.name}");
            Death();
        }

        return reducedDamage;
    }

    protected override void Death()
    {
        if (bossType == BossType.Normal && roarSkillCoroutine != null)
        {
            StopCoroutine(roarSkillCoroutine);
        }

        base.Death();
    }
}
