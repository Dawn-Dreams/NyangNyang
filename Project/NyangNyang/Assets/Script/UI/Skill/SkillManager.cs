using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private static SkillManager instance;
    public static SkillManager GetInstance() => instance;

    private Skill[] skills = new Skill[25];
    private Dictionary<string, int> skillDic = new Dictionary<string, int>();
    public Sprite[] sprites;

    // 밸런스 패치 필요
    private int[] levelUpCosts = new int[9] { 5000, 10000, 50000, 100000, 250000, 500000, 1000000, 2500000, 5000000};
    private int[] levelUpNeeds = new int[9] { 5, 10, 20, 20, 30, 50, 100, 500, 1000 };

    private void Awake()
    {
        if ( instance == null)
        {
            instance = this;
        }
        InitializedSkills();
    }

    public void InitializedSkills()
    {
        // TODO: 서버에서 데이터 받아오기

        for (int i = 0; i < 25; ++i)
        {
            skills[i] = new Skill(i, i.ToString(), 2000, 1, 5000);
            skillDic[i.ToString()] = i;
        }
    }

    public Skill GetSkill(int id)
    {
        if ( id >= 0 && id < skills.Length)
        {
            return skills[id];
        }
        return null;
    }

    public Skill GetSkill(string name)
    {
        int id;
        if (skillDic.TryGetValue(name, out id))
        {
            return skills[id];
        }
        return null;
    }

    public Sprite GetSprite(int id)
    {
        if ( id >= 0 && id < skills.Length)
        {
            return sprites[id];
        }
        return null;
    }

    public int LevelUpSkill(int id)
    {
        Skill skill = GetSkill(id);
        if ( skill != null && skill.GetLevel() < 10 && skill.GetPossession() >= levelUpNeeds[skill.GetLevel()-1] )
        {
            skill.SetPossession(-levelUpNeeds[skill.GetLevel() - 1]);
            skill.SetLevelUpCost(levelUpCosts[skill.GetLevel() - 1]);
            skill.AddLevel(1);
            return skill.GetLevel();
        }
        return -1;
    }

    public void AddSkillPossession(int id, int count)
    {
        Skill skill = GetSkill(id);
        if ( skill != null )
        {
            skill.SetPossession(count);

            // 11.12 이윤석 - 스킬 획득 퀘스트
            if (QuestManager.GetInstance().OnUserGetSkill != null)
            {
                QuestManager.GetInstance().OnUserGetSkill(count);
            }
        }
    }

    public int GetLevelUpCostPerLevel(int level)
    {
        if ( level >= 0 && level < levelUpCosts.Length)
        {
            return levelUpNeeds[level];
        }
        return -1;
    }

}
