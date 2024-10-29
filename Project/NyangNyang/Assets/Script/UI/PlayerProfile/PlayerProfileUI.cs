using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class PlayerProfileUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerTitleText;
    [SerializeField] private TextMeshProUGUI owningTitleEffectTypeText;
    [SerializeField] private TextMeshProUGUI owningTitleEffectValueText;
    
    void Start()
    {
        PlayerTitle.OnSelectTitleChange += SetPlayerTitleText;
        PlayerTitle.OnOwningTitleChange += SetAllTitleOwningEffectText;

        SetPlayerTitleText();
        SetAllTitleOwningEffectText();
    }

    void SetPlayerTitleText()
    {
        TitleInfo titleInfo = TitleDataManager.GetInstance().titleInfoDic[PlayerTitle.PlayerCurrentTitleID];
        playerTitleText.text = titleInfo.name;
        playerTitleText.color = TitleDataManager.titleGradeColors[(TitleGrade)titleInfo.grade];
    }

    void SetAllTitleOwningEffectText()
    {
        string typeText ="";
        string valueText = "";
        foreach (var owningEffect in Player.playerStatus.titleOwningEffectValue)
        {
            StatusLevelType type = owningEffect.Key;
            int value = owningEffect.Value;
            typeText += (type + "\n");
            valueText += (value + "\n");
        }
        owningTitleEffectTypeText.text  = typeText;
        owningTitleEffectValueText.text = valueText;
    }
}
