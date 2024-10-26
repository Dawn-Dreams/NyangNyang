using UnityEngine;
using UnityEngine.UI;

public class SliderPoint : MonoBehaviour
{
    public Slider slider;      // Slider 컴포넌트
    public Transform center;   // 중앙 위치 (예: 슬라이더의 중심점)
    public Transform point;    // 이동할 점 위치

    private float radius;      // 반지름
    private float angle;       // 각도

    private void Start()
    {
        // 반지름 설정: 점의 첫 위치와 중앙 위치 간 거리 계산
        radius = Vector3.Distance(point.position, center.position);

        // Slider의 값이 변경될 때 OnValueChanged 이벤트 호출
        slider.onValueChanged.AddListener(UpdatePointPosition);
    }

    private void UpdatePointPosition(float sliderValue)
    {
        // 각도 계산: slider의 값에 -360을 곱한 후 90도를 더하여 시작 위치를 12시 방향으로 설정
        angle = -sliderValue * 360f + 90f;

        // 각도를 라디안으로 변환 후, x와 y 좌표 계산
        float radian = angle * Mathf.Deg2Rad;
        float x = center.position.x + Mathf.Cos(radian) * radius;
        float y = center.position.y + Mathf.Sin(radian) * radius;

        // 점의 위치 업데이트
        point.position = new Vector3(x, y, point.position.z);
    }

    private void OnDestroy()
    {
        // OnValueChanged 리스너 제거
        slider.onValueChanged.RemoveListener(UpdatePointPosition);
    }
}
