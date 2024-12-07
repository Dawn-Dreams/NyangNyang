using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AlertManager : MonoBehaviour
{
    public static AlertManager instance;
    public static AlertManager GetInstance() { return instance; }


    [SerializeField]
    private GameObject alertPopUp;

    [SerializeField]
    private TextMeshProUGUI Ment;

    private void Awake()
    {
        if ( instance == null)
        {
            instance = this;
        }
    }

    public void SetText(string text)
    {
        alertPopUp.SetActive(true);
        Ment.text = text;
    }
}
