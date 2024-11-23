using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public abstract class MiniGameBase : MonoBehaviour
{
    protected string gameName;
    protected int rewardShellIndex; // 보상으로 받을 소탕권 인덱스
    protected bool isGameCleared;    // 게임 클리어 여부
    private int baseReward;

    private EventSystem mainSceneEventSystem;  // 원래 씬의 EventSystem 참조용
    public AudioClip bgmSource;        // 각 게임별 배경음악
    private int score;

    // 점수를 관리하는 속성
    public int Score
    {
        get => score;
        protected set => score = value;
    }

    // 점수 반환 메서드
    public virtual int GetScore()
    {
        return score;
    }

    protected void Initialize(string gameName, int rewardShellIndex, int initialScore)
    {
        this.gameName = gameName;
        this.rewardShellIndex = rewardShellIndex;
        GameManager.isMiniGameActive = true;
        isGameCleared = false;
        Score = initialScore;
        AudioManager.Instance.PauseBGM();
        AudioManager.Instance.PlayMiniGameBGM();
        baseReward = 100;
        //DisableMainSceneEventSystem();  // 원래 씬의 EventSystem 비활성화
    }

    private void OnDestroy()
    {

    }

    private void DisableMainSceneEventSystem()
    {
        // 샘플 씬의 EventSystem을 비활성화하고 미니게임 전용 EventSystem 생성
        mainSceneEventSystem = FindObjectOfType<EventSystem>();
        if (mainSceneEventSystem != null)
        {
            mainSceneEventSystem.gameObject.SetActive(false);
        }

        // 미니게임에 EventSystem이 없다면 새로 생성
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject newEventSystem = new GameObject("EventSystem");
            newEventSystem.AddComponent<EventSystem>();
            newEventSystem.AddComponent<StandaloneInputModule>();
        }
    }

    private void EnableMainSceneEventSystem()
    {
        // 원래 씬의 EventSystem을 다시 활성화
        if (mainSceneEventSystem != null)
        {
            mainSceneEventSystem.gameObject.SetActive(true);
        }
    }

    // 미니게임 시작
    public void StartGame()
    {
        GameManager.isMiniGameActive = true;
    }

    // 미니게임 클리어 처리
    protected void ClearGame()
    {
        isGameCleared = true;
        GameManager.isMiniGameActive = false;
        AudioManager.Instance.StopMiniGameBGM();
        AudioManager.Instance.ResumeBGM();
        //EnableMainSceneEventSystem();  // 미니게임이 종료되면 원래 씬의 EventSystem 활성화
        RewardCheese(Score);     
        UnloadMiniGameScene("MiniGame1");
    }

     protected void EndGame()
    {
        GameManager.isMiniGameActive = false;
        AudioManager.Instance.StopMiniGameBGM();
        AudioManager.Instance.ResumeBGM(); 
        UnloadMiniGameScene("MiniGame1");
    }


    // 점수에 따른 가중치 테이블
    private readonly Dictionary<int, float> scoreWeightTable = new Dictionary<int, float>
{
    { 0, 1.0f },   // 0점 이상 10점 미만은 가중치 1.0
    { 10, 1.5f },  // 10점 이상 20점 미만은 가중치 1.5
    { 20, 2.0f },  // 20점 이상은 가중치 2.0
};

    // 클리어 시 Cheese 보상 지급
    private void RewardCheese(int score)
    {
        // 점수에 따른 가중치 계산
        float weight = CalculateWeight(score);

        // 보상 값 계산
        int rewardValue = Mathf.CeilToInt(baseReward * weight);

        // 플레이어에게 보상 지급
        Player.Cheese += rewardValue;

        Debug.Log($"Score: {score}, Weight: {weight}, Reward: {rewardValue}");
    }

    // 점수에 따른 가중치 계산 함수
    private float CalculateWeight(int score)
    {
        float weight = 1.0f;

        foreach (var entry in scoreWeightTable)
        {
            if (score >= entry.Key)
            {
                weight = entry.Value;
            }
            else
            {
                break;
            }
        }

        return weight;
    }

    protected void UnloadMiniGameScene(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName).completed += (operation) =>
            {
                Debug.Log($"{sceneName} 씬이 성공적으로 언로드되었습니다.");
            };
        }
        else
        {
            Debug.LogWarning($"{sceneName} 씬이 로드되어 있지 않습니다.");
        }
    }
    // 미니게임 시작 로직 (각 미니게임에서 구현)
    protected abstract void StartGameLogic();

    // 미니게임 종료 로직 (각 미니게임에서 구현)
    protected abstract void EndGameLogic();
}
