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

    // ����� �ο� �� -> �� ����(�� ����) -> �� ��� -> ������ -> ����
    // StageManager������ ȣ�� (����̰� ���� or ���� ���� �¸� ��)
    void SpawnEnemy()
    {
        Cat cat = GameManager.GetInstance().catObject;

        Debug.Log("���� ����");
        // �� ����
        Enemy enemy = Instantiate(enemyPrefab, enemySpawnPosition).GetComponent<Enemy>();

        // ���� ������� ���� ������Ʈ ����
        enemy.enemyObject = cat.gameObject;
        cat.enemyObject = enemy.gameObject;
    }
}
