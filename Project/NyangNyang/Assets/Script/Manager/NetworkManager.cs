using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Networking;

enum MailType
{
    Event = 0,
    Reward = 1,
    Friend = 2
}


public class NetworkManager : MonoBehaviour
{
    private static NetworkManager instance;
    private List<string> _mailTitle = new List<string>();

    public static NetworkManager GetStatusManager()
    {
        return instance;
    }

    //gcp 통신 외부ip35.232.170.22
    string _baseUrl = "http://35.232.170.226:11500";

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (PlayerPrefs.HasKey("uid"))
            {
                //playerpfer에 uid가 있으면 유저임
                var uid = PlayerPrefs.GetInt("uid");
                UserLogin(uid);
            }
            else
            { 
                ReqUserRegist();
            }
     
        }

        Debug.Log("networkd instatnce  초기화완료");
    }



    IEnumerator CoSendNetRequest(string url, object obj, Action<UnityWebRequest> callback)
    {
        string sendUrl = $"{_baseUrl}/{url}";

        byte[] jsonByte = null;
        if (obj != null)
        {
            string jsonStr = JsonUtility.ToJson(obj);
            jsonByte = Encoding.UTF8.GetBytes(jsonStr);

        }

        UnityWebRequest uwr = new UnityWebRequest(sendUrl, "POST");
        uwr.uploadHandler = new UploadHandlerRaw(jsonByte);

        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        yield return uwr.SendWebRequest();


        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            callback.Invoke(uwr);
        }
    }
    public void ReqUserRegist()
    {
        Debug.Log("UserRegister");

        ReqRegist req = new ReqRegist();
        StartCoroutine(CoSendNetRequest("Regist", req, GetUserId));


    }
    void GetUserId(UnityWebRequest uwr)
    {
        var res = JsonUtility.FromJson<ResRegist>(uwr.downloadHandler.text);

        if (res.result != (int)ErrorCode.None)
        {
            Debug.Log("Failed get register data");
        }
        else
        {
            Debug.Log(string.Format("Register Success User ID {0}", res.uid));

            Debug.Log(string.Format("Register ger userId {0}", Player.GetUserID()));

            Player.SetUserId(res.uid);
        }


    }
    public void UserLogin(int uid)
    {
        Debug.Log("User Login");

        RequestLogin req = new RequestLogin
        {
            uid = uid
        };
        StartCoroutine(CoSendNetRequest("Login", req, GetPlayerGameData));

    }
    void GetPlayerGameData(UnityWebRequest uwr)
    {
        var res = JsonUtility.FromJson<ResponseLogin>(uwr.downloadHandler.text);

        if (res.result != ErrorCode.None)
        {

            Debug.Log("Failed gacha");
        }
        else
        {
            Debug.Log("Success Login");
        }
    }

    public void ChangeNickname(int uid, string oldNickname, string newNickname)
    {
        ReqChangeNickname req = new ReqChangeNickname(uid, oldNickname, newNickname);

        StartCoroutine(CoSendNetRequest("ChangeNickname", req,
            (result) => ChangePlayerNickname(result, newNickname)));

    }

    void ChangePlayerNickname(UnityWebRequest uwr, string nichname)
    {
        var res = JsonUtility.FromJson<ResChangeNickname>(uwr.downloadHandler.text);
        if (res.result != ErrorCode.None)
        {
            //Player.SetPlayerNickname(nichname);

        }
        else
        {
        }
    }

    public void UpdatePlayerStatus(int uid, Status data)
    {
        Debug.Log("Update Player Status in DB");

        PlayerStatusData req = new PlayerStatusData
        {

            uid = uid,
            hp = data.hp,
            mp = data.mp,
            attackPower = data.attackPower,
            defence = data.defence,
            healHPPerSec = data.healHPPerSec,
            healMPPerSec = data.healMPPerSec,
            critPercent = data.critPercent,
            attackSpeed = data.attackSpeed,
            weaponEffect = data.weaponEffect
        };

        if (req != null)
        {
            StartCoroutine(CoSendNetRequest("UpdatePlayerStat", req, UpdateStats));

        }
    }
    public void UpdatePlayerStatusLv(int uid, StatusLevelData data)
    {
        Debug.Log("Update Player Status Lv in DB");

        PlayerStatusLevelData req = new PlayerStatusLevelData
        {
            uid = uid,
            hpLevel = data.statusLevels[0],
            mpLevel = data.statusLevels[1],
            strLevel = data.statusLevels[2],
            defenceLevel = data.statusLevels[3],
            healHpLevel = data.statusLevels[4],
            healMpLevel = data.statusLevels[5],
            critLevel = data.statusLevels[6],
            attackSpeedLevel = data.statusLevels[7],
            goldAcquisition = data.statusLevels[8],
            expAcquisition = data.statusLevels[9]
        };

        StartCoroutine(CoSendNetRequest("UpdatePlayerStatLv", req, UpdateStats));
    }


    public void UpdatePlayerScore(int uid, int score)
    {
        Debug.Log("Update Player Score To server");
        RequestUpdateScore req = new RequestUpdateScore
        {
            uid = uid,
            score = score
        };

        StartCoroutine(CoSendNetRequest("UpdatePlayerScore", req, UpdateStats));

    }

    public void UpdatePlayersRanking(List<RankingData> rankList)
    {
        RequestUpdateScore req = new RequestUpdateScore();

        //������ ��ŷ ������Ʈ ��û -> ��ŷui������ �θ���ɵ�?
        StartCoroutine(CoSendNetRequest("UpdateRanking", null,
            (result) => SettingRankData(result)));
        Debug.Log("SettingRankData");

    }
    void SettingRankData(UnityWebRequest uwr)
    {
        var res = JsonUtility.FromJson<ResponseRanking>(uwr.downloadHandler.text);

        if (res.result != (int)ErrorCode.None)
        {
            Debug.Log("Failed get ranking data");
        }
        else
        {

          //  OptionMenuUI.GetOptionManager().SetRankList(res.rankingData);

        }
    }
    public void EquipmentGacha(int uid)
    {
        ReqWeaponGacha req = new ReqWeaponGacha { uid = uid };

        Debug.Log("GachaEquipmentController");

        StartCoroutine(CoSendNetRequest("GachaEquipment", req, GetEquipmentGacha));

    }
    void GetEquipmentGacha(UnityWebRequest uwr)
    {
        var res = JsonUtility.FromJson<ResWeaponGacha>(uwr.downloadHandler.text);

        if (res.result != ErrorCode.None)
        {
            Debug.Log("Failed gacha");
        }
        else
        {
            //���� �̱� �Ϸ��

            //res.itemId�� �̿��ϸ�ȴ�.

            Debug.Log("Success gacha");
        }
    }
    public void SkillsGacha(int uid)
    {
        ReqSkiilGacha req = new ReqSkiilGacha { uid = uid };

        Debug.Log("GachaSillsController");

        StartCoroutine(CoSendNetRequest("GachaSkiils", req, GetSkiilGacha));

    }
    void GetSkiilGacha(UnityWebRequest uwr)
    {
        var res = JsonUtility.FromJson<ResSkillGacha>(uwr.downloadHandler.text);

        if (res.result != ErrorCode.None)
        {
            Debug.Log("Failed gacha");
        }
        else
        {
            //���� �̱� �Ϸ��

            //res.skillId �̿��ϸ�ȴ�.

            Debug.Log("Success gacha");
        }
    }



    void UpdateStats(UnityWebRequest uwr)
    {
        //������ DB�����û ������ �������� ���� ����

        var res = JsonUtility.FromJson<ResUpdateDbData>(uwr.downloadHandler.text);
        if (res.result != (int)ErrorCode.None)
        {
            Debug.Log("Failed saved DB");
        }
        else
        {
            Debug.Log("Success saved DB");
        }

    }







}