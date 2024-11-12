using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GoldSpendingQuestData", menuName = "ScriptableObjects/QuestData/Normal/GoldSpendingQuestData", order = 1)]
public class Normal_GoldSpendingQuestData : NormalQuestDataBase
{
    protected BigInteger spendingGold = 0;

    public int requireSpendingGold;
    public override void QuestActing(BaseQuest quest, QuestType type)
    {
        base.QuestActing(quest, QuestType.GoldSpending);
    }

    public override void RequestCurrentUserQuestProgress()
    {
        DummyQuestServer.SendQuestDataToPlayer(Player.GetUserID(), questCategory, questType);
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
        Player.playerCurrency.OnSpendingGold += QuestCountChange;
    }

    protected override void UnBindDelegate()
    {
        Player.OnRenewGoldSpendingQuest -= GetDataFromServer;
        Player.playerCurrency.OnSpendingGold -= QuestCountChange;
    }

    public override int GetRequireCount()
    {
        return requireSpendingGold;
    }

    public override void ChangeCurrentProgressCountAfterReward()
    {
        int clearCount = (int)MyBigIntegerMath.DivideToFloat(spendingGold, requireSpendingGold);
        spendingGold -= requireSpendingGold * clearCount;
    }

    public override BigInteger GetCurrentQuestCount()
    {
        return spendingGold;
    }

    // 서버로부터 데이터 값을 받아올 때
    public void GetDataFromServer(QuestCategory dataCategory, BigInteger newQuestDataValue)
    {
        if (questCategory != dataCategory)
        {
            return;
        }
        spendingGold = newQuestDataValue;
        RenewalUIAfterChangeQuestValue();
    }
    // 퀘스트를 수행하여 값이 바뀌었을 때,
    public void QuestCountChange(BigInteger newSpendingGoldVal)
    {
        spendingGold += newSpendingGoldVal;
        RenewalUIAfterChangeQuestValue();

        // TODO: 10.31) 추후엔 GameManager 라던가 기타 Monobehaviour 상속 클래스에서 보내도록 
        // 코루틴 써서
        SendDataToServer();
    }
    
    protected override void RenewalUIAfterChangeQuestValue()
    {
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
            QuestComp.SetRewardCountText(rewardCount, clearCount, IsRewardRepeatable());
        }
        else
        {
            QuestComp.SetRewardButtonInteractable(false, "진행중");
            QuestComp.SetRewardCountText(rewardCount,1,IsRewardRepeatable());
        }
    }
}
