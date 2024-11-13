using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedItem : MonoBehaviour
{
    Weapon EquippedWeapon;
    Weapon SelectedWeapon;
    List<Skill> EquippedSkills;

    public void OnClickedWeaponEquippedButton()
    {
        if (SelectedWeapon != null)
        {
            EquippedWeapon = SelectedWeapon;
        }
    }

    public void OnClickedWeapon(GameObject _obj)
    {
        SelectedWeapon = WeaponManager.GetInstance().GetWeapon(_obj.name);
    }

    void UpdateEquippedWeapon()
    {

    }

}
