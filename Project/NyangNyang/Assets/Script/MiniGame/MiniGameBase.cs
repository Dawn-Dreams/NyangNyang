using UnityEngine;

public abstract class MiniGameBase : MonoBehaviour
{
    protected string gameName;
    protected int rewardSweepTicketIndex; // 보상으로 받을 소탕권 인덱스
    protected bool isGameCleared;         // 게임 클리어 여부

    // 미니게임 초기화
    protected void Initialize(string gameName, int rewardSweepTicketIndex)
    {
        this.gameName = gameName;
        this.rewardSweepTicketIndex = rewardSweepTicketIndex;
        isGameCleared = false;

        Debug.Log($"{gameName} 초기화 완료. 보상 소탕권 인덱스: {rewardSweepTicketIndex}");
    }

    // 미니게임 시작
    public void StartGame()
    {
        StartGameLogic();
    }

    // 미니게임 클리어 처리
    protected void ClearGame()
    {
        isGameCleared = true;
        RewardSweepTicket();
        EndGameLogic();
    }

    // 클리어 시 소탕권 보상 지급
    private void RewardSweepTicket()
    {
        DummyServerData.AddSweepTicket(Player.GetUserID(), rewardSweepTicketIndex, 1);
        Debug.Log($"미니게임 클리어! 소탕권 {rewardSweepTicketIndex}번을 1개 획득했습니다.");
    }

    // 미니게임 시작 로직 (각 미니게임에서 구현)
    protected abstract void StartGameLogic();

    // 미니게임 종료 로직 (각 미니게임에서 구현)
    protected abstract void EndGameLogic();
}
