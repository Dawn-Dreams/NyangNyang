using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Events;

//제컴퓨터 기준 저기에 저장되었음
//"C:\Users\gaon7\AppData\LocalLow\DawnDreams\NyangNyang\Status.json"->윈도우기준 여기에 저장되어 있음


public class SaveLoadManager : MonoBehaviour
{
    #region instance
    public static SaveLoadManager _instance;
    public static SaveLoadManager GetSaveLoadManager()
    {
        return _instance;
    }
    public static SaveLoadManager GetInstance()
    {
        return _instance;
    }
    #endregion


    #region 지연 저장을 위한 변수/함수
    // 저장 시 딜레이를 주어 저장하기 위한 변수
    public List<SaveWithDelay> saveDataWithDelay = new List<SaveWithDelay>();
    public Coroutine saveWithDelayCoroutine = null;

    // 지연과 함께 데이터 전송을 보내는 코루틴에서 실행될 함수
    private IEnumerator SaveDataListAfterDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            for (int index = 0; index < saveDataWithDelay.Count; ++index)
            {
                SaveWithDelay saveData = saveDataWithDelay[index];
                if (saveData.startTime + saveData.delayTime <= Time.time)
                {
                    saveData.saveFunctionCallback();
                    saveDataWithDelay.RemoveAt(index);
                    --index;
                }
            }

            if (saveDataWithDelay.Count == 0)
            {
                StopCoroutine(saveWithDelayCoroutine);
                saveWithDelayCoroutine = null;
                yield return null;
            }
        }
        
    }
    // saveDataWithDelay 리스트에 추가하는 함수
    private void AddSaveDataWithDelay(SaveWithDelay newSaveData)
    {
        SaveWithDelay saveData = saveDataWithDelay.Find((a) => a.dataType == newSaveData.dataType);
        // 이미 사전에 저장 요청을 보냈을 경우,
        if (saveData != null)
        {
            saveData.startTime = Time.time;
            saveData.delayTime = newSaveData.delayTime;
            saveData.saveFunctionCallback = newSaveData.saveFunctionCallback;
        }
        // 새로운 저장 요청일 경우
        else
        {
            saveDataWithDelay.Add(newSaveData);
        }

        if (saveWithDelayCoroutine == null)
        {
            saveWithDelayCoroutine = StartCoroutine(SaveDataListAfterDelay());
        }
    }
