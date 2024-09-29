using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;


[System.Serializable]
public class RequestTest 
{
    public int uid;
}

[System.Serializable]
public class ResponseTest
{
    public string message;
}


public class NetworkManager : MonoBehaviour
{
    private static NetworkManager instance;
     //uwr;
    public static NetworkManager GetStatusManager() { return instance; }
    string _baseUrl = "http://127.0.0.1:11500";

    private void Start()
    {
        Debug.Log("networkd instatnce  초기화..");
        TestFunc();

    }
  

    public void TestFunc()
    {
        Debug.Log("Test Func TEST...");

        RequestTest t = new RequestTest { uid = 1 };


        StartCoroutine(CoSendNetRequest("Test", t, TestWithServer));
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

    // 서버로부터 특정 id의 status를 받아오는 함수
    void RecvStatusFromServer(Status status, int id)
    {

    }

    // 서버로 특정 id의 status를 보내는 함수
    void SendStatusToServer(string dataString) { } // int? double? ...

}