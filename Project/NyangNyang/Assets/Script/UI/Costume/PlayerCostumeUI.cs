using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCostumeUI : MonoBehaviour
{
    public RectTransform headCostumeContent;
    public RectTransform bodyCostumeContent;
    public RectTransform handRCostumeContent;
    public RectTransform furSkinContent;
    private Dictionary<CatCostumePart, RectTransform> _contentRectTransforms = new Dictionary<CatCostumePart, RectTransform>();

    public Costume profileCatCostume;

    public CostumeElement costumeElementPrefab;

    private Dictionary<CatCostumePart, int> _profileCatCurrentCostume = new Dictionary<CatCostumePart, int>();

    void Start()
    {
        InstantiateInitialElement();
    }

    public void LoadCurrentPlayerCatCostume()
    {
        // 메인 고양이가 장착중인 코스튬 연결
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

        _profileCatCurrentCostume[part] = costumeIndex;

        profileCatCostume.ChangeCatCostume(part,costumeIndex);
        profileCatCostume.SetAllCurrentCostumeLayerToUI();
    }

    public void ApplyCostumeToPlayerCat()
    {
        Costume playerCatCostume = GameManager.GetInstance().catObject.GetComponent<Costume>();
        foreach (var costumeData in _profileCatCurrentCostume)
        {
            playerCatCostume.ChangeCatCostume(costumeData.Key,costumeData.Value);
        }
    }
}
