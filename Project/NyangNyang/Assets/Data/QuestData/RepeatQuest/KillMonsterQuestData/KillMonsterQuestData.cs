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
    public QuestDataBase QuestInitialize(QuestCategory questCategory, int getRequireKillMonsterCount, int getRewardCount = 1)
    {
        QuestCategory = questCategory;
        mainQuestTitle = "몬스터 처치";
        subQuestTitle = "몬스터를 " + getRequireKillMonsterCount + "처치하세요.";
        requireKillMonsterCount = getRequireKillMonsterCount;

        rewardType = RewardType.Diamond;
        rewardCount = getRewardCount;
        return this;
    }
    public override void QuestActing(BaseQuest quest)
    {
        QuestType = QuestType.KillMonster;

        base.QuestActing(quest);

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
    }

    protected override void UnBindDelegate()
    {
        Player.OnMonsterKillQuestChange -= GetDataFromServer;
    }

    public void GetDataFromServer(long newQuestDataValue)
    {
        _killMonsterCount = newQuestDataValue;

        float newValue = Mathf.Min(1.0f, (float)_killMonsterCount / requireKillMonsterCount);
        QuestComp.SetSliderValue(newValue);

        SetRequireText();

        CheckQuestClear();
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
