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
        // TODO: 서버에서 데이터 받아오기


        weapons[0] = new Weapon(0, "낡은 횃불", 1, 1, 1, 10);
        weaponDic["낡은 횃불"] = 0;

        weapons[1] = new Weapon(1, "평범한 횃불", 1, 1, 1, 10);
        weaponDic["평범한 횃불"] = 1;

        weapons[2] = new Weapon(2, "정교한 횃불", 1, 1, 1, 10);
        weaponDic["정교한 횃불"] = 2;

        weapons[3] = new Weapon(3, "전설의 횃불", 1, 1, 1, 10);
        weaponDic["전설의 횃불"] = 3;

        weapons[4] = new Weapon(4, "낡은 밀대", 1, 1, 1, 10);
        weaponDic["낡은 밀대"] = 4;

        weapons[5] = new Weapon(5, "평범한 밀대", 1, 1, 1, 10);
        weaponDic["평범한 밀대"] = 5;

        weapons[6] = new Weapon(6, "정교한 밀대", 1, 1, 1, 10);
        weaponDic["정교한 밀대"] = 6;

        weapons[7] = new Weapon(7, "전설의 밀대", 1, 1, 1, 10);
        weaponDic["전설의 밀대"] = 7;

        weapons[8] = new Weapon(8, "낡은 잠자리채", 1, 1, 1, 10);
        weaponDic["낡은 잠자리채"] = 8;

        weapons[9] = new Weapon(9, "평범한 잠자리채", 1, 1, 1, 10);
        weaponDic["평범한 잠자리채"] = 9;

        weapons[10] = new Weapon(10, "정교한 잠자리채", 1, 1, 1, 10);
        weaponDic["정교한 잠자리채"] = 10;

        weapons[11] = new Weapon(11, "전설의 잠자리채", 1, 1, 1, 10);
        weaponDic["전설의 잠자리채"] = 11;

        for (int i = 12; i < 32; ++i)
        {
            weapons[i] = new Weapon(i, i.ToString(), i, i, 1, 0);
            weaponDic[i.ToString()] = i;
        }
    }


    public Sprite GetSprite(int id)
    {
        if ( id >= 0 && id < weapons.Length)
        {
            return sprites[id/4];
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

            // 11.12 이윤석 - 무기 획득 퀘스트
            if (QuestManager.GetInstance().OnUserObtainWeapon != null)
            {
                QuestManager.GetInstance().OnUserObtainWeapon(count);
            }
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
                
                // 11.12 이윤석 - 무기 합성 퀘스트
                if (QuestManager.GetInstance().OnUserWeaponCombine != null)
                {
                    QuestManager.GetInstance().OnUserWeaponCombine(1);
                }

                if (weapon != null)
                {
                    weapon.AddWeapon(1);
                }
            }
            return true;
        }
        return false;
    }
}
