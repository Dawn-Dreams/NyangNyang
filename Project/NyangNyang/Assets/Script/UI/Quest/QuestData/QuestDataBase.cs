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
    protected Slider questSlider;
    protected Button rewardButton;
    protected TextMeshProUGUI rewardCountText;
    protected TextMeshProUGUI questRequireText;
    protected TextMeshProUGUI questProgressText;

    protected QuestType QuestType;

    public virtual void QuestActing(Slider slider, Button button, TextMeshProUGUI rewardText, TextMeshProUGUI requireText, TextMeshProUGUI progressText)
    {
        BindDelegate();

        questSlider = slider;
        rewardButton = button;
        rewardButton.onClick.AddListener(RequestQuestReward);
        rewardCountText = rewardText;
        rewardCountText.text = "x " + rewardCount.ToString();
        questRequireText = requireText;
        SetRequireText();
        questProgressText = progressText;


        // 서버로부터 정보 요청
        DummyQuestServer.SendQuestDataToPlayer(Player.GetUserID(), QuestType);
    }

    public void RequestQuestReward()
    {
        DummyQuestServer.UserRequestReward(Player.GetUserID(), QuestType);
    }

    protected abstract void CheckQuestClear();

    protected abstract void SetRequireText();

    protected abstract void BindDelegate();
}
