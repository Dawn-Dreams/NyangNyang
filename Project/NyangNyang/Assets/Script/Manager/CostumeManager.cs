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
    Head, Hand_R, Body, Count
}

// 머리 장착 코스튬
public enum HeadCostumeType
{
    NotEquip = 0, 
    Meat, HikingHat, Grass, Glass_Red, Glass_Green, Glass_Blue, Count
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
    Bag, Fish, Pan, Butterfly, Count
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

    [SerializeField] private SkinnedMeshRenderer catSkinnedMesh;

    // 고양이 코스튬 소켓
    [SerializeField] private Transform catHeadTransform;
    [SerializeField] private Transform catBodyTransform;
    [SerializeField] private Transform catHandRTransform;
    private Dictionary<CatCostumePart, Transform> _catCostumeTransforms = new Dictionary<CatCostumePart, Transform>
    {
        { CatCostumePart.Head , null},
        { CatCostumePart.Body , null},
        { CatCostumePart.Hand_R , null},
    };

    // 고양이 털 스킨 머테리얼 에셋 관리 변수
    private List<AddressableHandle<Material>> _catFurMaterials = new List<AddressableHandle<Material>>();
    // 고양이 코스튬 프리팹 에셋 관리 변수
    private Dictionary<CatCostumePart, AddressableHandleAssets<GameObject>> _catCostumes =
        new Dictionary<CatCostumePart, AddressableHandleAssets<GameObject>>();
    

    // 현재 유저의 털 스킨
    private CatFurSkin _currentCatFurSkin = CatFurSkin.Cheese;
    // 현재 유저의 각 부위별 고양이 코스튬 번호
    private Dictionary<CatCostumePart, int> _currentCatCostume = new Dictionary<CatCostumePart, int>
    {
        { CatCostumePart.Head, (int)(HeadCostumeType.NotEquip)  },
        { CatCostumePart.Body, (int)(BodyCostumeType.NotEquip)  },
        { CatCostumePart.Hand_R, (int)(HandRCostumeType.NotEquip) },
    };
    // 해당 파츠의 코스튬 GameObject
    private Dictionary<CatCostumePart, GameObject> _currentCatCostumeGameObjects = new Dictionary<CatCostumePart, GameObject>
    {
        { CatCostumePart.Head, null },
        { CatCostumePart.Body, null },
        { CatCostumePart.Hand_R, null },
    };
    
    
    void Start()
    {
        // 고양이 코스튬 소켓(Transform) 연결
        _catCostumeTransforms[CatCostumePart.Head] = catHeadTransform;
        _catCostumeTransforms[CatCostumePart.Body] = catBodyTransform;
        _catCostumeTransforms[CatCostumePart.Hand_R] = catHandRTransform;


        AssetLoad();

        if (_instance == null)
        {
            _instance = this;
        }

        // TODO: 서버로부터 현재 유저의 CatFurSkin 정보 받기
        // TODO: 서버로부터 현재 고양이 코스튬 정보 받기
        
        ChangeCatFurSkin(_currentCatFurSkin);
        for (int i = 0; i < (int)CatCostumePart.Count; ++i)
        {
            ChangeCatCostume((CatCostumePart)i, _currentCatCostume[(CatCostumePart)i]);
        }
    }

    // 고양이의 털 스킨을 변경하는 함수
    public void ChangeCatFurSkin(CatFurSkin changeFurSkin)
    {
        Debug.Log("ChangeCatFurSkin");
        _currentCatFurSkin = changeFurSkin;

        Material[] mats = catSkinnedMesh.materials;
        mats[0] = _catFurMaterials[(int)changeFurSkin].obj;
        catSkinnedMesh.materials = mats;
    }

    public void ChangeCatCostume(CatCostumePart part, int costumeType)
    {
        // 현재 착용중인 코스튬은 삭제
        if (_currentCatCostumeGameObjects[part] != null)
        {
            Destroy(_currentCatCostumeGameObjects[part]);
            _currentCatCostumeGameObjects[part] = null;
        }

        // 해당 번호는 NotEquip
        if (costumeType == 0)
        {
            return;
        }

        GameObject changeCostumePrefab = _catCostumes[part].objs[costumeType];

        _currentCatCostume[part] = costumeType;
        _currentCatCostumeGameObjects[part] = Instantiate(changeCostumePrefab, _catCostumeTransforms[part]);
    }

    // Addressable 에셋 로드
    private void AssetLoad()
    {
        // 고양이 털 스킨 머테리얼 로드
        for (int i = 0; i < (int)CatFurSkin.Count; i++)
        {
            AddressableHandle<Material> furMaterial = new AddressableHandle<Material>();
            furMaterial.Load("CatFurSkin/" + (CatFurSkin)i);
            _catFurMaterials.Add(furMaterial);
        }

        // 고양이 코스튬 로드
        for(int i = 0; i < (int)CatCostumePart.Count; ++i)
        {
            // 에셋 로드 후
            _catCostumes.Add((CatCostumePart)i, new AddressableHandleAssets<GameObject>());
        }
        // TODO: 빠르게 진행하기 위해 해당 방식을 사용, 추후 for문으로 할 수 있도록 조정
        _catCostumes[(CatCostumePart.Head)].LoadAssets("Costume/Head", Enum.GetNames(typeof(HeadCostumeType)).ToList());
        _catCostumes[(CatCostumePart.Hand_R)].LoadAssets("Costume/Hand_R", Enum.GetNames(typeof(HandRCostumeType)).ToList());
        _catCostumes[(CatCostumePart.Body)].LoadAssets("Costume/Body", Enum.GetNames(typeof(BodyCostumeType)).ToList());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("고양이 털 스킨 바꾸기");
            ChangeCatFurSkin((CatFurSkin)(((int)_currentCatFurSkin + 1) % (int)(CatFurSkin.Count)));
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            Debug.Log("고양이 모든 스킨 바꾸기");
            
            ChangeCatCostume(CatCostumePart.Head, (_currentCatCostume[CatCostumePart.Head] + 1) % (int)HeadCostumeType.Count);
            ChangeCatCostume(CatCostumePart.Body, (_currentCatCostume[CatCostumePart.Body] + 1) % (int)BodyCostumeType.Count);
            ChangeCatCostume(CatCostumePart.Hand_R, (_currentCatCostume[CatCostumePart.Hand_R] + 1) % (int)HandRCostumeType.Count);
        }
    }
}
