using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform enemySpawnPosition;
    public Transform enemyCombatPosition;

    private Enemy currentEnemy; // 현재 적을 저장하는 변수

    void Start()
    {
        SpawnEnemy(); // 처음 적을 소환
    }

    // Gate 통과 시 또는 적이 사망했을 때 적을 다시 소환
    public void OnGatePassed()
    {
        if (currentEnemy == null || currentEnemy.IsDead()) // 적이 사망했는지 확인
        {
            SpawnEnemy();
        }
    }


    // 고양이 싸움 끝 -> 적 생성(적 연결) -> 적 출발 -> 적도착 -> 전투
    // StageManager내에서 호출 (고양이가 입장 or 이전 전투 승리 시)

    // 새로운 적을 소환하는 메서드
    void SpawnEnemy()
    {
        Cat cat = GameManager.GetInstance().catObject;

        // 적 스폰
        currentEnemy = Instantiate(enemyPrefab, enemySpawnPosition).GetComponent<Enemy>();
        currentEnemy.SetNumberOfEnemyInGroup(3);
        currentEnemy.GoToCombatArea(cat, enemyCombatPosition.position);
        // 적과 고양이의 적군 오브젝트 연결 -> 적군이 다 이동 한 뒤 오브젝트가 연결되도록 변경
        //currentEnemy.SetEnemy(cat);
        //cat.SetEnemy(currentEnemy);
    }
}
