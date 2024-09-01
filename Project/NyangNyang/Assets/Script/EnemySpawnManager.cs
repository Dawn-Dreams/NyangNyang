using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    void Start()
    {
        Invoke("SpawnEnemy", 1);
    }

    public GameObject enemyPrefab;
    public Transform enemySpawnPosition;

    // 고양이 싸움 끝 -> 적 생성(적 연결) -> 적 출발 -> 적도착 -> 전투
    // StageManager내에서 호출 (고양이가 입장 or 이전 전투 승리 시)
    void SpawnEnemy()
    {
        Cat cat = GameManager.GetInstance().catObject;

        Debug.Log("적군 생성");
        // 적 스폰
        Enemy enemy = Instantiate(enemyPrefab, enemySpawnPosition).GetComponent<Enemy>();

        // 적과 고양이의 적군 오브젝트 연결
        enemy.enemyObject = cat.gameObject;
        cat.enemyObject = enemy.gameObject;
    }
}
