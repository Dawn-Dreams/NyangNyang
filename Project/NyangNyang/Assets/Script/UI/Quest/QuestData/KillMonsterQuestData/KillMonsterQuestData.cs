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
    public override void QuestActing(Slider slider, Button button, TextMeshProUGUI rewardText, TextMeshProUGUI requireText, TextMeshProUGUI progressText)
    {
        QuestType = QuestType.KillMonster;

        base.QuestActing(slider, button, rewardText, requireText, progressText);

    }

    protected override void SetRequireText()
    {
        string newText = MyBigIntegerMath.GetAbbreviationFromBigInteger(_killMonsterCount) + " / " +
                         requireKillMonsterCount;
        questRequireText.text = newText;
    }

    protected override void BindDelegate()
    {
        Player.OnMonsterKillQuestChange += GetDataFromServer;
    }

    public void GetDataFromServer(long newQuestDataValue)
    {
        _killMonsterCount = newQuestDataValue;

        questSlider.value = Mathf.Min(1.0f, (float)_killMonsterCount / requireKillMonsterCount);
        SetRequireText();

        CheckQuestClear();
    }

    protected override void CheckQuestClear()
    {
        if (_killMonsterCount >= requireKillMonsterCount)
        {
            rewardButton.interactable = true;
            int clearCount = (int)(_killMonsterCount / requireKillMonsterCount);
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
