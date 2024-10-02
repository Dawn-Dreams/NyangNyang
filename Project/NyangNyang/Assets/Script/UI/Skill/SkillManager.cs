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

        for ( int i  = 0; i < 25; ++i )
        {
            skills[i] = new Skill(i, i.ToString(), 3, 1, 10);
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

    public void LevelUpSkill(int id)
    {
        Skill skill = GetSkill(id);
        if ( skill != null && skill.HasSkill() && skill.GetLevel() < 100 )
        {
            skill.AddLevel(1);
        }
    }

    public void AddSkillPossession(int id, int count)
    {
        Skill skill = GetSkill(id);
        if ( skill != null )
        {
            skill.SetPossession(count);
        }
    }

}
