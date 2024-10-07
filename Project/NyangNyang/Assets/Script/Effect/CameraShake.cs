using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;      // ī�޶� ��鸲�� ���� �ð�
    public float shakeMagnitude = 0.5f;     // ī�޶� ��鸲�� ����
    public float dampingSpeed = 1.0f;       // ��鸲�� ���� �����ϴ� �ӵ� (���� �ӵ�)

    private Vector3 originPos;      // ī�޶��� ���� ��ġ ���� ����
    private float shakingTime;      // ��鸮�� �ð� ī����

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
            // ��鸲 ������ �ð��� ����Ͽ� ����
            float currentShakeMagnitude = shakeMagnitude * (shakingTime / shakeDuration);
            transform.localPosition = originPos - Random.insideUnitSphere * currentShakeMagnitude;
            shakingTime -= Time.deltaTime * dampingSpeed;
        }
        else
            transform.localPosition = originPos;
        
    }
}
