using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTitle : Player
{
    // 플레이어가 보유중인 칭호 Id 리스트
    public static List<int> playerOwningTitles = new List<int>();

    // 플레이어 현재 칭호 ID
    private static int _playerCurrentTitleID;
    public static int PlayerCurrentTitleID
    {
        get { return _playerCurrentTitleID; }
        set
        {
            if (value == _playerCurrentTitleID) return;
            _playerCurrentTitleID = value;

            if (OnSelectTitleChange != null)
            {
                OnSelectTitleChange();
            }
        }
    }

    void Awake()
    {
        OnOwningTitleChange += SetTitleOwningEffectToStatus;
        playerOwningTitles = DummyPlayerTitleServer.UserRequestOwningTitles(GetUserID());
        PlayerCurrentTitleID = DummyPlayerTitleServer.UserRequestCurrentSelectedTitleID(GetUserID());
    }

    void Start()
    {
    }

    // 유저 착용 칭호 변경 델리게이트
    public delegate void OnSelectTitleChangeDelegate();
    public static event OnSelectTitleChangeDelegate OnSelectTitleChange;

    // 유저 보유 칭호 변경 델리게이트
    public delegate void OnOwningTitleChangeDelegate();
    public static event OnOwningTitleChangeDelegate OnOwningTitleChange;

    // 타이틀 획득 함수
    public static void AcquireTitle(int titleID)
    {
        if (!playerOwningTitles.Contains(titleID))
        {
            playerOwningTitles.Add(titleID);

            if (OnOwningTitleChange != null)
            {
                OnOwningTitleChange();
            }
        }
    }

    // 타이틀 보유효과 적용
    public static void SetTitleOwningEffectToStatus()
    {
        if (!TitleDataManager.GetInstance())
        {
            return;
        }
        List<TitleOwningEffect> titleOwningEffects = new List<TitleOwningEffect>();
        foreach (int owningTitleID in playerOwningTitles)
        {
            TitleInfo titleInfo = TitleDataManager.GetInstance().titleInfoDic[owningTitleID];
            foreach (TitleOwningEffect owningEffect in titleInfo.effect)
            {
                titleOwningEffects.Add(owningEffect);
            }
        }
        playerStatus.SetTitleOwningEffect(titleOwningEffects);
    }
}
