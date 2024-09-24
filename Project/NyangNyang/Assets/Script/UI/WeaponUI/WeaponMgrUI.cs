using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponMgrUI : MonoBehaviour
{

    public Slider[] sliders;
    public Text[] texts;

    private void OnEnable()
    {
        UpdateAllPossession();
    }

    public void UpdateAllPossession()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            Weapon weapon = WeaponManager.GetInstance().GetWeapon(i);
            if ( weapon != null )
            {
                sliders[i].value = (float)weapon.GetWeaponCount() / 5 >= 1 ? 1 : (float)weapon.GetWeaponCount() / 5;
                texts[i].text = weapon.GetWeaponCount().ToString() + "/5";
            }
        }
    }

    public void UpdatePossession(int id)
    {
        Weapon weapon = WeaponManager.GetInstance().GetWeapon(id);
        if (weapon != null)
        {
            sliders[id].value = (float)weapon.GetWeaponCount() / 5 >= 1 ? 1 : (float)weapon.GetWeaponCount() / 5;
            texts[id].text = weapon.GetWeaponCount().ToString() + "/5";
        }
    }

}
