using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayInfoManager : MonoBehaviour
{

    public NyangNyangPower nyang;

    private static PlayInfoManager instance;

    public static PlayInfoManager GetInstance() { return instance; }

    private PlayInfo info;

    private int[] LevelList = new int[12] { 0, 150, 300, 550, 1000, 2000, 4000, 10000, 20000, 40000, 40000, 40000 };

    private void Awake()
    {
        if ( instance == null)
        {
            instance = this;
        }

        // InitializedPlayInfo();
    }

    public void InitializedPlayInfo()
    {

        // ResetData();


        info = SaveDataManager.GetInstance().LoadPlayInfo();

        StartCoroutine(SaveData());

        nyang.InitializedNyangNyang();
    }

    IEnumerator SaveData()
    {

        yield return null;

        while (true)
        {
            SaveDataManager.GetInstance().SavePlayInfo(info);

            yield return new WaitForSeconds(1f);

        }
    }

    public PlayInfo GetInfo() { return info; }

    public void SetWeaponGachaCount(int _count) { 
        info.weaponGachaCount += _count;
        CheckWeaponGachaCount();
    }

    public void SetSkillGachaCount(int _count) { 
        info.skillGachaCount += _count;
        CheckSkillGachaCount();
    }

    public void SetNyangNyangCount()
    {
        info.nyangnyangCount++;
        CheckNyangNyangCount();
    }

    public void SetCurrentWeapon(int _id)
    {
        info.currentWeaponID = _id;
    }

    public void SetCurrentSkill(int _id)
    {
        info.currentSkillID = _id;
    }

    public void CheckWeaponGachaCount()
    {
        if ( info.weaponGachaLevel < 12 && info.weaponGachaCount >= LevelList[info.weaponGachaLevel])
        {
            info.weaponGachaLevel += 1;
        }
    }

    public void CheckSkillGachaCount()
    {
        if (info.skillGachaLevel < 12 && info.skillGachaCount >= LevelList[info.skillGachaLevel])
        {
            info.skillGachaLevel += 1;
        }
    }

    public void CheckNyangNyangCount()
    {
        if (info.nyangnyangLevel < 12 && info.nyangnyangCount >= LevelList[info.nyangnyangLevel])
        {
            info.nyangnyangLevel += 1;
        }
    }

    public void ResetData()
    {
        info = new PlayInfo();

        info.weaponGachaCount = 0;
        info.weaponGachaLevel = 1;

        info.skillGachaCount = 0;
        info.skillGachaLevel = 1;

        info.nyangnyangCount = 0;
        info.nyangnyangLevel = 1;

        info.currentWeaponID = 0;
        info.currentSkillID = -1;

        SaveDataManager.GetInstance().SavePlayInfo(info);
    }
}
