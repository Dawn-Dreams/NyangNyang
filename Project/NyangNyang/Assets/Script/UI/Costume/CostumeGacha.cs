using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostumeGacha : MonoBehaviour
{
    private Button gachaButton;

    private void Start()
    {
        gachaButton = GetComponent<Button>();
        gachaButton.onClick.AddListener(GachaCostume);
    }

    public void GachaCostume()
    {

    }
}
