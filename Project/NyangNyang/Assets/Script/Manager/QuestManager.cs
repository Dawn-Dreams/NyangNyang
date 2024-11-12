using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private static QuestManager _instance;

    public static QuestManager GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    // 퀘스트 델리게이트 ===========================================
    // 무기 획득
    public delegate void OnUserObtainWeaponDelegate(int getWeaponCount);
    public OnUserObtainWeaponDelegate OnUserObtainWeapon;

    // 무기 합성(레벨업)
    public delegate void OnUserWeaponCombineDelegate(int weaponCombineCount);
    public OnUserWeaponCombineDelegate OnUserWeaponCombine;

    // 스킬 획득
    public delegate void OnUserGetSkillDelegate(int getSkillCount);
    public OnUserGetSkillDelegate OnUserGetSkill;

    // 무기 합성(레벨업)
    public delegate void OnUserSkillLevelUpDelegate(int skillLevelUpCount);
    public OnUserSkillLevelUpDelegate OnUserSkillLevelUp;
    // ============================================================
    // 퀘스트 진행 상황 정보 델리게이트
    public delegate void OnRenewQuestProgressDataDelegate(QuestCategory questCategory, QuestType questType, BigInteger newCurrentObtainWeaponCount);
    public OnRenewQuestProgressDataDelegate OnRenewQuestProgressData;

    // 서버로부터 값을 받은 후 해당 함수 호출을 통해 퀘스트의 데이터를 갱신
    public void GetQuestProgressDataFromServer(QuestCategory questCategory, QuestType questType)
    {
        // TODO: 서버로부터 값받은 이후에 사용되는 함수, 다만 현재는 바로 값을 받도록 구현
        // TODO 현재는 테스트를 위함이므로 값을 고정
        questCategory = QuestCategory.Daily;
        questType = QuestType.ObtainWeapon;
        // TODO : 나중엔 서버에서 받은 후 실행되는거니까 값 갱신 지우기
        BigInteger currentCount =
            DummyQuestServer.SendQuestProgressDataToClient(Player.GetUserID(), questCategory,
                questType);

        if (OnRenewQuestProgressData != null)
        {
            OnRenewQuestProgressData(questCategory, questType, currentCount);
        }
    }
}
