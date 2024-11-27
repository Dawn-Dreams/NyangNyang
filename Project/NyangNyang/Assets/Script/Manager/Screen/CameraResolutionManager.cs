using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CameraResolutionManager : MonoBehaviour
{
#if UNITY_EDITOR
    private static bool _isEditorInitialize = false;
    void Update()
    {
        if (_isEditorInitialize)
        {
            SetCameraResolution();
        }

    }
#endif
    
    public Camera camera;
    void Start()
    {
        SetCameraResolution(); 
        Debug.Log("갱신 시작");
        _isEditorInitialize = true;
    }



    void SetCameraResolution()
    {
        if (camera == null) return;
        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16); // (가로 / 세로)

        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
            rect.x = 0;
            rect.width = 1;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
            rect.y = 0;
            rect.height = 1;
        }

        if (camera)
        {
            camera.rect = rect;
        }
    }
}