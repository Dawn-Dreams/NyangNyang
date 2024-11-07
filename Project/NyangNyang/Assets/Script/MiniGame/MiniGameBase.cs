using UnityEngine;
using UnityEngine.EventSystems;

public abstract class MiniGameBase : MonoBehaviour
{
    protected string gameName;
    protected int rewardTicketIndex; // 보상으로 받을 소탕권 인덱스
    protected bool isGameCleared;    // 게임 클리어 여부
    private EventSystem mainSceneEventSystem;  // 원래 씬의 EventSystem 참조용
    public AudioClip bgmSource;        // 각 게임별 배경음악

    protected void Initialize(string gameName, int rewardTicketIndex)
    {
        this.gameName = gameName;
        this.rewardTicketIndex = rewardTicketIndex;
        GameManager.isMiniGameActive = true;
        isGameCleared = false;
        AudioManager.Instance.StopMiniGameBGM();
        AudioManager.Instance.ResumeMainBGM();
        DisableMainSceneEventSystem();  // 원래 씬의 EventSystem 비활성화
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
        AudioManager.Instance.PauseMainBGM();
        AudioManager.Instance.PlayMiniGameBGM(bgmSource);
        EnableMainSceneEventSystem();  // 미니게임이 종료되면 원래 씬의 EventSystem 활성화
        RewardSweepTicket();
    }

    // 클리어 시 소탕권 보상 지급
    private void RewardSweepTicket()
    {
        DummyServerData.AddTicket(Player.GetUserID(), rewardTicketIndex, 1);
        DummyServerData.GetTicketCount(Player.GetUserID(), rewardTicketIndex);
    }

    // 미니게임 시작 로직 (각 미니게임에서 구현)
    protected abstract void StartGameLogic();

    // 미니게임 종료 로직 (각 미니게임에서 구현)
    protected abstract void EndGameLogic();
}
