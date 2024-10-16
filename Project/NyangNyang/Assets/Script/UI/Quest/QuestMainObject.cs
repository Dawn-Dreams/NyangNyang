using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestMainObject : MonoBehaviour
{
    [SerializeField] private RectTransform thisObjRectTransform;
    [SerializeField] private List<RectTransform> questListRectTransforms;

    [SerializeField] private Button startQuestListSelectButton;
    private void Start()
    {
        SetInActiveQuestUIAtStart();
        
    }

    void SetInActiveQuestUIAtStart()
    {
        //yield return null;

        thisObjRectTransform.offsetMin = thisObjRectTransform.offsetMax = new Vector2(0, 0);

        foreach (RectTransform rectTransform in questListRectTransforms)
        {
            rectTransform.offsetMin = rectTransform.offsetMax = new Vector2(0, 0);
        }
        
        startQuestListSelectButton.onClick.Invoke();
        gameObject.SetActive(false);
    }

}
