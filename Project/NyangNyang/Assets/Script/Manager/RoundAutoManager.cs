using System;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpritesRound
{
    private GameObject circleObject;  // Circle 객체
    private float rotationSpeed;      // 회전 속도

    public BackgroundSpritesRound(float getRotationSpeed)
    {
        circleObject = GameObject.Find("Circle");
        rotationSpeed = getRotationSpeed;
    }

    public void RotateCircle()
    {
        if (circleObject == null) return;

        // 중심축을 기준으로 회전
        circleObject.transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
    }

    // 스프라이트를 새 이미지로 교체하는 메서드
    public void SetNewSprite(Sprite newSprite)
    {
        if (circleObject == null)
        {
            Debug.LogError("Circle 객체가 존재하지 않습니다.");
            return;
        }

        var spriteRenderer = circleObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = newSprite;
        }
        else
        {
            Debug.LogError("Circle 객체에 SpriteRenderer 컴포넌트가 없습니다.");
        }
    }
}

public class RoundAutoManager : MonoBehaviour
{
    private BackgroundSpritesRound backgroundCircle;
    public float rotateSpeed = 15f;   // 회전 속도 (초당 5도)

    [SerializeField]
    private bool shouldRotate = false; // 회전 여부

    // 스프라이트 리스트
    public List<Sprite> spriteList; //0,1,2,3,4 index
    private int currentSpriteIndex = 0;

    void Start()
    {
        backgroundCircle = new BackgroundSpritesRound(rotateSpeed);

        // 리스트가 비어있는지 확인
        if (spriteList == null || spriteList.Count == 0)
        {
            Debug.LogError("spriteList에 스프라이트가 없습니다.");
        }
        ChangeSpriteByIndex(currentSpriteIndex);
    }

    void Update()
    {
        if (shouldRotate)
        {
            backgroundCircle.RotateCircle();
        }
    }

    // Circle의 회전을 켜거나 끄는 함수
    public void RotateCircle(bool rotateBackground)
    {
        shouldRotate = rotateBackground;
    }

    // 다음 스프라이트로 교체하는 함수
    public void ChangeToNextSprite()
    {
        if (spriteList == null || spriteList.Count == 0) return;

        currentSpriteIndex = (currentSpriteIndex + 1) % spriteList.Count;
        backgroundCircle.SetNewSprite(spriteList[currentSpriteIndex]);
    }

    // 특정 인덱스의 스프라이트로 교체하는 함수
    public void ChangeSpriteByIndex(int index)
    {
        if (spriteList == null || spriteList.Count == 0 || backgroundCircle == null || index > 4 || index < 0)
            return;
        currentSpriteIndex = index % spriteList.Count;
        backgroundCircle.SetNewSprite(spriteList[currentSpriteIndex]);
    }
}
