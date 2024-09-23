using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

class BackgroundSprites
{
    private List<GameObject> spriteObjects;
    private float layerMoveSpeed = 0.0f;
    private float objectSize = Mathf.Infinity;

    public BackgroundSprites(int layerNum, float getLayerMoveSpeed)
    {
        spriteObjects = new List<GameObject>();

        string name = "Layer_" + layerNum;
        spriteObjects.Add(GameObject.Find(name + "_M"));
        spriteObjects.Add(GameObject.Find(name + "_L"));
        spriteObjects.Add(GameObject.Find(name + "_R"));

        layerMoveSpeed = getLayerMoveSpeed;

        if (spriteObjects[0] != null)
        {
            objectSize = spriteObjects[0].GetComponent<SpriteRenderer>().size.x;
        }
    }

    public void MoveLayerImageObjects()
    {
        for (int i = 0; i < spriteObjects.Count; ++i)
        {
            Vector3 initPosition = spriteObjects[i].transform.localPosition;
            initPosition.x -= (layerMoveSpeed * Time.deltaTime);

            if (initPosition.x < -objectSize)
            {
                initPosition.x += objectSize * 2;
            }

            spriteObjects[i].transform.localPosition = initPosition;
            
        }
    }

    // 각 레이어의 3개 스프라이트 교체
    public void SetNewSprite(Sprite middleSprite, Sprite leftSprite, Sprite rightSprite)
    {
        if (spriteObjects.Count >= 3)
        {
            var middleRenderer = spriteObjects[0].GetComponent<SpriteRenderer>();
            var leftRenderer = spriteObjects[1].GetComponent<SpriteRenderer>();
            var rightRenderer = spriteObjects[2].GetComponent<SpriteRenderer>();

            if (middleRenderer != null) middleRenderer.sprite = middleSprite;
            if (leftRenderer != null) leftRenderer.sprite = leftSprite;
            if (rightRenderer != null) rightRenderer.sprite = rightSprite;
        }
    }
}

public class ParallaxScrollingManager : MonoBehaviour
{
    private List<BackgroundSprites> _backgroundObjects;
    // 맨 뒤 레이어 초기 속도
    public float initialMoveSpeed = 1f;
    // background 레이어 갯수
    public int layerCount = 6; 
    // 전방 레이어로 갈수록 감소되는 속도 비율
    public float IncreaseSpeed = 1.2f;

    [SerializeField]
    private bool shouldMove = false;

    // 프리팹을 통해 스프라이트를 교체할 때 사용할 프리팹
    // 프리팹 리스트로 관리
    public List<GameObject> spritePrefabs;

    // 현재 배경 프리팹 번호
    private int currentPrefabIndex = 1; // 처음 지정된 기본 배경 인덱스가 1

    void Start()
    {
        _backgroundObjects = new List<BackgroundSprites>();

        // 배경 오브젝트 씬에서 읽기
        for (int i = 0; i < layerCount; ++i)
        {
            float layerMoveSpeed = initialMoveSpeed * Mathf.Pow(IncreaseSpeed, i);
            _backgroundObjects.Add(new BackgroundSprites(i, layerMoveSpeed));
        }
    }

    void Update()
    {
        MoveSprites();
    }

    void MoveSprites()
    {
        if (!shouldMove) return;

        for (int i = 0; i < layerCount; ++i)
        {
            _backgroundObjects[i].MoveLayerImageObjects();
        }
    }

    // 프리팹에서 각 레이어에 맞는 스프라이트들을 가져와 배경을 교체하는 함수
    public void ChangeBackgroundImageFromPrefab(int index)
    {
        if (spritePrefabs == null || spritePrefabs.Count == 0)
        {
            Debug.LogError("Sprite Prefab이 NULL 입니다.");
            return;
        }

        // index가 6, 7, 8번인 경우 해당 인덱스의 프리팹으로 배경 교체
        if (index >= 6 && index <= 8)
        {
            if (index - 1 < spritePrefabs.Count)
            {
                currentPrefabIndex = index - 1;  // 6, 7, 8에 해당하는 프리팹은 리스트 인덱스로 5, 6, 7이 되도록 조정
            }
            else
            {
                Debug.LogError("유효하지 않은 인덱스입니다.");
                return;
            }
        }
        else 
        {
            // 1 ~ 5번 프리팹 순회
            currentPrefabIndex = (index - 1) % 5; // 순회는 프리팹 인덱스 0~4만 (즉, 1~5번 프리팹만)
        }
       

        GameObject currentPrefab = spritePrefabs[currentPrefabIndex];

        for (int layerIndex = 0; layerIndex < layerCount; layerIndex++)
        {
            string layerName = "Layer_" + layerIndex;

            // 프리팹 내부의 각 레이어의 스프라이트 오브젝트 찾기
            Transform layerTransform = currentPrefab.transform.Find(layerName);
            if (layerTransform == null)
            {
                Debug.LogError(layerName + "이 프리팹에 존재하지 않습니다.");
                continue;
            }

            // 해당 레이어의 "_M", "_L", "_R" 스프라이트 찾기
            SpriteRenderer middleSprite = layerTransform.Find(layerName + "_M")?.GetComponent<SpriteRenderer>();
            SpriteRenderer leftSprite = layerTransform.Find(layerName + "_L")?.GetComponent<SpriteRenderer>();
            SpriteRenderer rightSprite = layerTransform.Find(layerName + "_R")?.GetComponent<SpriteRenderer>();

            if (middleSprite == null || leftSprite == null || rightSprite == null)
            {
                Debug.LogError(layerName + "의 일부 스프라이트를 찾을 수 없습니다.");
                continue;
            }

            // 각 레이어에 새로운 스프라이트 적용
            _backgroundObjects[layerIndex].SetNewSprite(middleSprite.sprite, leftSprite.sprite, rightSprite.sprite);
        }

        //Debug.Log(index + "번으로 배경 이미지 변경");
        //Debug.Log(currentPrefab.name + " 이름");
    }

    public void MoveBackgroundSprites(bool moveBackground)
    {
        shouldMove = moveBackground;
    }
}
