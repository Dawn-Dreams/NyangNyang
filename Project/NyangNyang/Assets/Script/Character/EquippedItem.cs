using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedItem : MonoBehaviour
{
    Weapon EquippedWeapon;
    Weapon SelectedWeapon;
    List<Skill> EquippedSkills;

    [SerializeField]
    Image WeaponImage;

    public void OnClickedWeaponEquippedButton()
    {
        if (SelectedWeapon != null)
        {
            EquippedWeapon = SelectedWeapon;
            UpdateEquippedWeapon();
        }
    }

    public void OnClickedWeapon(GameObject _obj)
    {
        if ( _obj != null)
        {
            SelectedWeapon = WeaponManager.GetInstance().GetWeapon(_obj.name);
        }
    }

    void UpdateEquippedWeapon()
    {
        int id = SelectedWeapon.GetID();
        WeaponImage.sprite = WeaponManager.GetInstance().GetSprite(id);

        /*
            공격력 관련 코드 작성         
         */
    }

}
