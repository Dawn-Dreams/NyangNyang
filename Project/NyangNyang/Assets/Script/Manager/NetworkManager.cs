using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkManager : MonoBehaviour
{
    private static NetworkManager instance;
     //uwr;
    public static NetworkManager GetStatusManager() { return instance; }
    string _baseUrl = "http://127.0.0.1:11500";

    private void Start()
    {
        Debug.Log("networkd instatnce  초기화..");
        //TestFunc();

        UpdatePlayerStatus(3, 100, 100, 100, 100, 100, 100, 90.2f, 88.3f);
    }
  

    public void TestFunc()
    {
        Debug.Log("Test Func TEST...");
        RequestTest t = new RequestTest { uid = 1 };

        StartCoroutine(CoSendNetRequest("Test", t, TestWithServer));
    }

    public void UpdatePlayerStatus(int uid, int hp, int mp, int attack_power, int def, 
        int heal_hp_percent, int heal_mp_percent, float crit_percent, float attack_speed)
    {
        Debug.Log("Update Player Status()..Test...");
        //DB에 레벨값 저장 테스트완료

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
    public void UpdatePlayerStatusLevel()
    {
        Debug.Log("GetPlayerStatusLevel()");

       // StartCoroutine(CoSendNetRequest("Test", req, UpdateStats));
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
        yield return uwr.SendWebRequest();    //보내는거임


        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            callback.Invoke(uwr);
        }
    }


    void TestWithServer(UnityWebRequest uwr)
    {
        Debug.Log(uwr.downloadHandler.text);
       
        var ReqTest = JsonUtility.FromJson<ResponseTest>(uwr.downloadHandler.text);
        Debug.Log(ReqTest.message);


    }
    
    void UpdateStats(UnityWebRequest uwr)
    {
        var res = JsonUtility.FromJson<ResUpdateStatusData>(uwr.downloadHandler.text);
        if(res.errorCode != (int)ErrorCode.None)
        {
            Debug.Log("Failed saved DB");
        }
        else
        {
            Debug.Log("Success saved DB");
        }

    }


    // 서버로부터 특정 id의 status를 받아오는 함수
    void RecvStatusFromServer(Status status, int id)
    {

    }

    // 서버로 특정 id의 status를 보내는 함수
    void SendStatusToServer(string dataString) { } // int? double? ...

}