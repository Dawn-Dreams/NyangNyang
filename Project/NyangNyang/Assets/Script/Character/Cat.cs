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
        StartCoroutine(DelayedBaseAttack());    // 12.08 공격 모션 보다 대미지가 앞서 딜레이 추가
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

    // 파티클 실행 메서드
    private void SpawnParticleEffect(GameObject particlePrefab)
    {
        if (particlePrefab == null)
        {
            Debug.LogWarning("파티클 프리팹이 설정되지 않았습니다.");
            return;
        }

        // 위치 오프셋 설정 (X: 왼쪽, Y: 위, Z: 카메라 방향으로 이동)
        UnityEngine.Vector3 offset = new UnityEngine.Vector3(-2.0f, 2.0f, -10.0f);

        // 생성 위치 계산
        UnityEngine.Vector3 spawnPosition = enemyObject.gameObject.transform.position + offset;

        // 파티클 생성
        GameObject particleInstance = Instantiate(particlePrefab, spawnPosition, UnityEngine.Quaternion.identity);

        // 파티클 크기 1.5배 조정
        particleInstance.transform.localScale *= 1.5f;

        // 파티클 자동 삭제
        Destroy(particleInstance, 1f);
    }


}
