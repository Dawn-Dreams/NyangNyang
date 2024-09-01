using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;
    public static GameManager GetInstance() { return Instance; }

    public Cat catObject;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public Cat GetCatObject()
    {
        return FindObjectOfType<Cat>();
    }

}
