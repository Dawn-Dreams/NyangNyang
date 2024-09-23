using UnityEngine;

public abstract class MiniGameBase : MonoBehaviour
{
    protected string gameName;
    protected int rewardSweepTicketIndex; // �������� ���� ������ �ε���
    protected bool isGameCleared;         // ���� Ŭ���� ����

    // �̴ϰ��� �ʱ�ȭ
    protected void Initialize(string gameName, int rewardSweepTicketIndex)
    {
        this.gameName = gameName;
        this.rewardSweepTicketIndex = rewardSweepTicketIndex;
        isGameCleared = false;

        Debug.Log($"{gameName} �ʱ�ȭ �Ϸ�. ���� ������ �ε���: {rewardSweepTicketIndex}");
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
        DummyServerData.AddSweepTicket(Player.GetUserID(), rewardSweepTicketIndex, 1);
        Debug.Log($"�̴ϰ��� Ŭ����! ������ {rewardSweepTicketIndex}���� 1�� ȹ���߽��ϴ�.");
    }

    // �̴ϰ��� ���� ���� (�� �̴ϰ��ӿ��� ����)
    protected abstract void StartGameLogic();

    // �̴ϰ��� ���� ���� (�� �̴ϰ��ӿ��� ����)
    protected abstract void EndGameLogic();
}
