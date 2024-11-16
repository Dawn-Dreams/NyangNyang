using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using static DungeonBossEnemy;

public class DungeonBossEnemy : Enemy
{
    public enum BossType
    {
        Scarecrow,       // 허수아비
        SkillOnly,       // 스킬로만 데미지
        RoaringSkill     // N초 간격 포효 스킬
    }

    [SerializeField] private BossType bossType;
    [SerializeField] private int roarInterval;
    private Coroutine roarSkillCoroutine;     // 포효 스킬 관리 코루틴

    // 보스 전용 스킬 또는 패턴을 위한 변수들
    public float specialAttackCooldown = 10f; // 보스의 특수 공격 쿨다운
    private bool isSpecialAttackReady = true; // 특수 공격이 준비되었는지 여부

    protected override void Awake()
    {
        base.Awake();

        // 필요한 오브젝트 및 슬라이더를 할당
        GameObject dummyObject = GameObject.Find("DummyObjectName");
        Slider slider = FindObjectOfType<Slider>();

        if (dummyObject == null)
        {
            Debug.LogWarning("DungeonBossEnemy 초기화: DummyObjectName을 찾을 수 없어 기본 설정으로 진행합니다.");
            dummyObject = new GameObject("DefaultDummyObject");
        }

        if (slider == null)
        {
            Debug.LogWarning("DungeonBossEnemy 초기화: Slider를 찾을 수 없어 기본 슬라이더를 생성합니다.");
            GameObject sliderObject = new GameObject("DefaultSlider");
            slider = sliderObject.AddComponent<Slider>();
        }
       
        isIndependent = true;
    }

    private Status bossStatus;

    public void InitializeBossForDungeon(int dungeonIndex, int dungeonLevel)
    {
        // 보스 유형 설정
        switch (dungeonIndex)
        {
            case 0: bossType = BossType.Scarecrow; break;
            case 1: bossType = BossType.SkillOnly; break;
            case 2: bossType = BossType.RoaringSkill; break;
            default:
                Debug.LogError("Invalid index for BossType");
                return;
        }

        // 보스 데이터 로드
        BossMonsterData bossData = BossMonsterDataManager.GetBossDataByType(bossType);
        if (bossData == null)
        {
            Debug.LogWarning("MonsterData could not be loaded. Using default values.");
            bossData = ScriptableObject.CreateInstance<BossMonsterData>();
            bossData.baseHP = 5000;       // 기본값 예시
            bossData.baseAttack = 200;
            bossData.baseDefense = 100;
        }

        int hpMultiplier = dungeonLevel * 1000;   // 레벨에 따라 HP 증가
        int attackMultiplier = dungeonLevel * 50; // 레벨에 따라 공격력 증가
        int defenseMultiplier = dungeonLevel * 20; // 레벨에 따라 방어력 증가

        maxHP = bossData.baseHP + hpMultiplier;           // 기본 체력 + 레벨 보정
        status.hp = (int)maxHP;                                // 초기 체력은 최대 체력으로 설정
        status.attackPower = bossData.baseAttack + attackMultiplier; // 공격력 계산
        status.defence = bossData.baseDefense + defenseMultiplier;   // 방어력 계산

        Debug.Log($"status.hp: {status.hp}, status.attackPower: {status.attackPower}, status.defence: {status.defence}");

        // 더미 적 초기화
        SetNumberOfEnemyInGroup(1);

        Debug.Log("Boss initialized successfully.");
    }


    private void StartRoarSkill()
    {
        if (roarSkillCoroutine != null) StopCoroutine(roarSkillCoroutine);
        roarSkillCoroutine = StartCoroutine(RoarSkill());
    }

    private IEnumerator RoarSkill()
    {
        while (true)
        {
            yield return new WaitForSeconds(roarInterval);
            Debug.Log("Roar Skill 발동!");
            // 포효 스킬 발동 로직 (예: 플레이어 기절, 스킬 데미지)
            ApplyRoarEffect();
        }
    }

    private void ApplyRoarEffect()
    {
        // 포효 스킬 효과 구현
        Debug.Log("플레이어에게 포효 스킬 효과 적용");
    }

    // 보스 전용 특수 공격 메서드
    private void SpecialAttack()
    {
        if (isSpecialAttackReady)
        {
            Debug.Log("DungeonBossEnemy 특수 공격 시전");
            // 특수 공격 로직 구현

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

    // 공격 메서드 오버라이딩
    protected override void Attack()
    {
        // 허수아비는 공격하지 않음
        if (bossType == BossType.Scarecrow)
        {
            Debug.Log("허수아비 보스는 공격하지 않음");
            return;
        }

        base.Attack();

        // 스킬로만 데미지를 주는 보스는 일반 공격 대신 특수 공격
        if (bossType == BossType.SkillOnly)
        {
            SpecialAttack();
        }
    }

    protected override void Death()
    {
        Debug.Log($"DungeonBossEnemy({bossType}) 사망");
        if (bossType == BossType.RoaringSkill && roarSkillCoroutine != null)
        {
            StopCoroutine(roarSkillCoroutine);
        }

        base.Death();
    }
}

public static class BossMonsterDataManager
{
    private static Dictionary<BossType, BossMonsterData> bossDataDictionary = new Dictionary<BossType, BossMonsterData>
    {
        {
            BossType.Scarecrow, new BossMonsterData
            {
                baseHP = 10000,
                baseAttack = 500,
                baseDefense = 200
            }
        },
        {
            BossType.SkillOnly, new BossMonsterData
            {
                baseHP = 8000,
                baseAttack = 700,
                baseDefense = 150
            }
        },
        {
            BossType.RoaringSkill, new BossMonsterData
            {
                baseHP = 12000,
                baseAttack = 400,
                baseDefense = 300
            }
        }
    };

    public static BossMonsterData GetBossDataByType(BossType type)
    {
        if (bossDataDictionary.TryGetValue(type, out var data))
        {
            return data;
        }

        return null;
    }
}
