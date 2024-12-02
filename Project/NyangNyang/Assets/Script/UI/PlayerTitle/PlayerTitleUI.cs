using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerTitleUI : MonoBehaviour
{
    public PlayerTitleElement titleElementPrefab;
    private Dictionary<int, PlayerTitleElement> _titleElements = new Dictionary<int, PlayerTitleElement>();
    

    public Transform titleListContentTransform;

    
    void Start()
    {
        OnStartCreateTitleInfoElementObj();

        PlayerTitle.OnOwningTitleChange += SortingTitleElements;
        SortingTitleElements();
    }

    // TitleElement의 자식 순서를 변경하는 함수
    void SortingTitleElements()
    {
        if (_titleElements.Count == 0)
        {
            return;
        }

        // 보유 -> 미보유 / ID 오름차 순으로 정렬
        List<int> currentOwningTitles =  new List<int>(PlayerTitle.playerOwningTitles);

        List<int> notOwningTitles = new List<int>();
        // 추후 NotOwningTitles 추출 방식 변경
        TitleInfo[] titleInfo = TitleDataManager.GetInstance().titleData.title;
        for (int i = 0; i < titleInfo.Length; ++i)
        {
            notOwningTitles.Add(titleInfo[i].id);
        }
        foreach (int owningTitleID in currentOwningTitles)
        {
            notOwningTitles.Remove(owningTitleID);
        }

        int currentOrder = 0;
        foreach (int id in currentOwningTitles)
        {
            _titleElements[id].gameObject.transform.SetSiblingIndex(currentOrder++);
        }
        foreach (int id in notOwningTitles)
        {
            _titleElements[id].gameObject.transform.SetSiblingIndex(currentOrder++);
        }

        titleListContentTransform.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0.0f,-10000.0f,0.0f);

        SetTitleElementsOwning();
    }

    // TitleElement 의 버튼 상태를 미보유/보유중/선택중 으로 바꾸는 함수
    void SetTitleElementsOwning()
    {
        // 모든 타이틀에 대하여 미보유로 바꾼 뒤
        foreach (var titleElement in _titleElements)
        {
            titleElement.Value.SetIsOwning(false);
        }

        // 보유중인 타이틀에 대해서 보유중으로 바꿈, // 내부에서 플레이어가 선택중 임을 확인
        foreach (var titleID in PlayerTitle.playerOwningTitles)
        {
            _titleElements[titleID].SetIsOwning(true);
        }

    }

    // Start 시 TitleElement 오브젝트들을 만드는 함수
    void OnStartCreateTitleInfoElementObj()
    {
        // 타이틀 Info 오브젝트 생성
        TitleInfo[] titleInfo = TitleDataManager.GetInstance().titleData.title;

        for (int i = 0; i < titleInfo.Length; ++i)
        {
            // GM 타이틀, 예외 처리
            if (titleInfo[i].id == 999)
            {
                continue;
            }
            PlayerTitleElement titleElement = Instantiate(titleElementPrefab, titleListContentTransform);
            titleElement.SetTitle(titleInfo[i]);
            _titleElements.Add(titleInfo[i].id, titleElement);
        }

        RectTransform contentRectTransform = titleListContentTransform.gameObject.GetComponent<RectTransform>();
        Vector2 currentContentSize = contentRectTransform.offsetMax;
        float elementSizeY = _titleElements[0].gameObject.GetComponent<RectTransform>().offsetMax.y;
        currentContentSize.y = elementSizeY * (_titleElements.Count + 1);
        contentRectTransform.offsetMax = currentContentSize;

        BindClickEventToSelectTitleButtons();
    }

    // 타이틀 선택 버튼 클릭 이벤트
    void BindClickEventToSelectTitleButtons()
    {
        foreach (var element in _titleElements)
        {
            int titleID = element.Key;
            element.Value.selectTitleButton.onClick.AddListener(()=>OnClickSelectTitleButton(titleID));
        }
    }


    // 타이틀 선택 버튼 클릭시 OnClick 함수
    void OnClickSelectTitleButton(int titleID)
    {
        // 플레이어 타이틀 아이디 교체
        PlayerTitle.PlayerCurrentTitleID = titleID;
        // 서버에 해당 플레이어가 장착중인 타이틀 아이디 변경 요청
        //DummyPlayerTitleServer.UserRequestEquipTitle(Player.GetUserID(), titleID);
        SaveLoadManager.GetInstance().SavePlayerTitleData(new TitleJsonData(){currentSelectedTitle = PlayerTitle.PlayerCurrentTitleID, owningTitles = PlayerTitle.playerOwningTitles});

        // 선택중 UI 변경
        SetTitleElementsOwning();
    }
}
