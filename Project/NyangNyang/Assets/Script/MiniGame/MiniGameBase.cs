using UnityEngine;

public abstract class MiniGameBase : MonoBehaviour
{
    protected string gameName;
    protected int rewardTicketIndex; // �������� ���� ������ �ε���
    protected bool isGameCleared;         // ���� Ŭ���� ����

    // �̴ϰ��� �ʱ�ȭ
    protected void Initialize(string gameName, int rewardTicketIndex)
    {
        this.gameName = gameName;
        this.rewardTicketIndex = rewardTicketIndex;
        GameManager.isMiniGameActive = false;
        isGameCleared = false;
    }

    // �̴ϰ��� ����
    public void StartGame()
    {
        GameManager.isMiniGameActive = true;
        StartGameLogic();
    }

    // �̴ϰ��� Ŭ���� ó��
    protected void ClearGame()
    {
        isGameCleared = true;
        GameManager.isMiniGameActive = false;
        RewardSweepTicket();
        EndGameLogic();
    }

    // Ŭ���� �� ������ ���� ����
    private void RewardSweepTicket()
    {
        DummyServerData.AddTicket(Player.GetUserID(), rewardTicketIndex, 1);
        DummyServerData.GetTicketCount(Player.GetUserID(),rewardTicketIndex);
    }

    // �̴ϰ��� ���� ���� (�� �̴ϰ��ӿ��� ����)
    protected abstract void StartGameLogic();

    // �̴ϰ��� ���� ���� (�� �̴ϰ��ӿ��� ����)
    protected abstract void EndGameLogic();
}
