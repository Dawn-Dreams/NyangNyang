using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class GooglePlayGamesExampleScript : MonoBehaviour
{
    public string Token;
    public string Error;

    void Awake()
    {
        // PlayGamesPlatform 초기화
        PlayGamesPlatform.Activate();
        LoginGooglePlayGames();
    }

    public void LoginGooglePlayGames()
    {
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Google Play Games 로그인 성공.");
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    Debug.Log("인증 코드: " + code);
                    Token = code; // 이 토큰은 Google Play Games와의 연동에 사용됩니다.
                });
            }
            else
            {
                Error = "Google Play Games 로그인 실패";
                Debug.Log("로그인 실패");
            }
        });
    }
}
