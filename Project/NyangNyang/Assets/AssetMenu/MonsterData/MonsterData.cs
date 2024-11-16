using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/Enemy/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
{
    public int enemyCount;
    public List<EnemyMonsterType> monsterTypes;
    public StatusLevelData monsterStatus;
    public EnemyDropData enemyDropData;
    public int baseHP; // 체력
    public int baseAttack; // 공격력
    public int baseDefense; // 방어력
    public MonsterData SetMonsterDataFromOther(MonsterData other)
    {
        enemyCount = other.enemyCount;
        monsterTypes = other.monsterTypes;
        monsterStatus = new StatusLevelData(other.monsterStatus);
        enemyDropData = ScriptableObject.CreateInstance<EnemyDropData>().SetEnemyDropData(other.enemyDropData);
        return this;
    }
    
    public void InitializeMonsterStatus(int themeNumber, int stageNumber, int maxStage)
    {
        // 스테이지마다 n(default: 0.5) 추가
        // ex) 1-1 : 1 / 1-2 : 1 + n / 1-3 : 1 + 2n / ...
        // 보스 몬스터의 경우 자체적으로 2배 추가
        // ex) 1-1-3(보스) : 2 / 1-2-3(보스) : 2 * (1+n) / ...
        // 테마 스테이지는 maxStage * pow((themeNumber-1),2) 만큼 가중치
        // 해당 부분은 추후 조정
        // ex) (20 스테이지 기준) 1-1 : 0 + 1 // 2-2 : 20 + 1 + n // 3-2 : 80 + 1 + n // 4-2 : 180 + 1 + n

        float stageBuffValue = 0.5f;
        float levelMulValue = maxStage * Mathf.Pow((themeNumber - 1), 2) + (stageNumber + 1) * stageBuffValue;
        monsterStatus.MultipleLevel(levelMulValue);
        
        // TODO 적군 보상도 스테이지에 따라 선형이 아닌 log 방식으로 올라가도록 바꾸기
        float dropDataBuffPerStage = 0.5f;
        float dropDataMulValue = (maxStage * (themeNumber - 1) + stageNumber) * dropDataBuffPerStage;
        enemyDropData.MulDropData(dropDataMulValue);
    }

  
}

[CreateAssetMenu(fileName = "BossMonsterData", menuName = "ScriptableObjects/Enemy/BossMonsterData")]
public class BossMonsterData : ScriptableObject
{
    public DungeonBossEnemy.BossType bossType;
    public int baseHP;
    public int baseAttack;
    public int baseDefense;

    public BossMonsterData SetMonsterDataFromOther(BossMonsterData other)
    {
        baseHP = other.baseHP;
        baseAttack = other.baseAttack;
        baseDefense = other.baseDefense;
        return this;
    }

    public void InitializeForDungeon(int dungeonIndex, int dungeonLevel)
    {
        // 1. 던전 배수 계산
        float dungeonBuffValue = 1.0f + 0.2f * dungeonLevel; // 레벨당 20% 증가
        float indexMultiplier = 1.0f + 0.1f * dungeonIndex;  // 던전 ID에 따른 추가 보정

        // 2. 최종 배수 계산
        float finalMultiplier = dungeonBuffValue * indexMultiplier;

    }


}


