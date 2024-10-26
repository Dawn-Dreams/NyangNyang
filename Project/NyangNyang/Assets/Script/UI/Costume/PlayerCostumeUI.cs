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
                Instantiate(costumeElementPrefab, _contentRectTransforms[(CatCostumePart)i]).SetNameText(
                    CostumeManager.GetInstance().GetCostumeName((CatCostumePart)i,ii));
            }

            _contentRectTransforms[(CatCostumePart)i].localPosition = new Vector3(0,-10000,0);
        }
        
    }
}
