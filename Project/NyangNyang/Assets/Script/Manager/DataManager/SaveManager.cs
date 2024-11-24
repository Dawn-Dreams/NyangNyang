using System.IO;
using UnityEngine;

//����ǻ�� ���� ���⿡ ����Ǿ���
//"C:\Users\gaon7\AppData\LocalLow\DawnDreams\NyangNyang\Status.json"->��������� ���⿡ ����Ǿ� ����

public class SaveLoadManager : MonoBehaviour
{
    private static SaveLoadManager instance;

    //������ �������� ��θ� ����
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

            //����������+�����̸�
            playerStatusFilePath = Path.Combine(Application.persistentDataPath, "Status.json");
            playerStatusLevelFilePath = Path.Combine(Application.persistentDataPath, "StatusLevel.json");
            playerCurrencyFilePath = Path.Combine(Application.persistentDataPath, "CurrencyData.json");
            catCostumeFilePath = Path.Combine(Application.persistentDataPath, "CatCostumePart.json");

        }
        Debug.Log("SaveLoadManager instatnce  �ʱ�ȭ�Ϸ�");

    }

  
    // =================Status=========================
    // ����
    public void SavePlayerStatus(Status data)
    {
        Debug.Log($"Save Player Status: HP = {data.hp}, MP = {data.mp}");

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(playerStatusFilePath, json);
    }

    //�ҷ�����
    public PlayerStatusData LoadPlayerStatus()
    {
        if (File.Exists(playerStatusFilePath))
        {
            string json = File.ReadAllText(playerStatusFilePath);
            return JsonUtility.FromJson<PlayerStatusData>(json);
        }
        return null; // ������ ���� ��� null ��ȯ
    }

    // =================PlayerStatusLevelData=========================
    // ����
    public void SavePlayerStatusLevel(PlayerStatusLevelData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(playerStatusLevelFilePath, json);
    }

    //  �ҷ�����
    public PlayerStatusLevelData LoadPlayerStatusLevel()
    {
        if (File.Exists(playerStatusLevelFilePath))
        {
            string json = File.ReadAllText(playerStatusLevelFilePath);
            return JsonUtility.FromJson<PlayerStatusLevelData>(json);
        }
        return null; // ������ ���� ��� null ��ȯ
    }

    // =================CurrencyData=========================
    // CurrencyData ����
    public void SavePlayerCurrencyData(CurrencyData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(playerCurrencyFilePath, json);
    }

    // CurrencyData �ҷ�����
    public PlayerGoodsData LoadPlayerGoods()
    {
        if (File.Exists(playerCurrencyFilePath))
        {
            string json = File.ReadAllText(playerCurrencyFilePath);
            return JsonUtility.FromJson<PlayerGoodsData>(json);
        }
        return null; // ������ ���� ��� null ��ȯ
    }

    // =================CatCostumePart=========================
    //// ����
    /// public void Save��¼��(������Ŭ�����̸� data)
    /// {
    /// string json = JsonUtility.ToJson(data);
    /// File.WriteAllText(�����ҵ����������������س������, json);
    /// }
    /// 

    ////�ҷ�����
    //public ��ȯŬ�����̸� LoadCatCostumePart()
    //{
    //    if (File.Exists(�����ҵ����������������س������))
    //    {
    //        string json = File.ReadAllText(�����ҵ����������������س������);
    //        return JsonUtility.FromJson<��ȯŬ�����̸�>(json);
    //    }
    //    return null; // ������ ���� ��� null ��ȯ
    //}

}




/*
 * ��뿹��
    
private void Start() -> GameManager�� start �Լ���
    {
        //Awake���� SaveLoadManager�� ����ϸ� null������ �ڲ� �߻��ؼ� start�� �־����
        PlayerStatusData loadedStatus = SaveLoadManager.GetSaveLoadManager().LoadPlayerStatus();
        Debug.Log($"Loaded Player Status: HP = {loadedStatus.hp}, MP = {loadedStatus.mp}");

    }

 ������ ����
        saveLoadManager.SavePlayerStatus(playerStatus);
        saveLoadManager.SavePlayerStatusLevel(playerStatusLevel);
        saveLoadManager.SavePlayerGoods(playerGoods);

������ �ҷ�����
        PlayerStatusData loadedStatus = saveLoadManager.LoadPlayerStatus();
        PlayerStatusLevelData loadedStatusLevel = saveLoadManager.LoadPlayerStatusLevel();
        PlayerGoodsData loadedGoods = saveLoadManager.LoadPlayerGoods();

�ҷ��� ������ ���
        Debug.Log($"Loaded Player Status: HP = {loadedStatus.hp}, MP = {loadedStatus.mp}");
        Debug.Log($"Loaded Player Status Level: HP Level = {loadedStatusLevel.hpLevel}");
        Debug.Log($"Loaded Player Goods: Gold = {loadedGoods.gold}");
 */