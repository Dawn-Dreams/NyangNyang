using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeButton : MonoBehaviour
{
    public string sceneName;
    public Button sceneChangeButton;

    private void Start()
    {
        if (sceneChangeButton == null)
        {
            sceneChangeButton = GetComponent<Button>();
        }
        sceneChangeButton.onClick.AddListener(ChangeScene);
        
    }

    void ChangeScene()
    {
        Debug.Log("??");
        SceneManager.LoadScene(sceneName);
    }
}
