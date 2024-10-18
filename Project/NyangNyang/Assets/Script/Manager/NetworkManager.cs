using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;


public class NetworkManager : MonoBehaviour
{
    private static NetworkManager instance;
     //uwr;
    public static NetworkManager GetStatusManager() { return instance; }
    string _baseUrl = "http://127.0.0.1:11500";

    private void Start()
    {
        if(instance == null)
        {
            instance = new NetworkManager();
        }
        //UpdatePlayerStatus(3, 1, 1, 1, 1, 21, 1, 1, 1);
        UpdatePlayersRanking();
        Debug.Log("networkd instatnce  초기화..");
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

    //DB에 플레이어 정보 업데이트시 사용하는 함수 
    public void UpdatePlayerStatus(int uid, int hp, int mp, int attack_power, int def, 
        int heal_hp_percent, int heal_mp_percent, float crit_percent, float attack_speed)
    {
        Debug.Log("Update Player Status in DB");

        ReqUpdateStatusData req = new ReqUpdateStatusData
        {
            uid = uid,
            hp = hp,
            mp = mp,
            attack_power = attack_power,
            def = def,
            heal_hp_persec = heal_hp_percent,
            heal_mp_persec = heal_mp_percent,
            crit_percent = crit_percent,
            attack_speed = attack_speed

        };

        StartCoroutine(CoSendNetRequest("UpdatePlayerStat", req, UpdateStats));
    }
    public void UpdatePlayerStatusLv(int uid, int hp_lv, int mp_lv, int str_lv, int def_lv,
    int heal_hp_lv, int heal_mp_lv, int crit_lv, int attack_speed_lv,int gold_acq_lv, int exp_acq_lv)
    {
        Debug.Log("Update Player Status Lv in DB");

        ReqUpdateStatusLvData req = new ReqUpdateStatusLvData
        {
            uid = uid,
            hp_lv = hp_lv,
            mp_lv = mp_lv,
            str_lv = str_lv,
            def_lv = def_lv,
            heal_hp_lv = heal_hp_lv,
            heal_mp_lv = heal_mp_lv,
            crit_lv = crit_lv,
            attack_speed_lv = attack_speed_lv,
            gold_acq_lv = gold_acq_lv,
            exp_acq_lv = exp_acq_lv
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

    public void UpdatePlayersRanking()
    {
        //서버에 랭킹요청
        StartCoroutine(CoSendNetRequest("UpdateRanking", null, SettingRankData));
        Debug.Log("UpdatePlayersRanking");

    }

    void SettingRankData(UnityWebRequest uwr)
    {
        //랭킹리스트 받은거 사용하는 함수
        var res = JsonUtility.FromJson<ResponseRanking>(uwr.downloadHandler.text);

        if (res.ErrorCode != (int)ErrorCode.None)
        {
            Debug.Log("Failed get ranking data");
        }
        else
        {
            foreach(RankingData rank in res.rankingData)
            {
                Debug.Log(string.Format("UID : {0}, SCORE : {1}", rank.uid, rank.score));

            }
        }
    }
    void UpdateStats(UnityWebRequest uwr)
    {
        //서버에 DB저장요청 보내고 서버에서 받은 응답

        var res = JsonUtility.FromJson<ResUpdateDbData>(uwr.downloadHandler.text);
        if(res.errorCode != (int)ErrorCode.None)
        {
            Debug.Log("Failed saved DB");
        }
        else
        {
            Debug.Log("Success saved DB");
        }

    }


    //서버에서 DB 정보 받아올때 사용하는 함수들

    // 서버로부터 특정 id의 status를 받아오는 함수
    void RecvStatusFromServer(Status status, int id)
    {

    }


}