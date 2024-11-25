using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public enum SnackType
{
    Atk, Hp, Gold
}
public class SnackPanel : MonoBehaviour
{
    public SnackType snackType;

    public Button showAdButton;
    public GameObject eatingImageObject;
    public TextMeshProUGUI remainTimeText;
    public TextMeshProUGUI snackBuffValueText;

    public void SetRemainTimeText(string newTime)
    {
        remainTimeText.text = newTime;
    }

    public void SetSnackBuffValueText(float newBuffValue)
    {
        snackBuffValueText.text = newBuffValue.ToString("F1");
    }
}
