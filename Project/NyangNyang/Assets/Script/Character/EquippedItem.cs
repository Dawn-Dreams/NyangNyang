using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedItem : MonoBehaviour
{
    // ���
    Weapon EquippedWeapon;
    Weapon SelectedWeapon;

    // �нú� ��ų
    Skill[] EquippedSkills = new Skill[4];
    int CurSkillSlot;
    Skill SelectedSkill;

    // ��Ƽ�� ��ų
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
                    // ��Ƽ�� â�� ��������, ��Ƽ�� ��ų�� ������ ���
                    SelectedSkill = tmp;
                }
                else if ( !isOpenActiveSlot && tmp.GetID() > 4 && tmp.HasSkill() )
                {
                    Debug.Log(tmp.GetName());
                    // �нú� â�� ��������, �нú� ��ų�� ������ ���
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
