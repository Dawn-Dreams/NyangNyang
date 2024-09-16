using UnityEngine;
using UnityEngine.UI;

public class SpecialStageManager : MonoBehaviour
{
    [SerializeField]
    private Button specialStageButton;  // 스페셜 스테이지로 이동하는 버튼

    [SerializeField]
    private GameObject specialStageUI;  // 스페셜 스테이지에서 나타날 UI

    private bool isSpecialStageActive = false;  // 스페셜 스테이지 활성화 여부 체크
    private StageManager stageManager;          // StageManager 참조
    private int originalBackground;             // 원래 배경 테마 저장

    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();  // StageManager 찾기

        if (stageManager == null)
        {
            Debug.LogError("StageManager를 찾을 수 없습니다.");
        }

        if (specialStageButton != null)
        {
            specialStageButton.onClick.AddListener(StartSpecialStage); // 버튼 클릭 시 스페셜 스테이지 시작
        }
    }

    // 스페셜 스테이지 시작
    public void StartSpecialStage()
    {
        if (isSpecialStageActive)
        {
            Debug.LogWarning("스페셜 스테이지가 이미 활성화되어 있습니다.");
            return;
        }

        isSpecialStageActive = true;
        stageManager.isSpecial = true;  // 스페셜 스테이지 상태로 전환

        specialStageUI.SetActive(true);  // 스페셜 스테이지 UI 활성화

        if (stageManager != null)
        {
            originalBackground = stageManager.GetCurrentTheme(); // 원래 배경 테마 저장
            stageManager.ChangeBackgroundToSpecialStage(6); // 배경을 스페셜 스테이지 배경(6)으로 변경
        }

        // 10초 뒤에 스페셜 스테이지 종료
        Invoke("EndSpecialStage", 10f);
    }

    // 스페셜 스테이지 종료
    public void EndSpecialStage()
    {
        isSpecialStageActive = false;
        stageManager.isSpecial = false;  // 일반 스테이지로 상태 전환

        specialStageUI.SetActive(false);  // 스페셜 스테이지 UI 비활성화

        if (stageManager != null)
        {
            stageManager.ChangeBackgroundToSpecialStage(originalBackground); // 원래 배경으로 복구
        }

        Debug.Log("스페셜 스테이지 종료");
    }
}
