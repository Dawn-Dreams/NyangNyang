using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;
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
    public QuestType questType;
    public QuestCategory questCategory;

    public RewardType rewardType = RewardType.Diamond;

    // 보상 리워드 리소스 Addressable
    protected AsyncOperationHandle<Sprite> RewardSpriteHandle;

    protected bool CanRepeatReward = false;
    protected bool GetReward = false;

    // 퀘스트 값에 갱신이 발생하였을 경우 n초 뒤에 값을 보냄
    public float sendQuestDataToServerDelayTime = 5.0f;
    protected Coroutine SendQuestDataToServerCoroutine;

    public virtual void QuestActing(BaseQuest quest)
    {
        if (questCategory == QuestCategory.Repeat)
        {
            CanRepeatReward = true;
        }
        QuestComp = quest;
        
        QuestDataInitialize();

        // 서버로부터 정보 요청
        RequestQuestData();
        //DummyQuestServer.SendQuestDataToPlayer(Player.GetUserID(), QuestType);

        QuestComp.rewardButton.onClick.AddListener(RequestQuestReward);
        CheckQuestClear();
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
        DummyQuestServer.SendQuestDataToPlayer(Player.GetUserID(), questCategory, questType);
    }

    

    public virtual void RequestQuestReward()
    {
        DummyQuestServer.UserRequestReward(Player.GetUserID(), this);
        if (!CanRepeatReward)
        {
            QuestComp.rewardButton.onClick.RemoveListener(RequestQuestReward);
        }
    }

    protected abstract void CheckQuestClear();

    protected abstract void SetRequireText();

    protected abstract void BindDelegate();
    protected abstract void UnBindDelegate();

    public abstract int GetRequireCount();
    public abstract BigInteger GetCurrentQuestCount();

    protected IEnumerator SendCurrentQuestDataToServer()
    {
        Debug.Log($"{questCategory} - {questType} 전송 대기중");
        yield return new WaitForSeconds(sendQuestDataToServerDelayTime);

        Debug.Log($"{questCategory} - {questType} 전송 완료");
        SendDataToServer();
        
    }

    protected virtual void SendDataToServer()
    {
        DummyQuestServer.GetQuestDataFromClient(Player.GetUserID(), questCategory, questType, GetCurrentQuestCount());
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
        bool isGetReward = DummyQuestServer.SendRewardInfoToUser(Player.GetUserID(), questCategory, questType);
        GetReward = isGetReward;
    }

    public QuestCategory GetQuestCategory()
    {
        return questCategory;
    }

    public QuestType GetQuestType()
    {
        return questType;
    }
}
