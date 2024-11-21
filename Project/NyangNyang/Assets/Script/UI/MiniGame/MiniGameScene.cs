using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameScene : MonoBehaviour
{
    [SerializeField]
    private Button deleteButton;

    void Start()
    {
        // 버튼 생성 및 설정
        CreateDeleteButton();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateDeleteButton()
    {
        if (deleteButton != null)
            deleteButton.onClick.AddListener(UnloadMiniGame1Scene);
    }

    // 씬 삭제 메서드
    public void UnloadMiniGame1Scene()
    {
        // 씬 이름이 "MiniGame1"인 경우 언로드 진행
        if (SceneManager.GetSceneByName("MiniGame1").isLoaded)
        {
            SceneManager.UnloadSceneAsync("MiniGame1");
            GameManager.isMiniGameActive = false;
        }
        else
        {
            Debug.LogWarning("MiniGame1 씬이 로드되어 있지 않습니다.");
        }
    }
}
