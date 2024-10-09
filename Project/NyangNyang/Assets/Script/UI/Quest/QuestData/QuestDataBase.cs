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
    public Slider questSlider;
    public Button rewardButton;
    public TextMeshProUGUI rewardCountText;
    public TextMeshProUGUI questRequireText;
    public TextMeshProUGUI questProgressText;

    protected QuestType QuestType;

    public virtual void QuestActing(Slider slider, Button button, TextMeshProUGUI rewardText, TextMeshProUGUI requireText, TextMeshProUGUI progressText)
    {
        questSlider = slider;
        rewardButton = button;
        rewardButton.onClick.AddListener(RequestQuestReward);
        rewardCountText = rewardText;
        rewardCountText.text = "x " + rewardCount.ToString();
        questRequireText = requireText;
        SetRequireText();
        questProgressText = progressText;
    }

    public abstract void RequestQuestReward();

    protected abstract void CheckQuestClear();

    protected abstract void SetRequireText();


}
