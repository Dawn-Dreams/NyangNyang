using UnityEngine;

public class DeepLinkHandler : MonoBehaviour
{
    private void Start()
    {
        // ���� ���� ���� �� �� ��ũ URL Ȱ��ȭ �̺�Ʈ ó��
        Application.deepLinkActivated += OnDeepLinkActivated;

        // ���� ���� �� �̹� ���޵� URL Ȯ��
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
        // URL �Ľ�
        System.Uri uri = new System.Uri(url);
        string scheme = uri.Scheme;      // "dawndreams"
        string host = uri.Host;          // "nyangnyang"
        string path = uri.AbsolutePath;  // "/TitleScene" ��

        // �� ��ũ ���� Ȯ��
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
        // TitleScene �ε� ���� (��: SceneManager ���)
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
    }
}
