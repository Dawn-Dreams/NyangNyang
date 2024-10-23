using System.Collections;
using System;
using UnityEngine;
public class Tile : MonoBehaviour
{
    public int x;              // Ÿ���� X ��ǥ
    public int y;              // Ÿ���� Y ��ǥ
    public TileType tileType;  // Ÿ���� Ÿ��
    public bool isMerged;      // Ÿ���� ���յǾ����� ����
    public event Action OnTileTouched;  // Ÿ���� ��ġ�� �� �߻��ϴ� �̺�Ʈ

    private SpriteRenderer spriteRenderer;  // Ÿ���� ��������Ʈ ������

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Ÿ�� �ʱ�ȭ �޼���
    public void Initialize(int x, int y, TileType tileType)
    {
        this.x = x;
        this.y = y;
        this.tileType = tileType;

        // Ÿ���� ��������Ʈ�� ���� ����
        spriteRenderer.sprite = tileType.tileSprite;
        spriteRenderer.color = tileType.tileColor;
        isMerged = false; // Ÿ�� �ʱ�ȭ �� ���� ���´� false
    }

    // Ÿ���� ��ġ�Ǿ��� �� ����
    private void OnMouseDown()
    {
        if (OnTileTouched != null)
        {
            OnTileTouched.Invoke();
        }
    }

    // Ÿ�� ���� �� ó��
    public void SetMerged()
    {
        isMerged = true;
        // ���յ� Ÿ���� �ð��� ȿ�� (��: ũ�� ��ҳ� ����ȭ ��)
        StartCoroutine(MergeEffect());
    }

    // ���� ȿ�� (����: �����ϰ� ������� ����)
    private IEnumerator MergeEffect()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        Vector3 originalScale = transform.localScale;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            spriteRenderer.color = Color.Lerp(tileType.tileColor, Color.clear, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���յ� �� Ÿ�� ��Ȱ��ȭ
        gameObject.SetActive(false);
    }
}
