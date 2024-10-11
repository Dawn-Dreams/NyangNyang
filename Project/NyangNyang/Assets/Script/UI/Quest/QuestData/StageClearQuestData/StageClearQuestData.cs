using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class StageClearQuestData : QuestDataBase
{
    public int targetTheme;
    public int targetStage;

    private int _currentClearTheme;
    private int _currentClearStage;
    private bool _isQuestClear;

    // 서버 내 생성할 때 사용
    public QuestDataBase Initialize(int getTargetTheme, int getTargetStage, int getRewardCount = 100)
    {
        targetTheme = getTargetTheme;
        targetStage = getTargetStage;

        mainQuestTitle = getTargetTheme + " - " + getTargetStage + " 스테이지 클리어";
        
        rewardCount = getRewardCount;
        rewardType = RewardType.Diamond;

        return this;
    }
    

    public override void QuestActing(BaseQuest quest)
    {
        QuestType = QuestType.LevelUpStatus;
        
        base.QuestActing(quest);

    }

    public override void RequestQuestData()
    {
        DummyStoryQuestServer.SendStageClearQuestDataToUser(Player.GetUserID());
    }


    protected override void SetRequireText()
    {
        float currentProgress = _isQuestClear ? 1.0f : 0.0f;
        string newText = currentProgress + " / " + 1;
        QuestComp.SetRequireText(newText);
    }

    protected override void BindDelegate()
    {
        Player.OnStageClear += GetDataFromServer;
    }

    public override void BindDelegateOnServer()
    {
        DummyServerData.OnUserStageClear += DummyStoryQuestServer.SendStageClearQuestDataToUser;
    }

    public override void UnBindDelegateOnServer()
    {
        DummyServerData.OnUserStageClear -= DummyStoryQuestServer.SendStageClearQuestDataToUser;
    }


    public void GetDataFromServer(int clearTheme, int clearStage)
    {
        _currentClearTheme = clearTheme;
        _currentClearStage = clearStage;

        CheckQuestClear();
        
        QuestComp.SetSliderValue( _isQuestClear? 1.0f : 0.0f );
        SetRequireText();

    }

    protected override void CheckQuestClear()
    {
        Debug.Log("퀘스트 클리어 체크 진행");
        _isQuestClear = (_currentClearTheme > targetTheme) || ((_currentClearTheme == targetTheme) && _currentClearStage >= targetStage);
        QuestComp.SetRewardButtonInteractable(_isQuestClear, "");
    }
}
