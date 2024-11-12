using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBossEnemy : Enemy
{
    // 보스 전용 스킬 또는 패턴을 위한 변수들
    public float specialAttackCooldown = 10f; // 보스의 특수 공격 쿨다운
    private bool isSpecialAttackReady = true; // 특수 공격이 준비되었는지 여부
    private DummyEnemy _dummyEnemy;
    private MonsterData _monsterData;

    protected override void Awake()
    {
        base.Awake();
        isIndependent = true;
    }

    // 보스 전용 특수 공격 메서드
    private void SpecialAttack()
    {
        if (isSpecialAttackReady)
        {
            Debug.Log("던전보스, 특수 공격 시전");
            // 특수 공격 로직 구현

            _dummyEnemy.EnemyPlayAnimation(AnimationManager.AnimationState.ATK1);
            
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

    // 일반 공격 메서드 오버라이딩
    protected override void Attack()
    {
        base.Attack();

        // 특수 공격 발동 조건이 충족되면 특수 공격 실행
        if (isSpecialAttackReady)
        {
            SpecialAttack();
        }
    }

    protected override void Death()
    {
        Debug.Log("DungeonBossEnemy 사망.");

        // 보스 전용 사망 로직 추가 가능 (예: 보스 전용 드롭 아이템 처리)
        if (_monsterData.enemyDropData)
        {
            
        }

        base.Death();
    }
}
