using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class QuestSaveLoadManager : MonoBehaviour
{
    #region instance
    private static QuestSaveLoadManager _instance;
    public static QuestSaveLoadManager GetInstance()
    {
        return _instance;
    }
    #endregion
    public void OnAwake_CalledFromGameManager()
    {
        if (_instance == null)
        {
            _instance = this;

            //파일저장경로+파일이름
            //_playerStatusLevelFilePath = Path.Combine(Application.persistentDataPath, "StatusLevel.json");

            //CreateIfFileNotExist();
        }

    }

    private void Awake()
    {
        OnAwake_CalledFromGameManager();
    }
}
