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

    public bool isSpecial = false;  // 스페셜 스테이지 여부를 저장하는 변수
    private int originalTheme;  // 원래 테마를 저장할 변수

    // TODO: 초기값 설정은 추후 NetworkManager에서 각 유저별 스테이지 로 받아오는 것으로 설정
    private int currentTheme = 5;
    private int currentStage = 5;
    private int currentGate = 1;

    // 특정 컨텐츠의 경우(보스 레이드 등) 관문이 하나만 있을 수 있으니 조정 가능한 변수로
    public int maxGateCount = 3;
    public int maxStageCount = 5;

    void Start()
    {
        SetStageUI();

        continuousCombatButton.onClick.AddListener(GoToLastClearStageNextStage);
    }

    void Update()
    {
        // TODO: 임시 테스트 코드, 추후 조정 필요
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    GateClear();
        //}

        if (isSpecial)
        {
            // 스페셜 스테이지일 경우, cat이 계속 앞으로 이동
            MoveCatForward();
        }
    }

    private void SetStageUI()
    {
        ThemeUI.text = currentTheme.ToString();
        StageUI.text = "- " +currentStage.ToString();
        if (stageSlider)
        {
            stageSlider.CreateGateImage(maxGateCount);
        }
    }

    private IEnumerator GateClear(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (isSpecial)
        {
            Debug.Log("스페셜 스테이지에서는 관문을 통과하지 않습니다.");
            yield break; // 스페셜 스테이지일 경우, 관문 통과 시스템 비활성화
        }

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
            stageSlider.MoveToNextGate(currentGate,maxGateCount,1.0f);
        }
        parallaxScrollingManager.MoveBackgroundSprites(true);
        // TODO: 고양이 걷기 애니메이션

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
        
        SetStageUI();

        RequestEnemySpawn();
        yield break;
    }

    void RequestEnemySpawn()
    {
        parallaxScrollingManager.MoveBackgroundSprites(false);

        // 적군 생성
        if (enemySpawnManager != null)
        {
            enemySpawnManager.OnGatePassed(currentGate == maxGateCount); // 적을 스폰하도록 적 스폰 매니저 호출
        }
        else
        {
            Debug.LogError("EnemySpawnManager null. 적을 스폰할 수 없습니다.");
        }
    }

    private void StageClear()
    {
        Debug.Log("최고 단계 관문 클리어, 스테이지 이동");
        ChangeStage();
    }

    private IEnumerator StartFade(Action FuncAfterStartFade)
    {
        currentFadeTime = 0.0f;
        fadeImage.gameObject.SetActive(true);
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
            }

            yield return null;
        }
    }

    void FadeStartFuncWhileStageChange()
    {
        AddStage();
        StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(EndFade(FadeEndFuncWhileStageChange));
    }

    void FadeEndFuncWhileStageChange()
    {
        RequestEnemySpawn();
        StopCoroutine(fadeCoroutine);
    }

    void AddStage()
    {
        if (stageSlider)
        {
            stageSlider.ClearGateImages();
        }

        currentGate = 1;
        currentStage += 1;
        if (currentStage > maxStageCount)
        {
            parallaxScrollingManager.ChangeBackgroundImageFromPrefab(currentTheme);
            currentStage = 1;
            currentTheme += 1;
        }

        SetStageUI();
    }

    private void ChangeStage()
    {
        fadeCoroutine = StartCoroutine(StartFade(FadeStartFuncWhileStageChange));
    }

    // 스페셜 스테이지에서 고양이가 계속 앞으로 이동하는 함수
    private void MoveCatForward()
    {
        Cat cat = GameManager.GetInstance().catObject;
        if (cat != null)
        {
            parallaxScrollingManager.MoveBackgroundSprites(true);
        }
    }

    public void StopSpecialStage()
    {
        parallaxScrollingManager.MoveBackgroundSprites(false);
    }

    // 현재 테마를 반환하는 함수
    public int GetCurrentTheme()
    {
        return currentTheme;
    }

    // 스페셜 스테이지로 배경 테마 변경
    public void ChangeBackgroundToSpecialStage(int theme)
    {
        parallaxScrollingManager.ChangeBackgroundImageFromPrefab(theme); // 배경 이미지를 스페셜 테마로 변경
        SetStageUI();  // UI 업데이트
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
        DummyServerData.GetUserClearStageData(Player.GetUserID(), out clearStageTheme, out clearStage);
        SetContinuousCombat(false);

        enemySpawnManager.DestroyEnemy();
        currentTheme = clearStageTheme;
        currentStage = clearStage;
        StageClear();
    }


    void RespawnCat()
    {
        GameManager.GetInstance().catObject.CatRespawn();
        StopCoroutine(fadeCoroutine);
        enemySpawnManager.DestroyEnemy();

        currentStage -= 1;
        if (currentStage <= 0)
        {
            currentTheme -= 1;
            parallaxScrollingManager.ChangeBackgroundImageFromPrefab(currentTheme);
            currentStage = maxStageCount;
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