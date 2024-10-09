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
    public override void QuestActing(Slider slider, Button button, TextMeshProUGUI rewardText, TextMeshProUGUI requireText, TextMeshProUGUI progressText)
    {
        QuestType = QuestType.GoldSpending;
        
        base.QuestActing(slider,button, rewardText, requireText, progressText);

        questSlider.minValue = 0;
        questSlider.maxValue = 1;

        Player.OnGoldSpending += AddQuestValue;
        Player.OnRenewGoldSpendingQuest += GetDataFromServer;

        // 서버로부터 정보 요청
        DummyQuestServer.SendQuestDataToPlayer(Player.GetUserID(),QuestType);
    }

    public override void RequestQuestReward()
    {
        DummyQuestServer.UserRequestReward(Player.GetUserID(),QuestType);
    }


    protected override void SetRequireText()
    {
        string newText = MyBigIntegerMath.GetAbbreviationFromBigInteger(spendingGold) + " / " +
                         MyBigIntegerMath.GetAbbreviationFromBigInteger(requireSpendingGold);
        questRequireText.text = newText;
    }

    public void AddQuestValue(BigInteger spendingGoldVal)
    {
        DummyQuestServer.AddGoldOnServer(Player.GetUserID(), spendingGoldVal);

    }

    public void GetDataFromServer(BigInteger newQuestDataValue)
    {
        spendingGold = newQuestDataValue;
        float currentValue = MyBigIntegerMath.DivideToFloat(spendingGold, requireSpendingGold, 5);

        questSlider.value = Mathf.Min(1.0f, currentValue);
        SetRequireText();

        CheckQuestClear();
    }

    protected override void CheckQuestClear()
    {
        if (spendingGold >= requireSpendingGold)
        {
            rewardButton.interactable = true;
            int clearCount = (int)MyBigIntegerMath.DivideToFloat(spendingGold, requireSpendingGold, 5);
            rewardCountText.text = "x " + (rewardCount * clearCount).ToString();
            questProgressText.text = "보상받기";
        }
        else
        {
            rewardCountText.text = "x " + rewardCount.ToString();
            questProgressText.text = "진행중";
            rewardButton.interactable = false;
        }
    }
}
