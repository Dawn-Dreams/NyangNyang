using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private Weapon[] weapons = new Weapon[32];

    public void InitializedWeapons()
    {
        // TODO: 서버에서 데이터 받아오기

        for (int i = 0; i < 32; ++i)
        {
            weapons[i] = new Weapon(i.ToString(), i, i, 1, 4);
        }
    }

    public Weapon GetWeapon(int id)
    {
        if ( id < 0 || id >= weapons.Length)
        {
            return null;
        }
        return weapons[id];
    }

    public void LevelUpWeapon(int id)
    {
        Weapon weapon = GetWeapon(id);
        if ( weapon != null && weapon.HasWeapon() )
        {
            weapon.LevelUP();
        }
    }

    public void AddWeaponCount(int id, int count)
    {
        Weapon weapon = GetWeapon(id);
        if (weapon != null)
        {
            weapon.AddWeapon(count);
        }
    }

}
