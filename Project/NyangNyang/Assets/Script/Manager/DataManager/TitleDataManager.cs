using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TitleGrade
{
    Normal, Uncommon, Rare, Epic, Legendary
}

[Serializable]
public struct OwningEffect
{
    public string type;
    public int value;
}

[SerializeField]
public struct TitleOwningEffect
{
    public OwningEffect[] effects;
}

[Serializable]
public struct TitleInfo
{
    public int id;
    public string name;
    public int grade;
    public OwningEffect[] effect;
}

[Serializable]
public struct TitleData
{
    public TitleInfo[] title;
}

public class TitleDataManager : DataManager
{
    private static TitleDataManager _instance;

    public TextAsset titleDataJsonAsset;
    public TitleData titleData;

    public new static TitleDataManager GetInstance()
    {
        return _instance;
    }

    protected override void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        ReadTitleData();
    }

    private void ReadTitleData()
    {
        if (titleData.title.Length != 0)
        {
            return;
        }

        titleData = JsonUtility.FromJson<TitleData>(titleDataJsonAsset.ToString());
    }
}
