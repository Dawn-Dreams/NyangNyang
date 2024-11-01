using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


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

    protected bool GetReward = false;

    // 퀘스트 값에 갱신이 발생하였을 경우 n초 뒤에 값을 보냄
    public float sendQuestDataToServerDelayTime = 5.0f;
    protected Coroutine SendQuestDataToServerCoroutine;

    public virtual void QuestActing(BaseQuest quest, QuestType type)
    {
        QuestComp = quest;
        questType = type;
        
        QuestDataInitialize();

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

    public virtual void RequestQuestReward()
    {
        if (!IsRewardRepeatable())
        {
            QuestComp.rewardButton.onClick.RemoveListener(RequestQuestReward);
        }
    }

    protected abstract void CheckQuestClear();

    protected abstract void SetRequireText();

    // == 퀘스트 수행 을 체크하기 위한 Delegate를 연결하는 함수 ==
    protected abstract void BindDelegate();
    protected abstract void UnBindDelegate();
    // =======================================================


    // UI들을 바뀐 퀘스트 값에 맞게 리뉴얼하는 함수
    protected abstract void RenewalUIAfterChangeQuestValue();

    // Addressable 을 통해 보상에 대한 이미지를 로드하는 함수
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

    // Addressable 로 로드된 리소스를 해제하는 함수
    public void ReleaseResource()
    {
        if (RewardSpriteHandle.IsValid())
        {
            RewardSpriteHandle.Release();
        }
    }
    // 퀘스트가 반복 가능한 퀘스트인지
    public bool IsRewardRepeatable()
    {
        //return CanRepeatReward;
        return questCategory == QuestCategory.Repeat;
    }

    public bool IsGetReward()
    {
        return GetReward;
    }

    // 해당 퀘스트에 대해서 보상을 받았는지 값을 받아오는 함수
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
