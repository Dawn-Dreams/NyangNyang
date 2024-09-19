using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private Weapon[] weapons = new Weapon[32];

    public void InitializedWeapons()
    {
        // TODO: �������� ������ �޾ƿ���
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
