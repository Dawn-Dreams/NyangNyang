using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;
    public static GameManager GetInstance() { return Instance; }
    public static bool isMiniGameActive = false;
    public static bool isDungeonActive = false;

    public Cat catObject;
    public StageManager stageManager;

    public GameObject questUI;
    public BaseStoryQuest storyQuestObject;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;


        DummyQuestServer.ExecuteDummyQuestServer();
    }
}
