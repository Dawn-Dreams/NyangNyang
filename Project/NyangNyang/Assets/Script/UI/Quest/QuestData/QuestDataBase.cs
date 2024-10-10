using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public abstract class QuestDataBase : ScriptableObject
{
    // 퀘스트 설명
    public string mainQuestTitle;
    public string subQuestTitle;

    // 보상
    public Sprite rewardImage;
    public int rewardCount;

    // UI
    protected BaseQuest QuestComp;

    protected QuestType QuestType;

    public virtual void QuestActing(BaseQuest quest)
    {
        QuestComp = quest;

        BindDelegate();

        SetRequireText();
        
        // 서버로부터 정보 요청
        DummyQuestServer.SendQuestDataToPlayer(Player.GetUserID(), QuestType);

        QuestComp.rewardButton.onClick.AddListener(RequestQuestReward);
    }

    public void RequestQuestReward()
    {
        DummyQuestServer.UserRequestReward(Player.GetUserID(), QuestType);
    }

    protected abstract void CheckQuestClear();

    protected abstract void SetRequireText();

    protected abstract void BindDelegate();
}
