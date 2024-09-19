using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponMgrUI : MonoBehaviour
{

    public Slider[] sliders;
    public Text[] texts;

    private WeaponManager weaponManager;

    private void OnEnable()
    {
        weaponManager = gameObject.AddComponent<WeaponManager>();
        weaponManager.InitializedWeapons();

        UpdateAllPossession();
    }

    public void UpdateAllPossession()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            Weapon weapon = weaponManager.GetWeapon(i);
            if ( weapon != null )
            {
                sliders[i].value = (float)weapon.GetWeaponCount() / 5 >= 1 ? 1 : (float)weapon.GetWeaponCount() / 5;
                texts[i].text = weapon.GetWeaponCount().ToString() + "/5";
            }
        }
    }

}
