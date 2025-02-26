using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnManager : MonoBehaviour
{
    private static EnemySpawnManager _instance;

    public static EnemySpawnManager GetInstance()
    {
        return _instance;
    }

    public delegate void OnEnemyDeathDelegate(int enemyCount);
    public event OnEnemyDeathDelegate OnEnemyDeath;

    private AddressableHandle<GameObject> _enemyPrefab;
    private AddressableHandle<GameObject> _bossEnemyPrefab;

    public Transform enemySpawnPosition;
    public Transform enemyCombatPosition;

    private Enemy currentEnemy; // 현재 적을 저장하는 변수

    public List<Slider> enemyHealthSliders;
    

    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        LoadAsset();

        OnGatePassed(false);
    }

    private void LoadAsset()
    {
        _enemyPrefab = new AddressableHandle<GameObject>().Load("EnemyPrefab/Enemy");
        _bossEnemyPrefab = new AddressableHandle<GameObject>().Load("EnemyPrefab/BossEnemy");
    }

    private void OnDestroy()
    {
        if(_enemyPrefab != null) _enemyPrefab.Release();
        if(_bossEnemyPrefab != null) _bossEnemyPrefab.Release();
    }

    // Gate 통과 시 또는 적이 사망했을 때 적을 다시 소환
    public void OnGatePassed(bool isFinalStage)
    {
        if (currentEnemy == null || currentEnemy.IsDead()) // 적이 사망했는지 확인
        {
            if (isFinalStage)
            {
                SpawnEnemy(_bossEnemyPrefab.obj);
            }
            else
            {
                // TODO: 임시 무리 수, 추후 서버에서 정보를 받아올 예정
                SpawnEnemy(_enemyPrefab.obj);
            }
        }
    }


    // 고양이 싸움 끝 -> 적 생성(적 연결) -> 적 출발 -> 적도착 -> 전투
    // StageManager내에서 호출 (고양이가 입장 or 이전 전투 승리 시)

    // 새로운 적을 소환하는 메서드
    void SpawnEnemy(GameObject prefab)
    {
        Cat cat = GameManager.GetInstance().catObject;

        if (cat != null &&  prefab != null)
        {
            // 적 스폰
            currentEnemy = Instantiate(prefab, enemySpawnPosition).GetComponent<Enemy>();
            currentEnemy.GoToCombatArea(cat, enemyCombatPosition.position);
        }
    }

    public void DestroyEnemy(float waitTime = 0.0f)
    {
        Cat cat = GameManager.GetInstance().catObject;

        if (currentEnemy)
        {
            Destroy(currentEnemy.gameObject, waitTime);
        }

        currentEnemy = null;
        cat.SetEnemy(null);
    }

    public void EnemyDeath(int enemyCount)
    {
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath(enemyCount);
        }
    }
}
