using System.Collections;
using UnityEngine;
using TMPro;

public class WarningText : MonoBehaviour
{
    public static WarningText Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI warningText; // 하나의 텍스트
    [SerializeField] private float animationDuration = 1f; // 애니메이션 전체 지속 시간
    [SerializeField] private float scaleFactor = 1.15f; // 크기 증가 비율
    [SerializeField] public float moveDistance = 3f; // 원하는 이동 높이 (UI 상에서의 이동 거리)
    private Coroutine currentCoroutine;

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            ;
            Destroy(gameObject);
        }
    }

    public void Set(string message)
    {
        if (warningText == null)
        {
            return;
        }

        warningText.text = message;

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(AnimateWarningText());
    }

    private IEnumerator AnimateWarningText()
    {
        warningText.gameObject.SetActive(true);

        float elapsedTime = 0f;
        float initialScale = warningText.transform.localScale.x;
        Color originalColor = warningText.color;
        Vector3 initialPosition = warningText.transform.position;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / animationDuration;
            float scale = Mathf.Lerp(initialScale, initialScale * scaleFactor, progress); // 점차 커지게

            float alpha;
            if (progress <= 0.5f)
            {
                alpha = originalColor.a; // 첫 절반 동안 투명도 유지
            }
            else
            {
                float fadeProgress = (progress - 0.5f) / 0.5f; // 남은 절반 동안 투명도 감소
                alpha = Mathf.Lerp(originalColor.a, 0f, fadeProgress);
            }

            float verticalOffset = Mathf.Lerp(0f, moveDistance, progress); // 위로 이동

            warningText.transform.localScale = Vector3.one * scale;
            warningText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            warningText.transform.position = initialPosition + new Vector3(0f, verticalOffset, 0f);

            yield return null;
        }

        warningText.gameObject.SetActive(false);
        warningText.transform.localScale = Vector3.one * initialScale;
        warningText.color = originalColor;
        warningText.transform.position = initialPosition;

        currentCoroutine = null;
    }

}
