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
    private static SaveLoadManager _instance;

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


    //저장할 데이터의 경로를 저장
    private string playerStatusLevelFilePath;
    private string playerCurrencyFilePath;
    private string catCostumeFilePath;

    public static SaveLoadManager GetSaveLoadManager()
    {
        return _instance;
    }

    public static SaveLoadManager GetInstance()
    {
        return _instance;
    }

    public void OnAwake_CalledFromGameManager()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            //파일저장경로+파일이름
            playerStatusLevelFilePath = Path.Combine(Application.persistentDataPath, "StatusLevel.json");
            playerCurrencyFilePath = Path.Combine(Application.persistentDataPath, "CurrencyData.json");
            catCostumeFilePath = Path.Combine(Application.persistentDataPath, "CatCostumePart.json");

            CreateIfFileNotExist();
            Debug.Log("SaveLoadManager instatnce  초기화완료");
        }

    }

    private void Awake()
    {
        OnAwake_CalledFromGameManager();
    }


    // =================StatusLevelData=========================
    // 저장
    public void SavePlayerStatusLevel(StatusLevelData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(playerStatusLevelFilePath, json);
        Debug.Log($"저장 진행");
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
        if (File.Exists(playerStatusLevelFilePath))
        {
            string json = File.ReadAllText(playerStatusLevelFilePath);
            JsonUtility.FromJsonOverwrite(json, data);
            return true;
        }
        return false; // 파일이 없을 경우 null 반환
    }

    // =================CurrencyData=========================
    // CurrencyData 저장
    public void SavePlayerCurrencyData(CurrencyData data)
    {
        // BigInteger는 Serializable이 아니기에,
        data.BeforeSaveToJson();

        string json = JsonUtility.ToJson(data);
        Debug.Log(json);
        File.WriteAllText(playerCurrencyFilePath, json);
    }

    // CurrencyData 불러오기
    public bool LoadPlayerCurrencyData(CurrencyData overrideCurrencyData)
    {
        if (File.Exists(playerCurrencyFilePath))
        {
            string json = File.ReadAllText(playerCurrencyFilePath);
            JsonUtility.FromJsonOverwrite(json,overrideCurrencyData);
            overrideCurrencyData.AfterLoadFromJson();
            return true;
        }
        return false; // 파일이 없을 경우 null 반환
    }

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
        // PlayerStatus
        if (!File.Exists(playerStatusLevelFilePath))
        {
            SavePlayerStatusLevel(new StatusLevelData(0, 0, 0, 0));
        }
        // CurrencyData
        if (!File.Exists(playerCurrencyFilePath))
        {
            SavePlayerCurrencyData(ScriptableObject.CreateInstance<CurrencyData>());
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



