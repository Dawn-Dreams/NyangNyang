using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private static SkillManager instance;
    public static SkillManager GetInstance() => instance;


    public List<Skill> SkillList = new List<Skill>();

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

        // 액티브 스킬
        skills[0] = new ActiveSkill(0, "우주 냥경찰 출동!", 1, 1, 1, 1f);
        skillDic["우주 냥경찰 출동!"] = 0;

        skills[1] = new ActiveSkill(1, "캣닢비가 내려와", 1, 1, 1, 2f);
        skillDic["캣닢비가 내려와"] = 1;

        skills[2] = new ActiveSkill(2, "냥냥대원들아 도와줘", 1, 1, 1, 3f);
        skillDic["냥냥대원들아 도와줘"] = 2;

        skills[3] = new ActiveSkill(3, "자린고비냥", 0, 1, 1, 4f);
        skillDic["자린고비냥"] = 3;

        skills[4] = new ActiveSkill(4, "실타래 폭탄", 0, 1, 1, 4f);
        skillDic["실타래 폭탄"] = 4;

        for (int i = 5; i < 25; ++i)
        {
            skills[i] = new DefenseUpSkill(i, "털복숭이 갑옷", 1, 1, 1, 1f);
            skillDic["털복숭이 갑옷"] = i;
        }

        // 패시브 스킬
        //SkillList.Add(new DefenseUpSkill(9, "털복숭이 갑옷", 1, 1, 1, 1f));
        //SkillList.Add(new DefenseUpSkill(10, "꼬리 방패", 1, 1, 1, 2f));
        //SkillList.Add(new DefenseUpSkill(11, "퐁퐁 쿠션 아머", 0, 1, 1, 3f));
        //SkillList.Add(new DefenseUpSkill(12, "튼튼한 발톱 가드", 0, 1, 1, 4f));

        //SkillList.Add(new AttackUpSkill(21, "젤리 파워업", 0, 1, 1, 1f));
        //SkillList.Add(new AttackUpSkill(22, "모래 목욕", 0, 1, 1, 2f));
        //SkillList.Add(new AttackUpSkill(23, "포동포동 냥이", 0, 1, 1, 3f));
        //SkillList.Add(new AttackUpSkill(24, "별빛 힐링", 0, 1, 1, 4f));
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

    public void LetSkillActivate(int id)
    {
        Skill skill = GetSkill(id);
        if ( skill != null)
        {
            skill.Activate();
        }
    }

    public void LetSkillDeActivate(int id)
    {
        Skill skill = GetSkill(id);
        if (skill != null)
        {
            skill.Deactivate();
        }
    }

}
