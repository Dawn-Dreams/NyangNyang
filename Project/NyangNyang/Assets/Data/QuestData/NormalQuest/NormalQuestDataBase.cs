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
        LoadCurrentUserQuestProgress();
    }

    // == 퀘스트 데이터에 대한 변수에 대한 Get (서버 : 반복퀘스트 클리어 체크 시) / (클라 : 서버에 현재 QuestValue 보낼 때 사용) ==
    public abstract int GetRequireCount();
    
    public abstract BigInteger GetCurrentQuestCount();
    // ===============================================

    // 퀘스트 클리어 버튼 이후 클라 내에서 값을 갱신시키는 함수
    public abstract int ChangeCurrentProgressCountAfterReward();

    // 현재 유저의 퀘스트 진행 정도를 받는 함수
    public virtual void LoadCurrentUserQuestProgress()
    {
        QuestManager.GetInstance().LoadQuestProgressDataFromJson(questCategory, questType);
        CheckQuestClear();
    }

    protected virtual void SaveDataToJson()
    {
        QuestSaveLoadManager.GetInstance().SaveQuestProgressData(questCategory,questType,GetCurrentQuestCount().ToString());
    }
    // ======================================================================

    public override void RequestQuestReward()
    {

        base.RequestQuestReward();

        if (IsRewardRepeatable())
        {
            int clearCount = ChangeCurrentProgressCountAfterReward();
            if (rewardType == RewardType.Diamond)
            {
                Player.Diamond += rewardCount*clearCount;
            }
            QuestSaveLoadManager.GetInstance().SaveQuestProgressData(questCategory,questType,GetCurrentQuestCount().ToString());
        }
        else
        {
            if (rewardType == RewardType.Diamond)
            {
                Player.Diamond += rewardCount;
            }
            GetReward = true;
            QuestSaveLoadManager.GetInstance().UserGetRewardOnNotRepeatable(questCategory, questType);
        }
        RenewalUIAfterChangeQuestValue();
    }
}
