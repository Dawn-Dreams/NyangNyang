using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using static DungeonBossEnemy;

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
        isIndependent = true;
    }

    public void InitializeEnemyStats(int index, int level)
    {
        switch (index)
        {
            case 0:
                bossType = BossType.Scarecrow;
                break;
            case 1:
                bossType = BossType.SkillOnly;
                break;
            case 2:
                bossType = BossType.RoaringSkill;
                break;
            default:
                Debug.LogError("Invalid index for BossType");
                return;
        }

        Debug.Log($"BossType:{bossType} / index:{index}");

        // level에 따라 스탯 초기화
        switch (bossType)
        {
            case BossType.Scarecrow:
                //health = 100 + (level * 10); // 허수아비 체력 증가
                break;

            case BossType.SkillOnly:
                //health = 200 + (level * 20); // 스킬 전용 보스 체력 증가
                //attackDamage = 50 + (level * 5); // 스킬 전용 보스 공격력 증가
                break;

            case BossType.RoaringSkill:
                //health = 300 + (level * 30); // 포효 스킬 보스 체력 증가
                //attackDamage = 40 + (level * 4); // 포효 스킬 보스 공격력 증가
                //roarInterval = Mathf.Max(1, 10 - level); // 레벨이 올라갈수록 포효 간격 짧아짐
                break;
        }

        //Debug.Log($"Stats initialized: Health = {health}, AttackDamage = {attackDamage}, RoarInterval = {roarInterval}");
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
            Debug.Log("허수아비 보스는 공격하지 않음");
            return;
        }

        base.Attack();

        // 스킬로만 데미지를 주는 보스는 일반 공격 대신 특수 공격
        if (bossType == BossType.SkillOnly)
        {
            SpecialAttack();
        }
    }

    protected override void Death()
    {
        Debug.Log($"DungeonBossEnemy({bossType}) 사망");
        if (bossType == BossType.RoaringSkill && roarSkillCoroutine != null)
        {
            StopCoroutine(roarSkillCoroutine);
        }

        // 보스 전용 사망 로직 추가 가능 (예: 보스 전용 드롭 아이템 처리)
        base.Death();
    }
}
