using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour
{
    private static PetManager _instance;

    public static PetManager GetInstance()
    {
        return _instance;
    }

    private Dictionary<EnemyMonsterType, AddressableHandle<GameObject>> _petPrefabs;

    public Transform testSocket;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        AssetLoad();

        Instantiate(_petPrefabs[EnemyMonsterType.StarFish].obj, testSocket);
    }

    protected void AssetLoad()
    {
        if (_petPrefabs == null)
        {
            _petPrefabs = new Dictionary<EnemyMonsterType, AddressableHandle<GameObject>>();
            for (int i = 0; i < (int)EnemyMonsterType.Count; ++i)
            {
                EnemyMonsterType type = (EnemyMonsterType)i;
                AddressableHandle <GameObject> petPrefab = new AddressableHandle<GameObject>();
                petPrefab.Load("Enemy/" + type);
                _petPrefabs.Add(type, petPrefab);
            }
        }
    }
}
