using System.Numerics;
using UnityEngine;

public class DungeonEnemy : Enemy
{
    private float attackCooldown = 1.0f; // 공격 쿨다운 시간
    private float lastAttackTime = 0.0f; // 마지막 공격 시간
    private Character target; // 공격할 대상을 저장

    protected override void Awake()
    {
        base.Awake();
        SetNumberOfEnemyInGroup(1); // 1로 고정
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

    // 오버라이드해서 DungeonEnemy 특화 대미지 계산
    protected override BigInteger CalculateDamage()
    {
        BigInteger baseDamage = base.CalculateDamage();

        // 현재는 기본 데미지 두배
        return baseDamage * 2;
    }
}
