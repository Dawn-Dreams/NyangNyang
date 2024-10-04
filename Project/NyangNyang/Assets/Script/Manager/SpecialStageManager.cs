using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class SpecialStageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject specialStageUI;

    public int[] specialStageLevels = new int[3] { 1, 1, 1 };
    private int currentSpecialStageIndex;

    public float playSec = 13.0f;               // 스페셜 스테이지 지속 시간
    public float playDuration = 10.0f;
    public float goldInterval = 0.5f;
    public int baseGoldAmount = 100000;
    private Coroutine goldCoroutine;
    private bool isSuccess;
    private int gainGold = 100000;               // 기본 골드 획득량
    // 임시 객체로 사용할 cat과 enemy 프리팹
    public Character catPrefab;
    public Enemy enemyPrefab;
    private Character catInstance;
    private Enemy enemyInstance;

    // 싱글톤 인스턴스
    public static SpecialStageManager Instance { get; private set; }

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // 스페셜 스테이지 시작
    public void StartSpecialStage(int index, int level)
    {
        // 스페셜 스테이지가 이미 진행 중인지 확인
        if (GameManager.isSpecialStageActive)
        {
            Debug.LogWarning("스페셜 스테이지가 이미 활성화되어 있습니다.");
            return;
        }

        // 미니게임이 실행 중이면 스페셜 스테이지를 시작할 수 없음
        if (GameManager.isMiniGameActive)
        {
            Debug.Log("미니게임이 실행 중이므로 스페셜 스테이지를 시작할 수 없습니다.");
            return;
        }

        // 티켓 확인
        if (!DummyServerData.HasTicket(Player.GetUserID(), index))
        {
            Debug.Log("소탕권이 부족하여 스페셜 스테이지를 시작할 수 없습니다.");
            return;
        }
        // 이전 Invoke 호출이 있을 경우 취소
        CancelInvoke("TimeOut");

        // 던전 활성화
        GameManager.isSpecialStageActive = true;

        // 프리팹 인스턴스 생성
        catInstance = Instantiate(catPrefab, new Vector3(-2, 0, 0), Quaternion.identity).GetComponent<Character>();
        enemyInstance = Instantiate(enemyPrefab, new Vector3(2, 0, 0), Quaternion.identity).GetComponent<Enemy>();

        // 적군의 수
        enemyInstance.SetNumberOfEnemyInGroup(1);

        //enemyInstance.SetEnemyStatus(1);  // 레벨에 따라 적군의 스탯을 증가시킴
        // 고양이와 적군을 전투 상태로 설정
        catInstance.SetEnemy(enemyInstance);
        enemyInstance.SetEnemy(catInstance);

        currentSpecialStageIndex = index;
        specialStageUI.SetActive(true);  // UI 활성화

        // 일정 시간 동안 골드를 획득하는 코루틴
        // goldCoroutine = StartCoroutine(GainGoldOverTime(level));
        DummyServerData.UseTicket(Player.GetUserID(), index); // 티켓 차감

        // 전투 결과 체크 시작
        StartCoroutine(CheckBattleOutcome());
        // 스테이지 제한 시간 설정
        Invoke("TimeOut", playSec);
    }

    private IEnumerator GainGoldOverTime(int level)
    {
        while (GameManager.isSpecialStageActive)
        {
            Player.AddGold(baseGoldAmount * level);
            yield return new WaitForSeconds(goldInterval);
        }
    }

    // 전투 결과 체크
    private IEnumerator CheckBattleOutcome()
    {
        while (GameManager.isSpecialStageActive)
        {
            // 적군이 죽으면 클리어
            if (enemyInstance != null && enemyInstance.IsDead())
            {
                isSuccess = true;
                EndSpecialStage();
                yield break;
            }

            // 플레이어가 죽으면 실패
            if (catInstance != null && catInstance.IsDead())
            {
                isSuccess = false;
                EndSpecialStage();
                yield break;
            }

            yield return null;
        }
    }

    // 제한 시간 초과 시 실패 처리
    private void TimeOut()
    {
        if (GameManager.isSpecialStageActive)
        {
            Debug.Log("제한 시간이 초과되었습니다.");
            isSuccess = false;
            EndSpecialStage(); // 실패 처리
        }
    }

    public void EndSpecialStage()
    {
        GameManager.isSpecialStageActive = false;
        specialStageUI.SetActive(false);

        if (goldCoroutine != null)
            StopCoroutine(goldCoroutine);

        // 성공 처리
        if (isSuccess)
        {
            if (currentSpecialStageIndex >= 0 && currentSpecialStageIndex < specialStageLevels.Length)
            {
                specialStageLevels[currentSpecialStageIndex]++;
                Debug.Log($"스페셜 스테이지 {currentSpecialStageIndex + 1} 클리어! 다음 레벨이 활성화됩니다.");
                Player.AddGold(specialStageLevels[currentSpecialStageIndex]*gainGold);
                // SpecialStageMenuPanel에 클리어된 스테이지 업데이트
                var specialStageMenuPanel = FindObjectOfType<SpecialStageMenuPanel>();
                if (specialStageMenuPanel != null)
                {
                    specialStageMenuPanel.OnStageCleared(currentSpecialStageIndex, specialStageLevels[currentSpecialStageIndex]);
                }
            }
        }
        else
        {
            Debug.Log($"스페셜 스테이지 {currentSpecialStageIndex + 1} 실패.");
        }

        // cat과 enemy 객체 삭제
        if (catInstance != null)
        {
            Destroy(catInstance.gameObject);
            catInstance = null;
        }

        if (enemyInstance != null)
        {
            Destroy(enemyInstance.gameObject);
            enemyInstance = null;
        }
    }
}
