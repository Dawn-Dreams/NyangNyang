using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{

    private static SaveDataManager instance;

    private string weaponFilePath;
    private string skillFilePath;
    private string playFilePath;

    private void Awake()
    {
        if (instance == null)
        { 
            instance = this; 
        }

        weaponFilePath = Path.Combine(Application.persistentDataPath, "weapons.json");
        skillFilePath = Path.Combine(Application.persistentDataPath, "skills.json");
        playFilePath = Path.Combine(Application.persistentDataPath, "play.json");
    }

    private void Start()
    {
        WeaponManager.GetInstance().InitializedWeapons();
        SkillManager.GetInstance().InitializedSkills();
        PlayInfoManager.GetInstance().InitializedPlayInfo();
    }

    public static SaveDataManager GetInstance() { return instance; }

    public void SaveWeapons(WeaponInfo[] weapons)
    {
        WeaponData weaponData = new WeaponData { weapons = weapons };
        string json = JsonUtility.ToJson(weaponData, true);
        File.WriteAllText(weaponFilePath, json);
        Debug.Log($"Weapons saved to {weaponFilePath}");
    }

    public WeaponInfo[] LoadWeapons()
    {
        if (!File.Exists(weaponFilePath))
        {
            Debug.LogWarning("Weapon file not found!");
            return null;
        }

        string json = File.ReadAllText(weaponFilePath);
        WeaponData weaponData = JsonUtility.FromJson<WeaponData>(json);
        return weaponData.weapons;
    }

    public void SaveSkills(SkillInfo[] skills)
    {
        SkillData skillData = new SkillData { skills = skills };
        string json = JsonUtility.ToJson(skillData, true);
        File.WriteAllText(skillFilePath, json);
        Debug.Log($"Skills saved to {skillFilePath}");
    }

    public SkillInfo[] LoadSkills()
    {
        if (!File.Exists(skillFilePath))
        {
            Debug.LogWarning("Skill file not found!");
            return null;
        }

        string json = File.ReadAllText(skillFilePath);
        SkillData skillData = JsonUtility.FromJson<SkillData>(json);
        return skillData.skills;
    }

    public void SavePlayInfo(PlayInfo _info)
    {
        PlayData playData = new PlayData { info = _info };
        string json = JsonUtility.ToJson(playData, true);
        File.WriteAllText(playFilePath, json);
        // Debug.Log($"PlayInfo saved to {playFilePath}");
    }

    public PlayInfo LoadPlayInfo()
    {
        if (!File.Exists(playFilePath))
        {
            Debug.LogWarning("PlayInfo file not found!");
            return null;
        }

        string json = File.ReadAllText(playFilePath);
        PlayData playData = JsonUtility.FromJson<PlayData>(json);
        return playData.info;
    }
}
