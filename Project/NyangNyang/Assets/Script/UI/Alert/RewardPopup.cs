using TMPro; // TextMeshPro 사용 시 필요
using UnityEngine;

public class RewardPopUp : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;  // Gold 텍스트
    [SerializeField] private TMP_Text expText;   // EXP 텍스트

    // 텍스트 값을 설정하는 메서드
    public void SetValues(int gold, int exp)
    {
        if (goldText != null)
            goldText.text = $"{gold}";
        if (expText != null)
            expText.text = $"{exp}";
    }
}

