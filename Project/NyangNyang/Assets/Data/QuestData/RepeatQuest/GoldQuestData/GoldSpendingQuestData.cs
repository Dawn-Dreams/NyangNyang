using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GoldSpendingQuestData", menuName = "ScriptableObjects/QuestData/GoldSpendingQuestData", order = 1)]
public class GoldSpendingQuestData : QuestDataBase
{
    protected BigInteger spendingGold = 0;

    public int requireSpendingGold;
    public QuestDataBase QuestInitialize(QuestCategory questCategory, int getRequireSpendingGold, int getRewardCount = 1)
    {
        QuestCategory = questCategory;
        mainQuestTitle = "골드 소모";
        subQuestTitle = "골드를 " + MyBigIntegerMath.GetAbbreviationFromBigInteger(getRequireSpendingGold) + "소모하세요.";
        requireSpendingGold = getRequireSpendingGold;

        rewardType = RewardType.Diamond;
        rewardCount = getRewardCount;
        return this;
    }

    public override void QuestActing(BaseQuest quest)
    {
        QuestType = QuestType.GoldSpending;
        
        base.QuestActing(quest);
    }

    public override void RequestQuestData()
    {
        DummyQuestServer.SendQuestDataToPlayer(Player.GetUserID(), QuestCategory, QuestType);
    }

    protected override void SetRequireText()
    {
        string newText = MyBigIntegerMath.GetAbbreviationFromBigInteger(spendingGold) + " / " +
                         MyBigIntegerMath.GetAbbreviationFromBigInteger(requireSpendingGold);
        QuestComp.SetRequireText(newText);
    }

    protected override void BindDelegate()
    {
        Player.OnRenewGoldSpendingQuest += GetDataFromServer;
    }

    protected override void UnBindDelegate()
    {
        Player.OnRenewGoldSpendingQuest -= GetDataFromServer;
    }

    public void GetDataFromServer(QuestCategory questCategory, BigInteger newQuestDataValue)
    {
        if (questCategory != QuestCategory) return;
        
        spendingGold = newQuestDataValue;
        float currentValue = MyBigIntegerMath.DivideToFloat(spendingGold, requireSpendingGold, 5);

        QuestComp.SetSliderValue(currentValue);
        
        SetRequireText();

        CheckQuestClear();
    }

    protected override void CheckQuestClear()
    {
        if (spendingGold >= requireSpendingGold)
        {
            int clearCount = (int)MyBigIntegerMath.DivideToFloat(spendingGold, requireSpendingGold, 5);
            QuestComp.SetRewardButtonInteractable(true, "보상받기");

            QuestComp.SetRewardCountText(rewardCount, clearCount, CanRepeatReward);
        }
        else
        {
            QuestComp.SetRewardButtonInteractable(false, "진행중");
            QuestComp.SetRewardCountText(rewardCount,1,CanRepeatReward);
        }
    }
}
