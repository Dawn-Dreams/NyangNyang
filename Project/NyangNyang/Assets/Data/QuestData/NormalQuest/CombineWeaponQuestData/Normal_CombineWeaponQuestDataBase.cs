using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CombineWeaponQuestData", menuName = "ScriptableObjects/QuestData/Normal/CombineWeaponQuestData", order = 1)]
public class Normal_CombineWeaponQuestDataBase : NormalQuestDataBase
{
    protected int currentCombineWeaponCount = 0;

    public int requireCombineWeaponCount;

    public override void QuestActing(BaseQuest quest, QuestType type)
    {
        base.QuestActing(quest, QuestType.CombineWeapon);
    }


    protected override void SetRequireText()
    {
        string newText = currentCombineWeaponCount + " / " +
                         requireCombineWeaponCount;
        QuestComp.SetRequireText(newText);
    }

    protected override void BindDelegate()
    {
        QuestManager.GetInstance().OnRenewQuestProgressData += GetDataFromServer;
        QuestManager.GetInstance().OnUserWeaponCombine += QuestCountChange;

    }

    protected override void UnBindDelegate()
    {
        QuestManager.GetInstance().OnRenewQuestProgressData -= GetDataFromServer;
        QuestManager.GetInstance().OnUserWeaponCombine -= QuestCountChange;
    }

    public override int GetRequireCount()
    {
        return requireCombineWeaponCount;
    }

    public override int ChangeCurrentProgressCountAfterReward()
    {
        int clearCount = (int)currentCombineWeaponCount / requireCombineWeaponCount;

        currentCombineWeaponCount -= requireCombineWeaponCount * clearCount;
        return clearCount;
    }

    public override BigInteger GetCurrentQuestCount()
    {
        return currentCombineWeaponCount;
    }

    // 서버로부터 데이터 값을 받아올 때
    public void GetDataFromServer(QuestCategory dataCategory, QuestType type,BigInteger newQuestDataValue)
    {
        if (questCategory != dataCategory || questType != type)
        {
            return;
        }

        currentCombineWeaponCount = (int)newQuestDataValue;

        RenewalUIAfterChangeQuestValue();
    }
    // 퀘스트를 수행하여 값이 바뀌었을 때,
    public void QuestCountChange(int obtainWeaponCount)
    {
        currentCombineWeaponCount += obtainWeaponCount;
        RenewalUIAfterChangeQuestValue();

        // TODO: 10.31) 추후엔 GameManager 라던가 기타 Monobehaviour 상속 클래스에서 보내도록 
        // 코루틴 써서
        SaveDataToJson();
    }

    protected override void RenewalUIAfterChangeQuestValue()
    {
        float currentValue = (float)currentCombineWeaponCount / requireCombineWeaponCount;
        QuestComp.SetSliderValue(currentValue);

        SetRequireText();

        CheckQuestClear();
    }

    protected override void CheckQuestClear()
    {
        if (currentCombineWeaponCount >= requireCombineWeaponCount)
        {
            int clearCount = currentCombineWeaponCount / requireCombineWeaponCount;
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
