using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
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
            instance = gameObject.AddComponent<NetworkManager>();

            _mailTitle.Add("�̺�Ʈ ����");
            _mailTitle.Add("���� ����");
            _mailTitle.Add("ģ�� ��û");
        }

        Debug.Log("networkd instatnce  �ʱ�ȭ..");
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
    public void ReqUserRegist()
    {
        Debug.Log("UserRegister");

        ReqRegist req = new ReqRegist();
        StartCoroutine(CoSendNetRequest("Regist", req, GetUserId));


    }

    public void UserLogin()
    {
        Debug.Log("User Login");
        
        //api�������� �α��� ���� �����Ǹ� �߰�

        //ReqRegist req = new ReqRegist();
        //StartCoroutine(CoSendNetRequest("Regist", req, GetUserId));

    }

    public void ChangeNickname(int uid, string oldNickname, string newNickname)
    {
        ReqChangeNickname req = new ReqChangeNickname(uid, oldNickname, newNickname);

        StartCoroutine(CoSendNetRequest("ChangeNickname", req,
            (result) => SetNewNickname(result, newNickname)));

    }

    //DB�� �÷��̾� ���� ������Ʈ�� ����ϴ� �Լ� 
    public void UpdatePlayerStatus(int uid, int hp, int mp, int attack_power, int def,
        int heal_hp_percent, int heal_mp_percent, float crit_percent, float attack_speed)
    {
        Debug.Log("Update Player Status in DB");

        ReqUpdateStatusData req = new ReqUpdateStatusData
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
    public void UpdatePlayerStatusLv(int uid, int hp_lv, int mp_lv, int str_lv, int def_lv,
    int heal_hp_lv, int heal_mp_lv, int crit_lv, int attack_speed_lv, int gold_acq_lv, int exp_acq_lv)
    {
        Debug.Log("Update Player Status Lv in DB");

        ReqUpdateStatusLvData req = new ReqUpdateStatusLvData
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

        StartCoroutine(CoSendNetRequest("UpdatePlayerStatLv", req, UpdateStats));
    }
    

    public void UpdatePlayerScore(int uid, int score)
    {
        Debug.Log("Update Player Score To server");
        RequestUpdateScore req = new RequestUpdateScore
        {
            uid = uid,
            score = score
        };

        StartCoroutine(CoSendNetRequest("UpdatePlayerScore", req, UpdateStats));

    }

    public void UpdatePlayersRanking()
    {
        //������ ��ŷ ������Ʈ ��û -> ��ŷui������ �θ���ɵ�?
        StartCoroutine(CoSendNetRequest("UpdateRanking", null, SettingRankData));
        Debug.Log("UpdatePlayersRanking");

    }

    public void EquipmentGacha(int uid)
    {
        ReqWeaponGacha req = new ReqWeaponGacha { uid = uid };

        Debug.Log("GachaEquipmentController");

        StartCoroutine(CoSendNetRequest("GachaEquipment", req, GetEquipmentGacha));

    }
    public void SkillsGacha(int uid)
    {
        ReqSkiilGacha req = new ReqSkiilGacha { uid = uid };

        Debug.Log("GachaSillsController");

        StartCoroutine(CoSendNetRequest("GachaSkiils", req, GetSkiilGacha));

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
            Debug.Log(string.Format("Register Success User ID {0}", res.uid));

            //Player.SetUserId(res.uid);
        }

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
            //�ϴ� �޾ƿ��°ű��� �Ϸ�.
            foreach (RankingData rank in res.rankingData)
            {
                Debug.Log(string.Format("UID : {0}, SCORE : {1}", rank.uid, rank.score));

            }
        }
    }
    void UpdateStats(UnityWebRequest uwr)
    {
        //������ DB�����û ������ �������� ���� ����

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

    void SetNewNickname(UnityWebRequest uwr, string nichname)
    {
        var res = JsonUtility.FromJson<ResChangeNickname>(uwr.downloadHandler.text);
        if(res.result!=ErrorCode.None)
        {
            //�г��Ӻ���������Ѵ�.
            nichname = nichname+"�����غθ��~~";
        }
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