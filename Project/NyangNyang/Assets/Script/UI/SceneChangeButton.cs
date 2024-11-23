using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeButton : MonoBehaviour
{
    public string sceneName; // 전환할 씬 이름
    private Button _sceneChangeButton;

    private void Start()
    {
        if (_sceneChangeButton == null)
        {
            _sceneChangeButton = GetComponent<Button>();
            _sceneChangeButton.onClick.AddListener(ChangeScene);
        }
        // 배경 음악 시작
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBGM(0);
        }
    }

    private void OnEnable()
    {
        if (_sceneChangeButton == null)
        {
            _sceneChangeButton = GetComponent<Button>();
            _sceneChangeButton.onClick.AddListener(ChangeScene);
        }

        // 배경 음악 시작
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBGM(0);
        }
    }

    void ChangeScene()
    {
        StartCoroutine(LoadSceneWithMusicControl(sceneName));
    }

    private IEnumerator LoadSceneWithMusicControl(string sceneName)
    {
        // 배경 음악 멈춤
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PauseBGM();
        }

        // 씬을 비동기적으로 로드
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        // 로딩 진행 확인
        while (operation.progress < 0.9f)
        {
            yield return null; // 다음 프레임까지 대기
        }

        // 씬 로드 완료 후 활성화
        operation.allowSceneActivation = true;

        
        AudioManager.Instance.PlayMainBGM();

    }
}
