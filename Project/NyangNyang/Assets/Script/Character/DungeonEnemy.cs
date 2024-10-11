using System.Numerics;
using UnityEngine;

public class DungeonEnemy : Enemy
{
    private float attackCooldown = 1.0f; // ���� ��ٿ� �ð�
    private float lastAttackTime = 0.0f; // ������ ���� �ð�
    private Character target; // ������ ����� ����

    protected override void Awake()
    {
        base.Awake();
        SetNumberOfEnemyInGroup(1); // 1�� ����
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

    // �������̵��ؼ� DungeonEnemy Ưȭ ����� ���
    protected override BigInteger CalculateDamage()
    {
        BigInteger baseDamage = base.CalculateDamage();

        // ����� �⺻ ������ �ι�
        return baseDamage * 2;
    }
}
