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

        rewardType = RewardType.Diamond;

        requireStatusLevel = targetLevel;

        return this;
    }
    

    public override void QuestActing(BaseQuest quest)
    {
        QuestType = QuestType.LevelUpStatus;
        
        base.QuestActing(quest);

    }

    public override void RequestQuestData()
    {
        DummyStoryQuestServer.SendLevelUpStatusQuestDataToUser(Player.GetUserID(), questStatusType);
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

    protected override void UnBindDelegate()
    {
        Player.OnLevelUpStatusQuestChange -= GetDataFromServer;
    }

    public override void BindDelegateOnServer()
    {
        Debug.Log("스탯 레벨업 추가" + questStatusType);
        DummyServerData.OnUserStatusLevelUp += DummyStoryQuestServer.SendLevelUpStatusQuestDataToUser;
    }

    public override void UnBindDelegateOnServer()
    {
        Debug.Log("스탯 레벨업 삭제" + questStatusType);
        DummyServerData.OnUserStatusLevelUp -= DummyStoryQuestServer.SendLevelUpStatusQuestDataToUser;
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
        }
        else
        {
            QuestComp.SetRewardButtonInteractable(false, "진행중");
        }
    }
}
