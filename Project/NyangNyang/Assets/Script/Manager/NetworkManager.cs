using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager instance;
    NetworkManager GetStatusManager() { return instance; }

    // 서버로부터 특정 id의 status를 받아오는 함수
    void RecvStatusFromServer(Status status, int id)
    {

    }

    // 서버로 특정 id의 status를 보내는 함수
    void SendStatusToServer(string dataString) { } // int? double? ...



}