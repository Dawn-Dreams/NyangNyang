using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum ErrorCode : int
{
    None = 0,

    //���� �ʱ�ȭ ����
    FailRedisInit = 1,
    FailConnectDB = 2,


    //DB���� ����
    FailSaveUserInfoTable = 1001,
    FailSavePlayerTable = 1002,
    FailSaveInventoryTable = 1003,
    FailUpdatePlayerTable = 1004,

    //ȸ������ �� �α��� ���� �����ڵ�
    FailRegistByUid = 2001,

}




//��Ŷ�� �ϴ� ����ٰ� ����
//Test

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

//������ ���� �����ϴ��� ���� ���
[System.Serializable]
public class ReqUpdateStatusData
{
    public int uid;
    public int hp;
    public int mp;
    public int attack_power;
    public int def;
    public int heal_hp_persec;
    public int heal_mp_persec;
    public float crit_percent;
    public float attack_speed;
}
[System.Serializable]
public class ResUpdateStatusData
{
    public int errorCode;
}