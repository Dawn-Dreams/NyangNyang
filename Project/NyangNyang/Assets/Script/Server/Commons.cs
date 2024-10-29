using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
    public int mail_reward_item;
    public DateTime mali_create_dt;
    public DateTime mali_read_dt;
    public bool is_recived;

}


//메일관련


//[System.Serializable]
//public class MailData
//{
//    public int mail_type;
//    public string title;
//    public string content;
//    public int itemID;
//    public string date;
//    public bool is_recived;

//    public MailData(int id, string title, string content, int itemID, string date, bool isReceived)
//    {
//        this.mail_type = id;
//        this.title = title;
//        this.content = content;
//        this.itemID = itemID;
//        this.date = date;
//        this.is_recived = isReceived;
//    }
//}

//-------------------------------------------------


//Req -서버한테 요청할 요청패킷)
//Res - 서버한테 받은 응답패킷)

//랭킹관련 패킷 
[System.Serializable]
public class ResUpdateDbData
{
    public ErrorCode result;
}
[System.Serializable]
public class ResponseRanking
{
    public ErrorCode result;
    public List<RankingData> rankingData;
}

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

//회원가입 요청 -> uid 처음발급받을때
public class ReqRegist { }
[System.Serializable]
public class ResRegist
{
    public ErrorCode result;
    public int uid;
}

//닉네임변경
[System.Serializable]
public class ResChangeNickname
{
    public ErrorCode result;
}
[System.Serializable]
public class ReqChangeNickname
{
    public int uid;
    public string oldNickname;
    public string newNickname;

    public ReqChangeNickname(int uid, string oldNickname, string newNickname)
    {
        this.uid = uid;
        this.oldNickname = oldNickname;
        this.newNickname = newNickname;
    }
}

//gacha 관련 패킷
[System.Serializable]
public class ReqWeaponGacha
{
    public int uid;
}
[System.Serializable]
public class ResWeaponGacha
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

