using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;


public abstract class QuestDataBase : ScriptableObject
{
    // 퀘스트 설명
    public string mainQuestTitle;
    public string subQuestTitle;

    // 보상
    protected Sprite RewardSprite;
    public int rewardCount;

    // 퀘스트 컴퍼넌트
    protected BaseQuest QuestComp;
    protected QuestType QuestType;
    protected QuestCategory QuestCategory;

    public RewardType rewardType = RewardType.Diamond;

    // 보상 리워드 리소스 Addressable
    protected AsyncOperationHandle<Sprite> RewardSpriteHandle;

    protected bool CanRepeatReward = false;
    protected bool GetReward = false;

    public virtual void QuestActing(BaseQuest quest)
    {
        if (QuestCategory == QuestCategory.Repeat)
        {
            CanRepeatReward = true;
        }
        QuestComp = quest;
        
        QuestDataInitialize();

        // 서버로부터 정보 요청
        RequestQuestData();
        //DummyQuestServer.SendQuestDataToPlayer(Player.GetUserID(), QuestType);

        QuestComp.rewardButton.onClick.AddListener(RequestQuestReward);
    }

    public void RemoveQuest()
    {
        ReleaseResource();
        UnBindDelegate();
    }

    public void QuestDataInitialize()
    {
        LoadRewardImage(rewardType);
        QuestComp.SetRewardCountText(rewardCount,1,false);
        if (QuestComp.mainQuestText)
        {
            QuestComp.mainQuestText.text = mainQuestTitle;
        }
        if (QuestComp.subQuestText)
        {
            QuestComp.subQuestText.text = subQuestTitle;
        }
        if (QuestComp.rewardImage)
        {
            QuestComp.rewardImage.sprite = RewardSprite;
        }
        SetRequireText();

        BindDelegate();
    }

    public virtual void RequestQuestData()
    {
        DummyQuestServer.SendQuestDataToPlayer(Player.GetUserID(), QuestCategory, QuestType);
    }

    

    public virtual void RequestQuestReward()
    {
        DummyQuestServer.UserRequestReward(Player.GetUserID(),QuestCategory,  QuestType, this);
        if (!CanRepeatReward)
        {
            QuestComp.rewardButton.onClick.RemoveListener(RequestQuestReward);
        }
    }

    protected abstract void CheckQuestClear();

    protected abstract void SetRequireText();

    protected abstract void BindDelegate();
    protected abstract void UnBindDelegate();

    public virtual void BindDelegateOnServer()
    {
        // 서버에서 델리게이트 연결
    }

    public virtual void UnBindDelegateOnServer()
    {
        //....
    }

    protected void LoadRewardImage(RewardType type)
    {
        if (RewardSprite == null)
        {
            string key = "Icon/" + type.ToString();
            RewardSpriteHandle = Addressables.LoadAssetAsync<Sprite>(key);
            RewardSpriteHandle.WaitForCompletion();
            RewardSprite = RewardSpriteHandle.Result;
            
        }
    }

    public void ReleaseResource()
    {
        if (RewardSpriteHandle.IsValid())
        {
            RewardSpriteHandle.Release();
        }
        
    }

    public bool IsRewardRepeatable()
    {
        return CanRepeatReward;
    }

    public bool IsGetReward()
    {
        return GetReward;
    }

    public void RequestHasReceivedRewardToServer()
    {
        bool isGetReward = DummyQuestServer.SendRewardInfoToUser(Player.GetUserID(), QuestCategory, QuestType);
        GetReward = isGetReward;
    }
}
