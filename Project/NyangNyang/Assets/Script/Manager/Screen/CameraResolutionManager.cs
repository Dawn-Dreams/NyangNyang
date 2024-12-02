using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    private void OnValidate()
    {
        _isEditorInitialize = true;
    }
#endif

    [FormerlySerializedAs("camera")] public Camera myCamera;
    void Start()
    {
        SetCameraResolution(); 
    }



    void SetCameraResolution()
    {
        if (myCamera == null) return;
        Rect rect = myCamera.rect;
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


        if (myCamera)
        {
            myCamera.rect = rect;
        }
    }
}