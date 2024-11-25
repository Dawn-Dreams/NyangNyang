using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSkillManager : MonoBehaviour
{

    private static ActiveSkillManager instance;
    public static ActiveSkillManager GetInstance() => instance;

    private void Awake()
    {
        if ( instance == null) { 
            instance = this;
        }
    }

    [Header("우주 냥경찰")]
    public GameObject UFOCat;
    public GameObject Beam;
    public AnimationManager UFOCatAnim;
    public float UFOmoveDuration = 0.5f; // 이동에 걸리는 시간
    public float BeamDuration = 0.5f; // 빔이 유지되는 시간
    public Vector3 UFOstartPosition;  // 시작 위치
    public Vector3 UFOendPosition;    // 끝 위치
    public float beamOffset = 1f;     // 빔의 위치 조정을 위한 Y축 오프셋

    [Header("캣닢비")]
    public GameObject Leaf;
    public int numberOfLeaf = 20;
    public Vector2 spawnArea = new Vector2(10f, 5f); // 나뭇잎 생성 범위 (가로, 세로)
    public float minFallSpeed = 1f; // 최소 낙하 속도
    public float maxFallSpeed = 3f; // 최대 낙하 속도
    public float minScale = 0.5f;   // 최소 크기
    public float maxScale = 1.5f;   // 최대 크기

    [Header("도와줘 냥이")]
    public GameObject HelpCat;
    public AnimationManager HelpCatAnim;
    public float HelpmoveDuration = 0.5f; // 이동에 걸리는 시간
    public Vector3 HelpstartPosition;  // 시작 위치
    public Vector3 HelpendPosition;    // 끝 위치

    [Header("자린고비냥")]
    public GameObject ScroogeCat;
    public AnimationManager ScroogeCatAnim;
    public float ScroogemoveDuration = 0.5f; // 이동에 걸리는 시간
    public Vector3 ScroogestartPosition;  // 시작 위치
    public Vector3 ScroogeendPosition;    // 끝 위치

    [Header("실타래 폭탄")]
    public GameObject Skein;
    public Vector3 start;           // 실타래가 향할 목표 위치
    public Vector3 target;           // 실타래가 향할 목표 위치
    public float throwHeight = 5f;     // 포물선의 최고 높이
    public float duration = 2f;        // 이동에 걸리는 시간


    public float SkillCoolTime = 10f;
    public bool isWaiting = false;
    public Slider CoolTime;

    public void CurSkillActivate(int id)
    {

        if ( !isWaiting )
        {
            switch (id)
            {
                case 0:
                    StartCoroutine(UFOCatSkill());
                    break;
                case 1:
                    StartCoroutine(SpawnLeaves());
                    break;
                case 2:
                    StartCoroutine(HelpMeSkill());
                    break;
                case 3:
                    StartCoroutine(ScroogeSkill());
                    break;
                case 4:
                    StartCoroutine(ThrowToTarget());
                    break;

            }

        }
    }

    IEnumerator HelpMeSkill()
    {

        HelpCat.SetActive(true);
        HelpCat.transform.position = HelpstartPosition;
        HelpCatAnim.PlayAnimation(AnimationManager.AnimationState.Run);

        float elapsedTime = 0f;

        while (elapsedTime < HelpmoveDuration)
        {
            // 이동 비율 계산
            float t = elapsedTime / HelpmoveDuration;

            // Ease-Out 방식으로 속도 조절 (t^2 사용)
            float easedT = Mathf.Pow(t, 2);

            // 고양이 오브젝트를 선형으로 보간하여 이동
            HelpCat.transform.position = Vector3.Lerp(HelpstartPosition, HelpendPosition, easedT);

            elapsedTime += Time.deltaTime;

            yield return null; // 다음 프레임까지 대기
        }

        // 이동 종료 후 정확한 위치 설정
        HelpCat.transform.position = HelpendPosition;


        StartCoroutine(WaitForCoolTime());
    }

    IEnumerator ScroogeSkill()
    {

        ScroogeCat.SetActive(true);
        ScroogeCat.transform.position = ScroogestartPosition;
        ScroogeCatAnim.PlayAnimation(AnimationManager.AnimationState.Run);

        float elapsedTime = 0f;

        while (elapsedTime < ScroogemoveDuration)
        {
            // 이동 비율 계산
            float t = elapsedTime / ScroogemoveDuration;

            // Ease-Out 방식으로 속도 조절 (t^2 사용)
            float easedT = Mathf.Pow(t, 2);

            // 고양이 오브젝트를 선형으로 보간하여 이동
            ScroogeCat.transform.position = Vector3.Lerp(ScroogestartPosition, ScroogeendPosition, easedT);

            elapsedTime += Time.deltaTime;

            yield return null; // 다음 프레임까지 대기
        }

        // 이동 종료 후 정확한 위치 설정
        ScroogeCat.transform.position = ScroogeendPosition;

        StartCoroutine(WaitForCoolTime());
    }

    IEnumerator UFOCatSkill()
    {

        UFOCat.SetActive(true);
        UFOCat.transform.position = UFOstartPosition;
        UFOCatAnim.PlayAnimation(AnimationManager.AnimationState.Run);

        float elapsedTime = 0f;

        while (elapsedTime < UFOmoveDuration)
        {
            // 이동 비율 계산
            float t = elapsedTime / UFOmoveDuration;

            // Ease-Out 방식으로 속도 조절 (t^2 사용)
            float easedT = Mathf.Pow(t, 2);

            // 고양이 오브젝트를 선형으로 보간하여 이동
            UFOCat.transform.position = Vector3.Lerp(UFOstartPosition, UFOendPosition, easedT);

            elapsedTime += Time.deltaTime;

            yield return null; // 다음 프레임까지 대기
        }

        // 이동 종료 후 정확한 위치 설정
        UFOCat.transform.position = UFOendPosition;

        yield return new WaitForSeconds(1f);

        // 빔 생성 및 위치 설정
        GameObject beam = Instantiate(Beam, new Vector3(UFOCat.transform.position.x, UFOCat.transform.position.y - 3f, 0), Quaternion.identity);

        // 빔이 수직으로 보이도록 방향 설정
        // beam.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        // 빔 유지 시간 동안 대기
        yield return new WaitForSeconds(BeamDuration);

        // 빔 제거
        Destroy(beam);
        UFOCat.SetActive(false);

        StartCoroutine(WaitForCoolTime());

    }

    private IEnumerator SpawnLeaves()
    {
        for (int i = 0; i < numberOfLeaf; i++)
        {
            // 나뭇잎 생성
            GameObject leaf = Instantiate(Leaf);

            // 시작 위치 설정 (랜덤 가로 위치 + 스폰 높이)
            float randomX = Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f);
            float startY = spawnArea.y;
            leaf.transform.position = new Vector2(randomX, startY);

            // 크기 설정
            float randomScale = Random.Range(minScale, maxScale);
            leaf.transform.localScale = new Vector3(randomScale, randomScale, 1f);

            // 낙하 코루틴 시작
            float fallSpeed = Random.Range(minFallSpeed, maxFallSpeed);
            StartCoroutine(FallLeaf(leaf, fallSpeed));

            // 나뭇잎 생성 간격
            yield return new WaitForSeconds(0.1f);
        }

        StartCoroutine(WaitForCoolTime());
    }

    private IEnumerator FallLeaf(GameObject leaf, float speed)
    {
        while (leaf.transform.position.y > -spawnArea.y / 2f)
        {
            // 낙하
            leaf.transform.position += Vector3.down * speed * Time.deltaTime;

            // 살짝 흔들림 효과 추가 (Optional)
            float oscillation = Mathf.Sin(Time.time * speed) * 0.1f;
            leaf.transform.position += Vector3.right * oscillation;

            yield return null;
        }

        // 바닥에 닿은 나뭇잎 제거
        Destroy(leaf);
    }

    private IEnumerator ThrowToTarget()
    {
        GameObject skein = Instantiate(Skein);

        Vector3 startPosition = start;  // 시작 위치
        Vector3 targetPosition = target;    // 목표 위치

        float elapsedTime = 0f;  // 경과 시간 초기화

        while (elapsedTime < duration)
        {
            // t는 현재 진행 비율 (0에서 1까지)
            float t = elapsedTime / duration;

            // XZ 평면에서 선형 보간
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // Y 좌표에 포물선 효과 추가
            float height = Mathf.Sin(t * Mathf.PI) * throwHeight; // 포물선 공식
            currentPosition.y += height;

            // 실타래 위치 갱신
            skein.transform.position = currentPosition;

            // 시간 갱신
            elapsedTime += Time.deltaTime;

            yield return null; // 다음 프레임까지 대기
        }

        // 목표 위치에 도달
        Skein.transform.position = targetPosition;
        Debug.Log("도착 완료!");

        yield return new WaitForSeconds(1f);

        Destroy(skein);

        StartCoroutine(WaitForCoolTime());
    }

    IEnumerator WaitForCoolTime()
    {
        float elapsedTime = 0f;
        
        isWaiting = true;

        while (elapsedTime < SkillCoolTime)
        {
            CoolTime.value = elapsedTime / SkillCoolTime;
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        isWaiting = false;
    }

    public void SetSkillCoolTime(float _time)
    {
        SkillCoolTime *= (_time + 1f);
    }

    public void ResetSkillCoolTime()
    {
        SkillCoolTime = 1f;
    }
}
