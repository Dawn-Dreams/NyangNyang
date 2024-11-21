using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonHandler : MonoBehaviour
{
    public GameObject popupPanel; // 팝업 패널 오브젝트

    private void Start()
    {
        // 팝업 패널 초기 상태는 비활성화
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("PopupPanel is not assigned in the inspector!");
        }
    }

    private void Update()
    {
        // 뒤로가기 버튼 입력 처리
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleBackButton();
        }
    }

    private void HandleBackButton()
    {
        // 팝업이 이미 활성화된 상태라면 무시
        if (popupPanel.activeSelf)
            return;

        // 팝업 활성화
        ShowPopup();
    }

    public void ShowPopup()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
        }
    }

    public void HidePopup()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }

    public void ConfirmAction()
    {
        // 확인 버튼 동작
        int activeSceneCount = SceneManager.sceneCount;

        if (activeSceneCount > 1)
        {
            // 가장 최근에 열린 씬 닫기
            Scene lastScene = SceneManager.GetSceneAt(activeSceneCount - 1);
            if (lastScene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(lastScene);
            }
        }
        else
        {
            // 마지막 씬에서는 게임 종료
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    public void CancelAction()
    {
        // 취소 버튼 동작: 팝업 닫기
        HidePopup();
    }
}
