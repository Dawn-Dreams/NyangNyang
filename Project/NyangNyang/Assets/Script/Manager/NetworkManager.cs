using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager instance;
    NetworkManager GetStatusManager() { return instance; }

    // �����κ��� Ư�� id�� status�� �޾ƿ��� �Լ�
    void RecvStatusFromServer(Status status, int id)
    {

    }

    // ������ Ư�� id�� status�� ������ �Լ�
    void SendStatusToServer(string dataString) { } // int? double? ...



}