using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetailUI : MonoBehaviour
{
    private Weapon choosedWeapon;

    private void OnEnable()
    {
        choosedWeapon = null;
    }

    public void OnClickedWeapon(GameObject _obj)
    {
        if ( choosedWeapon == null)
        {
            choosedWeapon = WeaponManager.GetInstance().GetWeapon(_obj.name);
        }
    }

    public void OnClickedCancle()
    {
        if ( choosedWeapon != null )
        {
            choosedWeapon = null;
        }
    }
}
