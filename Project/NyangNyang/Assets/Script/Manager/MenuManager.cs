using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Parent Object for Buttons")]
    public GameObject buttonParentObject; // 버튼들을 담고 있는 부모 오브젝트

    [Header("Panels")]
    public GameObject settingsPanel; // 설정창 패널
    public GameObject noticePanel;   // 공지 패널
    public GameObject rankingPanel;  // 랭킹 패널
    public GameObject boardPanel;    // 게시판 패널

    private Button noticeButton;
    private Button rankingButton;
    private Button boardButton;
    private Button cafeButton;
    private Button settingsButton;

    private void Start()
    {
        // 동적으로 버튼들을 찾음
        FindButtons();

        // 각 버튼에 함수 연결
        noticeButton.onClick.AddListener(OpenNotice);
        rankingButton.onClick.AddListener(OpenRanking);
        boardButton.onClick.AddListener(OpenBoard);
        cafeButton.onClick.AddListener(OpenNaverCafe);
        settingsButton.onClick.AddListener(OpenSettings);

        // 시작 시 패널들 비활성화
        settingsPanel.SetActive(false);
        noticePanel.SetActive(false);
        rankingPanel.SetActive(false);
        boardPanel.SetActive(false);
    }

    // 부모 오브젝트로부터 버튼 찾기
    void FindButtons()
    {
        Button[] buttons = buttonParentObject.GetComponentsInChildren<Button>();

        foreach (Button button in buttons)
        {
            switch (button.gameObject.name)
            {
                case "NoticeButton":
                    noticeButton = button;
                    break;
                case "RankingButton":
                    rankingButton = button;
                    break;
                case "BoardButton":
                    boardButton = button;
                    break;
                case "CafeButton":
                    cafeButton = button;
                    break;
                case "SettingsButton":
                    settingsButton = button;
                    break;
                default:
                    Debug.LogWarning($"{button.gameObject.name}은 등록되지 않은 버튼입니다.");
                    break;
            }
        }
    }

    // 공지글 창 열기 함수
    void OpenNotice()
    {
        Debug.Log("공지글 창 열기");
        // 공지 패널을 활성화
        noticePanel.SetActive(true);
    }

    // 랭킹 창 열기 함수
    void OpenRanking()
    {
        Debug.Log("랭킹 창 열기");
        // 랭킹 패널을 활성화
        rankingPanel.SetActive(true);
    }

    // 게시판 열기 함수
    void OpenBoard()
    {
        Debug.Log("게시판 창 열기");
        // 게시판 패널을 활성화
        boardPanel.SetActive(true);
    }

    // 네이버 카페 연동 함수
    void OpenNaverCafe()
    {
        Debug.Log("네이버 카페 열기");
        // 네이버 카페 URL로 이동
        Application.OpenURL("https://cafe.naver.com/yourcafeurl");
    }

    // 설정창 열기 함수
    void OpenSettings()
    {
        Debug.Log("설정 창 열기");
        // 설정 패널을 활성화
        settingsPanel.SetActive(true);
    }

    // 설정창 닫기 함수
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}
