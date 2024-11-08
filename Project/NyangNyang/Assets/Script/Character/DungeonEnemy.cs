using System.Numerics;
using UnityEngine;

public class DungeonEnemy : Character
{
    private float attackCooldown = 1.0f; // ���� ��ٿ� �ð�
    private float lastAttackTime = 0.0f; // ������ ���� �ð�
    private int baseHealth;
    private int baseAttackPower;
    private Character target; // ������ ����� ����
    public AnimationManager animationManager;
    private AddressableHandle<GameObject> dungeonBossMonsterPrefab;
    private Status _status;

    protected override void Awake()
    {
        characterID = 101;
        _status = Player.playerStatus; // �ӽ�
        status = _status;
        base.Awake();
    }

    public void InitializeEnemyStats(int index, int level)
    {
        // index�� level�� ���� ���� ����°� ���ݷ��� �ٸ��� ����
        Debug.Log("InitializeEnemyStats - index:" + index + " level:" + level);
        status.hp = CalculateHealthByIndexAndLevel(index, level);
        status.attackPower = CalculateAttackPowerByIndexAndLevel(index, level);
        attackCooldown = CalculateAttackCooldownByIndexAndLevel(index, level);
        base.Awake();
    }
    private void Update()
    {
        // ���� ����� �����Ǿ��� ��ٿ��� �����ٸ� ����
        if (target != null && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time; // ��Ÿ�� Ÿ�̸� �ʱ�ȭ
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
        return 50 * (index + 1) * level; // �⺻�� ����
    }

    private int CalculateAttackPowerByIndexAndLevel(int index, int level)
    {
        // ���ݷ� ��� ����
        return 10 * (index + 1) * level; // �⺻�� ����
    }

    private float CalculateAttackCooldownByIndexAndLevel(int index, int level)
    {
        // ���� ����(��Ÿ��) ��� ����
        return Mathf.Max(0.5f, 2.0f - (level * 0.1f)); // ��: ������ �ö󰥼��� ������ ������
    }

    // �������̵��ؼ� DungeonEnemy Ưȭ ����� ���
    protected override BigInteger CalculateDamage()
    {
        BigInteger baseDamage = 10;
        return baseDamage * baseAttackPower; // ���� ���ݷ��� �ݿ�
    }
}
