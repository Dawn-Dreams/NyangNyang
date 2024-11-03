using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Quaternion = UnityEngine.Quaternion;
using Transform = UnityEngine.Transform;
using Vector3 = UnityEngine.Vector3;

public enum EnemyMonsterType
{
    Null = 0,
    // 숲

    // 바다
    StarFish, Octopus, Puffe, Shellfish, Krake,
    // 사막
    // 얼음
    // 하늘

    Count
}

public class DummyEnemy
{
    static GameObject floatingDamage;
    private GameObject dummyGameObject;
    private BigInteger currentHP;
    private BigInteger maxHP;
    private TextMesh hpText;

    public AnimationManager animationManager;
    public EnemyMonsterType monsterType;

    private AddressableHandle<GameObject> _enemyMonsterPrefab;

    
    public DummyEnemy(GameObject dummyObject, EnemyMonsterType type, BigInteger maxHP)
    {
        this.dummyGameObject = dummyObject;

        monsterType = type;

        LoadEnemyMonsterAsset();

        this.currentHP = this.maxHP = maxHP;

        hpText = dummyObject.GetComponentInChildren<TextMesh>();
        hpText.text = currentHP + " / " + maxHP;
    }

    private void LoadEnemyMonsterAsset()
    {
        _enemyMonsterPrefab = new AddressableHandle<GameObject>().Load("Enemy/"+monsterType);
        animationManager = GameObject.Instantiate(_enemyMonsterPrefab.obj, dummyGameObject.transform).GetComponent<AnimationManager>();
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

        // 피격 애니메이션
        if (animationManager)
        {
            animationManager.PlayAnimation(AnimationManager.AnimationState.Damage);
        }
        

        // 대미지 출력
        // TODO 오브젝트 풀링 방식으로 바꾸기
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
        if (animationManager)
        {
            animationManager.PlayAnimation(AnimationManager.AnimationState.DieA);
        }
        
        //dummyGameObject.GetComponent<SpriteRenderer>().color = new Color(0.25f, 0.25f, 0.25f);
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

    [SerializeField] private GameObject[] dummyEnemyObj;
    private List<DummyEnemy> _dummyEnemies;

    public int initialNumOfDummyEnemy = 0;

    // 전투 위치와 관련된 변수들
    private Vector3 spawnPosition;
    private Vector3 combatPosition;
    private Coroutine moveToCombatAreaCoroutine;
    private static float moveToCombatAreaRequiredTime = 0.5f;
    private float currentMoveTime = 0.0f;
    private Character catObject;

    // 몬스터 정보에 대한 변수
    public MonsterData monsterData;

    protected override void Awake()
    {
        // stage manager 
        if (stageManager == null)
        {
            stageManager = GameManager.GetInstance().stageManager;
        }

        DummyEnemy.SetFloatingDamage(floatingDamage);
        _dummyEnemies = new List<DummyEnemy>();

        // 09.23 - EnemyID 별개로 관리하도록 변경
        characterID = 0;
        IsEnemy = true;

        // --몬스터 정보 받기 (임시 코드 & 서버에서 미리 받아서 적용될 수 있도록 or SpawnerManager 에서 할 수 있도록 )--
        // TODO : 10/30. 몬스터 정보 클라에서 관리
        int currentTheme= GameManager.GetInstance().stageManager.GetCurrentTheme();
        int currentStage = GameManager.GetInstance().stageManager.GetCurrentStage();
        int currentGate = GameManager.GetInstance().stageManager.GetCurrentGate();
        int maxGate = GameManager.GetInstance().stageManager.maxGateCount;

        status = new Status(monsterData.monsterStatus);

        base.Awake();
        
        SetNumberOfEnemyInGroup(monsterData.enemyCount);

        // ~~enemy drop data 받기~~  몬스터 정보 받기에서 진행
        //if (DropData == null)
        //{
        //    DropData = ScriptableObject.CreateInstance<EnemyDropData>().SetEnemyDropData(DummyServerData.GetEnemyDropData(characterID));
        //}
    }

    public void SetNumberOfEnemyInGroup(int numOfEnemy = 1)
    {
        if (numOfEnemy == 0)
        {
            numOfEnemy = 1;
        }

        // 적 개체는 최소 1마리에서 최대 5마리
        initialNumOfDummyEnemy = numOfEnemy = (int)Mathf.Clamp(numOfEnemy, 1.0f, dummyEnemyObj.Length);

        BigInteger dummyMaxHp = BigInteger.Divide(maxHP, numOfEnemy);
        for (int i = 0; i < dummyEnemyObj.Length; ++i)
        {
            // active dummy enemy
            if (i < numOfEnemy)
            {
                _dummyEnemies.Add(new DummyEnemy(dummyEnemyObj[i], monsterData.monsterTypes[i], dummyMaxHp));
            }
            else
            {
                dummyEnemyObj[i].SetActive(false);
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
        foreach (var dummyEnemy in _dummyEnemies)
        {
            if (dummyEnemy.animationManager)
            {
                dummyEnemy.animationManager.PlayAnimation(AnimationManager.AnimationState.IdleA);
            }
        }
        
        CombatManager.GetInstance().EnemyArriveCombatArea(this);

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

    protected override void Attack()
    {
        base.Attack();

        foreach (var dummyEnemy in _dummyEnemies)
        {
            if (dummyEnemy.animationManager)
            {
                dummyEnemy.animationManager.PlayAnimation(AnimationManager.AnimationState.ATK1);
            }
        }
    }

    public override BigInteger TakeDamage(BigInteger damage, bool isAOESkill = false)
    {
        if (_dummyEnemies.Count == 0)
        {
            Debug.Log("이미 적군이 사망함");
            return 0;
        }

        BigInteger amountOfDamage = 0;
        BigInteger applyDamage = BigInteger.Max(0, damage - status.defence);

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
        if (monsterData.enemyDropData)
        {
            monsterData.enemyDropData.GiveItemToPlayer();
        }

        CombatManager.GetInstance().CurrentEnemyDeath(this);

        base.Death();
    }

    public void SetMaxHP(BigInteger newMaxHP)
    {
        this.maxHP = newMaxHP;
        this.currentHP = newMaxHP; // 최대 체력을 변경할 때, 현재 체력도 최대 체력으로 설정
        ChangeHealthBar(); // 체력바 업데이트
    }
}

