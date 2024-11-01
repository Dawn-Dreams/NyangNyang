using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "KillMonsterQuestData", menuName = "ScriptableObjects/QuestData/KillMonsterQuestData", order = 1)]
public class KillMonsterQuestData : QuestDataBase
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
        Player.OnMonsterKillQuestChange += GetDataFromServer;
        EnemySpawnManager.GetInstance().OnEnemyDeath += QuestCountChange;
    }

    protected override void UnBindDelegate()
    {
        Player.OnMonsterKillQuestChange -= GetDataFromServer;
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

    public override BigInteger GetCurrentQuestCount()
    {
        return _killMonsterCount;
    }


    public void GetDataFromServer(QuestCategory dataCategory, long newQuestDataValue)
    {
        if (questCategory != dataCategory)
        {
            return;
        }

        _killMonsterCount = newQuestDataValue;
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

            QuestComp.SetRewardCountText(rewardCount, clearCount, CanRepeatReward);
        }
        else
        {
            QuestComp.SetRewardButtonInteractable(false, "진행중");

            QuestComp.SetRewardCountText(rewardCount, 1, CanRepeatReward);
        }
    }

    
}
