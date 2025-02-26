﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
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
    private int bossLevel = 1;
    private Coroutine roarSkillCoroutine;     // 포효 스킬 관리 코루틴

    // 보스 전용 스킬 또는 패턴을 위한 변수들
    static private float specialAttackCooldown = 5f; // 보스의 특수 공격 쿨다운
    private bool isSpecialAttackReady = true; // 특수 공격이 준비되었는지 여부
    // 보스 유형별 파티클 프리팹
    [SerializeField] private GameObject particlePrefab;

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

    // 2번 보스 전용 특수 공격 메서드
    private void SkillBossAttack()
    {
        if (isSpecialAttackReady)
        {
            BigInteger damage = CalculateDamage(bossLevel); // 던전 레벨 사용
            if (enemyObject && enemyObject.gameObject.activeSelf)
            {
                Debug.Log($"던전보스 공격 데미지:{damage}");
                enemyObject.TakeDamage(damage);
                SpawnParticleEffect(particlePrefab);
                StartCoroutine(AnimationBoss(AnimationManager.AnimationState.ATK1));
            }

            isSpecialAttackReady = false;
            StartCoroutine(SpecialAttackCooldown());
        }
    }

    private IEnumerator AnimationBoss(AnimationManager.AnimationState anim)
    {
        foreach (var dummyEnemy in _dummyEnemies)
        {
            // ATK1 애니메이션 실행
            dummyEnemy.EnemyPlayAnimation(anim);
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

    // 3번 보스 전용 특수 공격 메서드
    private void NormalBossAttack()
    {
        BigInteger damage = CalculateDamage(bossLevel); // 던전 레벨 사용
        if (enemyObject && enemyObject.gameObject.activeSelf)
        {
            Debug.Log($"던전보스 공격 데미지:{damage}");
            enemyObject.TakeDamage(damage);
           
            SpawnParticleEffect(particlePrefab);
            StartCoroutine(AnimationBoss(AnimationManager.AnimationState.ATK1));
        }
    }

    // 파티클 실행 메서드
    private void SpawnParticleEffect(GameObject particlePrefab)
    {
        if (particlePrefab == null)
        {
            Debug.LogWarning("파티클 프리팹이 설정되지 않았습니다.");
            return;
        }

        // 현재 적으로 설정된 고양이 위치에서 파티클 생성
        GameObject particleInstance = Instantiate(particlePrefab, enemyObject.gameObject.transform.position, UnityEngine.Quaternion.identity);

        // 파티클 자동 삭제
        Destroy(particleInstance, 1f);
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
            SkillBossAttack();
        }
        else
            NormalBossAttack();

        Debug.Log($"Cat HP :{enemyObject.CurrentHP}");
    }

    BigInteger CalculateDamage(int level)
    {
        BigInteger damage = base.CalculateDamage();
        BigInteger initialDamage = damage;

        // 제곱근 기반 공격력 상승 (sqrt(level))
        double multiplier;
        if(bossType == BossType.SkillOnly)
            multiplier = Math.Sqrt(level * 0.5f);
        else if (bossType == BossType.Normal)
            multiplier = Math.Sqrt(level * 0.3f);
        else
            multiplier = 0;

        multiplier = Math.Round(multiplier, 5);
        damage = MyBigIntegerMath.MultiplyWithFloat(initialDamage, (float)multiplier, 5);


        return damage;
    }

    public override BigInteger TakeDamage(BigInteger damage, bool isAOESkill = false)
    {
        BigInteger reducedDamage = damage;  //대미지 90%만 받음 -> 수정

        StartCoroutine(AnimationBoss(AnimationManager.AnimationState.Damage));

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
}
