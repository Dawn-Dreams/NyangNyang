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

    public int requireKillMonsterCount = 50;
    public override void QuestActing(BaseQuest quest)
    {
        QuestType = QuestType.Repeat_KillMonster;

        base.QuestActing(quest);

    }

    public override void RequestQuestData()
    {
        DummyQuestServer.SendQuestDataToPlayer(Player.GetUserID(), QuestType);
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

            string newText = "x " + (rewardCount * clearCount).ToString();
            QuestComp.SetRewardCountText(newText);
        }
        else
        {
            QuestComp.SetRewardButtonInteractable(false, "진행중");

            string newText = "x " + rewardCount.ToString();
            QuestComp.SetRewardCountText(newText);
        }
    }

    
}
