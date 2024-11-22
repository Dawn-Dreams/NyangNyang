using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
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
    private Slider _slider;
    
    public DummyEnemy(GameObject dummyObject, Slider slider, EnemyMonsterType type, BigInteger maxHP)
    {
        if (dummyObject == null || slider == null)
        {
            Debug.LogError("DummyEnemy: Null argument in constructor");
        }
        this.dummyGameObject = dummyObject;

        _slider = slider;

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

        // TODO: 나중에 지우기
        hpText.text = currentHP + " / " + maxHP;
        _slider.value = MyBigIntegerMath.DivideToFloat(currentHP, maxHP, 5);

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
        EnemyPlayAnimation(AnimationManager.AnimationState.DieA);
        
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

    public void EnemyPlayAnimation(AnimationManager.AnimationState state)
    {
        animationManager.PlayAnimation(state);
    }

    public void EnemyArriveAtCombatArea()
    {
        EnemyPlayAnimation(AnimationManager.AnimationState.IdleA);
        _slider.gameObject.SetActive(true);
        if (Camera.main != null)
        {
            Vector3 point = Camera.main.WorldToScreenPoint(dummyGameObject.transform.position);
            point.z = 0.0f;
            _slider.gameObject.GetComponent<RectTransform>().anchoredPosition3D = point;
        }
        
        _slider.value = 1f;
    }

    public void Initialize(Slider slider, EnemyMonsterType type, BigInteger maxHP)
    {
        this._slider = slider;
        this.monsterType = type;
        this.maxHP = maxHP;

        Debug.Log($"DummyEnemy initialized: Type = {type}, Max HP = {maxHP}");
    }
}

public class Enemy : Character
{
    public GameObject floatingDamage;
    [SerializeField] private StageManager stageManager;

    [SerializeField] private GameObject[] dummyEnemyObj;
    private List<DummyEnemy> _dummyEnemies;
    private List<EnemyMonsterType> _dummyEnemyMonsterTypes;

    public int initialNumOfDummyEnemy = 0;

    // 전투 위치와 관련된 변수들
    private Vector3 spawnPosition;
    private Vector3 combatPosition;
    private Coroutine moveToCombatAreaCoroutine;
    private static float moveToCombatAreaRequiredTime = 0.5f;
    private float currentMoveTime = 0.0f;
    private Character catObject;

    // 몬스터 정보에 대한 변수
    public MonsterData monsterDataTemplate;
    private MonsterData _monsterData;

    // 몬스터들의 체력을 나타내는 Slider
    [SerializeField] private List<Slider> sliders;

    protected override void Awake()
    {
        sliders = EnemySpawnManager.GetInstance().enemyHealthSliders;

        // stage manager 
        if (stageManager == null)
        {
            stageManager = GameManager.GetInstance().stageManager;
        }

        foreach (var slider in sliders)
        {
            slider.gameObject.SetActive(false);
        }

        DummyEnemy.SetFloatingDamage(floatingDamage);
        _dummyEnemies = new List<DummyEnemy>();

        // 09.23 - EnemyID 별개로 관리하도록 변경
        characterID = 0;
        IsEnemy = true;

        _monsterData = ScriptableObject.CreateInstance<MonsterData>().SetMonsterDataFromOther(monsterDataTemplate);
        int currentTheme= GameManager.GetInstance().stageManager.GetCurrentTheme();
        int currentStage = GameManager.GetInstance().stageManager.GetCurrentStage();
        int maxStage = GameManager.GetInstance().stageManager.maxStageCount;
        _monsterData.InitializeMonsterStatus(currentTheme,currentStage, maxStage);
        status = new Status(_monsterData.monsterStatus);

        base.Awake();
        
        SetNumberOfEnemyInGroup(_monsterData.enemyCount);

        // ~~enemy drop data 받기~~  몬스터 정보 받기에서 진행
        //if (DropData == null)
        //{
        //    DropData = ScriptableObject.CreateInstance<EnemyDropData>().SetEnemyDropData(DummyServerData.GetEnemyDropData(characterID));
        //}
    }

