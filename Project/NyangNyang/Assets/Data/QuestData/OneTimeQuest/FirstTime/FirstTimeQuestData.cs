using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[CreateAssetMenu(fileName = "FirstTimeQuestData", menuName = "ScriptableObjects/QuestData/AchievementQuest/FirstTimeQuestData", order = 1)]
public class FirstTimeQuestData : AchievementQuestData
{
    /*
     * 업적퀘스트 등 클라이언트 내에서 클리어를 체크하는 퀘스트들에 대한 클래스
     */
    public override void RequestCurrentUserQuestProgress()
    {
        // 클라에서 퀘스트 정보 체크
    }

    protected override void CheckQuestClear()
    {
        CheckIsOwningTitle();
    }

    protected override void SetRequireText()
    {
        //int progress = GetReward ? 0 : 1;
        //string progressText = progress.ToString();
        //progressText+= " / 1";

        // 해당 퀘스트는 퀘스트가 열리는 시점에서 무조건 클리어 됨
        QuestComp.SetRequireText("1 / 1");
        QuestComp.SetSliderValue(1);
    }

    protected override void BindDelegate()
    {
    }

    protected override void UnBindDelegate()
    {
    }

    protected override void RenewalUIAfterChangeQuestValue()
    {
        throw new System.NotImplementedException();
    }

    public override int GetRequireCount()
    {
        throw new System.NotImplementedException();
    }

    public override BigInteger GetCurrentQuestCount()
    {
        throw new System.NotImplementedException();
    }
}
