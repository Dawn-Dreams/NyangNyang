using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CostumeElement : MonoBehaviour
{
    public TextMeshProUGUI costumeNameText;

    public void SetNameText(string nameString)
    {
        if (costumeNameText)
        {
            costumeNameText.text = nameString;
        }
    }
}
