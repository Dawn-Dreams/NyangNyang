using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public int x;              // Ÿ���� X ��ǥ
    public int y;              // Ÿ���� Y ��ǥ
    public TileType tileType;  // Ÿ���� Ÿ��
    public bool isMerged;      // Ÿ���� ���յǾ����� ����
    public event Action<Direction, int, int> OnTileDragged;  // �巡�� �̺�Ʈ

    private Image image;  // Ÿ���� �̹��� ������Ʈ
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

    // Ÿ���� ��ġ�� �����ϰ� �̵�
    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
        UpdatePosition();
    }

    // Ÿ���� ���� x, y ��ǥ�� �´� ��ġ�� �̵�
    private void UpdatePosition()
    {
        transform.localPosition = new Vector3(x, -y, 0);
    }

    // ��ġ ���� �� ȣ��
    public void OnPointerDown(PointerEventData eventData)
    {
        startDragPosition = eventData.position; // �巡�� ���� ��ġ ����
    }

    // �巡�� �� ȣ��
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dragOffset = eventData.position - startDragPosition;

        if (dragOffset.magnitude > 20) // �巡�װ� ���� �Ÿ� �̻��� �� ���� �Ǻ�
        {
            Direction direction;
            if (Mathf.Abs(dragOffset.x) > Mathf.Abs(dragOffset.y))
                direction = dragOffset.x > 0 ? Direction.Right : Direction.Left;
            else
                direction = dragOffset.y > 0 ? Direction.Up : Direction.Down;

            OnTileDragged?.Invoke(direction, x, y); // �巡�� ����� Ÿ�� ��ǥ ����
            startDragPosition = eventData.position; // ���ο� ���� ��ġ ����
        }
    }

    // ��ġ ���� �� ȣ��
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