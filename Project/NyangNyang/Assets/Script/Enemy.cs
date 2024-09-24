using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
    public GameObject floatingDamage;

    [SerializeField] private StageManager stageManager;

    protected EnemyDropData DropData = null;

    protected override void Awake()
    {
        // 09.23 - EnemyID 별개로 관리하도록 변경
        characterID = 0;
        IsEnemy = true;

        base.Awake();

        // stage manager 
        if (stageManager == null)
        {
            stageManager = FindObjectOfType<StageManager>();
        }

        // enemy drop data 받기
        if (DropData == null)
        {
            DropData = ScriptableObject.CreateInstance<EnemyDropData>().SetEnemyDropData(DummyServerData.GetEnemyDropData(characterID));
        }
    }

    protected override bool TakeDamage(int damage)
    {
        bool getDamaged = base.TakeDamage(damage);

        if (getDamaged)
        {
            // 대미지 출력
            GameObject textObject = Instantiate(floatingDamage, transform.position, Quaternion.identity);
            textObject.GetComponentInChildren<TextMesh>().text = " " + damage + " ";
            Destroy(textObject,2.0f);
        }

        return getDamaged;
    }

    // 적군이 사망했는지 여부를 반환
    public bool IsDead()
    {
        return currentHP <= 0;
    }

    protected override void Death()
    {
        if (DropData)
        {
            DropData.GiveItemToPlayer();
        }

        if (stageManager)
        {
            stageManager.GateClearAfterEnemyDeath(0.5f);
        }

        base.Death();
    }
}

