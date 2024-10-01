using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpecialStageManager : MonoBehaviour
{
    
    [SerializeField]
    private GameObject specialStageUI;  // 스페셜 스테이지에서 나타날 UI

    private bool isSpecialStageActive = false;  // 스페셜 스테이지 활성화 여부 체크
    private StageManager stageManager;          // StageManager 참조
    private int originalBackground;             // 원래 배경 테마 저장

    public float playSec = 10.0f;               // 스페셜 스테이지 지속 시간
    public float goldInterval = 0.5f;           // 골드 획득 주기 (초)
    public int baseGoldAmount = 10;             // 기본 골드 획득량

    private Coroutine goldCoroutine;            // 골드 획득 코루틴

    // 스페셜 스테이지별 레벨을 저장할 배열
    private int[] specialStageLevels = new int[3] { 1, 1, 1 };  // 각 스페셜 스테이지의 레벨 (3개 스테이지)
    private int currentSpecialStageIndex;  // 현재 활성화된 스페셜 스테이지 인덱스

    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();  // StageManager 찾기

        if (stageManager == null)
        {
            Debug.LogError("StageManager를 찾을 수 없습니다.");
        }
    }

    // 스페셜 스테이지 시작
    public void StartSpecialStage(int index)
    {
        if (isSpecialStageActive)
        {
            Debug.LogWarning("스페셜 스테이지가 이미 활성화되어 있습니다.");
            return;
        }


        isSpecialStageActive = true;
        stageManager.isSpecial = true;  // 스페셜 스테이지 상태로 전환
        currentSpecialStageIndex = index;  // 현재 스페셜 스테이지 인덱스 저장

        specialStageUI.SetActive(true);  // 스페셜 스테이지 UI 활성화

        if (stageManager != null)
        {
            originalBackground = stageManager.GetCurrentTheme(); // 원래 배경 테마 저장
            stageManager.ChangeBackgroundToSpecialStage(index + 6); // 배경을 스페셜 스테이지 배경(6)으로 변경
        }

        // 골드 획득 코루틴 시작
        goldCoroutine = StartCoroutine(GainGoldOverTime());

        // playSec 뒤 스페셜 스테이지 종료
        Invoke("EndSpecialStage", playSec);
    }

    // 스페셜 스테이지 종료
    public void EndSpecialStage()
    {
        isSpecialStageActive = false;
        stageManager.isSpecial = false;  // 일반 스테이지로 상태 전환

        specialStageUI.SetActive(false);  // 스페셜 스테이지 UI 비활성화

        if (goldCoroutine != null)
        {
            StopCoroutine(goldCoroutine);
        }

        if (stageManager != null)
        {
            stageManager.ChangeBackgroundToSpecialStage(originalBackground); // 원래 배경으로 복구
            stageManager.StopSpecialStage();
        }

        // 현재 스페셜 스테이지의 레벨 상승
        if (currentSpecialStageIndex >= 0 && currentSpecialStageIndex < specialStageLevels.Length)
        {
            specialStageLevels[currentSpecialStageIndex]++;
        }
        
    }

    // 일정 시간마다 골드를 얻는 코루틴
    private IEnumerator GainGoldOverTime()
    {
        while (isSpecialStageActive)
        {
            // 현재 스페셜 스테이지의 레벨에 따른 골드 획득
            if (currentSpecialStageIndex >= 0 && currentSpecialStageIndex < specialStageLevels.Length)
            {
                int goldEarned = 100*baseGoldAmount * specialStageLevels[currentSpecialStageIndex];
                Player.AddGold(goldEarned); // 골드 추가
                //Debug.Log($"골드 획득: {goldEarned}, 레벨: {specialStageLevels[currentSpecialStageIndex]} (스테이지 {currentSpecialStageIndex})");
            }
            else
                Debug.Log("인덱스 넘어감" + currentSpecialStageIndex);
            yield return new WaitForSeconds(goldInterval);
        }
    }
}
