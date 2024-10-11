using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject DungeonUI;

    public int[] DungeonLevels = new int[3] { 1, 1, 1 };
    private int currentDungeonIndex;

    // 스페셜 스테이지 지속 시간
    public float playDuration = 10.0f;

    public int baseGoldAmount = 100000;
    private Coroutine goldCoroutine;
    private bool isSuccess;
    private int gainGold = 100000;               // 기본 골드 획득량
    // 임시 객체로 사용할 cat과 enemy 프리팹
    public Character catPrefab;
    public DungeonEnemy enemyPrefab;
    private Character catInstance;
    private DungeonEnemy enemyInstance;

    // 싱글톤 인스턴스
    public static DungeonManager Instance { get; private set; }

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
    public void StartDungeon(int index, int level)
    {
        if (GameManager.isDungeonActive)
        {
            Debug.Log("스페셜 스테이지가 이미 활성화되어 있습니다.");
            return;
        }

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
        GameManager.isDungeonActive = true;

        // 프리팹 인스턴스 생성
        catInstance = Instantiate(catPrefab, new Vector3(-10, 40, 0), Quaternion.identity).GetComponent<Character>();
        enemyInstance = Instantiate(enemyPrefab, new Vector3(5, 40, 0), Quaternion.identity).GetComponent<DungeonEnemy>();

        // 고양이와 적군을 전투 상태로 설정
        catInstance.SetEnemy(enemyInstance);
        enemyInstance.SetEnemy(catInstance);

        currentDungeonIndex = index;
        DungeonUI.SetActive(true);  // UI 활성화

        // 일정 시간 동안 골드를 획득하는 코루틴
        // goldCoroutine = StartCoroutine(GainGoldOverTime(level));

        DummyServerData.UseTicket(Player.GetUserID(), index); // 티켓 차감

        // 전투 결과 체크 시작
        StartCoroutine(CheckBattleOutcome());
        // 스테이지 제한 시간 설정
        Invoke("TimeOut", playDuration);
    }

    private IEnumerator GainGoldOverTime(int level)
    {
        while (GameManager.isDungeonActive)
        {
            Player.AddGold(baseGoldAmount * level);
            yield return new WaitForSeconds(gainGold);
        }
    }

    // 전투 결과 체크
    private IEnumerator CheckBattleOutcome()
    {
        while (GameManager.isDungeonActive)
        {
            // 적군이 죽으면 클리어
            if (enemyInstance != null && enemyInstance.IsDead())
            {
                isSuccess = true;
                EndDungeonStage();
                yield break;
            }

            // 플레이어가 죽으면 실패
            if (catInstance != null && catInstance.IsDead())
            {
                isSuccess = false;
                EndDungeonStage();
                yield break;
            }

            yield return null;
        }
    }

    // 제한 시간 초과 시 실패 처리
    private void TimeOut()
    {
        if (GameManager.isDungeonActive)
        {
            Debug.Log("제한 시간이 초과되었습니다.");
            isSuccess = false;
            EndDungeonStage(); // 실패 처리
        }
    }

    public void EndDungeonStage()
    {
        if (goldCoroutine != null)
            StopCoroutine(goldCoroutine);

        // 성공 처리
        if (isSuccess)
        {
            if (currentDungeonIndex >= 0 && currentDungeonIndex < DungeonLevels.Length)
            {
                DungeonLevels[currentDungeonIndex]++;
                Debug.Log($"던전 {currentDungeonIndex + 1} 클리어! 다음 레벨이 활성화됩니다.");
                Player.AddGold(DungeonLevels[currentDungeonIndex] * gainGold);

                var DungeonPanel = FindObjectOfType<DungeonPanel>();
                if (DungeonPanel != null)
                {
                    DungeonPanel.OnStageCleared(currentDungeonIndex, DungeonLevels[currentDungeonIndex]);
                }
            }
        }
        else
        {
            Debug.Log($"스페셜 스테이지 {currentDungeonIndex + 1} 실패.");
        }
        // 고양이, 적, 배경 등이 3초 후에 사라지도록 처리
        StartCoroutine(DestroyObjectsWithDelay(3.0f));  // 3초 대기
    }

    private IEnumerator DestroyObjectsWithDelay(float delay)
    {
        // 3초 대기
        yield return new WaitForSeconds(delay);

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
        DungeonUI.SetActive(false);

        GameManager.isDungeonActive = false;
    }
}

public class DungeonRewardManager
{
    private static DungeonRewardManager _instance;
    public static DungeonRewardManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DungeonRewardManager();
            }
            return _instance;
        }
    }

    public void GiveDungeonReward(string userID, int level)
    {
        // 보상 로직
        Debug.Log("유저 " + userID + "에게 레벨 " + level + " 보상을 지급합니다.");
    }
}