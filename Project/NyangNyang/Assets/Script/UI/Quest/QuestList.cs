using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestList : MonoBehaviour
{
    private static List<GameObject> _questListObjects = new List<GameObject>();

    private void Awake()
    {
        _questListObjects.Add(this.gameObject);
    }

    public void OnClickSelectQuestListButton(Button pressedButton)
    {
        foreach (GameObject questListObject in _questListObjects)
        {
            questListObject.SetActive(false);
        }
        
        pressedButton.Select();
        gameObject.SetActive(true);
    }
}
