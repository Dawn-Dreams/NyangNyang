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
    public BigInteger currentExp = 0;

    private static int addExpPerLevel = 500;
    [SerializeField] private static GameObject _levelUpIconObject;

    public static BigInteger CalculateExp(int userCurrentLevel)
    {
        addExpPerLevel = DummyServerData.GetAddExpPerLevelValue();

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

        // TODO: 후에 서버에서 확인하는 코드 생성해야함
        DummyServerData.UserLevelUp(Player.GetUserID(), levelUpCount, currentUserExp - currentExp);

        // TODO: 임시 레벨업 아이콘 코드
        if (levelUpCount > 0)
        {
            GameObject.Find("Manager").GetComponent<Player>().ShowLevelUpIcon();
            
        }
    }

    public UserLevelData SetUserLevelData(int getCurrentLevel, BigInteger getExp = default(BigInteger))
    {
        currentLevel = getCurrentLevel;
        currentExp = getExp;
        return this;
    }

    public static UserLevelData GetNewDataFromSource(UserLevelData data)
    {
        return ScriptableObject.CreateInstance<UserLevelData>().SetUserLevelData(data.currentLevel, data.currentExp);
    }

}
