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
    public event Action OnTileTouched;  // Ÿ���� ��ġ�� �� �߻��ϴ� �̺�Ʈ

    private Image image;  // Ÿ���� �̹��� ������Ʈ

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
        if (OnTileTouched != null && !isMerged)
        {
            OnTileTouched.Invoke();
        }
        Debug.Log($"Tile ({x}, {y}) touched.");
    }

    // �巡�� �� ȣ��
    public void OnDrag(PointerEventData eventData)
    {
        if (OnTileTouched != null && !isMerged)
        {
            OnTileTouched.Invoke();
        }
        //Debug.Log($"Tile ({x}, {y}) is being dragged.");
    }

    // ��ġ ���� �� ȣ��
    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log($"Tile ({x}, {y}) touch released.");
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
