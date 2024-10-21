using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CatStatusUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerCurrentTitleText;

    void Start()
    {
        Player.OnSelectTitleChange += ChangePlayerCurrentTitleText;
    }

    public void ChangePlayerCurrentTitleText()
    {
        TitleInfo currentTitleInfo = TitleDataManager.GetInstance().titleInfoDic[Player.PlayerCurrentTitleID];
        playerCurrentTitleText.text = currentTitleInfo.name;
        playerCurrentTitleText.color = TitleDataManager.titleGradeColors[(TitleGrade)currentTitleInfo.grade];
    }
}
