using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BaseQuest : MonoBehaviour
{
    // 보상 클릭 버튼
    public Button rewardButton;
    public TextMeshProUGUI questProgressText;

    // 퀘스트 텍스트
    public TextMeshProUGUI mainQuestText;
    public TextMeshProUGUI subQuestText;

    // 달성 슬라이더
    public Slider questSlider;
    public TextMeshProUGUI questRequireText;

    // 보상
    public Image rewardImage;
    [FormerlySerializedAs("rewardCount")] public TextMeshProUGUI rewardCountText;

    // 퀘스트 데이터
    public QuestCategory requestQuestCategory;
    public QuestType requestQuestType;
    public QuestDataBase questData;

    protected virtual void Start()
    {
        LoadQuest();
    }

    protected void LoadQuest()
    {
        if (!questData)
        {
            questData = DummyQuestServer.SendQuestInfoToUser(Player.GetUserID(), requestQuestCategory, requestQuestType);
        }

        questData.QuestActing(this);
    }

    public void SetRequireText(string newText)
    {
        if (questRequireText)
        {
            questRequireText.text = newText;
        }
        
    }

    public void SetSliderValue(float newValue)
    {
        if (questSlider)
        {
            questSlider.value = Mathf.Min(1.0f, newValue);
        }
        
    }

    public virtual void SetRewardButtonInteractable(bool newActive, string newText= "진행중")
    {
        if (!questData.IsRewardRepeatable())
        {
            questData.RequestHasReceivedRewardToServer();
        }
        if (questData.IsGetReward())
        {
            // TODO: 새로운 이미지로 변경
            rewardButton.interactable = false;
            questProgressText.text = "완료(새이미지)";
            return;
        }

        if (rewardButton)
        {
            rewardButton.interactable = newActive;
        }

        if (questProgressText)
        {
            questProgressText.text = newText;
        }
    }

    public void SetRewardCountText(BigInteger rewardCount, long clearCount, bool canRepeatReward)
    {
        if (rewardCountText)
        {
            if (canRepeatReward)
            {
                rewardCount *= clearCount;
            }
            string newText = "x " + rewardCount.ToString();
            rewardCountText.text = newText;
        }
    }

}
