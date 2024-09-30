using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageMapButton : MonoBehaviour
{
    [SerializeField] Button mapButton;
    [SerializeField] private GameObject changeStageUI;

    private void Awake()
    {
        mapButton.onClick.AddListener(() => SetChangeStageUIVisibility(true));
    }

    void SetChangeStageUIVisibility(bool newVisibility)
    {
        changeStageUI.SetActive(newVisibility);
    }
}
