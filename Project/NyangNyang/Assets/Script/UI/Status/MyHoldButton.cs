using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool isPressed = false;
    public Action onPressStartEvent = null;
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        if (onPressStartEvent != null)
        {
            onPressStartEvent();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPressed = false;
    }
}
