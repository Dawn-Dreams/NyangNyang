using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CatFurSkin
{
    // 하얀'냥이, 모래'냥이
    White, Sand,
    // 치즈'냥이, 고등어'냥이, 
    Cheese, Mackerel,
    // 샴'냥이, 랙돌'냥이
    Siamese, RagDoll,
    // 삼색'냥이, 양말'냥이
    ThreeColor, Socks,
    // 턱시도'냥이, 눈'냥이
    Tuxedo, Snow,
    Count
}

// 장착 가능 부위
[Serializable]
public enum CatCostumePart
{
    Head, Hand_R, Body,FurSkin, Pet, Emotion, Count
}

// 머리 장착 코스튬
public enum HeadCostumeType
{
    NotEquip = 0, 
    Meat, HikingHat, Grass, Glass_Red, Count
}

// 오른손 장착 코스튬
public enum HandRCostumeType
{
    NotEquip = 0, 
    Flower, Fork, PinWheel, Wood, Count
}

// 몸 장착 코스튬
public enum BodyCostumeType
{
    NotEquip = 0,
    Bag, Fish, Pan, Count
}

public enum EmotionCostumeType
{// 1,2,3,4, 9 , 11
    Smile, Questioning, CatMouth, BrightlySmile, WhatsWrong, Ggyu, Count
}

// !!TODO: 현재는 코스튬 매니저에서 고양이가 입은 옷을 갈아입히는 역할까지 모두 하지만,
// 나중에는 에셋만 관리하는 느낌으로 고양이 내부 컴퍼넌트에서 에셋을 받아오는 역할을 하는것으로 변경 예정

public class CostumeManager : MonoBehaviour
{
    private static CostumeManager _instance;
    public static CostumeManager GetInstance()
    {
        return _instance;
    }
    // * Awake시 서버로부터 데이터를 받아 에셋을 연결하는데,
    // 해당 프레임에 CostumeManager 가 AssetLoad를 진행하지않아 에셋을 찾을 수 없는 현상을 해결하기 위해 따로 설정
    public static void SetInstance(CostumeManager cm)
    {
        if (_instance == null)
        {
            _instance = cm;
        }
        cm.AssetLoad();
    }

    // 고양이 털 스킨 머테리얼 에셋 관리 변수
    private List<AddressableHandle<Material>> _catFurMaterials = new List<AddressableHandle<Material>>();
    // 고양이 표정 머테리얼 에셋 관리 변수
    private List<AddressableHandle<Material>> _catEmotionMaterials = new List<AddressableHandle<Material>>();
    // 고양이 코스튬 프리팹 에셋 관리 변수
    private Dictionary<CatCostumePart, AddressableHandleAssets<GameObject>> _catCostumes =
        new Dictionary<CatCostumePart, AddressableHandleAssets<GameObject>>();
    
    void Awake()
    {
        AssetLoad();
        if (_instance == null)
        {
            _instance = this;
        }
        
    }

    void OnDestroy()
    {
        foreach (var VARIABLE in _catFurMaterials)
        {
            VARIABLE.Release();
        }

        foreach (var VARIABLE in _catCostumes)
        {
            //VARIABLE.Value.assetsHandle.Release();
        }
    }

