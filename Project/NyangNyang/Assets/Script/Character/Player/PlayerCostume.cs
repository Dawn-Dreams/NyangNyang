using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCostume : Player
{
    // 플레이어가 보유중인 코스튬 리스트
    private static Dictionary<CatCostumePart, List<int>> _playerOwningCostume = new Dictionary<CatCostumePart, List<int>>();

    private static Costume _playerCatCostume;

    public static Dictionary<CatCostumePart, List<int>> playerOwningCostume
    {
        get => _playerOwningCostume;
        set
        {
            if (value == _playerOwningCostume)
            {
                return;
            }
            _playerOwningCostume = value;

            // 보유중인 코스튬 변화에 대한 델리게이트 실행
            if (OnOwningCostumeChange != null)
            {
                OnOwningCostumeChange();
            }
        }
    }

    // 플레이어 현재 칭호 ID
    private static Dictionary<CatCostumePart, int> _playerCurrentEquipCostumes = new Dictionary<CatCostumePart, int>();
    public static Dictionary<CatCostumePart, int> playerCurrentEquipCostumes
    {
        get => _playerCurrentEquipCostumes;
        set
        {
            if (value == _playerCurrentEquipCostumes) return;
            _playerCurrentEquipCostumes = value;

            // 메인 캐릭터 고양이 의상 바꾸기
            foreach (var costumeData in _playerCurrentEquipCostumes)
            {
                _playerCatCostume.ChangeCatCostume(costumeData.Key,costumeData.Value);

                if (OnCurrentCostumeChange != null)
                {
                    OnCurrentCostumeChange(costumeData.Key, costumeData.Value);
                }
            }
        }
    }


    public static void OnAwake_CallInGameManager()
    {
        _playerCatCostume = GameManager.GetInstance().catObject.GetComponent<Costume>();
        PlayerCostumeJsonData data = new PlayerCostumeJsonData();
        SaveLoadManager.GetInstance().LoadPlayerPlayerCostume(out data);
        
        playerCurrentEquipCostumes = new Dictionary<CatCostumePart, int>();
        for (int i = 0; i < (int)CatCostumePart.Count; ++i)
        {
            playerCurrentEquipCostumes[(CatCostumePart)i] = data.currentEquipCostume[i];
        }

        // 하드코딩
        playerOwningCostume = new Dictionary<CatCostumePart, List<int>>();
        playerOwningCostume[CatCostumePart.Head] = data.headOwningCostume;
        playerOwningCostume[CatCostumePart.Body] = data.bodyOwningCostume;
        playerOwningCostume[CatCostumePart.Hand_R] = data.handROwningCostume;
        playerOwningCostume[CatCostumePart.FurSkin] = data.furSkinOwningCostume;
        playerOwningCostume[CatCostumePart.Pet] = data.petOwningCostume;
        playerOwningCostume[CatCostumePart.Emotion] = data.emotionOwningCostume;
    }

    public static void OnStart_ApplyCostumeToPlayerCat_CallInGameMgr()
    {
        for (int i = 0; i < (int)CatCostumePart.Count; ++i)
        {
            CatCostumePart part = (CatCostumePart)i;
            _playerCatCostume.ChangeCatCostume(part, playerCurrentEquipCostumes[part]);
        }
        
    }

    public static void SaveToJson()
    {
        PlayerCostumeJsonData data = new PlayerCostumeJsonData();
        data.currentEquipCostume = new List<int>(){};
        for (int i = 0; i < (int)CatCostumePart.Count; ++i)
        {
            data.currentEquipCostume.Add(playerCurrentEquipCostumes[(CatCostumePart)i]);
        }
        // 하드코딩
        data.headOwningCostume = playerOwningCostume[CatCostumePart.Head];
        data.bodyOwningCostume = playerOwningCostume[CatCostumePart.Body];
        data.handROwningCostume = playerOwningCostume[CatCostumePart.Hand_R];
        data.furSkinOwningCostume = playerOwningCostume[CatCostumePart.FurSkin];
        data.petOwningCostume = playerOwningCostume[CatCostumePart.Pet];
        data.emotionOwningCostume = playerOwningCostume[CatCostumePart.Emotion];
        SaveLoadManager.GetInstance().SavePlayerCostumeData(data);
    }

    void Start()
    {
    }

    // 유저 착용 코스튬 변경 델리게이트
    public delegate void OnCurrentEquipCostumeChangeDelegate(CatCostumePart part, int index);
    public static event OnCurrentEquipCostumeChangeDelegate OnCurrentCostumeChange;

    // 유저 보유 코스튬 변경 델리게이트
    public delegate void OnOwningCostumeChangeDelegate();
    public static event OnOwningCostumeChangeDelegate OnOwningCostumeChange;

    // 코스튬 획득 함수
    public static void AcquireCostume(CatCostumePart part, int index)
    {
        if (!playerOwningCostume[part].Contains(index))
        {
            playerOwningCostume[part].Add(index);
            
            if (OnOwningCostumeChange != null)
            {
                OnOwningCostumeChange();
            }
        }
        else
        {
            Debug.Log($"이미 {part} - {index} 에 대한 코스튬을 보유중");
        }
    }

    public static OwningSelectButtonType GetPlayerCostumeType(CatCostumePart part, int index)
    {
        // 현재 장착중이라면
        if (playerCurrentEquipCostumes[part] == index)
        {
            return OwningSelectButtonType.CurrentSelect;
        }

        // 현재 보유중이라면
        if (playerOwningCostume[part].Contains(index))
        {
            return OwningSelectButtonType.Owning;
        }

        return OwningSelectButtonType.NotOwning;
    }

}
