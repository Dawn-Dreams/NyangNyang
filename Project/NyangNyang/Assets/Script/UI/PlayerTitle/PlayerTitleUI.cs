using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTitleUI : MonoBehaviour
{
    public PlayerTitleElement titleElementPrefab;
    private Dictionary<int, PlayerTitleElement> _titleElements = new Dictionary<int, PlayerTitleElement>();
    

    public Transform titleListContentTransform;

    
    void Start()
    {
        TitleInfo[] titleInfo = TitleDataManager.GetInstance().titleData.title;
        

        for (int i = 0; i < titleInfo.Length; ++i)
        {
            PlayerTitleElement titleElement = Instantiate(titleElementPrefab, titleListContentTransform);
            titleElement.SetTitle(titleInfo[i]);
            _titleElements.Add(titleInfo[i].id, titleElement);
        }

        RectTransform contentRectTransform = titleListContentTransform.gameObject.GetComponent<RectTransform>();
        Vector2 currentContentSize = contentRectTransform.offsetMax;
        float elementSizeY = _titleElements[0].gameObject.GetComponent<RectTransform>().offsetMax.y;
        currentContentSize.y = elementSizeY * (_titleElements.Count+1);
        contentRectTransform.offsetMax = currentContentSize;

        SortingTitleElements();
    }

    void SortingTitleElements()
    {
        // 보유 -> 미보유 / ID 오름차 순으로 정렬
        int[] currentOwningTitles =  Player.PlayerOwningTitles;

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
            _titleElements[id].SetIsOwning(true);
        }
        foreach (int id in notOwningTitles)
        {
            _titleElements[id].gameObject.transform.SetSiblingIndex(currentOrder++);
            _titleElements[id].SetIsOwning(false);
        }
    }
}
