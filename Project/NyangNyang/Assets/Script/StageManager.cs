using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int moveBackgroundSpeed = 1;

    GameObject[] backgroundSprites;

    void Start()
    {
        backgroundSprites = new GameObject[6];
    }

    void GoNextStage()
    {
        // TODO: 스테이지 다음 관문으로 n초뒤 넘어가도록
        
        // 뒤에 배경 넘기기
        StartCoroutine(MoveBackground());
    }

    void ArriveStage()
    {
        // 스테이지 값이 넘어가도록

        // 뒤에 배경 멈추기
        StopCoroutine(MoveBackground());
    }

    IEnumerator MoveBackground()
    {

        // 모든 배경 스프라이트 옮기기
        // 초과되는 부분에 대해서는 반대편으로 넘기기(왼쪽스프라이트를 오른쪽 끝으로)
        yield break;
    }
}
