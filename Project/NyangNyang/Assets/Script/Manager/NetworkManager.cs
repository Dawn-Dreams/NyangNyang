using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


public class RequestTest 
{
    public int uid {  get; set; }
}

public class ResponseTest
{
    public string? message { get; set; }
}


public class NetworkManager : MonoBehaviour
{
    private static NetworkManager instance;
    NetworkManager GetStatusManager() { return instance; }
    string _baseUrl = "http://127.0.0.1:11500";


    void TestFunc()
    {
        RequestTest t = new RequestTest { uid = 1 };

        
        StartCoroutine(CoSendNetRequest(_baseUrl, t, TestWithServer));
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

        var uwr = new UnityWebRequest(sendUrl, "POST");
        uwr.uploadHandler = new UploadHandlerRaw(jsonByte);

        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        yield return uwr.SendWebRequest();    //�����°���


        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            Debug.Log("Success Recv Data from Server");

            callback.Invoke(uwr);
        }
    }


    void TestWithServer(UnityWebRequest uwr)
    {
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