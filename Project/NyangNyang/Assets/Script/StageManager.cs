using System.Collections;
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
    private Text GateUI;

    [SerializeField]
    private EnemySpawnManager enemySpawnManager;  // 적 스폰 매니저 변수

    public bool isSpecial = false;  // 스페셜 스테이지 여부를 저장하는 변수
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
        SetStageUI();
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
        ThemeUI.text = "THEME " + currentTheme.ToString();
        StageUI.text = "STAGE " + currentStage.ToString();
        GateUI.text = "GATE " + currentGate.ToString();
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
        parallaxScrollingManager.MoveBackgroundSprites(true);
        // TODO: 고양이 걷기 애니메이션

        StartCoroutine(ArriveGate());
    }

    IEnumerator ArriveGate()
    {
        yield return new WaitForSeconds(1.0f);

        // 관문 index 1, 2, ..., maxGateCount 순환
        currentGate++;

        SetStageUI();

        parallaxScrollingManager.MoveBackgroundSprites(false);

        // 적군 생성
        if (enemySpawnManager != null)
        {
            enemySpawnManager.OnGatePassed(); // 적을 스폰하도록 적 스폰 매니저 호출
        }
        else
        {
            Debug.LogError("EnemySpawnManager null. 적을 스폰할 수 없습니다.");
        }

        yield break;
    }

    private void StageClear()
    {
        Debug.Log("최고 단계 관문 클리어, 스테이지 이동");
        if (currentStage >= maxStageCount)
        {
            ChangeTheme();
        }
        else
        {
            ChangeStage();
        }

    }

    private void ChangeStage()
    {
        // TODO : 추후 페이드 기법을 통해 변하게 진행, 현재는 그냥 바로 화면이 변경되도록
        currentGate = 1;
        currentStage++;

        SetStageUI();
    }

    private void ChangeTheme()
    {
        // TODO : 추후 페이드 기법을 통해 변하게 진행, 현재는 그냥 바로 화면이 변경되도록
        parallaxScrollingManager.ChangeBackgroundImageFromPrefab(currentTheme);
        currentTheme++;
        currentStage = 1;
        currentGate = 1;

        SetStageUI();
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

}