using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class SpecialStageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject specialStageUI;
    private StageManager stageManager;

    public int[] specialStageLevels = new int[3] { 1, 1, 1 };
    private int currentSpecialStageIndex;
    private int originalBackground;             // 원래 배경 테마 저장

    public float playSec = 3.0f;               // 스페셜 스테이지 지속 시간
    public float playDuration = 10.0f;
    public float goldInterval = 0.5f;
    public int baseGoldAmount = 100000;
    private Coroutine goldCoroutine;
    private bool isSuccess;
    // 추가: 각 스테이지의 난이도 (예: 10, 15, 20 등)
    private int[] stageDifficulties = new int[3] { 10, 15, 20 };

    private void Start()
    {
        stageManager = FindObjectOfType<StageManager>() ?? throw new NullReferenceException("StageManager is missing.");
    }

    // 스페셜 스테이지 시작
    public void StartSpecialStage(int index, int level)
    {
        // 스페셜 스테이지가 이미 진행 중인지 확인
        if (GameManager.isSpecialStageActive)
        {
            Debug.LogWarning("스페셜 스테이지가 이미 활성화되어 있습니다.");
            return;
        }

        // 미니게임이 실행 중이면 스페셜 스테이지를 시작할 수 없음
        if (GameManager.isMiniGameActive)
        {
            Debug.Log("미니게임이 실행 중이므로 스페셜 스테이지를 시작할 수 없습니다.");
            return;
        }

        // 티켓 확인
        if (!DummyServerData.HasTicket(Player.GetUserID(), index))
        {
            Debug.Log("소탕권이 부족하여 스페셜 스테이지를 시작할 수 없습니다.");
            return;
        }

        // 난이도 체크: 플레이어의 공격력이 난이도보다 낮으면 실패
        if (Player.playerStatus.attackPower < stageDifficulties[index])
        {
            Debug.Log("플레이어의 공격력이 부족하여 스페셜 스테이지 실패.");
            isSuccess = false;
            EndSpecialStage(); // 실패 처리
            return;
        }
        // 스페셜 스테이지 시작
        GameManager.isSpecialStageActive = true;
        currentSpecialStageIndex = index;  // 현재 스페셜 스테이지 인덱스 저장

        specialStageUI.SetActive(true);  // 스페셜 스테이지 UI 활성화

        if (stageManager != null)
        {
            //originalBackground = stageManager.GetCurrentTheme(); // 원래 배경 테마 저장
            //stageManager.ChangeBackgroundToSpecialStage(index + 6); // 배경을 스페셜 스테이지 배경(6)으로 변경
        }

        // 골드 획득 코루틴 시작
        goldCoroutine = StartCoroutine(GainGoldOverTime(level));
        isSuccess = true;
        // playSec 뒤 스페셜 스테이지 종료
        Invoke("EndSpecialStage", playSec);

        // 티켓 차감 - 스테이지가 정상적으로 시작되었을 때만 티켓을 차감함
        DummyServerData.UseTicket(Player.GetUserID(), index);
        //Debug.Log($"스페셜 스테이지 {index + 1}번 티켓이 차감되었습니다.");
    }


    private IEnumerator GainGoldOverTime(int level)
    {
        while (GameManager.isSpecialStageActive)
        {
            Player.AddGold(baseGoldAmount * level);
            yield return new WaitForSeconds(goldInterval);
        }
    }

    public void EndSpecialStage()
    {
        GameManager.isSpecialStageActive = false;
        specialStageUI.SetActive(false);

        if (goldCoroutine != null) StopCoroutine(goldCoroutine);

        // 성공 처리
        if (isSuccess)
        {
            // 현재 스페셜 스테이지의 레벨 상승
            if (currentSpecialStageIndex >= 0 && currentSpecialStageIndex < specialStageLevels.Length)
            {
                // 레벨 상승
                specialStageLevels[currentSpecialStageIndex]++;
                stageDifficulties[currentSpecialStageIndex] *= 2;
                Debug.Log($"스페셜 스테이지 {currentSpecialStageIndex + 1} 클리어! 다음 레벨이 활성화됩니다.");
                Debug.Log($"({Player.playerStatus.attackPower} , {stageDifficulties[currentSpecialStageIndex]})");
                // SpecialStageMenuPanel에 OnStageCleared 호출
                var specialStageMenuPanel = FindObjectOfType<SpecialStageMenuPanel>();
                if (specialStageMenuPanel != null)
                {
                    specialStageMenuPanel.OnStageCleared(currentSpecialStageIndex, specialStageLevels[currentSpecialStageIndex]);
                }
            }
        }
        else
        {
            Debug.Log($"스페셜 스테이지 {currentSpecialStageIndex + 1} 실패.");
        }
    }

}
