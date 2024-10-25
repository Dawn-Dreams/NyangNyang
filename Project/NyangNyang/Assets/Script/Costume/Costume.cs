using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Costume : MonoBehaviour
{
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

    // 현재 고양이의 털 스킨
    private CatFurSkin _currentCatFurSkin = CatFurSkin.Cheese;
    // 현재 고양이의 각 부위별 고양이 코스튬 번호
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

        // TODO: 해당 정보는 플레이어에서 CatObject 에 접근해서 하기.
        // // TODO: 서버로부터 현재 유저의 CatFurSkin 정보 받기
        // // TODO: 서버로부터 현재 고양이 코스튬 정보 받기

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
        mats[0] = CostumeManager.GetInstance().GetCatFurSkinMaterial(_currentCatFurSkin);
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

        _currentCatCostume[part] = costumeType;
        // 해당 번호는 NotEquip
        if (costumeType == 0)
        {
            return;
        }

        GameObject changeCostumePrefab = CostumeManager.GetInstance().GetCatCostumePrefab(part, costumeType);

        _currentCatCostumeGameObjects[part] = Instantiate(changeCostumePrefab, _catCostumeTransforms[part]);
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
