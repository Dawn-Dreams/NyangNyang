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


//-------------------------------------------------
//DB에서 긁어올 내용들

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
public class ReqUpdateStatusLvData
{
    public int uid;
    public int hp_lv;
    public int mp_lv;
    public int str_lv;
    public int def_lv;
    public int heal_hp_lv;
    public int heal_mp_lv;
    public int crit_lv;
    public int attack_speed_lv;
    public int gold_acq_lv;
    public int exp_acq_lv;
}
[System.Serializable]

public class mail
{
    public int uid;
    public int mail_type;
    public string mail_content;
    public DateTime mali_create_dt;
    public DateTime mali_read_dt;
    public int mail_reward_item;
    public bool is_recived;

}

//-------------------------------------------------



[System.Serializable]

public class RankingData
{
    public int rank;
    public int uid;
    public string nickname;
    public int score;
    public RankingData(int rank, int uid, string name, int score)
    {
        this.rank = rank;
        this.uid = uid;
        this.nickname = name;
        this.score = score;
    }
}


[System.Serializable]
public class ResponseRanking
{
    public ErrorCode result;
    public List<RankingData> rankingData;
}


[System.Serializable]
public class ResUpdateDbData
{
    public ErrorCode result;
}
//-------------------------------------------------



//-------------------------------------------------
//스코어 업데이트
[System.Serializable]
public class RequestUpdateScore
{
    public int uid { get; set; }
    public int score { get; set; }
}

[System.Serializable]
public class ResponseUpdateScore
{
    public ErrorCode result { get; set; }

}


//gacha
[System.Serializable]
public class ReqEquipGacha
{
    public int uid;
}
[System.Serializable]
public class ResEquipGacha
{
    public ErrorCode result;
    public int itemId;
}

[System.Serializable]
public class ReqSkiilGacha
{
    public int uid;

}
[System.Serializable]
public class ResSkillGacha
{
    public ErrorCode result;
    public int skillId;
}

