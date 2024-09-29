using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum ErrorCode : int
{
    None = 0,

    //서버 초기화 엥러
    FailRedisInit = 1,
    FailConnectDB = 2,


    //DB관련 에러
    FailSaveUserInfoTable = 1001,
    FailSavePlayerTable = 1002,
    FailSaveInventoryTable = 1003,
    FailUpdatePlayerTable = 1004,

    //회원가입 및 로그인 관련 에러코드
    FailRegistByUid = 2001,

}




//패킷을 일단 여기다가 정의
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

//데이터 공유 어케하는지 지금 고민
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