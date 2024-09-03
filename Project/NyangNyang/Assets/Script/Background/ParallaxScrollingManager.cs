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

    public bool shouldMove = false;

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
}
