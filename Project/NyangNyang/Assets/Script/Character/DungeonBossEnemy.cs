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
        RoaringSkill     // N초 간격 포효 스킬
    }

    [SerializeField] private BossType bossType;
    [SerializeField] private int roarInterval;
    private int bossLevel;
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
                _monsterData.monsterTypes[0] = EnemyMonsterType.FireGolem;
                _dummyEnemyMonsterTypes[0] = EnemyMonsterType.FireGolem;
                maxHP = new BigInteger(1000 * (dungeonLevel + 1) * (dungeonLevel + 1));
                currentHP = maxHP;
                break;

            case 1: // SkillOnly - 체력 증가폭 작게 설정
                bossType = BossType.SkillOnly;
                _monsterData.monsterTypes[0] = EnemyMonsterType.IceBear;
                _dummyEnemyMonsterTypes[0] = EnemyMonsterType.IceBear;
                maxHP = new BigInteger(500 * (dungeonLevel + 1) * (dungeonLevel + 1));
                currentHP = maxHP;
                break;

            case 2: // RoaringSkill - 중간 체력 증가폭 설정
                bossType = BossType.RoaringSkill;
                _monsterData.monsterTypes[0] = EnemyMonsterType.MegaGolem;
                _dummyEnemyMonsterTypes[0] = EnemyMonsterType.MegaGolem;
                maxHP = new BigInteger(800 * (dungeonLevel + 1) * (dungeonLevel + 1));
                currentHP = maxHP;
                break;

            default:
                Debug.LogError("Invalid index for BossType");
                return;
        }


        bossLevel = dungeonLevel;
        

        Debug.Log($"보스 체력 설정 level:{bossLevel} maxHP:{maxHP}, currentHP:{currentHP}, _dummyEnemyMonsterTypes:{_dummyEnemyMonsterTypes[0]}");
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
            BigInteger damage = CalculateDamage(bossLevel); // 던전 레벨 사용

            if (enemyObject && enemyObject.gameObject.activeSelf)
            {
                enemyObject.TakeDamage(damage);
            }

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

        Debug.Log($"레벨:{level}, 데미지 멀티플라이어:{multiplier}, 결과 데미지:{damage}");
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
        if (bossType == BossType.RoaringSkill && roarSkillCoroutine != null)
        {
            StopCoroutine(roarSkillCoroutine);
        }

        base.Death();
    }
}
