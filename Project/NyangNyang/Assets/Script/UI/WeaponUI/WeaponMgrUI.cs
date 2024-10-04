using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponMgrUI : MonoBehaviour
{
    public GameObject[] weapons;
    WeaponUnlock[] weaponList = new WeaponUnlock[32];
    Slider[] sliders = new Slider[32];
    Text[] texts = new Text[32];

    public void Awake()
    {
        for ( int i = 0; i < weapons.Length; i++)
        {
            weaponList[i] = weapons[i].GetComponent<WeaponUnlock>();
            sliders[i] = weapons[i].transform.Find("Slider").GetComponent<Slider>();
            texts[i] = weapons[i].transform.Find("possession").GetComponent<Text>();
        }
    }

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
                if (weapon.HasWeapon())
                {
                    weaponList[i].GetComponent<WeaponUnlock>().Unlock();
                }
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
            weaponList[id].Unlock();
            sliders[id].value = (float)weapon.GetWeaponCount() / 5 >= 1 ? 1 : (float)weapon.GetWeaponCount() / 5;
            texts[id].text = weapon.GetWeaponCount().ToString() + "/5";
        }
    }

}
