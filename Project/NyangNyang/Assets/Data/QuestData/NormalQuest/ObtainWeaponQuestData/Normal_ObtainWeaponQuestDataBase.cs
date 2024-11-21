using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[CreateAssetMenu(fileName = "ObtainWeaponQuestDataBase", menuName = "ScriptableObjects/QuestData/Normal/ObtainWeaponQuestDataBase", order = 1)]
public class Normal_ObtainWeaponQuestDataBase : NormalQuestDataBase
{
    protected int currentWeaponObtainCount = 0;

    public int requireWeaponObtainCount;

    public override void QuestActing(BaseQuest quest, QuestType type)
    {
        base.QuestActing(quest, QuestType.ObtainWeapon);
    }


    protected override void SetRequireText()
    {
        string newText = currentWeaponObtainCount + " / " +
                         requireWeaponObtainCount;
        QuestComp.SetRequireText(newText);
    }

    protected override void BindDelegate()
    {
        QuestManager.GetInstance().OnRenewQuestProgressData += GetDataFromServer;
        QuestManager.GetInstance().OnUserObtainWeapon += QuestCountChange;

    }

    protected override void UnBindDelegate()
    {
        QuestManager.GetInstance().OnRenewQuestProgressData -= GetDataFromServer;
        QuestManager.GetInstance().OnUserObtainWeapon -= QuestCountChange;
    }

    public override int GetRequireCount()
    {
        return requireWeaponObtainCount;
    }

    public override void ChangeCurrentProgressCountAfterReward()
    {
        int clearCount = (int)currentWeaponObtainCount / requireWeaponObtainCount;

        currentWeaponObtainCount -= requireWeaponObtainCount * clearCount;
    }

    public override BigInteger GetCurrentQuestCount()
    {
        return currentWeaponObtainCount;
    }

    // 서버로부터 데이터 값을 받아올 때
    public void GetDataFromServer(QuestCategory dataCategory, QuestType type,BigInteger newQuestDataValue)
    {
        if (questCategory != dataCategory || questType != type)
        {
            return;
        }

        currentWeaponObtainCount = (int)newQuestDataValue;

        RenewalUIAfterChangeQuestValue();
    }
    // 퀘스트를 수행하여 값이 바뀌었을 때,
    public void QuestCountChange(int obtainWeaponCount)
    {
        currentWeaponObtainCount += obtainWeaponCount;
        RenewalUIAfterChangeQuestValue();

        // TODO: 10.31) 추후엔 GameManager 라던가 기타 Monobehaviour 상속 클래스에서 보내도록 
        // 코루틴 써서
        SendDataToServer();
    }

    protected override void RenewalUIAfterChangeQuestValue()
    {
        float currentValue = (float)currentWeaponObtainCount / requireWeaponObtainCount;
        QuestComp.SetSliderValue(currentValue);

        SetRequireText();

        CheckQuestClear();
    }

    protected override void CheckQuestClear()
    {
        if (currentWeaponObtainCount >= requireWeaponObtainCount)
        {
            int clearCount = currentWeaponObtainCount / requireWeaponObtainCount;
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
