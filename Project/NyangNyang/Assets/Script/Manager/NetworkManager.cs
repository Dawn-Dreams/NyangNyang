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
        Debug.Log("networkd instatnce  �ʱ�ȭ..");
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
        yield return uwr.SendWebRequest();    //�����°���


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

    // �����κ��� Ư�� id�� status�� �޾ƿ��� �Լ�
    void RecvStatusFromServer(Status status, int id)
    {

    }

    // ������ Ư�� id�� status�� ������ �Լ�
    void SendStatusToServer(string dataString) { } // int? double? ...

}