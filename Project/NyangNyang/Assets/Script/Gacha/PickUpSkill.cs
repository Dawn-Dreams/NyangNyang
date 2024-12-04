using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PickUpSkill : MonoBehaviour
{
    public GameObject AllContent;
    public GameObject OneContent;


    private void OnDisable()
    {
        AllContent.SetActive(false);
        OneContent.SetActive(false);
    }

    public void ShowPickUpSkill()
    {
        // 한 개 뽑기

        OneContent.SetActive(true);

        // TODO: OneContent 속의 내용 작성하기 뽑기에서 나온 결과물로**
        int id = 0;

        SetPickUPSkill(id, OneContent);

    }

    public void ShowPickUpSkills()
    {
        // 일괄 뽑기
        AllContent.SetActive(true);

        Transform _allT = AllContent.transform;
        foreach (Transform child in _allT)
        {
            // TODO: child 속의 내용 작성하기 뽑기에서 나온 결과물로**
            int id = 0;

            SetPickUPSkill(id, child.gameObject);
        }
    }

    public void SetPickUPSkill(int id, GameObject _obj)
    {
        Image img = _obj.transform.Find("Image").GetComponent<Image>();
        img.sprite = SkillManager.GetInstance().GetSprite(id);
        _obj.GetComponent<WeaponUnlock>().Unlock();
        SkillManager.GetInstance().AddSkillPossession(id, 1);

        Skill skill = SkillManager.GetInstance().GetSkill(id);

        int num = SkillManager.GetInstance().GetLevelUpCostPerLevel(skill.GetLevel() - 1);

        Slider slider = _obj.transform.Find("Slider").GetComponent<Slider>();
        slider.value = (float)skill.GetCount() / num >= 1 ? 1 : (float)skill.GetCount() / num;

        Text text = _obj.transform.Find("possession").GetComponent<Text>();
        text.text = skill.GetCount().ToString() + "/" + num;
    }
}
