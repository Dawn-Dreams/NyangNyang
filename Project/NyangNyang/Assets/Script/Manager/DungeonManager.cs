using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject DungeonUI;
    private GameObject[] dungeonPanels;
    private TextMeshProUGUI DungeonResultText;

    public int[] dungeonHighestClearLevel = new int[3] { 1, 1, 1 };
    public int currentDungeonLevel;
    private int currentDungeonIndex;

    // 스페셜 스테이지 지속 시간
    public float playDuration = 10.0f;

    public int baseGoldAmount = 100000;
    private Coroutine goldCoroutine;
    private bool isSuccess;

    // 임시 객체로 사용할 cat과 enemy 프리팹
    public Cat catPrefab;
    public DungeonBossEnemy enemyPrefab;
    private Cat catInstance;
    private DungeonBossEnemy enemyInstance;

    // 싱글톤 인스턴스
    public static DungeonManager Instance { get; private set; }
    private void Start()
    {
        LoadAndAssignDungeonLevel();
    }

    private void Awake()
    {
        if (transform.parent != null)
        {
            // 부모가 있는 경우, 루트로 이동
            transform.parent = null;
        }

        DontDestroyOnLoad(gameObject);

        // DungeonUI >> Panel >> Dungeon 하위에서 Dungeon 패널들을 동적으로 찾음
        Transform panelTransform = DungeonUI.transform.Find("Panel");
        if (panelTransform != null)
        {
            dungeonPanels = new GameObject[3];
            for (int i = 0; i < 3; i++)
            {
                dungeonPanels[i] = panelTransform.Find($"Dungeon ({i})").gameObject;  // Dungeon[0], Dungeon[1], Dungeon[2]을 찾음
            }
        }
        else
        {
            Debug.LogError("Panel을 찾을 수 없습니다.");
        }

        // DungeonUI >> Panel >> DungeonResultText 동적으로 찾음
        DungeonResultText = panelTransform.Find("DungeonResultText").GetComponent<TextMeshProUGUI>();
        if (DungeonResultText == null)
        {
            Debug.LogError("DungeonResultText를 찾을 수 없습니다.");
        }

        
    }
    public void LoadAndAssignDungeonLevel()
    {
        // SaveLoadManager를 통해 친구 데이터 로드
        List<FriendData> friends = SaveLoadManager._instance.LoadFriends();

        if (friends != null && friends.Count > 0)
        {
            // 1번 친구의 레벨을 가져옴
            int friendLevel = friends[0].friendLevel;

            // dungeonHighestClearLevel[0]에 값 할당
            dungeonHighestClearLevel[0] = friendLevel;
            Debug.Log($"1번 친구의 레벨: {friendLevel}, dungeonHighestClearLevel[0]에 할당 완료.");
        }
        else
        {
            Debug.Log("로드된 친구 데이터가 없습니다. dungeonHighestClearLevel[0]을 기본값으로 유지합니다.");
        }
    }

    private void SaveFriendLevelForDungeon()
    {
        FriendData myFriend = new FriendData(0, "Me", dungeonHighestClearLevel[0]);
        List<FriendData> friendsList = new List<FriendData> { myFriend };
        SaveLoadManager.GetInstance().SaveFriends(friendsList);

    }
    private void InitializeClonedCat(Cat clone)
    {
        // 복제본 초기화 (원본 고양이의 Awake 메서드와 동일한 초기화 작업 수행)
        clone.characterID = Player.GetUserID();
        clone.status = Player.playerStatus;
        Player.playerStatus.OnStatusLevelChange += clone.HPLevelChanged;
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

        if (!DummyServerData.HasShell(Player.GetUserID(), index))
        {
            Debug.Log("소탕권이 부족하여 스페셜 스테이지를 시작할 수 없습니다.");
            return;
        }

        // 이전 Invoke 호출 취소
        CancelInvoke("TimeOut");

        // 던전 활성화
        GameManager.isDungeonActive = true;

        ShowDungeonResultText("START!!", 1);

        for (int i = 0; i < dungeonPanels.Length; i++)
        {
            dungeonPanels[i].SetActive(i == index);
        }

        // 프리팹 인스턴스 생성
        catInstance = Instantiate(catPrefab, new Vector3(-10, 40, 0), Quaternion.identity).GetComponent<Cat>();
        enemyInstance = Instantiate(enemyPrefab, new Vector3(10, 40, 0), Quaternion.identity).GetComponent<DungeonBossEnemy>();

        // 적 초기화
        enemyInstance.InitializeBossForDungeon(index, level); // 보스 초기화
        InitializeClonedCat(catInstance); // 플레이어 캐릭터 초기화

        currentDungeonIndex = index;
        DungeonUI.SetActive(true);

        Player.SetShell(index, Player.GetShell(index) - 1);
        StartCoroutine(StartCombatAfterDelay(1.0f));
        StartCoroutine(CheckBattleOutcome());
        Invoke("TimeOut", playDuration); // 제한 시간 초과 시 처리
    }

    // 일정 시간 딜레이 후 전투 시작
    private IEnumerator StartCombatAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (catInstance == null || enemyInstance == null)
        {
            Debug.LogError("객체 초기화 실패! 전투를 시작할 수 없습니다.");
            EndDungeonStage();
            yield break;
        }

        // 전투 시작 준비
        if (enemyInstance.IsDead())
        {
            Debug.LogError("보스가 전투 시작 전에 사망 상태로 감지되었습니다!");
            EndDungeonStage();
            yield break;
        }

        catInstance.isIndependent = true;
        enemyInstance.isIndependent = true;
        catInstance.SetEnemy(enemyInstance);
        enemyInstance.SetEnemy(catInstance);
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
            ShowDungeonResultText("<color=#E5E1DA>TIMEOUT</color>", 2);
            isSuccess = false;
            EndDungeonStage(); // 실패 처리
        }
    }

    public void EndDungeonStage()
    {
        LoadAndAssignDungeonLevel();
        if (goldCoroutine != null)
            StopCoroutine(goldCoroutine);

        // 성공 처리
        if (isSuccess)
        {
            // 현재 최고 단계 클리어 시
            if (currentDungeonLevel == dungeonHighestClearLevel[currentDungeonIndex])
            {
                dungeonHighestClearLevel[currentDungeonIndex]++;
                ShowDungeonResultText($"<color=#BFECFF>{currentDungeonLevel} CLEAR!!</color>", 2);
            }
            else
            {
                ShowDungeonResultText($"<color=#BFECFF>CLEAR!!</color>", 2);
            }

            var DungeonPanel = FindObjectOfType<DungeonPanel>();
            if (DungeonPanel != null)
            {
                DungeonPanel.OnStageCleared(currentDungeonIndex, dungeonHighestClearLevel[currentDungeonIndex]);
            }
            catInstance.animationManager.PlayAnimation(AnimationManager.AnimationState.Victory);
            enemyInstance._dummyEnemies[0].animationManager.PlayAnimation(AnimationManager.AnimationState.DieA);
        }
        else
        {
            catInstance.animationManager.PlayAnimation(AnimationManager.AnimationState.DieB);
            enemyInstance._dummyEnemies[0].animationManager.PlayAnimation(AnimationManager.AnimationState.Victory);
            ShowDungeonResultText("<color=#E5E1DA>FAIL...</color>", 2);
        }
        StopCombatActions();
        SaveFriendLevelForDungeon();
        StartCoroutine(DestroyObjectsWithDelay(3.0f));
    }

    // 공격 동작 멈추기
    private void StopCombatActions()
    {
        if (catInstance != null)
        {
            catInstance.StopAllCoroutines();  // 모든 코루틴 중단
        }

        if (enemyInstance != null)
        {
            enemyInstance.StopAllCoroutines();  // 모든 코루틴 중단
        }
    }
    private IEnumerator DestroyObjectsWithDelay(float delay)
    {
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

    // DungeonResultText
    public void ShowDungeonResultText(string message, float displayDuration)
    {
        DungeonResultText.text = message;
        DungeonResultText.gameObject.SetActive(true);
        StartCoroutine(DisableDungeonResultTextAfterDelay(displayDuration));
    }

    private IEnumerator DisableDungeonResultTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DungeonResultText.gameObject.SetActive(false);
    }
}
