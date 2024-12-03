using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "UserLevelData", menuName = "ScriptableObjects/UserLevelData", order = 1)]
public class UserLevelData : ScriptableObject
{
    public int currentLevel = 1;

    public string currentExpStringForJson;
    public BigInteger currentExp = 0;

    private static int addExpPerLevel = 500;
    [SerializeField] private static GameObject _levelUpIconObject;


    // 경험치 변화 델리게이트 이벤트
    public delegate void OnExpChangeDelegate(UserLevelData newLevelData);
    public event OnExpChangeDelegate OnExpChange;

    public static BigInteger CalculateExp(int userCurrentLevel)
    { 
        return userCurrentLevel * addExpPerLevel;
    }

    public void AddExp(BigInteger addExp)
    {
        BigInteger currentUserExp = currentExp + addExp;
        int levelUpCount = 0;
        while (true)
        {
            BigInteger currentRequireExp = CalculateExp(currentLevel);

            if (currentUserExp < currentRequireExp) break;

            levelUpCount += 1;
            currentUserExp -= currentRequireExp;
        }
        
        SetUserLevelData(currentLevel + levelUpCount, currentUserExp);

        if (levelUpCount > 0)
        {
            PlayerLevelUpUI.GetInstance().UserLevelUp();
        }

        ExecuteExpUpDelegate();

        SaveLoadManager.GetInstance().SavePlayerLevelData(this);
    }

    public void ExecuteExpUpDelegate()
    {
        if (OnExpChange != null)
        {
            OnExpChange(this);
        }
    }

    public UserLevelData SetUserLevelData(int getCurrentLevel, BigInteger getExp = default(BigInteger))
    {
        currentLevel = getCurrentLevel;
        
        currentExp = BigInteger.Max(getExp, 0); ;

        if (OnExpChange != null)
        {
            OnExpChange(this);
        }

        return this;
    }

    public static UserLevelData GetNewDataFromSource(UserLevelData data)
    {
        return ScriptableObject.CreateInstance<UserLevelData>().SetUserLevelData(data.currentLevel, data.currentExp);
    }

    public void BeforeSaveToJson()
    {
        currentExpStringForJson = currentExp.ToString();
    }

    public void AfterLoadFromJson()
    {
        currentExp = BigInteger.Parse(currentExpStringForJson);
    }
}
