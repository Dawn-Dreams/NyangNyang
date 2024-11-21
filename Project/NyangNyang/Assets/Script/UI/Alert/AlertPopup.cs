using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public GameObject popupPanel; // 팝업 패널 오브젝트

    void Start()
    {
        // 팝업 초기 상태는 비활성화
        popupPanel.SetActive(false);
    }

    public void ShowPopup()
    {
        // 팝업 활성화
        popupPanel.SetActive(true);
    }

    public void HidePopup()
    {
        // 팝업 비활성화
        popupPanel.SetActive(false);
    }

    public void ConfirmAction()
    {
        // 확인 버튼 동작: 게임 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void CancelAction()
    {
        // 취소 버튼 동작: 팝업 닫기
        HidePopup();
    }
}
