using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class PlayerProfileUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerTitleText;
    void Start()
    {
        Player.OnSelectTitleChange += SetPlayerTitleText;
    }

    void SetPlayerTitleText()
    {
        TitleInfo titleInfo = TitleDataManager.GetInstance().titleInfoDic[Player.PlayerCurrentTitleID];
        playerTitleText.text = titleInfo.name;
        playerTitleText.color = TitleDataManager.titleGradeColors[(TitleGrade)titleInfo.grade];
    }
}
