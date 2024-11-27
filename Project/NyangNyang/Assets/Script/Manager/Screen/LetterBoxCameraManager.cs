using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LetterBoxCameraManager : MonoBehaviour
{

    // 유니티 에디터가 켜질 때 awake 에서 screen.width를 요구하고 ,이 때 Canvas 가 만들어지기 전에 스크린 좌표가 바뀌면서 canvas의 rect transform 의 값에 NAN이 들어감.
    // 이로인해 Assertion failed on expression : 'IsFinite(~~)' 에러가 발생
    // 'Invalid AABB aabb / a' 에러도 발생
    // 따라서 Start로 바꿔주어 에디터 켰을 당시에 캔버스가 초기화 되고서 될 수 있도록 조정,
    // 실제 기기에서도 실행할 때 정상적으로 되는지 확인 필요
    // Canvas 가 UICamera에 연결되면서 발생한 현상

#if UNITY_EDITOR
    private static bool _isEditorInitialize = false;
    void Update()
    {
        if (_isEditorInitialize)
        {
            SetLetterBoxCameraResolution();
        }
    }

    private void OnValidate()
    {
        _isEditorInitialize = true;
    }
#endif
    public Camera camera;
    public bool isDownside = false;


    private void Start()
    {
        SetLetterBoxCameraResolution();
    }

    void SetLetterBoxCameraResolution()
    {
        if (camera == null) return;
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