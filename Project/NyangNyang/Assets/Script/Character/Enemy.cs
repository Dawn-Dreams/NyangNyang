using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;

public class DummyEnemy
{
    private GameObject dummyGameObject;
    private BigInteger currentHP;
    private BigInteger maxHP;

    public DummyEnemy(GameObject dummyObject, BigInteger currentHP, BigInteger maxHP)
    {
        this.dummyGameObject = dummyObject;
        this.currentHP = currentHP;
        this.maxHP = maxHP;
    }
}

public class Enemy : Character
{
    public GameObject floatingDamage;
    [SerializeField] private StageManager stageManager;
    protected EnemyDropData DropData = null;

    [SerializeField] private GameObject[] dummyEnemyImages;
    private DummyEnemy[] dummyEnemies;

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

    public void SetNumberOfEnemyInGroup(int numOfEnemy = 1)
    {
        // 적 개체는 최소 1마리에서 최대 5마리
        numOfEnemy = (int)Mathf.Clamp(numOfEnemy, 1.0f, 5.0f);


    }

    protected override BigInteger TakeDamage(BigInteger damage)
    {
        BigInteger applyDamage = base.TakeDamage(damage);

        if (applyDamage > 0)
        {
            // 대미지 출력
            GameObject textObject = Instantiate(floatingDamage, transform.position, Quaternion.identity);
            textObject.GetComponentInChildren<TextMesh>().text = " " + applyDamage + " ";
            Destroy(textObject,2.0f);
        }

        return applyDamage;
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

