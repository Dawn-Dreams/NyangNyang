using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    /*
     * 기존 StageManager 내에서 관리하던 전투와 관련된 부분(캐릭터들의 애니메이션, 전투 시작 등) 을 담당하는 클래스
     */

    // 싱글톤 인스턴스
    private static CombatManager _instance;
    public static CombatManager GetInstance()
    {
        return _instance;
    }

    // 관문 클리어 이후 몬스터 사망 애니메이션, 아이템 드랍, 고양이 승리 모션 등이 진행될 시간
    public float gateClearWaitTime = 3.0f;

    // 두 캐릭터의 생존, 전투 지역에 도착 했을 때 전투 가능하게
    public bool canFight = false;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void CurrentEnemyDeath(Enemy currentEnemy)
    {
        canFight = false;

        // 코루틴을 통해 관문 이동에 대한 딜레이
        StageManager.GetInstance().GateClearAfterEnemyDeath(gateClearWaitTime);

        // 고양이 캐릭터 승리 애니메이션
        GameManager.GetInstance().catObject.animationManager.PlayAnimation(AnimationManager.AnimationState.Victory);
        GameManager.GetInstance().catObject.SetEnemy(null);
        
        // 아이템 드랍(currentEnemy의 monsterData.enemyDropData 참조)
        Debug.Log("적군에게서 아이템 드랍");

        // 적군 gateClearWaitTime 이후 Destroy
        EnemySpawnManager.GetInstance().DestroyEnemy(gateClearWaitTime);

        // 스폰매니져에게도 적이 죽었다는 것을 알림
        EnemySpawnManager.GetInstance().EnemyDeath(currentEnemy.initialNumOfDummyEnemy);
    }

    public void CatArriveNewGate(bool isMaxGate)
    {
        if (EnemySpawnManager.GetInstance() == null)
        {
            Debug.Log("EnemySpawnManager가 존재하지 않음");
            return;
        }

        // 적군 소환
        EnemySpawnManager.GetInstance().OnGatePassed(isMaxGate);
        
        // 고양이 IDLE 자세 설정
        GameManager.GetInstance().catObject.animationManager.PlayAnimation(AnimationManager.AnimationState.IdleA);
    }

    public void PlayerCatDeath()
    {
        canFight = false;

        GameManager.GetInstance().catObject.animationManager.PlayAnimation(AnimationManager.AnimationState.DieA);
        
        StageManager.GetInstance().PlayerDie();
    }

    public void EnemyArriveCombatArea(Enemy enemy)
    {
        canFight = true;
    }
}
