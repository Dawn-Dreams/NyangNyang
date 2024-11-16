using System;
using System.Collections;
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

    public int[] DungeonLevels = new int[3] { 1, 1, 1 };
    private int currentDungeonIndex;

    // 스페셜 스테이지 지속 시간
    public float playDuration = 10.0f;

    public int baseGoldAmount = 100000;
    private Coroutine goldCoroutine;
    private bool isSuccess;
    private int gainGold = 100000;               // 기본 골드 획득량
    // 임시 객체로 사용할 cat과 enemy 프리팹
    public Cat catPrefab;
    public DungeonBossEnemy enemyPrefab;
    private Cat catInstance;
    private DungeonBossEnemy enemyInstance;

    // 싱글톤 인스턴스
    public static DungeonManager Instance { get; private set; }

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 필요한 경우
        }
        else
        {
            Destroy(gameObject);
            return; // 기존 인스턴스가 있으므로 초기화 코드 실행을 방지
        }

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
        if (!DummyServerData.HasTicket(Player.GetUserID(), index))
        {
            Debug.Log("소탕권이 부족하여 스페셜 스테이지를 시작할 수 없습니다.");
            return;
        }

        // 이전 Invoke 호출이 있을 경우 취소
        CancelInvoke("TimeOut");

        // 던전 활성화
        GameManager.isDungeonActive = true;

        ShowDungeonResultText("START!!", 1);
        // index에 맞는 UI 패널만 활성화
        for (int i = 0; i < dungeonPanels.Length; i++)
        {
            dungeonPanels[i].SetActive(i == index); // 현재 선택한 index에 해당하는 패널만 활성화
        }

        // 프리팹 인스턴스 생성
        catInstance = Instantiate(catPrefab, new Vector3(-10, 40, 0), Quaternion.identity).GetComponent<Cat>();
        enemyInstance = Instantiate(enemyPrefab, new Vector3(10, 40, 0), Quaternion.identity).GetComponent<DungeonBossEnemy>();
        
        // 적의 생명력, 공격력, 공격 패턴 설정 (index와 level에 따라 다르게 설정)
        enemyInstance.InitializeEnemyStats(index, level);
        InitializeClonedCat(catInstance);


        currentDungeonIndex = index;
        DungeonUI.SetActive(true);  // UI 활성화

        DummyServerData.UseTicket(Player.GetUserID(), index); // 티켓 차감

        StartCoroutine(StartCombatAfterDelay(1.0f));
        StartCoroutine(CheckBattleOutcome());
        // 스테이지 제한 시간 설정
        Invoke("TimeOut", playDuration);
    }

    // 일정 시간 딜레이 후 전투 시작
    private IEnumerator StartCombatAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // 전투 시작
        catInstance.isIndependent = true;       // CombatManager 무관하게 작동하도록
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
            ShowDungeonResultText("TIME OUT...", 2);
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
                ShowDungeonResultText("CLEAR!!", 2);
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
            ShowDungeonResultText("FAIL...", 2);
        }

        StartCoroutine(DestroyObjectsWithDelay(3.0f));
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
public class DungeonRewardManager
{
    public void GiveDungeonReward(string userID, int index, int level)
    {
        int goldReward = CalculateGoldReward(index, level);
        int itemReward = CalculateItemReward(index, level);

        // 보상 지급
        Player.AddGold(goldReward);
        //Player.AddItem(itemReward); // 아이템 보상... 추가 예정

        Debug.Log($"유저 {userID}에게 던전 {index + 1} 레벨 {level} 보상을 지급했습니다. 골드: {goldReward}, 아이템: {itemReward}");
    }

    private int CalculateGoldReward(int index, int level)
    {
        // index와 level에 따라 보상을 다르게 설정
        return 10000 * (index + 1) * level; // 기본 골드 보상 예시
    }

    private int CalculateItemReward(int index, int level)
    {
        // 특정 레벨 도달 시 아이템 보상 지급 로직
        return (index + 1) * level; // 아이템 보상 예시
    }
}


