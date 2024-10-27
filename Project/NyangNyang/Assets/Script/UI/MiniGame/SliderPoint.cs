using UnityEngine;
using UnityEngine.UI;

public class SliderPoint : MonoBehaviour
{
    public Slider slider;      // Slider ������Ʈ
    public Transform center;   // �߾� ��ġ (��: �����̴��� �߽���)
    public Transform point;    // �̵��� �� ��ġ

    private float radius;      // ������
    private float angle;       // ����

    private void Start()
    {
        // ������ ����: ���� ù ��ġ�� �߾� ��ġ �� �Ÿ� ���
        radius = Vector3.Distance(point.position, center.position);

        // Slider�� ���� ����� �� OnValueChanged �̺�Ʈ ȣ��
        slider.onValueChanged.AddListener(UpdatePointPosition);
    }

    private void UpdatePointPosition(float sliderValue)
    {
        // ���� ���: slider�� ���� -360�� ���� �� 90���� ���Ͽ� ���� ��ġ�� 12�� �������� ����
        angle = -sliderValue * 360f + 90f;

        // ������ �������� ��ȯ ��, x�� y ��ǥ ���
        float radian = angle * Mathf.Deg2Rad;
        float x = center.position.x + Mathf.Cos(radian) * radius;
        float y = center.position.y + Mathf.Sin(radian) * radius;

        // ���� ��ġ ������Ʈ
        point.position = new Vector3(x, y, point.position.z);
    }

    private void OnDestroy()
    {
        // OnValueChanged ������ ����
        slider.onValueChanged.RemoveListener(UpdatePointPosition);
    }
}
