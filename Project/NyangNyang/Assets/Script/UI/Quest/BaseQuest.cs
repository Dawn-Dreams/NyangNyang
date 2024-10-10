using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    public TextMeshProUGUI rewardCount;

    // 퀘스트 데이터
    public QuestDataBase questData;

    protected virtual void Start()
    {
        LoadQuest();
    }

    private void OnDestroy()
    {
        questData.ReleaseResource();
    }

    void LoadQuest()
    {
        if (questData)
        {
            if (mainQuestText)
            {
                mainQuestText.text = questData.mainQuestTitle;
            }
            if (subQuestText)
            {
                subQuestText.text = questData.subQuestTitle;
            }
            if (rewardImage)
            {
                rewardImage.sprite = questData.rewardImage;
            }
            questData.QuestActing(this);
        }
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

    public virtual void SetRewardButtonInteractable(bool newActive, string newText)
    {
        if (rewardButton)
        {
            rewardButton.interactable = newActive;
        }

        if (questProgressText)
        {
            questProgressText.text = newText;
        }
    }

    public void SetRewardCountText(string newText)
    {
        if (rewardCount)
        {
            rewardCount.text = newText;
        }
    }
}
