using System;
[Serializable]
public enum QuestType
{
    // 일반 퀘스트
    GoldSpending, KillMonster, ObtainWeapon, CombineWeapon, ObtainSkill, SkillLevelUp, 
    // 스토리
    LevelUpStatus, StageClear,
    // 업적
    FirstTime,
    KillStarfish, KillOctopus,KillPuffe, KillShellfish, KillKrake,
}
[Serializable]
public enum QuestCategory
{
    Repeat,
    Daily,
    Weekly,
    Achievement,
    Story,
    Count
}
[Serializable]
public enum RewardType
{
    Gold, Diamond
}
