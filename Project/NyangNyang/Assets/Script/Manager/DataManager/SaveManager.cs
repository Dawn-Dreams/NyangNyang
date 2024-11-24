using System.IO;
using UnityEngine;

//제컴퓨터 기준 저기에 저장되었음
//"C:\Users\gaon7\AppData\LocalLow\DawnDreams\NyangNyang\Status.json"->윈도우기준 여기에 저장되어 있음

public class SaveLoadManager : MonoBehaviour
{
    private static SaveLoadManager instance;

    //저장할 데이터의 경로를 저장
    private string playerStatusFilePath;
    private string playerStatusLevelFilePath;
    private string playerCurrencyFilePath;
    private string catCostumeFilePath;


    public static SaveLoadManager GetSaveLoadManager()
    {
        return instance;
    }
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            //파일저장경로+파일이름
            playerStatusFilePath = Path.Combine(Application.persistentDataPath, "Status.json");
            playerStatusLevelFilePath = Path.Combine(Application.persistentDataPath, "StatusLevel.json");
            playerCurrencyFilePath = Path.Combine(Application.persistentDataPath, "CurrencyData.json");
            catCostumeFilePath = Path.Combine(Application.persistentDataPath, "CatCostumePart.json");

        }
        Debug.Log("SaveLoadManager instatnce  초기화완료");

    }

  
    // =================Status=========================
    // 저장
    public void SavePlayerStatus(Status data)
    {
        Debug.Log($"Save Player Status: HP = {data.hp}, MP = {data.mp}");

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(playerStatusFilePath, json);
    }

    //불러오기
    public PlayerStatusData LoadPlayerStatus()
    {
        if (File.Exists(playerStatusFilePath))
        {
            string json = File.ReadAllText(playerStatusFilePath);
            return JsonUtility.FromJson<PlayerStatusData>(json);
        }
        return null; // 파일이 없을 경우 null 반환
    }

    // =================PlayerStatusLevelData=========================
    // 저장
    public void SavePlayerStatusLevel(PlayerStatusLevelData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(playerStatusLevelFilePath, json);
    }

    //  불러오기
    public PlayerStatusLevelData LoadPlayerStatusLevel()
    {
        if (File.Exists(playerStatusLevelFilePath))
        {
            string json = File.ReadAllText(playerStatusLevelFilePath);
            return JsonUtility.FromJson<PlayerStatusLevelData>(json);
        }
        return null; // 파일이 없을 경우 null 반환
    }

    // =================CurrencyData=========================
    // CurrencyData 저장
    public void SavePlayerCurrencyData(CurrencyData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(playerCurrencyFilePath, json);
    }

    // CurrencyData 불러오기
    public PlayerGoodsData LoadPlayerGoods()
    {
        if (File.Exists(playerCurrencyFilePath))
        {
            string json = File.ReadAllText(playerCurrencyFilePath);
            return JsonUtility.FromJson<PlayerGoodsData>(json);
        }
        return null; // 파일이 없을 경우 null 반환
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