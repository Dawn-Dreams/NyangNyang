using System.Numerics;
using UnityEngine;

public class DungeonEnemy : Character
{
    private float attackCooldown = 1.0f; // 공격 쿨다운 시간
    private float lastAttackTime = 0.0f; // 마지막 공격 시간
    private int baseHealth;
    private int baseAttackPower;
    private Character target; // 공격할 대상을 저장
    public AnimationManager animationManager;
    private AddressableHandle<GameObject> dungeonBossMonsterPrefab;
    private Status _status;

    protected override void Awake()
    {
        characterID = 101;
        _status = Player.playerStatus; // 임시
        status = _status;
        base.Awake();
    }

    public void InitializeEnemyStats(int index, int level)
    {
        // index와 level에 따라 적의 생명력과 공격력을 다르게 설정
        Debug.Log("InitializeEnemyStats - index:" + index + " level:" + level);
        status.hp = CalculateHealthByIndexAndLevel(index, level);
        status.attackPower = CalculateAttackPowerByIndexAndLevel(index, level);
        attackCooldown = CalculateAttackCooldownByIndexAndLevel(index, level);
        base.Awake();
    }
    private void Update()
    {
        // 공격 대상이 설정되었고 쿨다운이 끝났다면 공격
        if (target != null && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time; // 쿨타임 타이머 초기화
            Attack();
        }

    }
    protected override void Attack()
    {
        base.Attack();

        Debug.Log("DungeonEnemy Attack:" + status.attackPower);

    }
    private int CalculateHealthByIndexAndLevel(int index, int level)
    {
        return 50 * (index + 1) * level; // 기본값 예시
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
        BigInteger baseDamage = 10;
        return baseDamage * baseAttackPower; // 계산된 공격력을 반영
    }
}