    public void SetNumberOfEnemyInGroup(int numOfEnemy = 1)
    {
        // 적 개체는 최소 1마리에서 최대 5마리
        initialNumOfDummyEnemy = numOfEnemy = (int)Mathf.Clamp(numOfEnemy, 1.0f, dummyEnemyObj.Length);
        
        
        // 더미 몬스터들의 타입을 설정
        _dummyEnemyMonsterTypes = SelectEnemyTypes(numOfEnemy, _monsterData.monsterTypes);

        // 더미 몬스터 생성
        BigInteger dummyMaxHp = BigInteger.Divide(maxHP, numOfEnemy);
        for (int i = 0; i < dummyEnemyObj.Length; ++i)
        {
            if (i < numOfEnemy)
            {
                if (dummyEnemyObj[i] != null && sliders[i] != null)
                {
                    //_dummyEnemies.Add(new DummyEnemy(dummyEnemyObj[i], sliders[i], _monsterData.monsterTypes[i], dummyMaxHp));
                    _dummyEnemies.Add(new DummyEnemy(dummyEnemyObj[i], sliders[i], _dummyEnemyMonsterTypes[i], dummyMaxHp));
                }
                else
                {
                    Debug.LogError($"dummyObject 또는 slider가 null입니다: Index {i}");
                }
            }
            else
            {
                dummyEnemyObj[i].SetActive(false);
                sliders[i].gameObject.SetActive(false);
            }
        }

        currentHP = maxHP = dummyMaxHp * numOfEnemy;
        ChangeHealthBar();
    }

    // 해당 적군의 몬스터 타입들을 구하는 함수
    private List<EnemyMonsterType> SelectEnemyTypes(int numOfEnemy, List<EnemyMonsterType> monsterDataMonsterTypes)
    {
        /*
           1. 최초 스테이지
           - 정해진 형태로, AAA BB / AA BB C / D
           2. 반복 스테이지
           - 정해진 애들 안에서 확률에 의해 나오도록
           ABCDE 내 50%/25%/12.5%/6.25%/3.125% / 남은 경우 A 로 해서
           총 53.125% / 25% / 12.5% / 6.25% / 3.125% 가 되도록,
         */

        // 한마리 (보스의 경우)
        if (numOfEnemy == 1)
        {
            return new List<EnemyMonsterType>() { monsterDataMonsterTypes[0] };
        }

        // 최초 스테이지일 경우
        if (!Player.continuousCombat)
        {
            // 주어진 형태로,
            if (stageManager.GetCurrentGate() == 0)
            {
                return new List<EnemyMonsterType>()
                {
                    monsterDataMonsterTypes[0], monsterDataMonsterTypes[0], monsterDataMonsterTypes[0],
                    monsterDataMonsterTypes[1], monsterDataMonsterTypes[1]
                };
            }
            else
            {
                return new List<EnemyMonsterType>()
                {
                    monsterDataMonsterTypes[0], monsterDataMonsterTypes[0], 
                    monsterDataMonsterTypes[1], monsterDataMonsterTypes[1], monsterDataMonsterTypes[2]
                };
            }
        }
        // 반복 스테이지일 경우
        else
        {
            List<EnemyMonsterType> types = new List<EnemyMonsterType>();
            // 확률 구하기
            List<int> enemyTypePercent = new List<int>();
            int currentPercent = 0;
            for (int i = 0; i < numOfEnemy; ++i)
            {
                int percent = (int)(100 / Mathf.Pow(2, i+1));
                enemyTypePercent.Add(percent);
                currentPercent += percent;
            }
            if (currentPercent < 100)
            {
                enemyTypePercent[0] += 100 - currentPercent;
            }
            // 적군 뽑기
            for (int enemyIndex = 0; enemyIndex < numOfEnemy; ++enemyIndex)
            {
                int rand = Random.Range(1, 100);
                
                for (int percentIndex = 0; percentIndex < enemyTypePercent.Count; ++percentIndex)
                {
                    // ex) 5종류 기준 54 / 25 / 12 / 6 / 3
                    if (rand >= enemyTypePercent[percentIndex])
                    {
                        types.Add(monsterDataMonsterTypes[percentIndex]);
                        break;
                    }
                }
            }
            
            if (types.Count != numOfEnemy)
            {
                for (int count = types.Count; count < numOfEnemy; ++count)
                {
                    types.Add(monsterDataMonsterTypes[0]);
                }
            }
            

            return types;
        }
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
            dummyEnemy.EnemyArriveAtCombatArea();
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
            dummyEnemy.EnemyPlayAnimation(AnimationManager.AnimationState.ATK1);
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
        if (_monsterData.enemyDropData)
        {
            _monsterData.enemyDropData.GiveItemToPlayer();
        }

        
        for (int i = 0; i < _dummyEnemyMonsterTypes.Count; ++i)
        {
            if (QuestManager.GetInstance().OnUserKillEnemyType != null)
            {
                QuestManager.GetInstance().OnUserKillEnemyType(_dummyEnemyMonsterTypes[i]);
            }
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