    // Addressable 에셋 로드
    private void AssetLoad()
    {
        if (_catFurMaterials.Count != 0)
        {
            // 이미 로드가 완료됨
            return;
        }

        // 고양이 털 스킨 머테리얼 로드
        for (int i = 0; i < (int)CatFurSkin.Count; i++)
        {
            AddressableHandle<Material> furMaterial = new AddressableHandle<Material>();
            furMaterial.Load("CatFurSkin/" + (CatFurSkin)i);
            _catFurMaterials.Add(furMaterial);
        }

        // 고양이 표정 머테리얼 로드
        for (int i = 0; i < (int)EmotionCostumeType.Count; i++)
        {
            AddressableHandle<Material> emotionMaterial = new AddressableHandle<Material>();
            emotionMaterial.Load("Costume/Emotion/" + (EmotionCostumeType)i);
            _catEmotionMaterials.Add(emotionMaterial);
        }

        // 고양이 코스튬 로드
        for (int i = 0; i < (int)CatCostumePart.Count; ++i)
        {
            // 에셋 로드 후
            _catCostumes.Add((CatCostumePart)i, new AddressableHandleAssets<GameObject>());
        }

        // TODO: 빠르게 진행하기 위해 해당 방식을 사용, 추후 for문으로 할 수 있도록 조정
        _catCostumes[(CatCostumePart.Head)].LoadAssets("Costume/Head", Enum.GetNames(typeof(HeadCostumeType)).ToList());
        _catCostumes[(CatCostumePart.Hand_R)].LoadAssets("Costume/Hand_R", Enum.GetNames(typeof(HandRCostumeType)).ToList());
        _catCostumes[(CatCostumePart.Body)].LoadAssets("Costume/Body", Enum.GetNames(typeof(BodyCostumeType)).ToList());
        //_catCostumes[(CatCostumePart.FurSkin)].LoadAssets("Costume/FurSkin", Enum.GetNames(typeof(CatFurSkin)).ToList());
        // 펫은 바로바로 에셋 찾아서 쓰도록, Pet.cs 내에서 관리
        //_catCostumes[(CatCostumePart.Pet)].LoadAssets("Costume/Pet", Enum.GetNames(typeof(EnemyMonsterType)).ToList());
        //_catCostumes[(CatCostumePart.Emotion)]
          //  .LoadAssets("Costume/Emotion", Enum.GetNames(typeof(EmotionCostumeType)).ToList());

    }

    public Material GetCatFurSkinMaterial(CatFurSkin furSkinType)
    {
        return _catFurMaterials[(int)furSkinType].obj;
    }

    public Material GetCatEmotionMaterial(EmotionCostumeType emotionIndex)
    {
        return _catEmotionMaterials[(int)emotionIndex].obj;
    }

    public GameObject GetCatCostumePrefab(CatCostumePart part, int costumeType)
    {
        return _catCostumes[part].objs[costumeType];
    }

    public int GetCostumeCountByPart(CatCostumePart part)
    {
        int retVal = 0;
        switch (part)
        {
            case CatCostumePart.Head:
                retVal = (int)HeadCostumeType.Count;
                break;
            case CatCostumePart.Hand_R:
                retVal = (int)HandRCostumeType.Count;
                break;
            case CatCostumePart.Body:
                retVal = (int) BodyCostumeType.Count;
                break;
            case CatCostumePart.FurSkin:
                retVal = (int)CatFurSkin.Count;
                break;
            case CatCostumePart.Pet:
                // Pet은 행성별 1개체씩만 있도록 변경
                // Null , A B C D E , Count
                retVal = 6;//(int)EnemyMonsterType.Count;
                break;
            case CatCostumePart.Emotion:
                retVal = (int)EmotionCostumeType.Count;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(part), part, null);
        }

        return retVal;
    }

    public string GetCostumeName(CatCostumePart part, int index, bool inKor = true)
    {
        // TODO: inKor에 대한 처리 

        string retVal = "";
        switch (part)
        {
            case CatCostumePart.Head:
                retVal = ((HeadCostumeType)index).ToString();
                break;
            case CatCostumePart.Hand_R:
                retVal = ((HandRCostumeType)index).ToString();
                break;
            case CatCostumePart.Body:
                retVal = ((BodyCostumeType)index).ToString();
                break;
            case CatCostumePart.FurSkin:
                retVal = ((CatFurSkin)index).ToString();
                break;
            case CatCostumePart.Pet:
                retVal = ((EnemyMonsterType)index).ToString();
                break;
            case CatCostumePart.Emotion:
                retVal = ((EmotionCostumeType)index).ToString();
                break;
            case CatCostumePart.Count:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(part), part, null);
        }
        return retVal;
    }

}
