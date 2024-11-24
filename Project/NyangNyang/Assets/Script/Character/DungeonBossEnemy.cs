using System;
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
        RoaringSkill     // N초 간격 포효 스킬
    }

    [SerializeField] private BossType bossType;
    [SerializeField] private int roarInterval;
    private Coroutine roarSkillCoroutine;     // 포효 스킬 관리 코루틴

    // 보스 전용 스킬 또는 패턴을 위한 변수들
    public float specialAttackCooldown = 10f; // 보스의 특수 공격 쿨다운
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
            case 0: bossType = BossType.Scarecrow; break;
            case 1: bossType = BossType.SkillOnly; break;
            case 2: bossType = BossType.RoaringSkill; break;
            default:
                Debug.LogError("Invalid index for BossType");
                return;
        }

        maxHP = new BigInteger(10000 * (dungeonLevel+1) * (dungeonLevel + 1));
        currentHP = maxHP;


        Debug.Log($"보스 체력 설정 완료: maxHP = {maxHP}, currentHP = {currentHP}");
    }

    private void StartRoarSkill()
    {
        if (roarSkillCoroutine != null) StopCoroutine(roarSkillCoroutine);
        roarSkillCoroutine = StartCoroutine(RoarSkill());
    }

    private IEnumerator RoarSkill()
    {
        while (true)
        {
            yield return new WaitForSeconds(roarInterval);
            Debug.Log("Roar Skill 발동!");
            // 포효 스킬 발동 로직 (예: 플레이어 기절, 스킬 데미지)
            ApplyRoarEffect();
        }
    }

    private void ApplyRoarEffect()
    {
        // 포효 스킬 효과 구현
        Debug.Log("플레이어에게 포효 스킬 효과 적용");
    }

    // 보스 전용 특수 공격 메서드
    private void SpecialAttack()
    {
        if (isSpecialAttackReady)
        {
            Debug.Log("DungeonBossEnemy 특수 공격 시전");
            // 특수 공격 로직 구현

            isSpecialAttackReady = false;
            StartCoroutine(SpecialAttackCooldown());
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
        if (bossType == BossType.SkillOnly)
        {
            SpecialAttack();
        }

        foreach (var dummyEnemy in _dummyEnemies)
        {
            dummyEnemy.EnemyPlayAnimation(AnimationManager.AnimationState.ATK1);
        }

        base.Attack();
    }
    public override BigInteger TakeDamage(BigInteger damage, bool isAOESkill = false)
    {
        BigInteger reducedDamage = damage * 9 / 10; // 대미지 90%만 받음

        currentHP -= reducedDamage;
        if (currentHP < 0)
            currentHP = 0;

        healthBarSlider.value = (float)(currentHP) / (float)(maxHP);
        textMeshPro.text = $"{currentHP} / {maxHP}";

        // 사망 처리
        if (currentHP <= 0)
        {
            Debug.Log($"보스 사망: {gameObject.name}");
            Death();
        }

        return reducedDamage;
    }

    protected override void Death()
    {
        if (bossType == BossType.RoaringSkill && roarSkillCoroutine != null)
        {
            StopCoroutine(roarSkillCoroutine);
        }
        
        base.Death();
    }
}
