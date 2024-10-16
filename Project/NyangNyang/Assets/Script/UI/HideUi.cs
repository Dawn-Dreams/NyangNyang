using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUi : MonoBehaviour
{
    RectTransform rectTransform;

    public void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void HideUIInVisible()
    {
        rectTransform.offsetMin = rectTransform.offsetMax = new Vector2(10000, 10000);
    }

    public void ShowUI()
    {
        rectTransform.offsetMin = rectTransform.offsetMax = new Vector2(0,0);
    }
}
