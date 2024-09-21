using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "UserLevelData", menuName = "ScriptableObjects/UserLevelData", order = 1)]
public class UserLevelData : ScriptableObject
{
    public BigInteger currentLevel;
    public BigInteger currentExp;

    public UserLevelData SetUserLevelData(BigInteger getCurrentLevel,  BigInteger getExp = default(BigInteger))
    {
        currentLevel = getCurrentLevel;
        currentExp = getExp;
        return this;
    }
}
