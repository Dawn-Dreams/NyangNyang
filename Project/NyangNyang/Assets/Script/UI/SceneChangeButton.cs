using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeButton : MonoBehaviour
{
    public string sceneName;
    private Button _sceneChangeButton;

    private void Start()
    {
        if (_sceneChangeButton == null)
        {
            _sceneChangeButton = GetComponent<Button>();
            _sceneChangeButton.onClick.AddListener(ChangeScene);
        }
        
    }

    private void OnEnable()
    {
        if (_sceneChangeButton == null)
        {
            _sceneChangeButton = GetComponent<Button>();
            _sceneChangeButton.onClick.AddListener(ChangeScene);
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