#endregion

    //저장할 데이터의 경로를 저장
    private string _playerStatusLevelFilePath;
    private string _playerCurrencyFilePath;
    private string _playerLevelDataFilePath;
    private string _playerStageDataFilePath;
    private string _playerSnackBuffFilePath;
    private string _playerTitleDataFilePath;
    private string _playerCostumeDataFilePath;
    //
    private string _noticeFilePath;
    private string _mailFilePath;
    private string _dungeonFilePath;
    private string _rankingFilePath;
    private string _boardFilePath;

    public void OnAwake_CalledFromGameManager()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            //파일저장경로+파일이름
            string basePath = Application.persistentDataPath;
            _playerStatusLevelFilePath = Path.Combine(basePath, "StatusLevel.json");
            _playerCurrencyFilePath = Path.Combine(basePath, "CurrencyData.json");
            _playerLevelDataFilePath = Path.Combine(basePath, "LevelData.json");
            _playerStageDataFilePath = Path.Combine(basePath, "StageData.json");
            _playerSnackBuffFilePath = Path.Combine(basePath, "SnackBuff.json");
            _playerTitleDataFilePath = Path.Combine(basePath, "TitleData.json");
            _playerCostumeDataFilePath = Path.Combine(basePath, "PlayerCostume.json");
            
            _noticeFilePath = Path.Combine(basePath, "Notices.json");
            _mailFilePath = Path.Combine(basePath, "Mails.json");
            _dungeonFilePath = Path.Combine(basePath, "Dungeon.json");
            _rankingFilePath = Path.Combine(basePath, "Rankings.json");
            _boardFilePath = Path.Combine(basePath, "Boards.json");
            CreateIfFileNotExist();
        }

    }

    private void Awake()
    {
        OnAwake_CalledFromGameManager();
    }

    // 데이터 저장 함수 
    #region StatusLevelData

    // 저장
    public void SavePlayerStatusLevel(StatusLevelData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(_playerStatusLevelFilePath, json);
    }
    // 저장(딜레이)
    public void SavePlayerStatusLevel(StatusLevelData data, float delayTime)
    {
        SaveWithDelay statusSaveData = new SaveWithDelay(
            SaveDataType.StatusLevel,
            () => { SavePlayerStatusLevel(data); },
            delayTime
        );
        AddSaveDataWithDelay(statusSaveData);
    }

    //  불러오기
    public bool LoadPlayerStatusLevel(StatusLevelData data)
    {
        if (File.Exists(_playerStatusLevelFilePath))
        {
            string json = File.ReadAllText(_playerStatusLevelFilePath);
            JsonUtility.FromJsonOverwrite(json, data);
            return true;
        }
        return false; // 파일이 없을 경우 null 반환
    }
    #endregion
    
    #region CurrencyData
    // =================CurrencyData=========================
    // CurrencyData 저장
    public void SavePlayerCurrencyData(CurrencyData data)
    {
        // BigInteger는 Serializable이 아니기에,
        data.BeforeSaveToJson();

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(_playerCurrencyFilePath, json);
    }
    // 저장(딜레이)
    public void SavePlayerCurrencyData(CurrencyData data, float delayTime)
    {
        SaveWithDelay currencySaveData = new SaveWithDelay(
            SaveDataType.CurrencyData,
            () => { SavePlayerCurrencyData(data); },
            delayTime
        );
        AddSaveDataWithDelay(currencySaveData);
    }

    // CurrencyData 불러오기
    public bool LoadPlayerCurrencyData(CurrencyData overrideCurrencyData)
    {
        if (File.Exists(_playerCurrencyFilePath))
        {
            string json = File.ReadAllText(_playerCurrencyFilePath);
            JsonUtility.FromJsonOverwrite(json,overrideCurrencyData);
            overrideCurrencyData.AfterLoadFromJson();
            return true;
        }
        return false; // 파일이 없을 경우 null 반환
    }
    #endregion

    #region LevelData
    // =================LevelData=========================
    // LevelData 저장
    public void SavePlayerLevelData(UserLevelData data)
    {
        // BigInteger는 Serializable이 아니기에,
        data.BeforeSaveToJson();
        
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(_playerLevelDataFilePath, json);
    }
    // 저장(딜레이)
    public void SavePlayerLevelData(UserLevelData data, float delayTime)
    {
        SaveWithDelay levelDataSaveData = new SaveWithDelay(
            SaveDataType.LevelData,
            () => { SavePlayerLevelData(data); },
            delayTime
        );
        AddSaveDataWithDelay(levelDataSaveData);
    }

    // CurrencyData 불러오기
    public bool LoadPlayerLevelData(UserLevelData overrideUserLevelData)
    {
        if (File.Exists(_playerLevelDataFilePath))
        {
            string json = File.ReadAllText(_playerLevelDataFilePath);
            JsonUtility.FromJsonOverwrite(json, overrideUserLevelData);
            overrideUserLevelData.AfterLoadFromJson();
            return true;
        }
        return false; // 파일이 없을 경우 null 반환
    }
    #endregion

    #region StageData
    // =================StageData=========================
    // StageData 저장
    public void SavePlayerStageData(StageData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(_playerStageDataFilePath, json);
    }

    // StageData 불러오기
    public bool LoadPlayerStageData(out int highestTheme, out int highestStage)
    {
        highestTheme = 1; highestStage = 0;
        if (File.Exists(_playerStageDataFilePath))
        {
            string json = File.ReadAllText(_playerStageDataFilePath);
            StageData data = JsonUtility.FromJson<StageData>(json);
            highestTheme = data.highestTheme;
            highestStage = data.highestStage;
            return true;
        }
        return false; // 파일이 없을 경우 null 반환
    }
    #endregion

    #region SnackBuff
    // =================SnackBuff=========================
    // SnackBuff 저장
    public void SavePlayerSnackBuffData(SnackBuffJsonData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(_playerSnackBuffFilePath, json);
    }
    // 저장(딜레이)
    public void SavePlayerSnackBuffData(SnackBuffJsonData data, float delayTime)
    {
        SaveWithDelay snackBuffDataSaveData = new SaveWithDelay(
            SaveDataType.SnackBuff,
            () => { SavePlayerSnackBuffData(data); },
            delayTime
        );
        AddSaveDataWithDelay(snackBuffDataSaveData);
    }
    // SnackBuff 불러오기
    public bool LoadPlayerSnackBuffData(out SnackBuffJsonData outData)
    {
        outData = new SnackBuffJsonData();
        if (File.Exists(_playerSnackBuffFilePath))
        {
            string json = File.ReadAllText(_playerSnackBuffFilePath);
            outData = JsonUtility.FromJson<SnackBuffJsonData>(json);
            return true;
        }
        return false; // 파일이 없을 경우 null 반환
    }
    #endregion

    #region TitleData
    // =================TitleData=========================
    // TitleData 저장
    public void SavePlayerTitleData(TitleJsonData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(_playerTitleDataFilePath, json);
    }

    // TitleData 불러오기
    public bool LoadPlayerTitleData(out int currentSelectTitle, out List<int> owningTitles)
    {
        currentSelectTitle = 0;
        owningTitles = new List<int>{0};
        if (File.Exists(_playerTitleDataFilePath))
        {
            string json = File.ReadAllText(_playerTitleDataFilePath);
            TitleJsonData data = JsonUtility.FromJson<TitleJsonData>(json);
            currentSelectTitle = data.currentSelectedTitle;
            owningTitles = data.owningTitles;
            return true;
        }
        return false; // 파일이 없을 경우 null 반환
    }
    #endregion

    #region Notice
    public void SaveNotices(List<NoticeData> notices)
    {
        string json = JsonUtility.ToJson(new Wrapper<NoticeData> { items = notices });
        File.WriteAllText(_noticeFilePath, json);
    }

    public List<NoticeData> LoadNotices()
    {
        if (File.Exists(_noticeFilePath))
        {
            string json = File.ReadAllText(_noticeFilePath);
            return JsonUtility.FromJson<Wrapper<NoticeData>>(json).items;
        }
        return new List<NoticeData>();
    }
    #endregion

    #region Mail
    public void SaveMails(List<MailData> mails)
    {
        string json = JsonUtility.ToJson(new Wrapper<MailData> { items = mails });
        File.WriteAllText(_mailFilePath, json);
    }

    public List<MailData> LoadMails()
    {
        if (File.Exists(_mailFilePath))
        {
            string json = File.ReadAllText(_mailFilePath);
            return JsonUtility.FromJson<Wrapper<MailData>>(json).items;
        }
        return new List<MailData>();
    }
    #endregion

    #region Dungeon
    public void SaveDungeonLevel(List<DungeonData> dungeon)
    {
        string json = JsonUtility.ToJson(new Wrapper<DungeonData> { items = dungeon });
        File.WriteAllText(_dungeonFilePath, json);
    }

    public List<DungeonData> LoadDungeonData()
    {
        if (File.Exists(_dungeonFilePath))
        {
            string json = File.ReadAllText(_dungeonFilePath);
            return JsonUtility.FromJson<Wrapper<DungeonData>>(json).items;
        }
        return new List<DungeonData>();
    }
    #endregion

    #region Ranking
    public void SaveRankings(List<RankingData> rankings)
    {
        if (rankings == null || rankings.Count == 0)
        {
            //Debug.LogWarning("No ranking data to save.");
            return;
        }

        string json = JsonUtility.ToJson(new Wrapper<RankingData> { items = rankings }, true); // Pretty print
        Debug.Log("Saving rankings to: " + _rankingFilePath);
        Debug.Log("JSON Data: " + json);

        File.WriteAllText(_rankingFilePath, json);
        Debug.Log("File saved successfully.");
    }

    public List<RankingData> LoadRankings()
    {
        if (File.Exists(_rankingFilePath))
        {
            string json = File.ReadAllText(_rankingFilePath);
            return JsonUtility.FromJson<Wrapper<RankingData>>(json).items;
        }
        return new List<RankingData>();
    }
    #endregion

    #region Board
    public void SaveBoards(List<BoardData> boards)
    {
        string json = JsonUtility.ToJson(new Wrapper<BoardData> { items = boards });
        File.WriteAllText(_boardFilePath, json);
    }

    public List<BoardData> LoadBoards()
    {
        if (File.Exists(_boardFilePath))
        {
            string json = File.ReadAllText(_boardFilePath);
            return JsonUtility.FromJson<Wrapper<BoardData>>(json).items;
        }
        return new List<BoardData>();
    }
    #endregion

    // Generic Wrapper for lists (JsonUtility doesn't support direct List<T> serialization)
    [Serializable]
    private class Wrapper<T>
    {
        public List<T> items;
    }

    #region PlayerCostume
    // =================PlayerCostume=========================
    // PlayerCostume 저장
    public void SavePlayerCostumeData(PlayerCostumeJsonData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(_playerCostumeDataFilePath, json);
    }

    // TitleData 불러오기
    public bool LoadPlayerPlayerCostume(out PlayerCostumeJsonData data)
    {
        data = new PlayerCostumeJsonData();
        if (File.Exists(_playerCostumeDataFilePath))
        {
            string json = File.ReadAllText(_playerCostumeDataFilePath);
            data = JsonUtility.FromJson<PlayerCostumeJsonData>(json);
            return true;
        }
        return false; // 파일이 없을 경우 null 반환
    }
    #endregion
    // =================CatCostumePart=========================
    //// 저장
    /// public void Save어쩌구(저장할클래스이름 data)
    /// {
    /// string json = JsonUtility.ToJson(data);
    /// File.WriteAllText(저장할데이터의위에서정해놓은경로, json);
    /// }
    /// 

    ////불러오기
    //public 반환클래스이름 LoadCatCostumePart()
    //{
    //    if (File.Exists(저장할데이터의위에서정해놓은경로))
    //    {
    //        string json = File.ReadAllText(저장할데이터의위에서정해놓은경로);
    //        return JsonUtility.FromJson<반환클래스이름>(json);
    //    }
    //    return null; // 파일이 없을 경우 null 반환
    //}


    private void CreateIfFileNotExist()
    {
        // 필요한 상위 디렉터리가 없으면 생성
        string baseDirectory = Path.GetDirectoryName(_playerStatusLevelFilePath);
        if (!Directory.Exists(baseDirectory))
        {
            Directory.CreateDirectory(baseDirectory);
            Debug.Log($"Created base directory: {baseDirectory}");
        }

        // PlayerStatus
        if (!File.Exists(_playerStatusLevelFilePath))
        {
            SavePlayerStatusLevel(new StatusLevelData(0, 0, 0, 0));
        }
        // CurrencyData
        if (!File.Exists(_playerCurrencyFilePath))
        {
            SavePlayerCurrencyData(ScriptableObject.CreateInstance<CurrencyData>());
        }
        // LevelData
        if (!File.Exists(_playerLevelDataFilePath))
        {
            SavePlayerLevelData(ScriptableObject.CreateInstance<UserLevelData>());
        }
        // StageData
        if (!File.Exists(_playerStageDataFilePath))
        {
            SavePlayerStageData(new StageData { highestTheme = 1, highestStage = 0 });
        }
        // SnackBuff
        if (!File.Exists(_playerSnackBuffFilePath))
        {
            SavePlayerSnackBuffData(new SnackBuffJsonData());
        }
        // TitleData
        if (!File.Exists(_playerTitleDataFilePath))
        {
            SavePlayerTitleData(new TitleJsonData { currentSelectedTitle = 0, owningTitles = new List<int> { 0 } });
        }
        // Costume
        if (!File.Exists(_playerCostumeDataFilePath))
        {
            PlayerCostumeJsonData data = new PlayerCostumeJsonData();
            data.currentEquipCostume = new List<int>();
            for (int i = 0; i < (int)CatCostumePart.Count; ++i)
            {
                data.currentEquipCostume.Add(0);
            }
            // 하드코딩
            data.headOwningCostume = new List<int>() { 0 };
            data.bodyOwningCostume = new List<int>() { 0 };
            data.handROwningCostume = new List<int>() { 0 };
            data.furSkinOwningCostume = new List<int>() { 0 };
            data.petOwningCostume = new List<int>() { 0 };
            data.emotionOwningCostume = new List<int>() { 0 };
            SavePlayerCostumeData(data);
        }

        // Notices
        if (!File.Exists(_noticeFilePath))
        {
            SaveNotices(new List<NoticeData>());
        }
        if (!File.Exists(_mailFilePath))
        {
            SaveMails(new List<MailData>());
        }
        if (!File.Exists(_dungeonFilePath))
        {
            SaveDungeonLevel(new List<DungeonData>());
        }
        if (!File.Exists(_rankingFilePath))
        {
            SaveRankings(new List<RankingData>());
        }
        if (!File.Exists(_boardFilePath))
        {
            SaveBoards(new List<BoardData>());
        }
    }

}




