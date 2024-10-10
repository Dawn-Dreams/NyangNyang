using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LevelUpStatusQuestData : QuestDataBase
{
    private BigInteger currentStatusLevel = 0;

    public BigInteger requireStatusLevel;

    public StatusLevelType questStatusType;

    // 서버 내 생성할 때 사용
    public QuestDataBase Initialize(StatusLevelType statusType, BigInteger targetLevel, int diamondRewardCount = 100)
    {
        questStatusType = statusType;

        mainQuestTitle = statusType.ToString() + " 스탯 레벨" + MyBigIntegerMath.GetAbbreviationFromBigInteger(targetLevel) + "달성";
        rewardCount = diamondRewardCount;

        LoadRewardImage(RewardType.Diamond);

        requireStatusLevel = targetLevel;

        return this;
    }

    public override void QuestActing(BaseQuest quest)
    {
        QuestType = QuestType.LevelUpStatus;
        
        base.QuestActing(quest);

        DummyStroyQuestServer.SendLevelUpStatusQuestDataToUser(Player.GetUserID(),questStatusType);
    }

    protected override void SetRequireText()
    {

        string newText = MyBigIntegerMath.GetAbbreviationFromBigInteger(currentStatusLevel) + " / " +
                         MyBigIntegerMath.GetAbbreviationFromBigInteger(requireStatusLevel);
        QuestComp.SetRequireText(newText);
    }

    protected override void BindDelegate()
    {
        Player.OnLevelUpStatusQuestChange += GetDataFromServer;
    }


    public void GetDataFromServer(StatusLevelType type, BigInteger newQuestDataValue)
    {
        currentStatusLevel = newQuestDataValue;
        float currentValue = MyBigIntegerMath.DivideToFloat(currentStatusLevel, requireStatusLevel, 5);

        QuestComp.SetSliderValue(currentValue);
        
        SetRequireText();

        CheckQuestClear();
    }

    protected override void CheckQuestClear()
    {
        if (currentStatusLevel >= requireStatusLevel)
        {
            int clearCount = (int)MyBigIntegerMath.DivideToFloat(currentStatusLevel, requireStatusLevel, 5);
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
