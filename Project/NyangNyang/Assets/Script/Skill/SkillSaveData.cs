using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class SkillSaveData
{
    public int id;
    public string name;
    public int possession;
    public int level;
    public int levelUpCost;
    public float effect;
    public bool isLock;
    public string type;
    public string subType;
}

[System.Serializable]
public class WeaponSaveData
{
    public int id;
    public string name;
    public int possession;
    public string grade;
    public string subGrade;
    public int level;
    public int levelUpCost;
    public bool isLock;
    public float effect;
    public float status;
    public float nextStatus;
    public int coin;
}

[System.Serializable]
public class SkillWeaponData
{
    public WeaponSaveData[] weapons = new WeaponSaveData[32];
    public SkillSaveData[] skills = new SkillSaveData[25];
}

public class SkillWeaponJson : MonoBehaviour
{
    private string savePath;

    private void Awake()
    {

    }

    public void SaveSkillData(SkillWeaponData gameData)
    {
        savePath = Path.Combine(Application.persistentDataPath, "SkillData.json");
        string json = JsonUtility.ToJson(gameData, true); // JSON 포맷으로 변환 (true는 보기 좋은 포맷 옵션)
        File.WriteAllText(savePath, json); // 파일에 JSON 저장
        Debug.Log("Game data saved to: " + savePath);
    }

    public SkillWeaponData LoadGameData()
    {
        savePath = Path.Combine(Application.persistentDataPath, "SkillData.json");
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath); // JSON 파일 읽기
            SkillWeaponData gameData = JsonUtility.FromJson<SkillWeaponData>(json); // JSON 문자열을 객체로 변환
            Debug.Log("Game data loaded from: " + savePath);
            return gameData;
        }
        else
        {
            Debug.LogWarning("No save file found. Returning new GameData.");
            return new SkillWeaponData(); // 파일이 없으면 새로운 데이터 반환
        }
    }
}