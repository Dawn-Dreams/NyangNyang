using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "StageClearQuestData", menuName = "ScriptableObjects/QuestData/StoryQuest/StageClearQuestData", order = 1)]
public class Story_StageClearQuestData : StoryQuestDataBase
{
    public int targetTheme;
    public int targetStage;

    private int _currentClearTheme;
    private int _currentClearStage;
    private bool _isQuestClear;

    public override void QuestActing(BaseQuest quest, QuestType type)
    {
        mainQuestTitle = "[스테이지 클리어]\n" + targetTheme + " - " + targetStage + " 스테이지 클리어";
        Player.GetPlayerHighestClearStageData(out _currentClearTheme, out _currentClearStage);

        base.QuestActing(quest, QuestType.StageClear);
    }

    protected override void SetRequireText()
    {
        float currentProgress = _isQuestClear ? 1.0f : 0.0f;
        string newText = currentProgress + " / " + 1;
        QuestComp.SetRequireText(newText);
    }

    protected override void BindDelegate()
    {
        GameManager.GetInstance().stageManager.OnStageClear += ChangeQuestData;
    }

    protected override void UnBindDelegate()
    {
        GameManager.GetInstance().stageManager.OnStageClear -= ChangeQuestData;
    }

    protected override void RenewalUIAfterChangeQuestValue()
    {
        QuestComp.SetSliderValue(_isQuestClear ? 1.0f : 0.0f);
        SetRequireText();
    }

    public void ChangeQuestData(int clearTheme, int clearStage)
    {
        _currentClearTheme = clearTheme;
        _currentClearStage = clearStage;
        CheckQuestClear();

    }

    protected override void CheckQuestClear()
    {
        _isQuestClear = (_currentClearTheme > targetTheme) || ((_currentClearTheme == targetTheme) && _currentClearStage >= targetStage);
        QuestComp.SetRewardButtonInteractable(_isQuestClear, "");
        RenewalUIAfterChangeQuestValue();
    }
}
