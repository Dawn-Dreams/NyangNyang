using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankUI : MonoBehaviour
{
    public GameObject toggleParentObject;   // Toggle들을 담고 있는 부모 오브젝트
    public GameObject panelParentObject;    // 패널들을 담고 있는 부모 오브젝트

    public GameObject rankUserButtonPrefab; // 랭킹 버튼 프리팹

    private Toggle[] toggles;               // 동적으로 찾은 Toggle들을 저장할 배열
    private GameObject[] panels;            // 동적으로 찾은 패널들을 저장할 배열
    
    public GameObject rankingPopup; // 랭킹 팝업
    private GameObject currentPopup; // 현재 활성화된 팝업

    private void OnEnable()
    {
        InitializeRankUI();
    }
    private void OnDisable()
    {
        InitializeRankUI();
    }

    private void InitializeRankUI()
    {
        FindTogglesAndPanels();

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

        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        // 0번 패널 활성화 및 데이터 초기화
        if (toggles.Length > 0 && panels.Length > 0)
        {
            toggles[0].isOn = true;        // 0번 토글 활성화
            panels[0].SetActive(true);    // 0번 패널 활성화
            OpenRankingPanel(0);          // 0번 패널 데이터 초기화
        }
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

    void OpenPanel(int index)
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        if (index >= 0 && index < panels.Length)
        {
            panels[index].SetActive(true);
            CallPanelFunction(index);
        }
        else
        {
        }
    }

    void CallPanelFunction(int index)
    {
        OpenRankingPanel(index);
    }

    // 랭킹 + index로 구분(0:메인스테이지/ 1,2,3:던전1,2,3/ 4:미니게임)
    void OpenRankingPanel(int index)
    {
        // 랭킹 데이터 로드
        List<RankingData> rankList = SaveLoadManager._instance.LoadRankings();
        GameObject contentObj = GameObject.Find($"RankUI ({index})/Viewport/Content");

        if (contentObj == null)
        {
            Debug.LogError($"RankUI ({index})/Viewport/Content 오브젝트를 찾을 수 없습니다. 계층 구조를 확인하세요.");
            return;
        }

        // 기존에 생성된 요소들을 모두 제거
        foreach (Transform child in contentObj.transform)
        {
            Destroy(child.gameObject);
        }

        if (rankList.Count > 0)
        {
            foreach (RankingData rankData in rankList)
            {
                // 랭킹 항목 버튼 생성
                GameObject rankUserButton = Instantiate(rankUserButtonPrefab, contentObj.transform);

                TMP_Text rankNumberText = rankUserButton.transform.Find("RankNumber").GetComponent<TMP_Text>();
                TMP_Text rankUserNameText = rankUserButton.transform.Find("RankUserName").GetComponent<TMP_Text>();
                TMP_Text rankScoreText = rankUserButton.transform.Find("RankScore").GetComponent<TMP_Text>();

                // 데이터 적용
                rankNumberText.text = rankData.rank.ToString(); // 순위
                rankUserNameText.text = rankData.userName;      // 유저 이름
                rankScoreText.text = rankData.score.ToString(); // 점수

                // 클릭 이벤트 추가
                Button rankButtonComponent = rankUserButton.GetComponent<Button>();
                if (rankButtonComponent != null)
                {
                    rankButtonComponent.onClick.AddListener(() => ShowRankingPopup(rankData));
                }
            }
        }
        else
        {
            Debug.LogWarning("랭킹 데이터가 없습니다.");
        }
    }


    // 랭킹 팝업 열기 메서드
    void ShowRankingPopup(RankingData rankData)
    {
        // 기존 팝업 닫기
        if (currentPopup != null)
        {
            Destroy(currentPopup);
        }

        // 팝업 생성
        currentPopup = Instantiate(rankingPopup, transform);

        // 팝업 데이터 설정
        TMP_Text rankNumberText = currentPopup.transform.Find("RankNumberText").GetComponent<TMP_Text>();
        TMP_Text userIDText = currentPopup.transform.Find("UserIDText").GetComponent<TMP_Text>();
        TMP_Text userNameText = currentPopup.transform.Find("UserNameText").GetComponent<TMP_Text>();
        TMP_Text scoreText = currentPopup.transform.Find("ScoreText").GetComponent<TMP_Text>();
        Button closeButton = currentPopup.transform.Find("CloseButton").GetComponent<Button>();

        rankNumberText.text = $"{rankData.rank}위";
        userIDText.text = $"ID : {rankData.userUID}";
        userNameText.text = $"닉네임 {rankData.userName}";
        scoreText.text = $"Score {rankData.score}";

        // 닫기 버튼 이벤트 설정
        closeButton.onClick.AddListener(() =>
        {
            Destroy(currentPopup);
            currentPopup = null;
        });
    }
 
}
