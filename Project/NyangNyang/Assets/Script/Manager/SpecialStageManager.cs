using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class SpecialStageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject specialStageUI;
    private StageManager stageManager;

    public int[] specialStageLevels = new int[3] { 1, 1, 1 };
    private int currentSpecialStageIndex;
    private int originalBackground;             // 원래 배경 테마 저장

    public float playSec = 13.0f;               // 스페셜 스테이지 지속 시간
    public float playDuration = 10.0f;
    public float goldInterval = 0.5f;
    public int baseGoldAmount = 100000;
    private Coroutine goldCoroutine;
    private bool isSuccess;

    // 난이도 배열
    private int[] stageDifficulties = new int[3] { 10, 15, 20 };

    // 임시 객체로 사용할 cat과 enemy 프리팹
    public Character playerPrefab;
    public Enemy enemyPrefab;
    private Character playerInstance;
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

    private void Start()
    {
        stageManager = FindObjectOfType<StageManager>() ?? throw new NullReferenceException("StageManager is missing.");
    }

    public void EndSpecialStage(bool playerWon)
    {
        if (playerWon)
        {
            Debug.Log("클리어!");
        }
        else
        {
            Debug.Log("실패");
        }

        // 객체 정리 (cat과 enemy 삭제)
        if (playerInstance != null) Destroy(playerInstance);
        if (enemyInstance != null) Destroy(enemyInstance);
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

        // 난이도 체크: 플레이어의 공격력이 난이도보다 낮으면 실패
        if (Player.playerStatus.attackPower < stageDifficulties[index])
        {
            Debug.Log("플레이어의 공격력이 부족하여 스페셜 스테이지 실패.");
            isSuccess = false;
            EndSpecialStage(); // 실패 처리
            return;
        }

        // 던전 활성화
        GameManager.isSpecialStageActive = true;

        // 고양이 프리팹 인스턴스 생성
        playerInstance = Instantiate(playerPrefab, new Vector3(-2, 0, 0), Quaternion.identity).GetComponent<Character>();
        Debug.Log("고양이 생성 완료.");

        // 적군 프리팹 인스턴스 생성
        enemyInstance = Instantiate(enemyPrefab, new Vector3(2, 0, 0), Quaternion.identity).GetComponent<Enemy>();
        Debug.Log("적군 생성 완료.");

        // 적군의 수를 설정 (1마리 또는 여러 마리 가능)
        enemyInstance.SetNumberOfEnemyInGroup(3); // 여기서 enemyInstance에 대해 메서드를 호출

        // 고양이와 적군을 전투 상태로 설정
        playerInstance.SetEnemy(enemyInstance);
        enemyInstance.SetEnemy(playerInstance);

        currentSpecialStageIndex = index;
        specialStageUI.SetActive(true);  // UI 활성화

        if (stageManager != null)
        {
            // originalBackground = stageManager.GetCurrentTheme();
            // stageManager.ChangeBackgroundToSpecialStage(index + 6);
        }

        goldCoroutine = StartCoroutine(GainGoldOverTime(level));
        DummyServerData.UseTicket(Player.GetUserID(), index); // 티켓 차감

        // 전투 결과 감시 시작
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
                EndSpecialStage(true); // 클리어 처리
                yield break;
            }

            // 플레이어가 죽으면 실패
            if (playerInstance != null && playerInstance.IsDead())
            {
                EndSpecialStage(false); // 실패 처리
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
            EndSpecialStage(false); // 실패 처리
        }
    }
    // 스페셜 스테이지 종료 메소드
    public void EndSpecialStage()
    {
        GameManager.isSpecialStageActive = false;
        specialStageUI.SetActive(false);

        if (goldCoroutine != null) StopCoroutine(goldCoroutine);

        // 성공 처리
        if (isSuccess)
        {
            if (currentSpecialStageIndex >= 0 && currentSpecialStageIndex < specialStageLevels.Length)
            {
                specialStageLevels[currentSpecialStageIndex]++;
                stageDifficulties[currentSpecialStageIndex] *= 2;
                Debug.Log($"스페셜 스테이지 {currentSpecialStageIndex + 1} 클리어! 다음 레벨이 활성화됩니다.");
                if (playerInstance != null) Destroy(playerInstance);
                if (enemyInstance != null) Destroy(enemyInstance);
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
            if (playerInstance != null) Destroy(playerInstance);
            if (enemyInstance != null) Destroy(enemyInstance);
        }

        // cat과 enemy 객체 삭제
        if (playerInstance != null) Destroy(playerInstance);
        if (enemyInstance != null) Destroy(enemyInstance);
    }
}
