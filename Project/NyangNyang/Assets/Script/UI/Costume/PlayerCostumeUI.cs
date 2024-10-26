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

    void Start()
    {
        InstantiateInitialElement();
    }

    void InstantiateInitialElement()
    {
        _contentRectTransforms.Add(CatCostumePart.Head, headCostumeContent);
        _contentRectTransforms.Add(CatCostumePart.Body, bodyCostumeContent);
        _contentRectTransforms.Add(CatCostumePart.Hand_R, handRCostumeContent);
        _contentRectTransforms.Add(CatCostumePart.FurSkin, furSkinContent);

        for (int i = 0; i < (int)CatCostumePart.Count; ++i)
        {
            int costumeCount = CostumeManager.GetInstance().GetCostumeCountByPart((CatCostumePart)i);
            for (int ii = 0; ii < costumeCount; ++ii)
            {
                CostumeElement element = Instantiate(costumeElementPrefab, _contentRectTransforms[(CatCostumePart)i]);
                element.SetNameText(CostumeManager.GetInstance().GetCostumeName((CatCostumePart)i, ii));
                
                // TODO: 10/27 임시코드. 추후 서버에서 보유중인 코스튬 정보를 받아와 갱신 예정
                element.SetSelectButtonType(OwningSelectButtonType.Owning);
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

        profileCatCostume.ChangeCatCostume(part,costumeIndex);
        profileCatCostume.SetAllCurrentCostumeLayerToUI();
    }
}
