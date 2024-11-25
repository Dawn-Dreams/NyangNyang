using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ObtainSkillQuestDataBase", menuName = "ScriptableObjects/QuestData/Normal/ObtainSkillQuestDataBase", order = 1)]
public class Normal_ObtainSkillQuestDataBase : NormalQuestDataBase
{
    protected int currentSkillObtainCount = 0;

    public int requireSkillObtainCount;

    public override void QuestActing(BaseQuest quest, QuestType type)
    {
        base.QuestActing(quest, QuestType.ObtainSkill);
    }


    protected override void SetRequireText()
    {
        string newText = currentSkillObtainCount + " / " +
                         requireSkillObtainCount;
        QuestComp.SetRequireText(newText);
    }

    protected override void BindDelegate()
    {
        QuestManager.GetInstance().OnRenewQuestProgressData += GetDataFromServer;
        QuestManager.GetInstance().OnUserGetSkill += QuestCountChange;

    }

    protected override void UnBindDelegate()
    {
        QuestManager.GetInstance().OnRenewQuestProgressData -= GetDataFromServer;
        QuestManager.GetInstance().OnUserGetSkill -= QuestCountChange;
    }

    public override int GetRequireCount()
    {
        return requireSkillObtainCount;
    }

    public override void ChangeCurrentProgressCountAfterReward()
    {
        int clearCount = (int)currentSkillObtainCount / requireSkillObtainCount;

        currentSkillObtainCount -= requireSkillObtainCount * clearCount;
    }

    public override BigInteger GetCurrentQuestCount()
    {
        return currentSkillObtainCount;
    }

    // 서버로부터 데이터 값을 받아올 때
    public void GetDataFromServer(QuestCategory dataCategory, QuestType type,BigInteger newQuestDataValue)
    {
        if (questCategory != dataCategory || questType != type)
        {
            return;
        }

        currentSkillObtainCount = (int)newQuestDataValue;

        RenewalUIAfterChangeQuestValue();
    }
    // 퀘스트를 수행하여 값이 바뀌었을 때,
    public void QuestCountChange(int obtainWeaponCount)
    {
        currentSkillObtainCount += obtainWeaponCount;
        RenewalUIAfterChangeQuestValue();

        // TODO: 10.31) 추후엔 GameManager 라던가 기타 Monobehaviour 상속 클래스에서 보내도록 
        // 코루틴 써서
        SaveDataToJson();
    }

    protected override void RenewalUIAfterChangeQuestValue()
    {
        float currentValue = (float)currentSkillObtainCount / requireSkillObtainCount;
        QuestComp.SetSliderValue(currentValue);

        SetRequireText();

        CheckQuestClear();
    }

    protected override void CheckQuestClear()
    {
        if (currentSkillObtainCount >= requireSkillObtainCount)
        {
            int clearCount = currentSkillObtainCount / requireSkillObtainCount;
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
