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
    public Sprite rewardImage;
    public int rewardCount;

    // 퀘스트 컴퍼넌트
    protected BaseQuest QuestComp;
    protected QuestType QuestType;

    // 보상 리워드 리소스 Addressable
    protected AsyncOperationHandle<Sprite> RewardSpriteHandle;


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

    protected void LoadRewardImage(RewardType type)
    {
        if (rewardImage == null)
        {
            string key = "Icon/" + type.ToString();
            RewardSpriteHandle = Addressables.LoadAssetAsync<Sprite>(key);
            RewardSpriteHandle.WaitForCompletion();
            rewardImage = RewardSpriteHandle.Result;
        }
    }

    public void ReleaseResource()
    {
        if (RewardSpriteHandle.IsValid())
        {
            RewardSpriteHandle.Release();
        }
        
    }
}
