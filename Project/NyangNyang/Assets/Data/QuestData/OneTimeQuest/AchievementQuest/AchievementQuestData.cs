using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class AchievementQuestData : OneTimeQuestDataBase
{
    public int rewardTitleID;


    public override void QuestActing(BaseQuest quest, QuestType type)
    {
        questCategory = QuestCategory.Achievement;
        
        base.QuestActing(quest, type);

        QuestActingAction();
    }

    protected virtual void QuestActingAction()
    {
        // 등급에 맞게 칭호 이름 색상 변경
        if (QuestComp.mainQuestText)
        {
            TitleInfo titleInfo = TitleDataManager.GetInstance().titleInfoDic[rewardTitleID];
            QuestComp.mainQuestText.text = titleInfo.name;
            QuestComp.mainQuestText.color = TitleDataManager.titleGradeColors[(TitleGrade)titleInfo.grade];
        }
        PlayerTitle.OnOwningTitleChange += CheckIsOwningTitle;

        // 현재 칭호를 보유중인지에 따라 퀘스트 클리어 유무를 적용
        CheckIsOwningTitle();
    }

    // (타이틀 획득 시 delegate) 타이틀을 보유중인지에 따라 퀘스트 클리어 가능 여부 적용 함수
    public void CheckIsOwningTitle()
    {
        foreach (int titleID in PlayerTitle.playerOwningTitles)
        {
            if (titleID == rewardTitleID)
            {
                QuestComp.SetRewardButtonInteractable(false,"보유중");
                GetReward = true;
                return;
            }
        }
        QuestComp.SetRewardButtonInteractable(true);
        GetReward = false;
    }

    public override void RequestQuestReward()
    {
        Debug.Log($"유저가 타이틀 ID: {rewardTitleID} 타이틀을 흭득하였습니다.");
        //DummyPlayerTitleServer.UserRequestAcquireTitle(Player.GetUserID(), rewardTitleID);
        PlayerTitle.AcquireTitle(rewardTitleID);
        

        GetReward = true;
        RenewalUIAfterChangeQuestValue();
    }

}
