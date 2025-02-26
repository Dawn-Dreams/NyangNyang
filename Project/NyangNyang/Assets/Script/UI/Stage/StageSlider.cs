using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSlider : MonoBehaviour
{
    Slider slider;
    [SerializeField] private GameObject gateImageObject;
    [SerializeField] private GameObject gateParentObject;
    private List<GameObject> _gateImages = new List<GameObject>();
    private float parentWidth = 0.0f;


    void Awake()
    {
        slider = GetComponent<Slider>();
        parentWidth = GetComponent<RectTransform>().sizeDelta.x;
    }

    public void CreateGateImage(int maxGateCount)
    {
        if (maxGateCount == _gateImages.Count)
        {
            return;
        }

        ClearGateImages();
        for (int i = 0; i < maxGateCount; ++i)
        {
            GameObject gateImageObj = Instantiate(gateImageObject,gateParentObject.transform);
            gateImageObj.transform.localPosition = new Vector3(
                Mathf.Lerp(0, parentWidth, (float)i / (maxGateCount - 1)) - 10 * i, 0
            );
            _gateImages.Add(gateImageObj);
        }
    }

    public void ClearGateImages()
    {
        foreach (GameObject gateObj in _gateImages)
        {
            Destroy(gateObj);
        }
        _gateImages.Clear();
        slider.value = 0;
    }

    public void WinCombatInGate(int gateNumber)
    {
        gateNumber -= 1;

        // 인덱스 범위 체크
        if (gateNumber < 0 || gateNumber >= _gateImages.Count)
        {
            Debug.LogError($"잘못된 gateNumber 값: {gateNumber + 1}. _gateImages 리스트 크기: {_gateImages.Count}");
            return;
        }

        if (_gateImages[gateNumber])
        {
            Image image = _gateImages[gateNumber].GetComponent<Image>();
            //image.color = new Color(255, 210, 0);
        }
    }
    public void MoveToNextGate(int currentGateNum, int nextGateNum, float moveTime)
    {
        WinCombatInGate(currentGateNum);
        StartCoroutine(SliderMoveToNextGate(currentGateNum, nextGateNum, moveTime));
    }

    IEnumerator SliderMoveToNextGate(int currentGateNum, int nextGateNum, float moveTime)
    {
        if (_gateImages.Count < nextGateNum)
        {
            yield break;
        }

        float valuePerGate = 1.0f / (_gateImages.Count - 1);
        float curTime = 0.0f;
        while (true)
        {
            curTime = Mathf.Min(curTime + Time.deltaTime, moveTime);
            float moveValue = Mathf.Lerp(0.0f, valuePerGate, curTime / moveTime);

            slider.value = valuePerGate * (currentGateNum - 1) + moveValue;

            if (curTime >= moveTime)
            {
                yield break;
            }
            yield return null;
        }
    }
}
