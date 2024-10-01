using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpWeapon : MonoBehaviour
{

    public ScrollRect ScrollRect;
    public float space = 50f;
    public GameObject uiPrefeb;
    public List<RectTransform> uiObjects = new List<RectTransform>();

    private void Start()
    {
    }

    public void ShowPickUpWeapon(int n)
    {
        // »Ì±â

        for ( int j = 0; j < n; ++j)
        {        
            var newUI = Instantiate(uiPrefeb, ScrollRect.content).GetComponent<RectTransform>();
            uiObjects.Add(newUI);

        }
            float y = 0f;
            for ( int i = 0; i < uiObjects.Count; i++ ) {
                uiObjects[i].anchoredPosition = new Vector2(0f, -y);
                y += uiObjects[i].sizeDelta.y + space;
            }

            ScrollRect.content.sizeDelta = new Vector2(ScrollRect.content.sizeDelta.x, y);
    }

    public void ShowPickUpWeapons()
    {
        // ÀÏ°ý »Ì±â
    }
}
