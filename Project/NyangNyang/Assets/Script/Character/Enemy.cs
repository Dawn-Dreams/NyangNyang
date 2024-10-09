using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Quaternion = UnityEngine.Quaternion;
using Transform = UnityEngine.Transform;
using Vector3 = UnityEngine.Vector3;

public class DummyEnemy
{
    static GameObject floatingDamage;
    private GameObject dummyGameObject;
    private BigInteger currentHP;
    private BigInteger maxHP;
    private TextMesh hpText;
    
    public DummyEnemy(GameObject dummyObject, BigInteger maxHP)
    {
        this.dummyGameObject = dummyObject;
        this.currentHP = this.maxHP = maxHP;

        hpText = dummyObject.GetComponentInChildren<TextMesh>();
        hpText.text = currentHP + " / " + maxHP;
    }

    public BigInteger TakeDamage(BigInteger getDamage)
    {
        BigInteger returnApplyDamage = getDamage;
        if (getDamage > currentHP)
        {
            returnApplyDamage = currentHP;
        }
        currentHP = currentHP - getDamage;

        hpText.text = currentHP + " / " + maxHP;

        // 대미지 출력
        GameObject textObject = (GameObject)GameObject.Instantiate(floatingDamage,
            dummyGameObject.transform.position, Quaternion.identity);
        textObject.GetComponentInChildren<TextMesh>().text = " " + getDamage + " ";
        GameObject.Destroy(textObject, 2.0f);

        return returnApplyDamage;
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }

    public void DestroyDummyEnemy()
    {
        // TODO : 임시 사망 코드 추후 애니메이션으로 변경 및 ...
        dummyGameObject.GetComponent<SpriteRenderer>().color = new Color(0.25f, 0.25f, 0.25f);
        hpText.gameObject.SetActive(false);
    }

    public static void SetFloatingDamage(GameObject getFloatingDamage)
    {
        if (floatingDamage == null)
        {
            floatingDamage = getFloatingDamage;
        }
    }
}

public class Enemy : Character
{
    public GameObject floatingDamage;
    [SerializeField] private StageManager stageManager;
    protected EnemyDropData DropData = null;

    [SerializeField] private GameObject[] dummyEnemyImages;
    private List<DummyEnemy> _dummyEnemies;

    private int initialNumOfDummyEnemy = 0;

    // 전투 위치와 관련된 변수들
    private Vector3 spawnPosition;
    private Vector3 combatPosition;
    private Coroutine moveToCombatAreaCoroutine;
    private static float moveToCombatAreaRequiredTime = 0.5f;
    private float currentMoveTime = 0.0f;
    private Character catObject;

    protected override void Awake()
    {
        DummyEnemy.SetFloatingDamage(floatingDamage);
        _dummyEnemies = new List<DummyEnemy>();

        // 09.23 - EnemyID 별개로 관리하도록 변경
        characterID = 0;
        IsEnemy = true;

        // 몬스터 정보 받기 (임시 코드 & 서버에서 미리 받아서 적용될 수 있도록 or SpawnerManager 에서 할 수 있도록 )
        int currentTheme= GameManager.GetInstance().stageManager.GetCurrentTheme();
        int currentStage = GameManager.GetInstance().stageManager.GetCurrentStage();
        MonsterData monsterData = new MonsterData().SetMonsterData(DummyServerData.GetEnemyData(currentTheme, currentStage,
            GameManager.GetInstance().stageManager.maxStageCount));
        DropData = monsterData.enemyDropData;
        status = new Status();
        status.SetStatusLevelData(monsterData.monsterStatus);

        base.Awake();
        
        // stage manager 
        if (stageManager == null)
        {
            stageManager = FindObjectOfType<StageManager>();
        }

        // ~~enemy drop data 받기~~  몬스터 정보 받기에서 진행
        //if (DropData == null)
        //{
        //    DropData = ScriptableObject.CreateInstance<EnemyDropData>().SetEnemyDropData(DummyServerData.GetEnemyDropData(characterID));
        //}
    }

    public void SetNumberOfEnemyInGroup(int numOfEnemy = 1)
    {
        // 적 개체는 최소 1마리에서 최대 5마리
        initialNumOfDummyEnemy = numOfEnemy = (int)Mathf.Clamp(numOfEnemy, 1.0f, dummyEnemyImages.Length);

        BigInteger dummyMaxHp = BigInteger.Divide(maxHP, numOfEnemy);
        for (int i = 0; i < dummyEnemyImages.Length; ++i)
        {
            // active dummy enemy
            if (i < numOfEnemy)
            {
                _dummyEnemies.Add(new DummyEnemy(dummyEnemyImages[i], dummyMaxHp));
            }
            else
            {
                dummyEnemyImages[i].SetActive(false);
            }
        }

        currentHP = maxHP = dummyMaxHp * numOfEnemy;
        ChangeHealthBar();
    }

