using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class AchievementQuestData : QuestDataBase
{
    public int rewardTitleID;

    public virtual AchievementQuestData QuestInitialize(int rewardingTitleID)
    {
        rewardTitleID = rewardingTitleID;
        questCategory = QuestCategory.Achievement;
        mainQuestTitle = TitleDataManager.GetInstance().titleInfoDic[rewardTitleID].name;
        
        return this;
    }

    public override void QuestActing(BaseQuest quest)
    {
        QuestInitialize(rewardTitleID);
        
        base.QuestActing(quest);

        // 등급에 맞게 칭호 이름 색상 변경
        if (QuestComp.mainQuestText)
        {
            TitleInfo titleInfo = TitleDataManager.GetInstance().titleInfoDic[rewardTitleID];
            QuestComp.mainQuestText.color = TitleDataManager.titleGradeColors[(TitleGrade)titleInfo.grade];
        }

        PlayerTitle.OnOwningTitleChange += CheckIsOwningTitle;

        // 현재 칭호를 보유중인지에 따라 퀘스트 클리어 유무를 적용
        CheckIsOwningTitle();
    }


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
        DummyPlayerTitleServer.UserRequestAcquireTitle(Player.GetUserID(), rewardTitleID);
        PlayerTitle.AcquireTitle(rewardTitleID);

        GetReward = true;
        CheckIsOwningTitle();
    }
}
