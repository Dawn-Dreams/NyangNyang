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

    // 적군 처치
    public delegate void OnUserKillEnemyTypeDelegate(EnemyMonsterType monsterType);
    public OnUserKillEnemyTypeDelegate OnUserKillEnemyType;
    // ============================================================
    // 퀘스트 진행 상황 정보 델리게이트
    public delegate void OnRenewQuestProgressDataDelegate(QuestCategory questCategory, QuestType questType, BigInteger newCurrentObtainWeaponCount);
    public OnRenewQuestProgressDataDelegate OnRenewQuestProgressData;

    // 서버로부터 값을 받은 후 해당 함수 호출을 통해 퀘스트의 데이터를 갱신
    public void LoadQuestProgressDataFromJson(QuestCategory questCategory, QuestType questType)
    {
        QuestJsonData data = new QuestJsonData();
        QuestSaveLoadManager.GetInstance().LoadQuestProgressData(questCategory, questType, out data);

        // 퀘스트를 받았는데, getReward 가 초기화 전에 이뤄졌다면 초기화
        if (questCategory == QuestCategory.Daily)
        {
            if (data.getReward)
            {
                //매일 오전 00시에 초기화 진행
                // -> month or date 가 다르면 바로 초기화
                DateTime getRewardTime = DateTime.Parse(data.getRewardTimeString);
                if (getRewardTime.Day != DateTime.Now.Day || getRewardTime.Month != DateTime.Now.Month ||
                    getRewardTime.Year != DateTime.Now.Year)
                {
                    data.getReward = false;
                    data.getRewardTimeString = "";
                    data.progressString = "0";
                    QuestSaveLoadManager.GetInstance().InitializeAndSaveQuestProgressData(questCategory, questType);
                }
            }
        }
        else if(questCategory == QuestCategory.Weekly)
        {
            if (data.getReward)
            {
                //매 주 월요일 오전 00시에 초기화 진행
                // 접속한 시간 기준, 월요일의 날자를 구한 뒤, 해당 날자보다 이전이면 초기화
                DateTime getRewardTime = DateTime.Parse(data.getRewardTimeString);
                DateTime today = DateTime.Today;
                DateTime mondayDateTime = DateTime.Today;
                int dayDiff = today.DayOfWeek - DayOfWeek.Monday;
                dayDiff = dayDiff + 7 % 7;
                mondayDateTime = today.AddDays(-dayDiff);

                if (getRewardTime <= mondayDateTime) 
                {
                    data.getReward = false;
                    data.getRewardTimeString = "";
                    data.progressString = "0";
                    QuestSaveLoadManager.GetInstance().InitializeAndSaveQuestProgressData(questCategory, questType);
                }
            }
            
        }


        BigInteger currentCount = BigInteger.Parse(data.progressString); 
            //DummyQuestServer.SendQuestProgressDataToClient(Player.GetUserID(), questCategory,questType);


        if (OnRenewQuestProgressData != null)
        {
            OnRenewQuestProgressData(questCategory, questType, currentCount);
        }
    }
}
