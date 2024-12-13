using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CostumeGacha : MonoBehaviour
{
    private Button gachaButton;
    public GameObject costumeRewardPanel;
    public TextMeshProUGUI costumeNameText;
    public Image costumeImage;


    private void Start()
    {
        gachaButton = GetComponent<Button>();
        gachaButton.onClick.AddListener(GachaCostume);
    }

    public void GachaCostume()
    {
        if (Player.Diamond < 100)
        {
            WarningText.Instance.Set("다이아가 부족합니다.");
            return;
        }

        if (CheckPlayerOwningAllCostume())
        {
            WarningText.Instance.Set("이미 모든 코스튬을 보유중입니다.");
            return;
        }

        Player.Diamond -= 100;

        GetRandomCostume();
    }

    void GetRandomCostume()
    {
        List<CatCostumePart> notAllOwningCostumeParts = new List<CatCostumePart>();
        for (int i = 0; i < (int)CatCostumePart.Count; ++i)
        {
            CatCostumePart part = (CatCostumePart)i;
            if (!CheckPlayerOwningAllCostumeByPart(part))
            {
                notAllOwningCostumeParts.Add(part);
            }
        }

        int gachaCostumePartIndex = Random.Range(0, notAllOwningCostumeParts.Count);
        CatCostumePart gachaCostumePart = notAllOwningCostumeParts[gachaCostumePartIndex];

        List<int> currentOwningCostumeAtPart = PlayerCostume.playerOwningCostume[gachaCostumePart];
        List<int> currentNotOwningCostumeAtPart = new List<int>();
        int costumeCountAtPart = CostumeManager.GetInstance().GetCostumeCountByPart(gachaCostumePart);
        for (int i = 0; i < costumeCountAtPart; ++i)
        {
            if (!currentOwningCostumeAtPart.Contains(i))
            {
                currentNotOwningCostumeAtPart.Add(i);
            }
        }

        int finalRewardCostumeIndex = Random.Range(0, currentNotOwningCostumeAtPart.Count);

        // 지급
        // 12.13 펫을 5마리로 감축
        if (gachaCostumePart == CatCostumePart.Pet)
        {
            finalRewardCostumeIndex = finalRewardCostumeIndex % 5 + 1;
        }
        PlayerCostume.playerOwningCostume[gachaCostumePart].Add(finalRewardCostumeIndex);

        PlayerCostume.SaveToJson();

        costumeRewardPanel.SetActive(true);
        //costumeImage
        costumeNameText.text = EnumTranslator.GetCatCostumePartText(gachaCostumePart) +"\n" + EnumTranslator.GetCostumeText(gachaCostumePart, finalRewardCostumeIndex);
    }

    bool CheckPlayerOwningAllCostumeByPart(CatCostumePart part)
    {
        int costumeCount = CostumeManager.GetInstance().GetCostumeCountByPart(part);
        // 해당 부위를 모두 가지고 있다면,
        if (PlayerCostume.playerOwningCostume[part].Count == costumeCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CheckPlayerOwningAllCostume()
    {
        for (int i = 0; i < (int)CatCostumePart.Count; ++i)
        {
            if (CheckPlayerOwningAllCostumeByPart((CatCostumePart)i))
            {
                continue;
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}
