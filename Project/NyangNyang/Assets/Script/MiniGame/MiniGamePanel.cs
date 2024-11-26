using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MiniGamePanel : MonoBehaviour
{
    public GameObject toggleParentObject;   // Toggle들을 담고 있는 부모 오브젝트
    public GameObject panelParentObject;    // 패널들을 담고 있는 부모 오브젝트

    private Toggle[] toggles;               // 동적으로 찾은 Toggle들을 저장할 배열
    private GameObject[] panels;            // 동적으로 찾은 패널들을 저장할 배열

    private void Start()
    {
        FindTogglesAndPanels();

        // 각 토글에 리스너 추가
        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i;
            toggles[i].onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                {
                    OpenPanel(index);
                }
            });
        }

        // 모든 패널 비활성화 후 첫 번째 패널 활성화
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        if (toggles.Length > 0 && panels.Length > 0)
        {
            toggles[0].isOn = true;       // 첫 번째 토글 활성화
            panels[0].SetActive(true);    // 첫 번째 패널 활성화
        }
        Debug.Log("미니게임 Start()");
    }

    void FindTogglesAndPanels()
    {
        toggles = toggleParentObject.GetComponentsInChildren<Toggle>();
        panels = new GameObject[panelParentObject.transform.childCount];

        for (int i = 0; i < panels.Length; i++)
        {
            panels[i] = panelParentObject.transform.GetChild(i).gameObject;
        }
    }

    // 선택한 패널만 활성화하고 나머지는 비활성화
    void OpenPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == index);  // 선택된 패널만 활성화, 나머지는 비활성화
        }
    }

    // 미니게임 시작 버튼 클릭 시 실행
    public void OnClickMinigameButton(int index)
    {
        // 스페셜 스테이지가 진행 중이면 미니게임을 시작하지 않음
        if (GameManager.isDungeonActive)
        {
            Debug.Log("스페셜 스테이지가 실행 중이므로 미니게임을 시작할 수 없습니다.");
            return;
        }

        // 미니게임이 이미 진행 중인지 확인
        if (GameManager.isMiniGameActive)
        {
            Debug.Log("다른 미니게임이 이미 실행 중입니다.");
            return;
        }


        switch (index)
        {
            case 0:
                SceneManager.LoadScene("MiniGame1", LoadSceneMode.Additive);
                Debug.Log("미니게임 1 시작버튼클릭");
                GameManager.isMiniGameActive = true;
                break;
            case 1:
                // FindObjectOfType<MiniGame2>().StartGame();
                Debug.Log("미니게임 2 시작버튼클릭");
                break;
            case 2:
                // FindObjectOfType<MiniGame3>().StartGame();
                Debug.Log("미니게임 3 시작버튼클릭");
                break;
            default:
                Debug.LogWarning("올바르지 않은 인덱스입니다.");
                break;
        }
    }
}