    public void GoToCombatArea(Character enemyCat, Vector3 combatTransform)
    {
        spawnPosition = transform.position;
        combatPosition = combatTransform;
        catObject = enemyCat;
        currentMoveTime = 0.0f;
        moveToCombatAreaCoroutine = StartCoroutine(MoveToCombatArea());
    }

    IEnumerator MoveToCombatArea()
    {
        while (true)
        {
            currentMoveTime = Mathf.Min(currentMoveTime+Time.deltaTime, moveToCombatAreaRequiredTime);
            if (currentMoveTime >= moveToCombatAreaRequiredTime)
            {
                ArriveCombatArea();
            }

            transform.position = Vector3.Lerp(spawnPosition, combatPosition, currentMoveTime / moveToCombatAreaRequiredTime);
            yield return null;
        }
        
    }

    void ArriveCombatArea()
    {
        if (moveToCombatAreaCoroutine != null)
        {
            StopCoroutine(moveToCombatAreaCoroutine);
            moveToCombatAreaCoroutine = null;
        }

        if (catObject && catObject.gameObject.activeSelf)
        {
            SetEnemy(catObject);
            catObject.SetEnemy(this);
        }
    }


    protected override BigInteger CalculateDamage()
    {
        BigInteger damage = base.CalculateDamage();
        BigInteger initialDamage = damage;
        float divideValue = ((float)_dummyEnemies.Count / initialNumOfDummyEnemy);
        damage = MyBigIntegerMath.MultiplyWithFloat(damage,divideValue,5);
        return damage;
    }

    public override BigInteger TakeDamage(BigInteger damage, bool isAOESkill = false)
    {
        if (_dummyEnemies.Count == 0)
        {
            Debug.Log("이미 적군이 사망함");
            return 0;
        }

        BigInteger amountOfDamage = 0;
        BigInteger applyDamage = damage - status.defence;

        int maxApplyDamageCount = 1;
        if (isAOESkill)
        {
            maxApplyDamageCount = _dummyEnemies.Count;
        }

        // apply damage
        for(int i = 0; i < maxApplyDamageCount; ++i)
        {
            amountOfDamage += _dummyEnemies[i].TakeDamage(applyDamage);
             
            if (_dummyEnemies[i].IsDead())
            {
                _dummyEnemies[i].DestroyDummyEnemy();
                _dummyEnemies.RemoveAt(i);
                --i;
                --maxApplyDamageCount;
            }
        }

        DecreaseHp(amountOfDamage);

        return applyDamage;
    }

    // 적군이 사망했는지 여부를 반환
    public override bool IsDead()
    {
        return _dummyEnemies.Count == 0;
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

        DummyQuestServer.UserMonsterKill(Player.GetUserID(),initialNumOfDummyEnemy);

        base.Death();
    }

    // 스탯 설정 함수 ... 수정필요
    public void SetEnemyStatus(int level)
    {
        // 임시 지정한 베이스 스탯
        BigInteger baseHealth = 100;
        int baseAttackPower =100, baseDefence = 100;

        // 기본 체력, 공격력, 방어력
        BigInteger totalHealth = baseHealth + (level * 100);         // 레벨당 전체 체력 증가
        status.attackPower = baseAttackPower + (level * 10);         // 레벨당 공격력 증가
        status.defence = baseDefence + (level * 5);                  // 레벨당 방어력 증가

        // 적들의 개체 수에 맞춰 체력 분배
        BigInteger dummyMaxHp = totalHealth / initialNumOfDummyEnemy;

        for (int i = 0; i < _dummyEnemies.Count; ++i)
        {
            _dummyEnemies[i] = new DummyEnemy(dummyEnemyImages[i], dummyMaxHp);  // 각 DummyEnemy에 개별 체력 설정
        }

        // 총 체력 설정
        maxHP = totalHealth;
        currentHP = maxHP;

        ChangeHealthBar(); // 체력바 업데이트
        Debug.Log($"레벨 {level}에 맞춰 적군 스탯 설정 완료: 총 체력 {totalHealth}, 공격력 {status.attackPower}, 방어력 {status.defence}");
    }

}

