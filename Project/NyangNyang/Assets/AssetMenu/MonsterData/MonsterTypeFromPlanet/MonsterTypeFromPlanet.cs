using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Planet_MonsterType", menuName = "ScriptableObjects/Enemy/Type/MonsterTypeFromPlanet", order = 0)]
public class MonsterTypeFromPlanet : ScriptableObject
{
    public StagePlanet planet;
    public List<EnemyMonsterType> monsterTypes;
}
