using UnityEngine;
using UnityEngine.SceneManagement;  // 씬 이동을 위한 네임스페이스
using UnityEngine.UI;

public class SpecialStageManager : MonoBehaviour
{
    [SerializeField]
    private Button specialStageButton;  // 스페셜 스테이지로 이동하는 버튼

    [SerializeField]
    private GameObject specialStageUI;  // 스페셜 스테이지에서 나타날 UI

    private bool isSpecialStageActive = false; // 스페셜 스테이지 활성화 여부 체크

    void Start()
    {
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

        // 기존 스테이지에서 UI를 비활성화하고, 스페셜 스테이지 UI 활성화
        specialStageUI.SetActive(true);
        Debug.Log("스페셜 스테이지 시작");

        // 스페셜 스테이지에서는 몬스터가 없고, 아이템만 등장함
        DisableMonsters();
        EnableSpecialItems();
    }

    // 스페셜 스테이지 종료
    public void EndSpecialStage()
    {
        isSpecialStageActive = false;

        // 스페셜 스테이지 UI를 비활성화하고, 원래 스테이지 UI 복구
        specialStageUI.SetActive(false);
        Debug.Log("스페셜 스테이지 종료");
    }

    // 몬스터를 비활성화하는 함수
    private void DisableMonsters()
    {
        // 몬스터를 비활성화하기 위한 로직. 예시로 모든 Enemy 오브젝트를 비활성화.
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(false);  // 모든 적을 비활성화
        }

        Debug.Log("스페셜 스테이지에서는 몬스터가 등장하지 않음.");
    }

    // 스페셜 아이템을 등장시키는 함수
    private void EnableSpecialItems()
    {
        // 스페셜 스테이지에 등장할 아이템을 활성화하는 로직
        // 필요한 아이템들을 활성화
        Debug.Log("스페셜 아이템을 등장시킴.");
    }

    // 씬을 이동하는 방법도 고려할 수 있음 (필요시 다른 씬으로 이동)
    public void LoadSpecialStageScene()
    {
        SceneManager.LoadScene("SpecialStageScene"); // 씬 이름에 따라 변경
    }
}
