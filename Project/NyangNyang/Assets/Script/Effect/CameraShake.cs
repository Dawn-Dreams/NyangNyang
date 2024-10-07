using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;      // 카메라 흔들림의 지속 시간
    public float shakeMagnitude = 0.5f;     // 카메라 흔들림의 강도
    public float dampingSpeed = 1.0f;       // 흔들림이 점차 감소하는 속도 (감쇠 속도)

    private Vector3 originPos;      // 카메라의 원래 위치 저장 변수
    private float shakingTime;      // 흔들리는 시간 카운터

    private void OnEnable()
    {
        originPos = transform.position;
    }

    public void TriggerShake()
    {
        shakingTime = shakeDuration;
    }

     public void TriggerShake(float _shakeDuration, float _shakeMagnitude)
    {
        shakeMagnitude = _shakeMagnitude;
        shakingTime = _shakeDuration;
    }

    void Update()
    {
        if (shakingTime > 0)
        {
            // 흔들림 강도를 시간에 비례하여 감쇠
            float currentShakeMagnitude = shakeMagnitude * (shakingTime / shakeDuration);
            transform.localPosition = originPos - Random.insideUnitSphere * currentShakeMagnitude;
            shakingTime -= Time.deltaTime * dampingSpeed;
        }
        else
            transform.localPosition = originPos;
        
    }
}
