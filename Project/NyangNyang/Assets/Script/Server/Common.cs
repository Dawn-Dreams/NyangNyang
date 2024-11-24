using System;
using System.Collections.Generic;


//패킷 주고받는거 

[System.Serializable]
public class PlayerStatusData
{
    public int uid;
    public int hp;             // 체력
    public int mp;             // 마나
    public int attackPower;    // 공격력
    public int defence;        // 방어력
    public int healHPPerSec;   // 초당 체력 회복량   
    public int healMPPerSec;   // 초당 마나 회복량
    public float critPercent; // 치명타 확률
    public float attackSpeed;    // 공격 속도(초기 1, 0.25 상한선 스탯) <- TODO: 회의 필요
    public float weaponEffect;
    public float skillAttackEffect;
    public float skillDefenceEffect;
}

[System.Serializable]
public class PlayerStatusLevelData
{
    public int uid;
    public int hpLevel;
    public int mpLevel;
    public int strLevel;
    public int defenceLevel;
    public int healHpLevel;
    public int healMpLevel;
    public int critLevel;
    public int attackSpeedLevel;
    public int goldAcquisition;
    public int expAcquisition;
}
[System.Serializable]
public class PlayerGoodsData
{
    public int uid;
    public int gold;
    public int jewerly;
    public int cheese;
    public int shell01;
    public int shell02;
    public int shell03;


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
[System.Serializable]
public class ResponseLogin
{
    public ErrorCode result;
    public PlayerStatusData status;
    public PlayerStatusLevelData statusLv;
    public PlayerGoodsData goods;
    public List<mail> mailList;

}
[System.Serializable]
public class RequestLogin
{
    public int uid;
}

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

//[System.Serializable]
//public class RankingData
//{
//    public int rank;
//    public int uid;
//    public string nickname;
//    public int score;
//    public RankingData(int rank, int uid, string name, int score)
//    {
//        this.rank = rank;
//        this.uid = uid;
//        this.nickname = name;
//        this.score = score;
//    }
//}


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

public class ReqRegist { }
[System.Serializable]
public class ResRegist
{
    public ErrorCode result;
    public int uid;
}

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

