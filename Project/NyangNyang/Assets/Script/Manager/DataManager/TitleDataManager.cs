using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TitleGrade
{
    Normal, Uncommon, Rare, Epic, Legendary
}

[Serializable]
public struct TitleOwningEffect
{
    public string type;
    public int value;
}

[Serializable]
public struct TitleInfo
{
    public int id;
    public string name;
    public int grade;
    public TitleOwningEffect[] effect;
}

[Serializable]
public struct TitleData
{
    public TitleInfo[] title;
}

public class TitleDataManager : DataManager
{
    private static TitleDataManager _instance;


    public static Dictionary<TitleGrade, Color> titleGradeColors = new Dictionary<TitleGrade, Color>
    {
        { TitleGrade.Normal, Color.white },
        { TitleGrade.Uncommon, Color.green },
        { TitleGrade.Rare, Color.blue },
        { TitleGrade.Epic, new Color(185.0f / 255, 80.0f / 255, 250.0f / 255) },
        { TitleGrade.Legendary, new Color(1.0f, 100.0f / 255, 0.0f) }
    };

    public TextAsset titleDataJsonAsset;
    public TitleData titleData;
    public Dictionary<int, TitleInfo> titleInfoDic = new Dictionary<int, TitleInfo>();

    public new static TitleDataManager GetInstance()
    {
        return _instance;
    }

    protected override void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        ReadTitleData();

        // 초기 한번은 설정
        Player.SetTitleOwningEffectToStatus();
    }

    private void ReadTitleData()
    {
        if (titleData.title.Length != 0)
        {
            return;
        }

        titleData = JsonUtility.FromJson<TitleData>(titleDataJsonAsset.ToString());

        foreach (TitleInfo titleInfo in titleData.title)
        {
            titleInfoDic.Add(titleInfo.id,titleInfo);
        }
    }
}
