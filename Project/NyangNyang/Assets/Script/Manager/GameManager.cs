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

    // 다른 스크립트에서 사용
    public Cat catObject;
    public StageManager stageManager;
    public BaseStoryQuest storyQuestObject;
    public ChangeStageUI changeStageUI;

    [SerializeField] private CostumeManager costumeManager;
    [SerializeField] private SaveLoadManager saveAndLoadManager;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        saveAndLoadManager.OnAwake_CalledFromGameManager();

        // 시작 시 서버로부터 정보 받기
        Player.OnAwakeGetInitialDataFromServer();

        // CostumeManager.SetInstance 내에 해당 함수를 이렇게 작성한 이유를 메모
        CostumeManager.SetInstance(costumeManager);

        PlayerCostume.OnAwake_CallInGameManager();
    }

}
