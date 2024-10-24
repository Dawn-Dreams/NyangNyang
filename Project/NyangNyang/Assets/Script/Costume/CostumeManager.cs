using System;
using System.Collections;
using System.Collections.Generic;
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

[Serializable]
public enum CatEquipmentPart
{
    Head, Hand_R, Hand_L, Body
}

public class CostumeManager : MonoBehaviour
{
    private static CostumeManager _instance;
    public static CostumeManager GetInstance()
    {
        return _instance;
    }

    [SerializeField] private SkinnedMeshRenderer catSkinnedMesh;
    private List<AddressableHandle<Material>> _catFurMaterials = new List<AddressableHandle<Material>>();
    private CatFurSkin _currentCatFurSkin = CatFurSkin.Cheese;
    void Start()
    {
        AssetLoad();

        if (_instance == null)
        {
            _instance = this;
        }

        // 서버로부터 현재 유저의 CatFurSkin 정보 받기
        
        ChangeCatFurSkin(_currentCatFurSkin);
    }

    public void ChangeCatFurSkin(CatFurSkin changeFurSkin)
    {
        Debug.Log("ChangeCatFurSkin");
        _currentCatFurSkin = changeFurSkin;

        //catSkinnedMesh.materials[0] = _catFurMaterials[(int)changeFurSkin].obj;
        Material[] mats = catSkinnedMesh.materials;
        mats[0] = _catFurMaterials[(int)changeFurSkin].obj;
        catSkinnedMesh.materials = mats;
    }

    private void AssetLoad()
    {
        for (int i = 0; i < (int)CatFurSkin.Count; i++)
        {
            AddressableHandle<Material> furMaterial = new AddressableHandle<Material>();
            furMaterial.Load("CatFurSkin/" + (CatFurSkin)i);
            _catFurMaterials.Add(furMaterial);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("고양이 털 스킨 바꾸기");
            ChangeCatFurSkin((CatFurSkin)(((int)_currentCatFurSkin + 1) % (int)(CatFurSkin.Count)));
        }   
    }
}
