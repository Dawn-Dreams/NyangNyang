using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public int x;              // 타일의 X 좌표
    public int y;              // 타일의 Y 좌표
    public TileType tileType;  // 타일의 타입
    public bool isMerged;      // 타일이 병합되었는지 여부
    public event Action<Direction, int, int> OnTileDragged;  // 드래그 이벤트

    private Image image;  // 타일의 이미지 컴포넌트
    private Vector2 startDragPosition;

    private void Awake()
    {
        image = GetComponent<Image>();

        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }

    public void Initialize(int x, int y, TileType tileType)
    {
        this.x = x;
        this.y = y;
        this.tileType = tileType;

        if (image != null)
        {
            image.sprite = tileType.tileSprite;
        }
        else
        {
            Debug.LogError("Image is null");
        }

        isMerged = false;
        UpdatePosition();
    }

    // 타일의 위치를 설정하고 이동
    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
        UpdatePosition();
    }

    // 타일의 현재 x, y 좌표에 맞는 위치로 이동
    private void UpdatePosition()
    {
        transform.localPosition = new Vector3(x, -y, 0);
    }

    // 터치 시작 시 호출
    public void OnPointerDown(PointerEventData eventData)
    {
        startDragPosition = eventData.position; // 드래그 시작 위치 저장
    }

    // 드래그 중 호출
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dragOffset = eventData.position - startDragPosition;

        if (dragOffset.magnitude > 20) // 드래그가 일정 거리 이상일 때 방향 판별
        {
            Direction direction;
            if (Mathf.Abs(dragOffset.x) > Mathf.Abs(dragOffset.y))
                direction = dragOffset.x > 0 ? Direction.Right : Direction.Left;
            else
                direction = dragOffset.y > 0 ? Direction.Up : Direction.Down;

            OnTileDragged?.Invoke(direction, x, y); // 드래그 방향과 타일 좌표 전달
            startDragPosition = eventData.position; // 새로운 시작 위치 설정
        }
    }

    // 터치 종료 시 호출
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"Tile ({x}, {y}) touch released.");
    }

    public void SetMerged()
    {
        if (!isMerged)
        {
            isMerged = true;
            StartCoroutine(MergeEffect());
        }
    }

    private IEnumerator MergeEffect()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        Vector3 originalScale = transform.localScale;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            image.color = Color.Lerp(tileType.tileColor, Color.clear, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}

public enum Direction
{
    Right,
    Left,
    Up,
    Down
}