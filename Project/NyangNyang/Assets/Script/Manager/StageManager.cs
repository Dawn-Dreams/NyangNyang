using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum StagePlanet
{
    Forest, Desert, Ocean, Lava, Ice, Count
}

public class StageManager : MonoBehaviour
{
    private static StageManager _instance;

    public static StageManager GetInstance()
    {
        return _instance;
    }

    [SerializeField]
    private ParallaxScrollingManager parallaxScrollingManager;



    [SerializeField]
    private Text ThemeUI;
    [SerializeField]
    private Text StageUI;

    [SerializeField]
    private StageSlider stageSlider;

    [SerializeField] private Button continuousCombatButton;

    [SerializeField]
    private Image fadeImage;
    [SerializeField]
    private float fadeTime = 0.5f;
    private float currentFadeTime = 0.0f;
    private Coroutine fadeCoroutine;

    [SerializeField]
    private EnemySpawnManager enemySpawnManager;  // 적 스폰 매니저 변수

    [SerializeField] private AnimationManager catAnimationManager;

    private int originalTheme;  // 원래 테마를 저장할 변수

    // TODO: 초기값 설정은 추후 NetworkManager에서 각 유저별 스테이지 로 받아오는 것으로 설정
    private int currentTheme = 1;
    private int currentStage = 1;
    private int currentGate = 1;

    // 특정 컨텐츠의 경우(보스 레이드 등) 관문이 하나만 있을 수 있으니 조정 가능한 변수로
    public int maxGateCount = 3;
    public int maxStageCount = 5;

