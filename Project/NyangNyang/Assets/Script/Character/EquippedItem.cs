using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedItem : MonoBehaviour
{
    // 장비
    Weapon EquippedWeapon;
    Weapon SelectedWeapon;

    // 패시브 스킬
    Skill[] EquippedSkills = new Skill[4];
    int CurSkillSlot;
    Skill SelectedSkill;

    // 액티브 스킬
    public ActiveSkillManager ActiveSkillManager;
    Skill CurActiveSkill;
    public Image ActiveSkillImage;

    [SerializeField]
    Image WeaponImage;

    [SerializeField]
    List<Image> SkillImages;

    [SerializeField]
    GameObject WeaponPopUp;

    [SerializeField]
    GameObject SkillPopUp;
    public bool isOpenActiveSlot = false;

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
        if (SelectedSkill != null && !isOpenActiveSlot)
        {
            SkillManager.GetInstance().LetSkillDeActivate(EquippedSkills[CurSkillSlot].GetID());
            EquippedSkills[CurSkillSlot] = SelectedSkill;
            UpdateEquippedSkill();
        }
    }

    public void OnClickedActiveSkillEquippedOKButton()
    {
        if (SelectedSkill != null && isOpenActiveSlot)
        {
            CurActiveSkill = SelectedSkill;
            UpdateEquippedActiveSkill();
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
            Skill tmp = SkillManager.GetInstance().GetSkill(_obj.name);
            if ( tmp != null )
            {
                if ( isOpenActiveSlot && tmp.GetID() < 5 && tmp.HasSkill() )
                {
                    // 액티브 창을 열었으며, 액티브 스킬을 선택한 경우
                    SelectedSkill = tmp;
                }
                else if ( !isOpenActiveSlot && tmp.GetID() > 4 && tmp.HasSkill() )
                {
                    Debug.Log(tmp.GetName());
                    // 패시브 창을 열었으며, 패시브 스킬을 선택한 경우
                    SelectedSkill = tmp;
                }
            }
        }
    }

    public void OnClickedCancleButton()
    {
        isOpenActiveSlot = false;
        SelectedSkill = null;
        SelectedWeapon = null;
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

    void UpdateEquippedActiveSkill()
    {
        int id = SelectedSkill.GetID();

        Sprite s = SkillManager.GetInstance().GetSprite(id);

        if (s != null)
        {
            ActiveSkillImage.sprite = s;

            SkillPopUp.SetActive(false);
            isOpenActiveSlot = false;
            SelectedSkill = null;
        }
    }

    public void OnClickedActiveSkillEquippedButton()
    {
        SkillPopUp.SetActive(true);
        isOpenActiveSlot = true;
    }

    public void ActivateActiveSkill()
    {
        if ( CurActiveSkill != null )
        {
            ActiveSkillManager.CurSkillActivate(CurActiveSkill.GetID());
        }
    }
}
