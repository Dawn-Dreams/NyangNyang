using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCostumeUI : MonoBehaviour
{
    public RectTransform headCostumeContent;
    public RectTransform bodyCostumeContent;
    public RectTransform handRCostumeContent;
    public RectTransform furSkinContent;
    private Dictionary<CatCostumePart, RectTransform> _contentRectTransforms = new Dictionary<CatCostumePart, RectTransform>();

    public CostumeElement costumeElementPrefab;

    // 고양이 코스튬 컴퍼넌트
    public Costume playerCatCostume;
    public Costume profileCatCostume;
    // 프로필 고양이의 코스튬 장착 정보 저장하는 변수
    private Dictionary<CatCostumePart, int> _profileCatCurrentCostume = new Dictionary<CatCostumePart, int>();

    // 코스튬 엘리멘트 관리 변수
    private Dictionary<CatCostumePart, List<CostumeElement>> _costumeElements =
        new Dictionary<CatCostumePart, List<CostumeElement>>();

    void Start()
    {
        InstantiateInitialElement();

        PlayerCostume.OnOwningCostumeChange += SetCostumeElementOwningImage;
    }

    public void OnPlayerProfileUIOpen()
    {
        LoadCurrentPlayerCatCostume();
        SetCostumeElementOwningImage();
    }

    public void LoadCurrentPlayerCatCostume()
    {
        foreach (var currentCostumeData in PlayerCostume.playerCurrentEquipCostumes)
        {
            profileCatCostume.ChangeCatCostume(currentCostumeData.Key,currentCostumeData.Value);
        }
    }

    // CostumeElement 내 보유중/장착중 등의 이미지를 설정하는 함수
    public void SetCostumeElementOwningImage()
    {
        // 디폴트 NotOwning 상태로 만들기
        foreach (var costumeElementData in _costumeElements)
        {
            foreach (CostumeElement element in costumeElementData.Value)
            {
                element.SetSelectButtonType(OwningSelectButtonType.NotOwning);
            }
        }

        // Owning 설정
        foreach (var owningCostumeData in PlayerCostume.playerOwningCostume)
        {
            // owningCostumeData.key 라는 파츠 내에 들은 인덱스들
            foreach (int index in owningCostumeData.Value)
            {
                _costumeElements[owningCostumeData.Key][index].SetSelectButtonType(OwningSelectButtonType.Owning);
            }
        }

        // CurrentSelect 설정
        foreach (var currentEquipCostumeData in PlayerCostume.playerCurrentEquipCostumes)
        {
            _costumeElements[currentEquipCostumeData.Key][currentEquipCostumeData.Value].SetSelectButtonType(OwningSelectButtonType.CurrentSelect);
        }

    }

    

    void InstantiateInitialElement()
    {
        _contentRectTransforms.Add(CatCostumePart.Head, headCostumeContent);
        _contentRectTransforms.Add(CatCostumePart.Body, bodyCostumeContent);
        _contentRectTransforms.Add(CatCostumePart.Hand_R, handRCostumeContent);
        _contentRectTransforms.Add(CatCostumePart.FurSkin, furSkinContent);

        for (int i = 0; i < (int)CatCostumePart.Count; ++i)
        {
            _profileCatCurrentCostume.Add((CatCostumePart)i, 0);
            _costumeElements.Add((CatCostumePart)i, new List<CostumeElement>());
        }

        for (int i = 0; i < (int)CatCostumePart.Count; ++i)
        {
            int costumeCount = CostumeManager.GetInstance().GetCostumeCountByPart((CatCostumePart)i);
            for (int ii = 0; ii < costumeCount; ++ii)
            {
                CostumeElement element = Instantiate(costumeElementPrefab, _contentRectTransforms[(CatCostumePart)i]);
                //element.SetNameText(CostumeManager.GetInstance().GetCostumeName((CatCostumePart)i, ii));
                element.SetCostumeData((CatCostumePart)i, ii);

                CatCostumePart eventParamPart = (CatCostumePart)i;
                int eventParamIndex = ii;
                element.selectButton.onClick.AddListener(()=>SelectEquipCostumeButton(eventParamPart,eventParamIndex));
                
                // costumeElement 추가
                _costumeElements[(CatCostumePart)i].Add(element);
            }

            _contentRectTransforms[(CatCostumePart)i].localPosition = new Vector3(0,-10000,0);
        }
    }

    void SelectEquipCostumeButton(CatCostumePart part, int costumeIndex)
    {
        if (profileCatCostume == null)
        {
            return;
        }

        if (profileCatCostume.GetCurrentCostumeIndex(part) == costumeIndex)
        {
            return;
        }

        // 이전에 profile cat이 장착중이던 코스튬의 element에 대한 owning image 수정
        OwningSelectButtonType changeType = OwningSelectButtonType.Owning;
        if (PlayerCostume.playerCurrentEquipCostumes[part] == profileCatCostume.GetCurrentCostumeIndex(part))
        {
            changeType = OwningSelectButtonType.CurrentSelect;
        }
        _costumeElements[part][_profileCatCurrentCostume[part]].SetSelectButtonType(changeType);

        // 현재 선택한 코스튬이 이미 선택중이던 코스튬이라면 select / 아니라면 preview    
        if (PlayerCostume.playerCurrentEquipCostumes[part] == costumeIndex)
        {
            Debug.Log("select");
            _costumeElements[part][costumeIndex].SetSelectButtonType(OwningSelectButtonType.CurrentSelect);
        }
        else
        {
            Debug.Log("preview");
            _costumeElements[part][costumeIndex].SetSelectButtonType(OwningSelectButtonType.CurrentPreview);
        }
        

        _profileCatCurrentCostume[part] = costumeIndex;

        profileCatCostume.ChangeCatCostume(part,costumeIndex);
        profileCatCostume.SetAllCurrentCostumeLayerToUI();
    }

    public void ApplyCostumeToPlayerCat()
    {
        // 코스튬에 변경이 있었는지 체크(미리보기로 인하여)
        bool isCostumeChange = false;
        for (int i = 0; i < (int)CatCostumePart.Count; ++i)
        {
            CatCostumePart part = (CatCostumePart)i;
            if (PlayerCostume.playerCurrentEquipCostumes[part] != _profileCatCurrentCostume[part])
            {
                isCostumeChange = true;
                break;
            }
        }
        if (!isCostumeChange)
        {
            Debug.Log("코스튬이 변경되지 않음");
            return;
        }

        // 현재 프로필 고양이가 장착중인 데이터 담기
        //PlayerCostume.playerCurrentEquipCostumes = _profileCatCurrentCostume;
        PlayerCostume.playerCurrentEquipCostumes = new Dictionary<CatCostumePart, int>(_profileCatCurrentCostume);

        // 버튼 타입 변경
        SetCostumeElementOwningImage();

        // 서버로 정보 전송
        // 실제 서버에서는 한 패킷에 모든 정보 담아서
        foreach (var currentCostumeData in PlayerCostume.playerCurrentEquipCostumes)
        {
            DummyPlayerCostumeServer.UserRequestEquipCostume(Player.GetUserID(), currentCostumeData.Key,currentCostumeData.Value);
        }
        

        


        
    }
}