/*
 * 사용예시
    
private void Start() -> GameManager의 start 함수임
    {
        //Awake에서 SaveLoadManager를 사용하면 null문제가 자꾸 발생해서 start에 넣어놓음
        PlayerStatusData loadedStatus = SaveLoadManager.GetSaveLoadManager().LoadPlayerStatus();
        Debug.Log($"Loaded Player Status: HP = {loadedStatus.hp}, MP = {loadedStatus.mp}");

    }

 데이터 저장
        saveLoadManager.SavePlayerStatus(playerStatus);
        saveLoadManager.SavePlayerStatusLevel(playerStatusLevel);
        saveLoadManager.SavePlayerGoods(playerGoods);

데이터 불러오기
        PlayerStatusData loadedStatus = saveLoadManager.LoadPlayerStatus();
        PlayerStatusLevelData loadedStatusLevel = saveLoadManager.LoadPlayerStatusLevel();
        PlayerGoodsData loadedGoods = saveLoadManager.LoadPlayerGoods();

불러온 데이터 출력
        Debug.Log($"Loaded Player Status: HP = {loadedStatus.hp}, MP = {loadedStatus.mp}");
        Debug.Log($"Loaded Player Status Level: HP Level = {loadedStatusLevel.hpLevel}");
        Debug.Log($"Loaded Player Goods: Gold = {loadedGoods.gold}");
 */

[Serializable]
public struct StageData
{
    public int highestTheme;
    public int highestStage;
    
}

[Serializable]
public struct SnackBuffRemainTimeJsonData
{
    public SnackType type;
    public long time;

    public SnackBuffRemainTimeJsonData(SnackType type, DateTime time)
    {
        this.type = type;
        this.time = time.ToFileTime();
    }

}

[Serializable]
public struct SnackBuffJsonData
{
    public int snackBuffAdViewCount;
    public List<SnackBuffRemainTimeJsonData> buffRemainTime;
}

[Serializable]
public struct TitleJsonData
{
    public int currentSelectedTitle;
    public List<int> owningTitles;
}

[Serializable]
public struct PlayerCostumeJsonData
{
    public List<int> currentEquipCostume;
    // 하드코딩
    public List<int> headOwningCostume;
    public List<int> bodyOwningCostume;
    public List<int> handROwningCostume;
    public List<int> furSkinOwningCostume;
    public List<int> petOwningCostume;
    public List<int> emotionOwningCostume;
}