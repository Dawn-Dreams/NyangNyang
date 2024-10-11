using System.Numerics;
using UnityEngine;

public class DungeonEnemy : Enemy
{
    private float attackCooldown = 1.0f; // 공격 쿨다운 시간
    private float lastAttackTime = 0.0f; // 마지막 공격 시간
    private int baseHealth;
    private int baseAttackPower;
    private Character target; // 공격할 대상을 저장

    protected override void Awake()
    {
        base.Awake();
        SetNumberOfEnemyInGroup(1); // 1로 고정
    }

    public void InitializeEnemyStats(int index, int level)
    {
        // index와 level에 따라 적의 생명력과 공격력을 다르게 설정
        baseHealth = CalculateHealthByIndexAndLevel(index, level);
        baseAttackPower = CalculateAttackPowerByIndexAndLevel(index, level);
        attackCooldown = CalculateAttackCooldownByIndexAndLevel(index, level);

        // 적의 기본 생명력 설정
        SetMaxHP(baseHealth);
    }
    private void Update()
    {
        // 공격 대상이 설정되었고 쿨다운이 끝났다면 공격
        if (target != null && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // 대미지 계산
        BigInteger damage = CalculateDamage();
        target.TakeDamage(damage);

        // 마지막 공격 시간 업데이트
        lastAttackTime = Time.time;

        // 공격 이펙트 또는 애니메이션 호출 등 추가 기능을 여기에 구현 가능
        Debug.Log($"DungeonEnemy가 {target.name}에게 {damage}의 피해를 주었습니다.");
    }

    private int CalculateHealthByIndexAndLevel(int index, int level)
    {
        // 생명력 계산 로직 (예: index에 따라 기본 배수 적용)
        return 10 * (index + 1) * level; // 기본값 예시
    }

    private int CalculateAttackPowerByIndexAndLevel(int index, int level)
    {
        // 공격력 계산 로직
        return 10 * (index + 1) * level; // 기본값 예시
    }

    private float CalculateAttackCooldownByIndexAndLevel(int index, int level)
    {
        // 공격 패턴(쿨타임) 계산 로직
        return Mathf.Max(0.5f, 2.0f - (level * 0.1f)); // 예: 레벨이 올라갈수록 공격이 빨라짐
    }

    // 오버라이드해서 DungeonEnemy 특화 대미지 계산
    protected override BigInteger CalculateDamage()
    {
        BigInteger baseDamage = base.CalculateDamage();
        return baseDamage * baseAttackPower; // 계산된 공격력을 반영
    }
}
