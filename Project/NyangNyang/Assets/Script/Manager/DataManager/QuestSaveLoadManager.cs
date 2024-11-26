using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
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

    #region NotStoryQuest

    // 데이터 로드
    public void LoadQuestProgressData(QuestCategory category, QuestType type, out QuestJsonData data)
    {
        string path = GetPath(category, type);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<QuestJsonData>(json);
        }
        // 새로 접속, 퀘스트 데이터 초기화
        else
        {
            //SaveQuestProgressData(category, type, "0");
            CreateFile(category, type);
            data = new QuestJsonData() { getReward = false, getRewardTimeString = "", progressString = "0" };
        }
    }

    public void CreateFile(QuestCategory questCategory, QuestType questType)
    {
        string path = GetPath(questCategory, questType);
        QuestJsonData data = new QuestJsonData();
        data.getReward = false;
        data.getRewardTimeString = "";
        data.progressString = "0";
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public void SaveQuestProgressData(QuestCategory questCategory, QuestType type, string progressDataString)
    {
        string path = GetPath(questCategory, type);

        QuestJsonData data = new QuestJsonData();
        LoadQuestProgressData(questCategory, type, out data);
        data.progressString = progressDataString;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public void InitializeAndSaveQuestProgressData(QuestCategory questCategory, QuestType questType)
    {
        string path = GetPath(questCategory, questType);
        QuestJsonData data = new QuestJsonData() { getReward = false, getRewardTimeString = "", progressString = "0" };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public void UserGetRewardOnNotRepeatable(QuestCategory questCategory, QuestType type)
    {
        string path = GetPath(questCategory, type);

        QuestJsonData data = new QuestJsonData();
        LoadQuestProgressData(questCategory, type, out data);
        data.getReward = true;
        data.getRewardTimeString = DateTime.Now.ToString("yyyy/MM/dd tt hh:mm:ss");

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public string GetPath(QuestCategory questCategory, QuestType questType)
    {
        string path = Path.Combine(Application.persistentDataPath, $"Q_{questCategory.ToString()[0]}_{questType}.json");
        return path;
    }

    #endregion

    #region StoryQuest
    // StoryQuest 저장
    public void SaveStoryQuestData(StoryQuestJsonData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(GetStoryQuestPath(), json);
    }

    // StoryQuest 불러오기
    public void LoadStoryQuestData(out StoryQuestJsonData data)
    {
        data = new StoryQuestJsonData();
        if (File.Exists(GetStoryQuestPath()))
        {
            string json = File.ReadAllText(GetStoryQuestPath());
            data = JsonUtility.FromJson<StoryQuestJsonData>(json);
        }
        // 새로 시작하여 파일이 없을 경우,
        else
        {
            data.currentStoryQuestID = 0;
            SaveStoryQuestData(data);
        }
    }

    public string GetStoryQuestPath()
    {
        return Path.Combine(Application.persistentDataPath, "StoryQuestData.json");
    }

    #endregion

}

[Serializable]
public struct QuestJsonData
{
    public bool getReward;
    public string getRewardTimeString;
    public string progressString;
}

[Serializable]
public struct StoryQuestJsonData
{
    public int currentStoryQuestID;
}