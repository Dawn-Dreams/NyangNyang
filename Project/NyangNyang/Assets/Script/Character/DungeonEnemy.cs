using System.Numerics;
using UnityEngine;

public class DungeonEnemy : Enemy
{
    private float attackCooldown = 1.0f; // ���� ��ٿ� �ð�
    private float lastAttackTime = 0.0f; // ������ ���� �ð�
    private int baseHealth;
    private int baseAttackPower;
    private Character target; // ������ ����� ����

    protected override void Awake()
    {
        base.Awake();
        SetNumberOfEnemyInGroup(1); // 1�� ����
    }

    public void InitializeEnemyStats(int index, int level)
    {
        // index�� level�� ���� ���� ����°� ���ݷ��� �ٸ��� ����
        baseHealth = CalculateHealthByIndexAndLevel(index, level);
        baseAttackPower = CalculateAttackPowerByIndexAndLevel(index, level);
        attackCooldown = CalculateAttackCooldownByIndexAndLevel(index, level);

        // ���� �⺻ ����� ����
        SetMaxHP(baseHealth);
    }
    private void Update()
    {
        // ���� ����� �����Ǿ��� ��ٿ��� �����ٸ� ����
        if (target != null && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // ����� ���
        BigInteger damage = CalculateDamage();
        target.TakeDamage(damage);

        // ������ ���� �ð� ������Ʈ
        lastAttackTime = Time.time;

        // ���� ����Ʈ �Ǵ� �ִϸ��̼� ȣ�� �� �߰� ����� ���⿡ ���� ����
        Debug.Log($"DungeonEnemy�� {target.name}���� {damage}�� ���ظ� �־����ϴ�.");
    }

    private int CalculateHealthByIndexAndLevel(int index, int level)
    {
        // ����� ��� ���� (��: index�� ���� �⺻ ��� ����)
        return 10 * (index + 1) * level; // �⺻�� ����
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
        BigInteger baseDamage = base.CalculateDamage();
        return baseDamage * baseAttackPower; // ���� ���ݷ��� �ݿ�
    }
}
