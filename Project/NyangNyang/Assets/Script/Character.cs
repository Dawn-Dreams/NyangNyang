using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Status status;
    public GameObject enemyObject;

    void Awake()
    {
        StartCoroutine(AttackEnemy());
        
    }

    IEnumerator AttackEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (enemyObject)
            {
                Debug.Log("공격 -> ");
            }
        }
    }

    void SetEnemy(GameObject targetObject)
    {
        if (targetObject)
        {
            enemyObject = targetObject;
        }
    }
}

//Character -> Cat / Enemy
//    Cat 내부 -> StatusManager::GetPlayerStatus;
//                StatusManager 내에 계정 스탯도 같이 보유하게 ,,
//Player




