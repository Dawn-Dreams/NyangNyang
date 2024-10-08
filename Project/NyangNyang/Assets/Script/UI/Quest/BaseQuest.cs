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

    void Start()
    {
        LoadQuest();

        
    }

    void LoadQuest()
    {
        if (questData)
        {
            mainQuestText.text = questData.mainQuestTitle;
            subQuestText.text = questData.subQuestTitle;
            rewardImage.sprite = questData.rewardImage;

            questData.QuestActing(questSlider,rewardButton, rewardCount, questRequireText, questProgressText);
        }
    }
}
