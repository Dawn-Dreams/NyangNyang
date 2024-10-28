using System.Collections;
using UnityEngine;

public class BackgroundSpritesRound
{
    private GameObject circleObject;  // Circle 객체
    private float rotationSpeed;      // 회전 속도

    public BackgroundSpritesRound(float getRotationSpeed)
    {
        // Circle 오브젝트를 찾고 회전 속도를 설정합니다.
        circleObject = GameObject.Find("Circle");
        rotationSpeed = getRotationSpeed;
    }

    public void RotateCircle()
    {
        if (circleObject == null) return;

        // 중심축을 기준으로 회전
        circleObject.transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
    }
}

public class RoundAutoManager : MonoBehaviour
{
    private BackgroundSpritesRound backgroundCircle;
    public float rotateSpeed = 15f;   // 회전 속도 (초당 5도)

    [SerializeField]
    private bool shouldRotate = false; // 회전 여부

    void Start()
    {
        // Circle 객체를 회전시킬 BackgroundSpritesRound 인스턴스를 초기화합니다.
        backgroundCircle = new BackgroundSpritesRound(rotateSpeed);
    }

    void Update()
    {
        // 회전을 활성화한 경우에만 Circle을 회전시킵니다.
        if (shouldRotate)
        {
            backgroundCircle.RotateCircle();
        }
    }

    // Circle의 회전을 켜거나 끄는 함수
    public void RotateCircle(bool rotateBackground)
    {
        shouldRotate = rotateBackground;
    }
}
