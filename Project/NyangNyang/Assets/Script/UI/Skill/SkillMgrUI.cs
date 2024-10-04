using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMgrUI : MonoBehaviour
{
    public GameObject[] skills;
    WeaponUnlock[] skillList = new WeaponUnlock[25];
    Slider[] sliders = new Slider[25];
    Text[] texts = new Text[25];

    private void Awake()
    {
        for ( int i = 0; i < skills.Length; i++)
        {
            skillList[i] = skills[i].GetComponent<WeaponUnlock>();
            sliders[i] = skills[i].transform.Find("Slider").GetComponent<Slider>();
            texts[i] = skills[i].transform.Find("possession").GetComponent<Text>();
        }
    }

    private void OnEnable()
    {
        UpdateAllPossession();
    }

    public void UpdateAllPossession()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            Skill skill = SkillManager.GetInstance().GetSkill(i);
            if (skill != null)
            {
                if (skill.HasSkill())
                {
                    skillList[i].GetComponent<WeaponUnlock>().Unlock();
                }
                sliders[i].value = (float)skill.GetPossession() / 5 >= 1 ? 1 : (float)skill.GetPossession() / 5;
                texts[i].text = skill.GetPossession().ToString() + "/5";
            }
        }
    }

    public void UpdatePossession(int id)
    {
        Skill skill = SkillManager.GetInstance().GetSkill(id);
        if (skill != null)
        {
            skillList[id].Unlock();
            sliders[id].value = (float)skill.GetPossession() / 5 >= 1 ? 1 : (float)skill.GetPossession() / 5;
            texts[id].text = skill.GetPossession().ToString() + "/5";
        }
    }
}
