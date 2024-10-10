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

    public GameObject questUI;
    public BaseStoryQuest storyQuestObject;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        StartCoroutine(SetInActiveQuestUIAtStart());

        DummyQuestServer.ExecuteDummyQuestServer();
    }

    IEnumerator SetInActiveQuestUIAtStart()
    {
        yield return null;

        RectTransform questUITransform = questUI.GetComponent<RectTransform>();
        questUITransform.offsetMin = new Vector2(0, 0);
        questUITransform.offsetMax = new Vector2(0, 0);
        questUI.SetActive(false);
    }




}
