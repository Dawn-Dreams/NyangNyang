using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SkillLevelUpQuestData", menuName = "ScriptableObjects/QuestData/Normal/SkillLevelUpQuestData", order = 1)]
public class Normal_SkillLevelUpQuestDataBase : NormalQuestDataBase
{
    protected int currentSkillLevelUpCount = 0;

    public int requireSkillLevelUpCount;

    public override void QuestActing(BaseQuest quest, QuestType type)
    {
        base.QuestActing(quest, questType);
    }


    protected override void SetRequireText()
    {
        string newText = currentSkillLevelUpCount + " / " +
                         requireSkillLevelUpCount;
        QuestComp.SetRequireText(newText);
    }

    protected override void BindDelegate()
    {
        QuestManager.GetInstance().OnRenewQuestProgressData += GetDataFromServer;
        QuestManager.GetInstance().OnUserSkillLevelUp += QuestCountChange;

    }

    protected override void UnBindDelegate()
    {
        QuestManager.GetInstance().OnRenewQuestProgressData -= GetDataFromServer;
        QuestManager.GetInstance().OnUserSkillLevelUp -= QuestCountChange;
    }

    public override int GetRequireCount()
    {
        return requireSkillLevelUpCount;
    }

    public override void ChangeCurrentProgressCountAfterReward()
    {
        int clearCount = (int)currentSkillLevelUpCount / requireSkillLevelUpCount;

        currentSkillLevelUpCount -= requireSkillLevelUpCount * clearCount;
    }

    public override BigInteger GetCurrentQuestCount()
    {
        return currentSkillLevelUpCount;
    }

    // 서버로부터 데이터 값을 받아올 때
    public void GetDataFromServer(QuestCategory dataCategory, QuestType type,BigInteger newQuestDataValue)
    {
        if (questCategory != dataCategory || questType != type)
        {
            return;
        }

        currentSkillLevelUpCount = (int)newQuestDataValue;

        RenewalUIAfterChangeQuestValue();
    }
    // 퀘스트를 수행하여 값이 바뀌었을 때,
    public void QuestCountChange(int obtainWeaponCount)
    {
        currentSkillLevelUpCount += obtainWeaponCount;
        RenewalUIAfterChangeQuestValue();

        // TODO: 10.31) 추후엔 GameManager 라던가 기타 Monobehaviour 상속 클래스에서 보내도록 
        // 코루틴 써서
        SendDataToServer();
    }

    protected override void RenewalUIAfterChangeQuestValue()
    {
        float currentValue = (float)currentSkillLevelUpCount / requireSkillLevelUpCount;
        QuestComp.SetSliderValue(currentValue);

        SetRequireText();

        CheckQuestClear();
    }

    protected override void CheckQuestClear()
    {
        if (currentSkillLevelUpCount >= requireSkillLevelUpCount)
        {
            int clearCount = currentSkillLevelUpCount / requireSkillLevelUpCount;
            QuestComp.SetRewardButtonInteractable(true, "보상받기");
            QuestComp.SetRewardCountText(rewardCount, clearCount, IsRewardRepeatable());
        }
        else
        {
            QuestComp.SetRewardButtonInteractable(false, "진행중");
            QuestComp.SetRewardCountText(rewardCount, 1, IsRewardRepeatable());
        }
    }
}
