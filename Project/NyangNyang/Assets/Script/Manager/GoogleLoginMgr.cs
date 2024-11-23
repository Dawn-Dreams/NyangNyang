using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class GooglePlayGamesExampleScript : MonoBehaviour
{
    public string Token;
    public string Error;

    void Awake()
    {
        // PlayGamesPlatform �ʱ�ȭ
        PlayGamesPlatform.Activate();
        LoginGooglePlayGames();
    }

    public void LoginGooglePlayGames()
    {
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Google Play Games �α��� ����.");
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    Debug.Log("���� �ڵ�: " + code);
                    Token = code; // �� ��ū�� Google Play Games���� ������ ���˴ϴ�.
                });
            }
            else
            {
                Error = "Google Play Games �α��� ����";
                Debug.Log("�α��� ����");
            }
        });
    }
}
