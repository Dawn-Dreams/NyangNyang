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
        // 초기 배경 설정
        ChangeBackgroundImageFromPrefab(currentPrefabIndex);
    }

    void Update()
    {
        if (shouldMove)
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

    // 현재 테마 가져오기 함수
    public int GetCurrentTheme()
    {
        return currentPrefabIndex;
    }

    // 다음 인덱스 배경으로 변경하는 함수
    public void ChangeNextBackgroundImage()
    {
        ChangeBackgroundImageFromPrefab(GetCurrentTheme() + 1);
    }

    // 특정 인덱스 배경으로 변경하는 함수
    public void ChangeIndexNumberBackgroundImage(int index)
    {
        // currentPrefabIndex를 index 값으로 갱신하되, 1부터 시작하는 인덱스를 맞추기 위해 아래와 같이 설정
        currentPrefabIndex = (index - 1) % spritePrefabs.Count;

        // 새 배경 이미지 적용
        ChangeBackgroundImageFromPrefab(currentPrefabIndex);
    }

    // 프리팹에서 각 레이어에 맞는 스프라이트들을 가져와 배경을 교체하는 함수
    public void ChangeBackgroundImageFromPrefab(int index)
    {
        if (spritePrefabs == null || spritePrefabs.Count == 0)
        {
            Debug.LogError("Sprite Prefab이 NULL 입니다.");
            return;
        }
        if (index < 0 || index >= spritePrefabs.Count)
        {
            Debug.LogError($"Index가 범위를 벗어났습니다. (index: {index}, spritePrefabs.Count: {spritePrefabs.Count})");
            return;
        }

        GameObject currentPrefab = spritePrefabs[index];

        if (currentPrefab == null)
        {
            Debug.LogError("현재 Prefab이 NULL 입니다.");
            return;
        }

        for (int layerIndex = 0; layerIndex < layerCount; layerIndex++)
        {
            // 레이어마다 스프라이트를 적용
            ApplySpriteToLayer(currentPrefab, layerIndex);
        }
    }


    // 프리팹에서 각 레이어에 스프라이트 적용하는 함수
    private void ApplySpriteToLayer(GameObject prefab, int layerIndex)
    {
        // 레이어 이름을 구성
        string layerName = "Layer_" + layerIndex;

        // 해당 레이어가 Prefab에 존재하는지 확인
        Transform layerTransform = prefab.transform.Find(layerName);

        if (layerTransform == null)
        {
            Debug.LogError($"{layerName}이 prefab에 존재하지 않습니다. (prefab 이름: {prefab.name})");
            return;
        }

        // 자식 스프라이트 찾기
        SpriteRenderer middleSprite = layerTransform.Find(layerName + "_M")?.GetComponent<SpriteRenderer>();
        SpriteRenderer leftSprite = layerTransform.Find(layerName + "_L")?.GetComponent<SpriteRenderer>();
        SpriteRenderer rightSprite = layerTransform.Find(layerName + "_R")?.GetComponent<SpriteRenderer>();

        // 스프라이트 컴포넌트 존재 여부 체크
        if (middleSprite == null || leftSprite == null || rightSprite == null)
        {
            Debug.LogError($"{layerName}의 스프라이트가 누락되었습니다. (middle: {middleSprite != null}, left: {leftSprite != null}, right: {rightSprite != null})");
            return;
        }

        if (_backgroundObjects == null || layerIndex < 0 || layerIndex >= _backgroundObjects.Count || _backgroundObjects[layerIndex] == null)
        {
            //Debug.LogError($"_backgroundObjects에서 레이어 {layerIndex}에 대한 객체를 찾을 수 없습니다.");
            return;
        }
        else
            _backgroundObjects[layerIndex].SetNewSprite(middleSprite.sprite, leftSprite.sprite, rightSprite.sprite);
    }






    public void MoveBackgroundSprites(bool moveBackground)
    {
        shouldMove = moveBackground;
    }
}
