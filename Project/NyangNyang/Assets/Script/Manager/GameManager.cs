using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;
    public static GameManager GetInstance() { return Instance; }


    public Cat catObject;
    public StageManager stageManager;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    
}
