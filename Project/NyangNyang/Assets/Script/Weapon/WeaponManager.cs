using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    private static WeaponManager Instance;
    public static WeaponManager GetInstance() { return Instance; }

    private Weapon[] weapons = new Weapon[32];
    private WeaponInfo[] weaponDatas = new WeaponInfo[32];
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
        // 이후는 일부러 연결 안 해둠 어차피 서버 연결 시 사라질 부분
        
        /*
        weaponDatas[0] = new WeaponInfo { ID = 0, name = "낡은 횃불", count = 1, grade = "Normal", subGrade = "A", isLock = false, effect = 0f, level = 1, coin = 0, ment = "\"겨우 불씨를 붙인 나무 조각. 바람이 불면 금방 꺼질 것 같다.\"" };
        weaponDatas[1] = new WeaponInfo { ID = 1, name = "평범한 횃불", count = 0, grade = "Normal", subGrade = "B", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"불꽃이 안정적으로 타오른다. 어둠을 밝혀주는 데 충분하다.\"" };
        weaponDatas[2] = new WeaponInfo { ID = 2, name = "정교한 횃불", count = 0, grade = "Normal", subGrade = "C", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"정교하게 만든 횃불. 오래 사용해도 불꽃이 꺼지지 않는다.\"" };
        weaponDatas[3] = new WeaponInfo { ID = 3, name = "전설의 횃불", count = 0, grade = "Normal", subGrade = "D", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"황금빛 불꽃이 강렬하게 타오른다. 주변을 환하게 비춘다.\"" };
        weaponDatas[4] = new WeaponInfo { ID = 4, name = "낡은 밀대", count = 0, grade = "Magic", subGrade = "A", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"낡고 흔들리는 밀대. 조금만 힘을 줘도 부러질 것 같다.\"" };
        weaponDatas[5] = new WeaponInfo { ID = 5, name = "평범한 밀대", count = 0, grade = "Magic", subGrade = "B", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"튼튼한 기본 밀대. 가벼운 작업에는 제법 유용하다.\"" };
        weaponDatas[6] = new WeaponInfo { ID = 6, name = "정교한 밀대", count = 0, grade = "Magic", subGrade = "C", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"강화된 밀대. 무엇이든 쉽게 밀어낼 수 있다.\"" };
        weaponDatas[7] = new WeaponInfo { ID = 7, name = "전설의 밀대", count = 0, grade = "Magic", subGrade = "D", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"빛나는 고급 밀대. 단 한 번 휘두르기만 해도 강력하다.\"" };
        weaponDatas[8] = new WeaponInfo { ID = 8, name = "낡은 잠자리채", count = 0, grade = "Rare", subGrade = "A", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"낡고 너덜너덜한 그물망. 잡으려던 물건이 자꾸 빠져나간다.\"" };
        weaponDatas[9] = new WeaponInfo { ID = 9, name = "평범한 잠자리채", count = 0, grade = "Rare", subGrade = "B", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"실용적인 잠자리채. 빠른 움직임도 어느 정도 따라잡을 수 있다.\"" };
        weaponDatas[10] = new WeaponInfo { ID = 10, name = "정교한 잠자리채", count = 0, grade = "Rare", subGrade = "C", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"튼튼하고 유연한 그물망. 놓치는 일이 거의 없다.\"" };
        weaponDatas[11] = new WeaponInfo { ID = 11, name = "전설의 잠자리채", count = 0, grade = "Rare", subGrade = "D", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"황금으로 강화된 채. 한 번 휘두르면 아무도 빠져나갈 수 없다.\"" };
        weaponDatas[12] = new WeaponInfo { ID = 12, name = "낡은 공구", count = 0, grade = "Unique", subGrade = "A", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"녹슨 공구 세트. 기본적인 기능은 하지만 조심히 써야 한다.\"" };
        weaponDatas[13] = new WeaponInfo { ID = 13, name = "평범한 공구", count = 0, grade = "Unique", subGrade = "B", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"일반적인 공구 세트. 간단한 작업에 적합하다.\"" };
        weaponDatas[14] = new WeaponInfo { ID = 14, name = "정교한 공구", count = 0, grade = "Unique", subGrade = "C", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"정밀한 작업이 가능한 고급 공구. 믿음직스럽다.\"" };
        weaponDatas[15] = new WeaponInfo { ID = 15, name = "전설의 공구", count = 0, grade = "Unique", subGrade = "D", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"빛나는 마법 공구. 무엇이든 척척 해결할 수 있다.\"" };
        weaponDatas[16] = new WeaponInfo { ID = 16, name = "낡은 삽", count = 0, grade = "Epic", subGrade = "A", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"녹슨 삽. 무언가를 파기엔 조금 부족해 보인다.\"" };
        weaponDatas[17] = new WeaponInfo { ID = 17, name = "평범한 삽", count = 0, grade = "Epic", subGrade = "B", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"견고한 삽. 땅을 파는 데 전혀 무리가 없다.\"" };
        weaponDatas[18] = new WeaponInfo { ID = 18, name = "정교한 삽", count = 0, grade = "Epic", subGrade = "C", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"강화된 금속으로 만든 삽. 단단한 땅도 손쉽게 판다.\"" };
        weaponDatas[19] = new WeaponInfo { ID = 19, name = "전설의 삽", count = 0, grade = "Epic", subGrade = "D", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"빛나는 삽. 무언가를 파헤치는 데 최적화되어 있다.\"" };
        weaponDatas[20] = new WeaponInfo { ID = 20, name = "낡은 갈고리", count = 0, grade = "Legend", subGrade = "A", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"날이 무뎌진 낡은 낫. 무언가를 베는 데 시간이 걸린다.\"" };
        weaponDatas[21] = new WeaponInfo { ID = 21, name = "평범한 갈고리", count = 0, grade = "Legend", subGrade = "B", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"일반적인 낫. 깔끔하게 무언가를 벨 수 있다.\"" };
        weaponDatas[22] = new WeaponInfo { ID = 22, name = "정교한 갈고리", count = 0, grade = "Legend", subGrade = "C", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"날카로운 날과 세련된 디자인의 낫. 효율적으로 사용 가능하다.\"" };
        weaponDatas[23] = new WeaponInfo { ID = 23, name = "전설의 갈고리", count = 0, grade = "Legend", subGrade = "D", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"황금빛으로 빛나는 낫. 한 번 휘두르면 모든 것을 깔끔히 베어낸다.\"" };
        weaponDatas[24] = new WeaponInfo { ID = 24, name = "낡은 도끼", count = 0, grade = "Star", subGrade = "A", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"날이 무뎌진 도끼. 나무를 베는 데 시간이 걸린다.\"" };
        weaponDatas[25] = new WeaponInfo { ID = 25, name = "평범한 도끼", count = 0, grade = "Star", subGrade = "B", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"기본적인 도끼. 일반적인 작업에는 무난하다.\"" };
        weaponDatas[26] = new WeaponInfo { ID = 26, name = "정교한 도끼", count = 0, grade = "Star", subGrade = "C", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"날이 날카롭고 균형 잡힌 도끼. 작업 속도가 눈에 띄게 빠르다.\"" };
        weaponDatas[27] = new WeaponInfo { ID = 27, name = "전설의 도끼", count = 0, grade = "Star", subGrade = "D", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"강력한 도끼. 무엇이든 단숨에 파괴할 수 있다.\"" };
        weaponDatas[28] = new WeaponInfo { ID = 28, name = "낡은 톱", count = 0, grade = "Galaxy", subGrade = "A", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"녹슨 톱날. 무언가를 자르는 데 상당한 노력이 필요하다.\"" };
        weaponDatas[29] = new WeaponInfo { ID = 29, name = "평범한 톱", count = 0, grade = "Galaxy", subGrade = "B", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"일반적인 톱. 나무를 자르는 데 적합하다.\"" };
        weaponDatas[30] = new WeaponInfo { ID = 30, name = "정교한 톱", count = 0, grade = "Galaxy", subGrade = "C", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"예리한 톱날이 달린 고급 톱. 작업이 빠르고 정교하다.\"" };
        weaponDatas[31] = new WeaponInfo { ID = 31, name = "전설의 톱", count = 0, grade = "Galaxy", subGrade = "D", isLock = true, effect = 0f, level = 1, coin = 0, ment = "\"황금빛으로 빛나는 톱. 단 한 번에 모든 것을 자를 수 있다.\"" };
        
         
        SaveDataManager.GetInstance().SaveWeapons(weaponDatas); 

        */
        weaponDatas = SaveDataManager.GetInstance().LoadWeapons();

        for ( int i  = 0; i < 32; ++i)
        {
            MatchWeaponFromWeaponData(i);
            weaponDic[weapons[i].GetName()] = i;
        }     
    }
    
    public void MatchWeaponDataFromWeapon(int id)
    {
        weaponDatas[id].count = weapons[id].GetCount();
        weaponDatas[id].isLock = weapons[id].GetIsLock();
        weaponDatas[id].effect = weapons[id].GetEffect();
        weaponDatas[id].level = weapons[id].GetLevel();
        weaponDatas[id].coin = weapons[id].GetCoin();

        SaveDataManager.GetInstance().SaveWeapons(weaponDatas);
    }

    public void MatchWeaponFromWeaponData(int id)
    {
        weapons[id] = new Weapon(weaponDatas[id].ID, weaponDatas[id].name, weaponDatas[id].count, weaponDatas[id].grade, weaponDatas[id].subGrade, weaponDatas[id].isLock, weaponDatas[id].effect, weaponDatas[id].level, weaponDatas[id].coin, weaponDatas[id].ment);
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
        if ( id >= 0 && id < weapons.Length )
        {
            return weapons[id];
        }
        return null;
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
        if (weapon != null && weapon.HasWeapon() && weapon.GetLevel() < 100)
        {
            int c = weapon.SetLevel();
            MatchWeaponDataFromWeapon(id);
            return c;
        }
        return 1000;
    }

    //public void EnhanceEffectWeapon(int id)
    //{
    //    Weapon weapon = GetWeapon(id);
    //    if ( weapon != null && weapon.HasWeapon() && weapon.GetLevel() < 100)
    //    {
    //        weapon.StatusUpgrade();
    //    }
    //}

    public void AddWeaponCount(int id, int count)
    {
        Weapon weapon = GetWeapon(id);
        if (weapon != null)
        {
            weapons[id].AddWeapon(count);

            // 11.12 이윤석 - 무기 획득 퀘스트
            if (QuestManager.GetInstance().OnUserObtainWeapon != null)
            {
                QuestManager.GetInstance().OnUserObtainWeapon(count);
            }
        }
    }

    public bool CombineWeapon(int id)
    {
        if (id >= 0 && id < weapons.Length - 1)
        {
            Weapon weapon = GetWeapon(id);
            if (weapon != null)
            {
                weapon.AddWeapon(-5);

                MatchWeaponDataFromWeapon(id);

                weapon = GetWeapon(id + 1);

                // 11.12 이윤석 - 무기 합성 퀘스트
                if (QuestManager.GetInstance().OnUserWeaponCombine != null)
                {
                    QuestManager.GetInstance().OnUserWeaponCombine(1);
                }

                if (weapon != null)
                {
                    weapon.SetIsLockToFALSE();
                    weapon.AddWeapon(1);

                    MatchWeaponDataFromWeapon(id + 1);
                }
            }
            return true;
        }
        return false;
    }
}
