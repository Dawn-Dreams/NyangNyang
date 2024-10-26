using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISwitcher : MonoBehaviour
{
    [SerializeField] private Toggle[] toggles;
    [SerializeField] private GameObject[] switcherGameObjects;

    // 해당 게임 오브젝트가 유지되어야 할 경우
    public bool switchUiInActive = false;

    void Start()
    {
        
        for (int i = 0; i < toggles.Length; ++i)
        {
            int id = i;

            toggles[i].onValueChanged.AddListener((bool b) => SwitchGameObject(b, id));
        }
        SwitchGameObject(true,0);
    }

    void SwitchGameObject(bool isPress, int buttonID)
    {
        if (!isPress)
        {
            return;
        }

        Debug.Log($"잘 되는가? {buttonID}");
        // UI 비활성화 or 자식 순서 변경
        if (!switchUiInActive)
        {
            foreach (GameObject obj in switcherGameObjects)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            switcherGameObjects[buttonID].transform.SetSiblingIndex(switcherGameObjects.Length-1);
        }

        switcherGameObjects[buttonID].SetActive(true);
    }
}