    // 스토리 퀘스트 내에서 스테이지 클리어를 판단하기 위한 이벤트 델리게이트
    public delegate void OnStageClearDelegate(int clearTheme, int clearStage);
    public event OnStageClearDelegate OnStageClear;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        AudioManager.Instance.PlayBGM("BGM_Funny");
    }

    void Start()
    {
        // 데이터 받기
        Player.GetPlayerHighestClearStageData(out int themeData, out int stageData);
        if (themeData == 0)
        {
            SaveLoadManager.GetInstance().LoadPlayerStageData(out themeData, out stageData);
        }
        currentTheme = themeData;
        currentStage = stageData;
        Player.SetPlayerHighestClearStageData(currentTheme, currentStage);

        SetNewStage(true);

        continuousCombatButton.onClick.AddListener(GoToLastClearStageNextStage);
    }

    // 현재 무슨 스테이지인지 UI를 통해 출력하는 함수
    private void SetStageUI()
    {
        ThemeUI.text = currentTheme.ToString();
        StageUI.text = "- " + currentStage.ToString();
        if (stageSlider)
        {
            stageSlider.CreateGateImage(maxGateCount);
        }

        // 새로운 스테이지 설정 시 ChangeStageUI의 정보도 갱신
        GameManager.GetInstance().changeStageUI.RenewalStageButtonTypeInCurrentTheme();
    }

    // 관문의 몬스터가 사망 후 불리는 함수
    public void GateClearAfterEnemyDeath(float waitTime)
    {
        StartCoroutine(GateClear(waitTime));
    }

    // 관문의 몬스터가 사망 후 waitTime 시간 후 이동
    private IEnumerator GateClear(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (currentGate >= maxGateCount)
        {
            // 관문을 모두 클리어했으면 스테이지 클리어 처리
            StageClear();
        }
        else
        {
            // 다음 관문으로 이동
            GoToNextGate();
        }
    }

    // 다음 관문으로 이동하는 함수
    private void GoToNextGate()
    {
        if (stageSlider && !Player.continuousCombat)
        {
            stageSlider.MoveToNextGate(currentGate, maxGateCount, 1.0f);
        }

        // 유저들이 ChangeStageUI를 클릭을 하지 못하도록 fadeImage 활성화
        fadeImage.gameObject.SetActive(true);
        GameManager.GetInstance().changeStageUI.SetChangeStageButtonInteractable(false);

        parallaxScrollingManager.MoveBackgroundSprites(true);
       

        // 애니메이션 실행
        GameManager.GetInstance().catObject.animationManager.PlayAnimation(AnimationManager.AnimationState.Walk);
        PetManager.GetInstance().playerPet.PlayPetAnim(AnimationManager.AnimationState.Move);

        StartCoroutine(ArriveGate());
    }

    // 다음 게이트에 도착했을 때의 함수
    IEnumerator ArriveGate()
    {
        // 다음게이트까지 이동하는데 걸리는 시간만큼 wait
        yield return new WaitForSeconds(1.0f);

        // 배경에 대한 움직임 중단
        parallaxScrollingManager.MoveBackgroundSprites(false);
       
        // 관문 index 1, 2, ..., maxGateCount 순환
        if (!Player.continuousCombat)
        {
            ++currentGate;
        }

        fadeImage.gameObject.SetActive(false);
        GameManager.GetInstance().changeStageUI.SetChangeStageButtonInteractable(true);

        SetStageUI();
        
        // CombatManager에게 게이트 도착을 알림으로써 몬스터 소환 등 처리
        CombatManager.GetInstance().CatArriveNewGate(currentGate == maxGateCount);

        yield break;
    }
    
    // 스테이지 내 최고 관문 클리어시 StageClear 함수 실행
    private void StageClear()
    {
        // 스테이지 관련 퀘스트에서 사용되는 델리게이트
        if (OnStageClear != null)
        {
            OnStageClear(currentTheme, currentStage);
        }
        
        // 페이드 시작
        fadeCoroutine = StartCoroutine(StartFade(() => FadeStartFuncWhileStageChange(true)));
    }
    // 서버에 스테이지를 클리어했다는 정보 전송
    void SaveStageClearDataToJson()
    {
        Player.GetPlayerHighestClearStageData(out var clearTheme, out var clearStage);
        if (currentTheme > clearTheme || (currentTheme == clearTheme && currentStage > clearStage))
        {
            //DummyServerData.PlayerClearStage(Player.GetUserID(), currentTheme, currentStage);
            
            SaveLoadManager.GetInstance().SavePlayerStageData(new StageData(){highestTheme =  clearTheme,highestStage = clearStage});
        }
    }

    private IEnumerator StartFade(Action FuncAfterStartFade)
    {
        currentFadeTime = 0.0f;
        fadeImage.gameObject.SetActive(true);
        GameManager.GetInstance().changeStageUI.SetChangeStageButtonInteractable(false);
        while (true)
        {
            currentFadeTime += Time.deltaTime;
            Color tempColor = fadeImage.color;
            tempColor.a = currentFadeTime / fadeTime;
            fadeImage.color = tempColor;

            if (currentFadeTime >= fadeTime)
            {
                FuncAfterStartFade();
            }
            yield return null;
        }
    }

    private IEnumerator EndFade(Action FuncAfterEndFade)
    {
        yield return new WaitForSeconds(fadeTime);
        currentFadeTime = 0.0f;

        while (true)
        {
            currentFadeTime += Time.deltaTime;
            Color tempColor = fadeImage.color;
            tempColor.a = 1 - currentFadeTime / fadeTime;
            fadeImage.color = tempColor;

            if (currentFadeTime >= fadeTime)
            {
                FuncAfterEndFade();
                fadeImage.gameObject.SetActive(false);
                GameManager.GetInstance().changeStageUI.SetChangeStageButtonInteractable(true);
            }

            yield return null;
        }
    }

    void FadeStartFuncWhileStageChange(bool addStage)
    {
        SetNewStage(addStage);

        StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(EndFade(FadeEndFuncWhileStageChange));
    }

    void FadeEndFuncWhileStageChange()
    {
        StopCoroutine(fadeCoroutine);
        CombatManager.GetInstance().CatArriveNewGate(currentGate == maxGateCount);
    }

    void SetNewStage(bool addStage)
    {
        if (stageSlider)
        {
            stageSlider.ClearGateImages();
        }

        // 최고 스테이지 클리어 했는지 정보 갱신
        SaveStageClearDataToJson();

        if (addStage)
        {
            currentGate = 1;
            currentStage += 1;
            if (currentStage > maxStageCount)
            {
                currentStage = 1;
                currentTheme += 1;
            }
        }

        parallaxScrollingManager.ChangeIndexNumberBackgroundImage(currentTheme);
       
        GameManager.GetInstance().catObject.CatRespawn();
        GameManager.GetInstance().catObject.animationManager.PlayAnimation(AnimationManager.AnimationState.IdleA);

        // 현재 스테이지가 최고 스테이지가 아니라면 반복 사냥 진행
        int clearThemeData = 0;
        int clearStageData = 0;
        Player.GetPlayerHighestClearStageData(out clearThemeData, out clearStageData);
        if (currentTheme < clearThemeData || ((currentTheme == clearThemeData) && currentStage < clearStageData))
        {
            SetContinuousCombat(true);
        }
        else
        {
            SetContinuousCombat(false);
        }


        SetStageUI();
    }

    // 현재 테마를 반환하는 함수
    public int GetCurrentTheme()
    {
        return currentTheme;
    }

    public int GetCurrentStage()
    {
        return currentStage;
    }

    public int GetCurrentGate()
    {
        return currentGate;
    }

    public void SetContinuousCombat(bool activeContinuousCombat)
    {
        Player.continuousCombat = activeContinuousCombat;
        continuousCombatButton.gameObject.SetActive(activeContinuousCombat);
        stageSlider.gameObject.SetActive(!activeContinuousCombat);
        if (activeContinuousCombat)
        {
            currentGate = 1;
        }
    }
    // 최고 스테이지로 가는 함수
    public void GoToLastClearStageNextStage()
    {
        int clearStageTheme = 1;
        int clearStage = 1;
        Player.GetPlayerHighestClearStageData(out clearStageTheme, out clearStage);
        SetContinuousCombat(false);

        EnemySpawnManager.GetInstance().DestroyEnemy();
        currentTheme = clearStageTheme;
        currentStage = clearStage;
        StageClear();
    }

    public void GoToSpecificStage(int stageThemeNum, int stageNum)
    {
        // TODO: 처음 실행 시 한번만 받고 적용되도록
        int clearStageTheme = 1;
        int clearStage = 1;
        Player.GetPlayerHighestClearStageData(out clearStageTheme, out clearStage);

        if (stageThemeNum <= clearStageTheme && stageNum <= clearStage)
        {
            SetContinuousCombat(true);
        }
        else
        {
            SetContinuousCombat(false);
        }
        enemySpawnManager.DestroyEnemy();
        currentTheme = stageThemeNum;
        currentStage = stageNum;

        fadeCoroutine = StartCoroutine(StartFade(() => FadeStartFuncWhileStageChange(false)));
    }


    void RespawnCat()
    {
        GameManager.GetInstance().catObject.CatRespawn();
        GameManager.GetInstance().catObject.animationManager.PlayAnimation(AnimationManager.AnimationState.IdleA);
        StopCoroutine(fadeCoroutine);
        enemySpawnManager.DestroyEnemy();
        currentStage -= 1;
        if (currentStage <= 0)
        {
            currentTheme -= 1;
            currentStage = maxStageCount;
            if (currentTheme <= 0)
            {
                currentTheme = 1;
                currentStage = 1;
            }
            parallaxScrollingManager.ChangeIndexNumberBackgroundImage(currentTheme);

        }
        SetStageUI();

        fadeCoroutine = StartCoroutine(EndFade(FadeEndFuncWhileStageChange));
    }
    public void PlayerDie()
    {
        fadeCoroutine = StartCoroutine(StartFade(RespawnCat));
        SetContinuousCombat(true);
    }
}