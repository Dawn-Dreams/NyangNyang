using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuManager : MonoBehaviour
{
    public GameObject buttonParentObject;   // 버튼들을 담고 있는 부모 오브젝트
    public GameObject panelParentObject;    // 패널들을 담고 있는 부모 오브젝트

    // 버튼과 패널의 순서(index)가 대응되어야 작동
    private Button[] buttons;               // 동적으로 찾은 버튼들을 저장할 배열
    private GameObject[] panels;            // 동적으로 찾은 패널들을 저장할 배열

    private void Start()
    {
        FindButtonsAndPanels();

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => OpenPanel(index));
        }

        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    void FindButtonsAndPanels()
    {
        buttons = buttonParentObject.GetComponentsInChildren<Button>();
        panels = new GameObject[panelParentObject.transform.childCount];

        for (int i = 0; i < panels.Length; i++)
        {
            panels[i] = panelParentObject.transform.GetChild(i).gameObject;
        }
    }

    // 고유 함수 호출 및 패널 열기
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
    }

    // 각 패널마다 고유한 함수 호출
    void CallPanelFunction(int index)
    {
        switch (index)
        {
            case 0:
                OpenNoticePanel();
                break;
            case 1:
                OpenRankingPanel();
                break;
            case 2:
                OpenBulletinBoardPanel();
                break;
            case 3:
                OpenCommunityPanel();
                panels[index].SetActive(false);
                break;
            case 4:
                OpenSettingsPanel();
                break;
            case 5:
                OpenFriendsPanel();
                break;
            case 6:
                OpenMessagePanel();
                break;
            default:
                Debug.LogWarning("해당 인덱스에 대한 고유 함수가 없습니다.");
                break;
        }
    }

    // 패널 고유 함수들
    void OpenNoticePanel()
    {
        Debug.Log("공지 패널이 열렸습니다.");
    }

    void OpenRankingPanel()
    {
        Debug.Log("랭킹 패널이 열렸습니다.");
    }

    void OpenBulletinBoardPanel()
    {
        Debug.Log("게시판 패널이 열렸습니다.");
    }

    void OpenCommunityPanel()
    {
        Application.OpenURL("https://cafe.naver.com/yourcafeurl");
    }

    void OpenSettingsPanel()
    {
        Debug.Log("설정 패널이 열렸습니다.");
    }

    void OpenFriendsPanel()
    {
        Debug.Log("친구 패널이 열렸습니다.");
    }

    void OpenMessagePanel()
    {
        Debug.Log("우편 패널이 열렸습니다.");
    }
}
