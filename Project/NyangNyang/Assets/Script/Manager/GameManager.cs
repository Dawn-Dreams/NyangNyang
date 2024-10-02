using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;
    public static GameManager GetInstance() { return Instance; }
    public static bool isMiniGameActive = false;
    public static bool isSpecialStageActive = false;

    public Cat catObject;
    public StageManager stageManager;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    

    
}
