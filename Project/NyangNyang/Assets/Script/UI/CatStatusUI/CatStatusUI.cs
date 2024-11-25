using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CatStatusUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerCurrentTitleText;

    void Awake()
    {
        PlayerTitle.OnSelectTitleChange += ChangePlayerCurrentTitleText;
    }

    public void ChangePlayerCurrentTitleText()
    {
        Debug.Log("??? 바뀌어야하는데 "+PlayerTitle.PlayerCurrentTitleID);
        TitleInfo currentTitleInfo = TitleDataManager.GetInstance().titleInfoDic[PlayerTitle.PlayerCurrentTitleID];
        playerCurrentTitleText.text = currentTitleInfo.name;
        playerCurrentTitleText.color = TitleDataManager.titleGradeColors[(TitleGrade)currentTitleInfo.grade];
    }
}
