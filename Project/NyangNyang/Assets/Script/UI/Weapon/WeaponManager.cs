using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    private static WeaponManager Instance;
    public static WeaponManager GetInstance() { return Instance; }

    private Weapon[] weapons = new Weapon[32];
    private Dictionary<string, int> weaponDic = new Dictionary<string, int>();
    public Sprite[] sprites;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        InitializedWeapons();
    }

    public void InitializedWeapons()
    {
        // TODO: �������� ������ �޾ƿ���

        for (int i = 0; i < 32; ++i)
        {
            weapons[i] = new Weapon(i, i.ToString(), i, i, 1, 10);
            weaponDic[i.ToString()] = i;
        }
    }


    public Sprite GetSprite(int id)
    {
        if ( id >= 0 && id < sprites.Length)
        {
            return sprites[id];
        }
        return null;
    }

    public Weapon GetWeapon(int id)
    {
        if ( id < 0 || id >= weapons.Length)
        {
            return null;
        }
        return weapons[id];
    }

    public Weapon GetWeapon(string name)
    {
        int id;
        if(weaponDic.TryGetValue(name, out id))
        {
            return weapons[id];
        }
        return null;
    }

    public int LevelUpWeapon(int id)
    {
        Weapon weapon = GetWeapon(id);
        if ( weapon != null && weapon.HasWeapon() && weapon.GetLevel() < 100 )
        {
            return weapon.LevelUP();
        }
        return 1000;
    }

    public void EnhanceEffectWeapon(int id)
    {
        Weapon weapon = GetWeapon(id);
        if ( weapon != null && weapon.HasWeapon() && weapon.GetLevel() < 100)
        {
            weapon.StatusUpgrade();
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

    public bool CombineWeapon(int id)
    {
        if ( id >= 0 && id < weapons.Length - 1 )
        {
            Weapon weapon = GetWeapon(id);
            if ( weapon != null )
            {
                weapon.AddWeapon(-5);

                weapon = GetWeapon(id + 1);

                if(weapon != null)
                {
                    weapon.AddWeapon(1);
                }
            }
            return true;
        }
        return false;
    }
}
