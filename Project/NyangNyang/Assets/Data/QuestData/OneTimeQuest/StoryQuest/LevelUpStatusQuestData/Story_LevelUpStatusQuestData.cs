using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "LevelUpStatusQuestData", menuName = "ScriptableObjects/QuestData/StoryQuest/LevelUpStatusQuestData", order = 1)]
public class Story_LevelUpStatusQuestData : StoryQuestDataBase
{
    private int currentStatusLevel = 0;

    public int requireStatusLevel;

    public StatusLevelType questStatusType;

    public override void QuestActing(BaseQuest quest, QuestType type)
    {
        
        mainQuestTitle = "성장\n" + "[" + EnumTranslator.GetStatusTypeText(questStatusType) + "] 스탯 레벨업";
        currentStatusLevel = Player.playerStatus.GetStatusLevelData().statusLevels[(int)questStatusType];

        base.QuestActing(quest, QuestType.LevelUpStatus);
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

}
