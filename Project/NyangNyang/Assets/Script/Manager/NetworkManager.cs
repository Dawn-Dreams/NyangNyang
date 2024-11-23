using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;

enum MailType
{
    Event = 0,
    Reward = 1,
    Friend = 2
}


public class NetworkManager : MonoBehaviour
{
    private static NetworkManager instance;
    private List<string> _mailTitle = new List<string>();

    public static NetworkManager GetStatusManager()
    {
        return instance;
    }
    string _baseUrl = "http://127.0.0.1:11500";

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            _mailTitle.Add("�̺�Ʈ ����");
            _mailTitle.Add("���� ����");
            _mailTitle.Add("ģ�� ��û");

            UserLogin(5);
        }


        Debug.Log("networkd instatnce  �ʱ�ȭ");
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
        yield return uwr.SendWebRequest();


        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            callback.Invoke(uwr);
        }
    }

    //Create New User Request - issus user id
    public void ReqUserRegist()
    {
        Debug.Log("UserRegister");

        ReqRegist req = new ReqRegist();
        StartCoroutine(CoSendNetRequest("Regist", req, GetUserId));

    }

    void GetUserId(UnityWebRequest uwr) 
    {
        var res = JsonUtility.FromJson<ResRegist>(uwr.downloadHandler.text);

        if (res.result != (int)ErrorCode.None)
        {
            Debug.Log("Failed get register data");
        }
        else
        {
            Player.SetUserID(res.uid);

            Debug.Log(string.Format("Register Success User ID {0}", res.uid));
            Debug.Log(string.Format("Register ger userId {0}", Player.GetUserID()));

        }

    }

    //Login Request with uid -> get userdata(ststus, lv, goods, malist ect.. from server db)
    public void UserLogin(int uid)
    {
        Debug.Log("User Login");

        RequestLogin req = new RequestLogin
        {
            uid = uid
        };
        StartCoroutine(CoSendNetRequest("Login", req, GetPlayerGameData));

    }

    void GetPlayerGameData(UnityWebRequest uwr)
    {
        var res = JsonUtility.FromJson<ResponseLogin>(uwr.downloadHandler.text);

        if (res.result != ErrorCode.None)
        {

            Debug.Log("Failed GetPlayerGameData");
        }
        else
        {
            //로그인 성공했을 때 여기서 데이터 세팅해야한다.
            
            Debug.Log("Success Login");
        }
    }

    //change nickname request -> 닉네임 변경 성공여부 리턴
    public void ChangeNickname(int uid, string oldNickname, string newNickname)
    {
        ReqChangeNickname req = new ReqChangeNickname(uid, oldNickname, newNickname);

        StartCoroutine(CoSendNetRequest("ChangeNickname", req,
            (result) => ChangePlayerNickname(result, newNickname)));

    }

    void ChangePlayerNickname(UnityWebRequest uwr, string nichname)
    {
        var res = JsonUtility.FromJson<ResChangeNickname>(uwr.downloadHandler.text);
        if (res.result != ErrorCode.None)
        {
            //닉네임 변경 성공
            //Player.SetPlayerNickname(nichname);

        }
        else
        {
        }
    }

    //플레이어 status 정보 db에 저장요청
    public void UpdatePlayerStatus(int uid, int hp, int mp, int attack_power, int def,
        int heal_hp_percent, int heal_mp_percent, float crit_percent, float attack_speed)
    {
        Debug.Log("Update Player Status in DB");

        PlayerStatusData req = new PlayerStatusData
        {
            uid = uid,
            hp = hp,
            mp = mp,
            attack_power = attack_power,
            def = def,
            heal_hp_persec = heal_hp_percent,
            heal_mp_persec = heal_mp_percent,
            crit_percent = crit_percent,
            attack_speed = attack_speed

        };

        if (req != null)
        {
            StartCoroutine(CoSendNetRequest("UpdatePlayerStat", req, UpdateStats));

        }
    }
    void UpdateStats(UnityWebRequest uwr)
    {

        var res = JsonUtility.FromJson<ResUpdateDbData>(uwr.downloadHandler.text);
        if (res.result != (int)ErrorCode.None)
        {
            Debug.Log("Failed saved DB");
        }
        else
        {
            Debug.Log("Success saved DB");
        }

    }

    //업데이트 레벨 
    public void UpdatePlayerStatusLv(int uid, int hp_lv, int mp_lv, int str_lv, int def_lv,
    int heal_hp_lv, int heal_mp_lv, int crit_lv, int attack_speed_lv, int gold_acq_lv, int exp_acq_lv)
    {
        Debug.Log("Update Player Status Lv in DB");

        PlayerStatusLevelData req = new PlayerStatusLevelData
        {
            uid = uid,
            hp_lv = hp_lv,
            mp_lv = mp_lv,
            str_lv = str_lv,
            def_lv = def_lv,
            heal_hp_lv = heal_hp_lv,
            heal_mp_lv = heal_mp_lv,
            crit_lv = crit_lv,
            attack_speed_lv = attack_speed_lv,
            gold_acq_lv = gold_acq_lv,
            exp_acq_lv = exp_acq_lv
        };

        StartCoroutine(CoSendNetRequest("UpdatePlayerStatLv", req, UpdateStatsLevel));
    }
    void UpdateStatsLevel(UnityWebRequest uwr)
    {

        var res = JsonUtility.FromJson<ResUpdateDbData>(uwr.downloadHandler.text);
        if (res.result != (int)ErrorCode.None)
        {
            Debug.Log("Failed saved DB");
        }
        else
        {
            Debug.Log("Success saved DB");
        }

    }


    //업데이트 점수
    public void UpdatePlayerScore(int uid, int score)
    {
        Debug.Log("Update Player Score To server");
        RequestUpdateScore req = new RequestUpdateScore
        {
            uid = uid,
            score = score
        };

        //todo. 함수 변경해야함 update statt가 아님 -> redis에 올려야할듯?
        //함수가 필요없는거 같긴함.
        StartCoroutine(CoSendNetRequest("UpdatePlayerScore", req, UpdateStats));

    }

    //랭킹 누를때 업데이트누르기
    public void UpdatePlayersRanking(List<RankingData> rankList)
    {
        RequestUpdateScore req = new RequestUpdateScore();

        StartCoroutine(CoSendNetRequest("UpdateRanking", null,
            (result) => SettingRankData(result)));
        Debug.Log("SettingRankData");

    }
    void SettingRankData(UnityWebRequest uwr)
    {
        //��ŷ����Ʈ ������ ����ϴ� �Լ�
        var res = JsonUtility.FromJson<ResponseRanking>(uwr.downloadHandler.text);

        if (res.result != (int)ErrorCode.None)
        {
            Debug.Log("Failed get ranking data");
        }
        else
        {

          //  OptionMenuManager.GetOptionManager().SetRankList(res.rankingData);

        }
    }

    //가차뽑기 누를때 
    public void EquipmentGacha(int uid)
    {
        ReqWeaponGacha req = new ReqWeaponGacha { uid = uid };

        Debug.Log("GachaEquipmentController");

        StartCoroutine(CoSendNetRequest("GachaEquipment", req, GetEquipmentGacha));

    }
    void GetEquipmentGacha(UnityWebRequest uwr)
    {
        var res = JsonUtility.FromJson<ResWeaponGacha>(uwr.downloadHandler.text);

        if (res.result != ErrorCode.None)
        {
            Debug.Log("Failed gacha");
        }
        else
        {
            //���� �̱� �Ϸ��

            //res.itemId�� �̿��ϸ�ȴ�.

            Debug.Log("Success gacha");
        }
    }

    //스킬가차 누를때
    public void SkillsGacha(int uid)
    {
        ReqSkiilGacha req = new ReqSkiilGacha { uid = uid };

        Debug.Log("GachaSillsController");

        StartCoroutine(CoSendNetRequest("GachaSkiils", req, GetSkiilGacha));

    }
    void GetSkiilGacha(UnityWebRequest uwr)
    {
        var res = JsonUtility.FromJson<ResSkillGacha>(uwr.downloadHandler.text);

        if (res.result != ErrorCode.None)
        {
            Debug.Log("Failed gacha");
        }
        else
        {
            //���� �̱� �Ϸ��

            //res.skillId �̿��ϸ�ȴ�.

            Debug.Log("Success gacha");
        }
    }








}