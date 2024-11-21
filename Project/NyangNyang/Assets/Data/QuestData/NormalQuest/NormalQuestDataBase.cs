using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public abstract class NormalQuestDataBase : QuestDataBase
{
    // 퀘스트 시작 시 실행되는 함수
    public override void QuestActing(BaseQuest quest, QuestType type)
    {
        base.QuestActing(quest, type);
        RequestCurrentUserQuestProgress();
    }

    // == 퀘스트 데이터에 대한 변수에 대한 Get (서버 : 반복퀘스트 클리어 체크 시) / (클라 : 서버에 현재 QuestValue 보낼 때 사용) ==
    public abstract int GetRequireCount();
    
    public abstract BigInteger GetCurrentQuestCount();
    // ===============================================

    // 퀘스트 클리어 버튼 이후 클라 내에서 값을 갱신시키는 함수
    public abstract void ChangeCurrentProgressCountAfterReward();

    // 현재 유저의 퀘스트 진행 정도를 받는 함수
    public virtual void RequestCurrentUserQuestProgress()
    {
        QuestManager.GetInstance().GetQuestProgressDataFromServer(questCategory, questType);
        CheckQuestClear();
    }

    // (클라->서버) 퀘스트 정보에 변화가 있을 경우 n 초 뒤 변경된 값을 보내는 함수
    protected IEnumerator SendCurrentQuestDataToServer()
    {
        Debug.Log($"{questCategory} - {questType} 전송 대기중");
        yield return new WaitForSeconds(sendQuestDataToServerDelayTime);

        Debug.Log($"{questCategory} - {questType} 전송 완료");
        SendDataToServer();

    }
    protected virtual void SendDataToServer()
    {
        DummyQuestServer.GetQuestDataFromClient(Player.GetUserID(), questCategory, questType, GetCurrentQuestCount());
    }
    // ======================================================================

    public override void RequestQuestReward()
    {
        DummyQuestServer.UserRequestReward(Player.GetUserID(), this);
        base.RequestQuestReward();

        if (IsRewardRepeatable())
        {
            ChangeCurrentProgressCountAfterReward();
        }
        else
        {
            GetReward = true;
        }
        RenewalUIAfterChangeQuestValue();
    }
}
