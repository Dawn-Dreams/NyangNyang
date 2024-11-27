using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LetterBoxCameraManager : MonoBehaviour
{
    public Camera camera;
    public bool isDownside = false;

    private void Awake()
    {
        SetLetterBoxCameraResolution();
    }

#if UNITY_EDITOR
    void Update()
    {
        SetLetterBoxCameraResolution();

    }
#endif
    void SetLetterBoxCameraResolution()
    {
        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16); // (가로 / 세로)
        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
            // 레터박스라서 값 역 조정
            rect.height = (1.0f - rect.height) / 2;
            rect.y = isDownside ? 0 : 1 - rect.height;

            rect.x = 0;
            rect.width = 1;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
            // 레터박스라서 값 역 조정
            rect.width = (1.0f - rect.width) / 2;
            // downSide 가 왼쪽
            rect.x = isDownside ? 0 : 1 - rect.width;

            rect.y = 0;
            rect.height = 1;
        }

        if (camera)
        {
            camera.rect = rect;
        }
    }
}