using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpSkill : MonoBehaviour
{
    public ScrollRect ScrollRect;
    public float space = 10f;
    public GameObject uiPrefeb;
    public List<RectTransform> uiObjects = new List<RectTransform>();
    public GameObject AllContent;

    private void Start()
    {
    }

    public void ShowPickUpWeapon()
    {
        // 한 개 뽑기
        var newUI = Instantiate(uiPrefeb, ScrollRect.content);
        RectTransform newUIRect = newUI.GetComponent<RectTransform>();

        // TODO: newUI 속의 내용 작성하기 뽑기에서 나온 결과물로**
        int id = 0;

        Image img = newUI.transform.Find("Image").GetComponent<Image>();
        img.sprite = SkillManager.GetInstance().GetSprite(id);
        newUI.GetComponent<WeaponUnlock>().Unlock();
        SkillManager.GetInstance().AddSkillPossession(id, 1);

        Skill skill = SkillManager.GetInstance().GetSkill(id);

        int num = SkillManager.GetInstance().GetLevelUpCostPerLevel(skill.GetLevel() - 1);

        Slider slider = newUI.transform.Find("Slider").GetComponent<Slider>();
        slider.value = (float)skill.GetPossession() / num >= 1 ? 1 : (float)skill.GetPossession() / num;

        Text text = newUI.transform.Find("possession").GetComponent<Text>();
        text.text = skill.GetPossession().ToString() + "/" + num;

        float contentHeight = ScrollRect.viewport.sizeDelta.y;
        float uiHeight = newUIRect.sizeDelta.y;

        float centerY = (contentHeight + uiHeight) / 2f;
        newUIRect.anchoredPosition = new Vector2(0f, -centerY);
    }

    public void ShowPickUpWeapons()
    {
        // 일괄 뽑기

        AllContent.SetActive(true);
        //for ( int i = 0; i < uiObjects.Count; ++i )
        //{

        //    // TODO: newUI 속의 내용 작성하기 뽑기에서 나온 결과물로**
        //}
    }
}
