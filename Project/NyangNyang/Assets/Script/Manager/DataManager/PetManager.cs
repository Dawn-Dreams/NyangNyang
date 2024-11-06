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

    public Pet playerPet;
    
    

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        GetPlayerPetDataFromServer();
    }

    void GetPlayerPetDataFromServer()
    {
        // TODO: 서버에 추가하기
        EnemyMonsterType currentEnemyType = EnemyMonsterType.StarFish;
        playerPet.SetPetType(currentEnemyType);
    }

}
