using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

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

    // TODO: 초기값 설정은 추후 NetworkManager에서 각 유저별 스테이지 로 받아오는 것으로 설정
    private int currentTheme = 1;
    private int currentStage = 1;
    private int currentGate = 1;

    // 특정 컨텐츠의 경우(보스 레이드 등) 관문이 하나만 있을 수 있으니 조정 가능한 변수로
    public int maxGateCount = 3;
    public int maxStageCount = 5;

    void Update()
    {
        // TODO: 임시 테스트 코드, 추후 조정 필요
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GateClear();
        }
        
    }

    private void SetStageUI()
    {
        ThemeUI.text = "THEME " + currentTheme.ToString();
        StageUI.text = "STAGE " + currentStage.ToString();
        GateUI.text = "GATE " + currentGate.ToString();
    }

    private void GateClear() 
    {
        // Debug.Log("관문 (" + currentTheme + " - " + currentStage + " - " + currentGate + ") 클리어");
        
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

    private void GoToNextGate()
    {
        Debug.Log("다음 관문으로 이동 시작");
        
        parallaxScrollingManager.MoveBackgroundSprites(true);
        // TODO: 고양이 걷기 애니메이션

        StartCoroutine(ArriveGate());
    }

    IEnumerator ArriveGate()
    {
        yield return new WaitForSeconds(0.5f);

        // 관문 index 1, 2, ..., maxGateCount 순환
        currentGate++;

        SetStageUI();
        Debug.Log("관문 도착");

        parallaxScrollingManager.MoveBackgroundSprites(false);
        // TODO : 적군이 생성 되도록

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
        // Debug.Log("새 스테이지 (" + currentTheme + " - " + currentStage + " - " + currentGate + ") 도착");
    }

    private void ChangeTheme()
    {
        // TODO : 추후 페이드 기법을 통해 변하게 진행, 현재는 그냥 바로 화면이 변경되도록
        parallaxScrollingManager.ChangeBackgroundImageFromPrefab();
        currentTheme++;
        currentStage = 1;
        currentGate = 1;

        SetStageUI();
        // Debug.Log("새 스테이지테마 (" + currentTheme + " - " + currentStage + " - " + currentGate + ") 도착");
    }
   
}
