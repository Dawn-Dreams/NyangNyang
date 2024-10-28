using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestList : MonoBehaviour
{
    private static List<GameObject> _questListObjects = new List<GameObject>();
    private static Button _currentPressedButton;
    private static Color _normalColor = new Color(0.647f, 0.392f, 0);
    private static Color _pressedColor = new Color(1.0f, 1.0f, 1.0f);

    private void Awake()
    {
        _questListObjects.Add(this.gameObject);
    }

    public void OnClickSelectQuestListButton(Button pressedButton)
    {
        if (_currentPressedButton)
        {
            _currentPressedButton.interactable = true;
            ColorBlock temp = _currentPressedButton.colors;
            temp.normalColor = temp.highlightedColor = temp.disabledColor = _normalColor;
            _currentPressedButton.colors = temp;
        }

        foreach (GameObject questListObject in _questListObjects)
        {
            questListObject.SetActive(false);
        }
        
        //pressedButton.Select();
        _currentPressedButton = pressedButton;
        _currentPressedButton.interactable = false;
        ColorBlock tempColorBlock = _currentPressedButton.colors;
        tempColorBlock.normalColor = tempColorBlock.highlightedColor = tempColorBlock.disabledColor= _pressedColor;
        _currentPressedButton.colors = tempColorBlock;

        gameObject.SetActive(true);
    }
}
