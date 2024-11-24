using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedItem : MonoBehaviour
{
    Weapon EquippedWeapon;
    Weapon SelectedWeapon;

    Skill[] EquippedSkills = new Skill[4];
    int CurSkillSlot;
    Skill SelectedSkill;

    [SerializeField]
    Image WeaponImage;

    [SerializeField]
    List<Image> SkillImages;

    [SerializeField]
    GameObject WeaponPopUp;

    [SerializeField]
    GameObject SkillPopUp;

    public void OnClickedWeaponEquippedButton()
    {
        if (SelectedWeapon != null)
        {
            
            EquippedWeapon = SelectedWeapon;
            UpdateEquippedWeapon();
        }
    }

    public void OnClickedSkillEquippedButton()
    {
        if (SelectedSkill != null)
        {
            EquippedSkills[CurSkillSlot] = SelectedSkill;
            UpdateEquippedSkill();
        }
    }

    public void OnClickedSkillSlot(int slot)
    {
        if ( slot >= 0 && slot < 4)
        {
            CurSkillSlot = slot;
        }
    }

    public void OnClickedWeapon(GameObject _obj)
    {
        if (_obj != null)
        {
            
            Weapon t = WeaponManager.GetInstance().GetWeapon(_obj.name);
            
            if ( t != null && !t.GetIsLock() )
            {
                Debug.Log(_obj.name);
                SelectedWeapon = t;
            }
        }
    }

    public void OnClickedSkill(GameObject _obj)
    {
        if (_obj != null)
        {
            SelectedSkill = SkillManager.GetInstance().GetSkill(_obj.name);
        }
    }

    void UpdateEquippedWeapon()
    {
        int id = SelectedWeapon.GetID();

        Sprite s = WeaponManager.GetInstance().GetSprite(id);

        if (s != null)
        {
            WeaponImage.sprite = s;
            Player.playerStatus.SetWeaponEffect(SelectedWeapon.GetEffect());
            WeaponPopUp.SetActive(false);
            SelectedWeapon = null;

        }

    }

    void UpdateEquippedSkill()
    {
        int id = SelectedSkill.GetID();

        Sprite s = SkillManager.GetInstance().GetSprite(id);

        if (s != null)
        {
            SkillImages[CurSkillSlot].sprite = s;
            SkillManager.GetInstance().LetSkillActivate(id);


            SkillPopUp.SetActive(false);
            SelectedSkill = null;
            CurSkillSlot = -1;

        }

    }

}
