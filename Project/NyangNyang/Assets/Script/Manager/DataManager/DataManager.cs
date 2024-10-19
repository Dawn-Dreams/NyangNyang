using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;
using TextAsset = UnityEngine.TextAsset;


public class DataManager : MonoBehaviour
{
    private static DataManager _instance;

    
    public static DataManager GetInstance()
    {
        return _instance;
    }

    protected virtual void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

   
}
