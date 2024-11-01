using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "LevelUpStatusQuestData", menuName = "ScriptableObjects/QuestData/StoryQuest/LevelUpStatusQuestData", order = 1)]
public class LevelUpStatusQuestData : QuestDataBase
{
    private int currentStatusLevel = 0;

    public int requireStatusLevel;

    public StatusLevelType questStatusType;

    public override void QuestActing(BaseQuest quest, QuestType type)
    {
        mainQuestTitle = questStatusType.ToString() + " 스탯 레벨" + requireStatusLevel + "달성";
        currentStatusLevel = Player.playerStatus.GetStatusLevelData().statusLevels[(int)questStatusType];

        base.QuestActing(quest, QuestType.LevelUpStatus);

    }

    public override void RequestCurrentUserQuestProgress()
    {
    }

    

    protected override void SetRequireText()
    {
        string newText = MyBigIntegerMath.GetAbbreviationFromBigInteger(currentStatusLevel) + " / " +
                         MyBigIntegerMath.GetAbbreviationFromBigInteger(requireStatusLevel);
        QuestComp.SetRequireText(newText);
    }

    protected override void BindDelegate()
    {
        Player.playerStatus.OnStatusLevelChange += ChangeQuestData;
    }

    protected override void UnBindDelegate()
    {
        Player.playerStatus.OnStatusLevelChange -= ChangeQuestData;
    }

    protected override void RenewalUIAfterChangeQuestValue()
    {
        float currentValue = MyBigIntegerMath.DivideToFloat(currentStatusLevel, requireStatusLevel, 5);

        QuestComp.SetSliderValue(currentValue);

        SetRequireText();

        CheckQuestClear();
    }

    public override int GetRequireCount()
    {
        // 썌얘 : 스토리 관련은 전부 다시 수정하기
        return 0;
    }

    public override BigInteger GetCurrentQuestCount()
    {
        return 0;
    }

    public void ChangeQuestData(StatusLevelType type)
    {
        if (type != questStatusType)
        {
            return;
        }
        currentStatusLevel = Player.playerStatus.GetStatusLevelData().statusLevels[(int)type];
        RenewalUIAfterChangeQuestValue();
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

    public override void RequestQuestReward()
    {
        DummyStoryQuestServer.UserSendStoryQuestClear(Player.GetUserID(),this);
        BaseStoryQuest.GetInstance().ClearStoryQuest();
        QuestComp.rewardButton.onClick.RemoveListener(RequestQuestReward);
    }
}
