using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private ParallaxScrollingManager parallaxScrollingManager;

    [SerializeField]
    private RoundAutoManager roundAutoManager;

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

    [SerializeField]
    private AnimationManager animationManager;

    private int originalTheme;  // 원래 테마를 저장할 변수

    // TODO: 초기값 설정은 추후 NetworkManager에서 각 유저별 스테이지 로 받아오는 것으로 설정
    private int currentTheme = 1;
    private int currentStage = 1;
    private int currentGate = 1;

    // 특정 컨텐츠의 경우(보스 레이드 등) 관문이 하나만 있을 수 있으니 조정 가능한 변수로
    public int maxGateCount = 3;
    public int maxStageCount = 5;

    void Start()
    {
        //SetStageUI();
        SetNewStage(false);

        continuousCombatButton.onClick.AddListener(GoToLastClearStageNextStage);
    }

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

    public void GateClearAfterEnemyDeath(float waitTime)
    {
        StartCoroutine(GateClear(waitTime));
    }

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
        roundAutoManager.RotateCircle(true);

        // 애니메이션 실행
        if (animationManager != null)
        {
            animationManager.PlayAnimation(AnimationManager.AnimationState.Walk);
        }

        StartCoroutine(ArriveGate());
    }

    IEnumerator ArriveGate()
    {
        yield return new WaitForSeconds(1.0f);

        // 관문 index 1, 2, ..., maxGateCount 순환
        if (!Player.continuousCombat)
        {
            currentGate++;
        }

        fadeImage.gameObject.SetActive(false);
        GameManager.GetInstance().changeStageUI.SetChangeStageButtonInteractable(true);

        SetStageUI();

        RequestEnemySpawn();

        yield break;
    }

    void RequestEnemySpawn()
    {
        parallaxScrollingManager.MoveBackgroundSprites(false);
        roundAutoManager.RotateCircle(false);

        // 적군 생성
        if (enemySpawnManager != null)
        {
            enemySpawnManager.OnGatePassed(currentGate == maxGateCount); // 적을 스폰하도록 적 스폰 매니저 호출
                                                                         // 애니메이션 실행
            if (animationManager != null)
            {
                animationManager.PlayAnimation(AnimationManager.AnimationState.ATK1);
            }
        }
        else
        {
            Debug.LogError("EnemySpawnManager null. 적을 스폰할 수 없습니다.");
        }
    }

    private void StageClear()
    {
        SendStageClearDataToServer();
        if (animationManager != null && currentGate == maxGateCount)
        {
            animationManager.PlayAnimation(AnimationManager.AnimationState.Victory);
        }
        ChangeStage();
    }

    void SendStageClearDataToServer()
    {
        int clearTheme;
        int clearStage;
        Player.GetPlayerHighestClearStageData(out clearTheme, out clearStage);
        if (currentTheme > clearTheme || (currentTheme == clearTheme && currentStage > clearStage))
        {
            DummyServerData.PlayerClearStage(Player.GetUserID(), currentTheme, currentStage);
            Player.playerHighestClearStageData = new int[] { currentTheme, currentStage };
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
        if (animationManager != null)
        {
            animationManager.PlayAnimation(AnimationManager.AnimationState.Idle03);
        }
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
        RequestEnemySpawn();
        StopCoroutine(fadeCoroutine);
    }

    void SetNewStage(bool addStage)
    {
        if (stageSlider)
        {
            stageSlider.ClearGateImages();
        }

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

    private void ChangeStage()
    {
        fadeCoroutine = StartCoroutine(StartFade(() => FadeStartFuncWhileStageChange(true)));
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
    public void GoToLastClearStageNextStage()
    {
        int clearStageTheme = 1;
        int clearStage = 1;
        Player.GetPlayerHighestClearStageData(out clearStageTheme, out clearStage);
        SetContinuousCombat(false);

        enemySpawnManager.DestroyEnemy();
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
        StopCoroutine(fadeCoroutine);
        enemySpawnManager.DestroyEnemy();
        // 애니메이션 실행
        if (animationManager != null)
        {
            animationManager.PlayAnimation(AnimationManager.AnimationState.Alert);
        }
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
        // 애니메이션 실행
        if (animationManager != null)
        {
            animationManager.PlayAnimation(AnimationManager.AnimationState.DieA);
        }
        fadeCoroutine = StartCoroutine(StartFade(RespawnCat));
        SetContinuousCombat(true);
    }
}