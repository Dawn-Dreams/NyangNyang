using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[CreateAssetMenu(fileName = "KillMonsterTypeQuestData", menuName = "ScriptableObjects/QuestData/AchievementQuest/KillMonsterTypeQuestData", order = 1)]
public class KillMonsterTypeQuestDataBase : AchievementQuestData
{
    /*
        KillMonsterTypeQuest의 경우 보상으로 코스튬(펫)을 제공

        따라서 CheckIsOwningTitle 대신 다른 함수를 사용
     */

    private int _currentKillCount;
    public int requireKillCount;
    public EnemyMonsterType targetMonsterType;
    private bool _isBindingDelegate = false;

    public override void QuestActing(BaseQuest quest, QuestType type)
    {
        mainQuestTitle = $"\n펫 [{targetMonsterType}]";
        base.QuestActing(quest, type);
    }

    protected override void QuestActingAction()
    {
        BindDelegate();
        QuestManager.GetInstance().GetQuestProgressDataFromServer(questCategory, questType);
    }

    protected override void CheckQuestClear()
    {
        CheckIsOwningPet();
    }

    private void CheckIsOwningPet()
    {
        bool isOwning = PlayerCostume.playerOwningCostume[CatCostumePart.Pet].Contains((int)targetMonsterType);
        if (isOwning)
        {
            if (_isBindingDelegate)
            {
                UnBindDelegate();
            }
            QuestComp.questSlider.gameObject.SetActive(false);
            QuestComp.SetRewardButtonInteractable(false, "보유중");
            GetReward = true;
        }
        else
        {
            QuestComp.SetRewardButtonInteractable(true);
            GetReward = false;
        }
    }

    protected override void SetRequireText()
    {
        QuestComp.SetRequireText($"{_currentKillCount} / {requireKillCount}");
        QuestComp.SetSliderValue((float)_currentKillCount/requireKillCount);
    }

    protected override void BindDelegate()
    {
        if (!_isBindingDelegate)
        {
            _isBindingDelegate = true;

            PlayerCostume.OnOwningCostumeChange += CheckIsOwningPet;
            QuestManager.GetInstance().OnUserKillEnemyType += UserKillEnemyType;
            QuestManager.GetInstance().OnRenewQuestProgressData += GetProgressDataFromServer;
        }
    }

    protected override void UnBindDelegate()
    {
        if (_isBindingDelegate)
        {
            _isBindingDelegate = false;

            PlayerCostume.OnOwningCostumeChange -= CheckIsOwningPet;
            QuestManager.GetInstance().OnUserKillEnemyType -= UserKillEnemyType;
            QuestManager.GetInstance().OnRenewQuestProgressData -= GetProgressDataFromServer;
        }
        
    }

    private void GetProgressDataFromServer(QuestCategory category, QuestType type, BigInteger newVal)
    {
        if (category != questCategory || type != questType)
        {
            return;
        }
        _currentKillCount = (int)newVal;

        RenewalUIAfterChangeQuestValue();
    }
    protected void UserKillEnemyType(EnemyMonsterType monsterType)
    {
        if (monsterType != targetMonsterType)
        {
            return;
        }
        _currentKillCount += 1;
        RenewalUIAfterChangeQuestValue();

        // TODO: 딜레이를 통해 서버에 정보를 저장 시킬 수 있도록
    }

    protected override void RenewalUIAfterChangeQuestValue()
    {
        SetRequireText();
        CheckQuestClear();
    }

    public override void RequestQuestReward()
    {
        
        if (!PlayerCostume.playerOwningCostume[CatCostumePart.Pet].Contains((int)targetMonsterType))
        {
            PlayerCostume.playerOwningCostume[CatCostumePart.Pet].Add((int)targetMonsterType);
            Debug.Log($"유저가 코스튬 {targetMonsterType} 펫을 흭득하였습니다.");
        }
        else
        {
            Debug.Log("오류 - 이미 보유중인 펫");
        }
        DummyPlayerCostumeServer.UserRequestAcquireCostume(Player.GetUserID(),CatCostumePart.Pet, (int)targetMonsterType);
        GetReward = true;
        RenewalUIAfterChangeQuestValue();
    }
}
