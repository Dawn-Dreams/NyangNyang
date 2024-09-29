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

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void CreateGateImage(int maxGateCount)
    {
        if (maxGateCount == _gateImages.Count)
        {
            return;
        }

        ClearGateImages();
        float parentWidth = 180.0f;
        for (int i = 0; i < maxGateCount; ++i)
        {
            GameObject gateImageObj = Instantiate(gateImageObject,gateParentObject.transform);
            gateImageObj.transform.localPosition = new Vector3(
                Mathf.Lerp(0, parentWidth, (float)i / (maxGateCount - 1)), 0
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
        if (_gateImages[gateNumber])
        {
            Image image = _gateImages[gateNumber].GetComponent<Image>();
            image.color = new Color(255, 210, 0);
        }
    }
    public void MoveToNextGate(int currentGateNum, int nextGateNum, float moveTime)
    {
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
