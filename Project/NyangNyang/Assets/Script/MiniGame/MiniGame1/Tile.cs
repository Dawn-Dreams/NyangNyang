using System.Collections;
using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x;              // 타일의 X 좌표
    public int y;              // 타일의 Y 좌표
    public TileType tileType;  // 타일의 타입
    public bool isMerged;      // 타일이 병합되었는지 여부
    public event Action OnTileTouched;  // 타일이 터치될 때 발생하는 이벤트

    private SpriteRenderer spriteRenderer;  // 타일의 스프라이트 렌더러

    // Awake에서 SpriteRenderer 초기화
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Collider2D가 없으면 자동으로 추가
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }

    // 타일 초기화 메서드
    public void Initialize(int x, int y, TileType tileType)
    {
        this.x = x;
        this.y = y;
        this.tileType = tileType;

        // 타일의 스프라이트와 색상 설정
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = tileType.tileSprite;
            spriteRenderer.color = tileType.tileColor;
        }
        else
        {
            Debug.LogError("SpriteRenderer is not assigned.");
        }

        isMerged = false; // 타일 초기화 시 병합 상태는 false
    }

    // 타일이 터치되었을 때 실행
    private void OnMouseDown()
    {
        if (OnTileTouched != null && !isMerged) // 이미 병합된 타일은 터치 불가
        {
            OnTileTouched.Invoke();
        }
        Debug.Log("안녕" + this.x +", "+ this.y);
    }

    // 타일 병합 시 처리
    public void SetMerged()
    {
        if (!isMerged)
        {
            isMerged = true;
            // 병합된 타일의 시각적 효과 (예: 크기 축소나 투명화 등)
            StartCoroutine(MergeEffect());
        }
    }

    // 병합 효과 (예시: 투명하게 사라지는 연출)
    private IEnumerator MergeEffect()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        Vector3 originalScale = transform.localScale;

        // 병합 효과 동안 크기와 색상 변화
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            spriteRenderer.color = Color.Lerp(tileType.tileColor, Color.clear, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 병합된 후 타일 비활성화
        gameObject.SetActive(false);
    }
}
