using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "KillMonsterQuestData", menuName = "ScriptableObjects/QuestData/Normal/KillMonsterQuestData", order = 1)]
public class Normal_KillMonsterQuestData : NormalQuestDataBase
{
    private long _killMonsterCount;

    public int requireKillMonsterCount;
    public override void QuestActing(BaseQuest quest, QuestType type)
    {
        base.QuestActing(quest, QuestType.KillMonster);
    }


    protected override void SetRequireText()
    {
        string newText = MyBigIntegerMath.GetAbbreviationFromBigInteger(_killMonsterCount) + " / " +
                         requireKillMonsterCount;
        QuestComp.SetRequireText(newText);
    }

    protected override void BindDelegate()
    {
        QuestManager.GetInstance().OnRenewQuestProgressData += GetDataFromServer;
        EnemySpawnManager.GetInstance().OnEnemyDeath += QuestCountChange;
    }

    protected override void UnBindDelegate()
    {
        QuestManager.GetInstance().OnRenewQuestProgressData -= GetDataFromServer;
        EnemySpawnManager.GetInstance().OnEnemyDeath -= QuestCountChange;
    }

    protected override void RenewalUIAfterChangeQuestValue()
    {
        float newValue = Mathf.Min(1.0f, (float)_killMonsterCount / requireKillMonsterCount);
        QuestComp.SetSliderValue(newValue);

        SetRequireText();

        CheckQuestClear();
    }

    public override int GetRequireCount()
    {
        return requireKillMonsterCount;
    }

    public override void ChangeCurrentProgressCountAfterReward()
    {
        int clearCount = (int)_killMonsterCount / requireKillMonsterCount;
            
        _killMonsterCount -= requireKillMonsterCount * clearCount;
    }

    public override BigInteger GetCurrentQuestCount()
    {
        return _killMonsterCount;
    }


    public void GetDataFromServer(QuestCategory dataCategory, QuestType type, BigInteger newQuestDataValue)
    {
        if (questCategory != dataCategory || questType != type)
        {
            return;
        }

        _killMonsterCount = (long)newQuestDataValue;
        RenewalUIAfterChangeQuestValue();
    }

    public void QuestCountChange(int killEnemyCount)
    {
        _killMonsterCount += killEnemyCount;
        float newValue = Mathf.Min(1.0f, (float)_killMonsterCount / requireKillMonsterCount);
        QuestComp.SetSliderValue(newValue);

        SetRequireText();

        CheckQuestClear();



        // TODO: 10.31) 추후엔 GameManager 라던가 기타 Monobehaviour 상속 클래스에서 보내도록 
        // 코루틴 써서
        SendDataToServer();
    }


    protected override void CheckQuestClear()
    {
        if (_killMonsterCount >= requireKillMonsterCount)
        {
            int clearCount = (int)(_killMonsterCount / requireKillMonsterCount);
            QuestComp.SetRewardButtonInteractable(true, "보상받기");

            QuestComp.SetRewardCountText(rewardCount, clearCount, IsRewardRepeatable());
        }
        else
        {
            QuestComp.SetRewardButtonInteractable(false, "진행중");

            QuestComp.SetRewardCountText(rewardCount, 1, IsRewardRepeatable());
        }
    }

    
}
