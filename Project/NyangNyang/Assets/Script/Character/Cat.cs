using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cat : Character
{
    public AnimationManager animationManager;

    private Coroutine _healHpOverTimeCoroutine = null;
    private float _healHPTime = 1.2f;
    [SerializeField] private GameObject particlePrefab;
    protected override void Awake()
    {
        //characterID = 0;
        characterID = Player.GetUserID();
        status = Player.playerStatus;

        Player.playerStatus.OnStatusLevelChange += HPLevelChanged;

        base.Awake();
    }

    public void HPLevelChanged(StatusLevelType type)
    {
        if (type != StatusLevelType.HP)
        {
            return;
        }
        BigInteger hpDifference = Player.playerStatus.hp - maxHP;
        maxHP = Player.playerStatus.hp;
        CurrentHP += hpDifference;
    }

    public override void SetEnemy(Character targetObject)
    {
        base.SetEnemy(targetObject);

        if (targetObject == null || !targetObject.gameObject.activeSelf)
        {
            if (_healHpOverTimeCoroutine != null)
            {
                StopCoroutine(_healHpOverTimeCoroutine);
                _healHpOverTimeCoroutine = null;
            }
            return;
        }
        if (_healHpOverTimeCoroutine == null)
        {
            _healHpOverTimeCoroutine = StartCoroutine(HealHPOverTime());
        }

    }

    protected override void Attack()
    {
        animationManager.PlayAnimation(AnimationManager.AnimationState.ATK1);
        StartCoroutine(DelayedBaseAttack());    // 12.08 ���� ��� ���� ������� �ռ� ������ �߰�
    }

    private IEnumerator DelayedBaseAttack()
    {
        yield return new WaitForSeconds(0.6f);
        SpawnParticleEffect(particlePrefab);
        base.Attack();
    }

    protected virtual IEnumerator HealHPOverTime()
    {
        while (true)
        {
            int healHP = (int)status.GetStatusLevelData().CalculateValueFromLevel(StatusLevelType.HEAL_HP);
            currentHP = BigInteger.Min(currentHP + healHP, maxHP);
            
            yield return new WaitForSeconds(_healHPTime);
        }
    }


    public void CatRespawn()
    {
        CurrentHP = maxHP;
        gameObject.SetActive(true);
    }

    protected override void Death()
    {
        if (isIndependent)
        {
            animationManager.PlayAnimation(AnimationManager.AnimationState.DieB);
            Debug.Log("CatDeathAnim");
        }
       
        base.Death();
        CombatManager.GetInstance().PlayerCatDeath();
    }

    // ��ƼŬ ���� �޼���
    private void SpawnParticleEffect(GameObject particlePrefab)
    {
        if (particlePrefab == null)
        {
            Debug.LogWarning("��ƼŬ �������� �������� �ʾҽ��ϴ�.");
            return;
        }

        // ��ġ ������ ���� (X: ����, Y: ��, Z: ī�޶� �������� �̵�)
        UnityEngine.Vector3 offset = new UnityEngine.Vector3(-2.0f, 2.0f, -10.0f);

        // ���� ��ġ ���
        UnityEngine.Vector3 spawnPosition = enemyObject.gameObject.transform.position + offset;

        // ��ƼŬ ����
        GameObject particleInstance = Instantiate(particlePrefab, spawnPosition, UnityEngine.Quaternion.identity);

        // ��ƼŬ ũ�� 1.5�� ����
        particleInstance.transform.localScale *= 1.5f;

        // ��ƼŬ �ڵ� ����
        Destroy(particleInstance, 1f);
    }


}
