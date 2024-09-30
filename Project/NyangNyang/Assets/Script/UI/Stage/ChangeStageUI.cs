using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeStageUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private StageManager stageManager;
    [SerializeField] private Button backImage;

    private void Awake()
    {
        closeButton.onClick.AddListener(CloseChangeStageUI);
        backImage.onClick.AddListener(CloseChangeStageUI);
    }

    void CloseChangeStageUI()
    {
        gameObject.SetActive(false);
    }
}
