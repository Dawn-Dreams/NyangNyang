using UnityEngine;

public class DeepLinkHandler : MonoBehaviour
{
    private void Start()
    {
        // 앱이 실행 중일 때 딥 링크 URL 활성화 이벤트 처리
        Application.deepLinkActivated += OnDeepLinkActivated;

        // 앱이 시작 시 이미 전달된 URL 확인
        if (!string.IsNullOrEmpty(Application.absoluteURL))
        {
            Debug.Log("App launched with deep link: " + Application.absoluteURL);
            HandleDeepLink(Application.absoluteURL);
        }
    }

    private void OnDeepLinkActivated(string url)
    {
        Debug.Log("Deep link activated: " + url);
        HandleDeepLink(url);
    }

    private void HandleDeepLink(string url)
    {
        // URL 파싱
        System.Uri uri = new System.Uri(url);
        string scheme = uri.Scheme;      // "dawndreams"
        string host = uri.Host;          // "nyangnyang"
        string path = uri.AbsolutePath;  // "/TitleScene" 등

        // 딥 링크 조건 확인
        if (scheme == "dawndreams" && host == "nyangnyang" && path == "/TitleScene")
        {
            OpenTitleScene();
        }
        else
        {
            Debug.LogWarning("Unhandled deep link: " + url);
        }
    }

    private void OpenTitleScene()
    {
        Debug.Log("Opening TitleScene...");
        // TitleScene 로딩 로직 (예: SceneManager 사용)
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
    }
}
