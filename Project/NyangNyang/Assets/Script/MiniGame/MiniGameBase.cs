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
        isGameCleared = false;

        Debug.Log($"{gameName} �ʱ�ȭ �Ϸ�. ���� ������ �ε���: {rewardTicketIndex}");
    }

    // �̴ϰ��� ����
    public void StartGame()
    {
        StartGameLogic();
    }

    // �̴ϰ��� Ŭ���� ó��
    protected void ClearGame()
    {
        isGameCleared = true;
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
