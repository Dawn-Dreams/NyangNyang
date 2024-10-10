using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GoldSpendingQuestData", menuName = "ScriptableObjects/QuestData/GoldSpendingQuestData", order = 1)]
public class GoldSpendingQuestData : QuestDataBase
{
    private BigInteger spendingGold = 0;

    public int requireSpendingGold;
    public override void QuestActing(BaseQuest quest)
    {
        QuestType = QuestType.GoldSpending;
        
        base.QuestActing(quest);
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

    public void GetDataFromServer(BigInteger newQuestDataValue)
    {
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
