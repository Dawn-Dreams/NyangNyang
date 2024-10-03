using UnityEngine;

public abstract class MiniGameBase : MonoBehaviour
{
    protected string gameName;
    protected int rewardTicketIndex; // 보상으로 받을 소탕권 인덱스
    protected bool isGameCleared;         // 게임 클리어 여부

    // 미니게임 초기화
    protected void Initialize(string gameName, int rewardTicketIndex)
    {
        this.gameName = gameName;
        this.rewardTicketIndex = rewardTicketIndex;
        GameManager.isMiniGameActive = false;
        isGameCleared = false;
    }

    // 미니게임 시작
    public void StartGame()
    {
        GameManager.isMiniGameActive = true;
        StartGameLogic();
    }

    // 미니게임 클리어 처리
    protected void ClearGame()
    {
        isGameCleared = true;
        GameManager.isMiniGameActive = false;
        RewardSweepTicket();
        EndGameLogic();
    }

    // 클리어 시 소탕권 보상 지급
    private void RewardSweepTicket()
    {
        DummyServerData.AddTicket(Player.GetUserID(), rewardTicketIndex, 1);
        DummyServerData.GetTicketCount(Player.GetUserID(),rewardTicketIndex);
    }

    // 미니게임 시작 로직 (각 미니게임에서 구현)
    protected abstract void StartGameLogic();

    // 미니게임 종료 로직 (각 미니게임에서 구현)
    protected abstract void EndGameLogic();
}
