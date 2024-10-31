using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestMainObject : MonoBehaviour
{
    [SerializeField] private RectTransform thisObjRectTransform;
    [SerializeField] private List<RectTransform> questListRectTransforms;
    [SerializeField] private List<Transform> questListContentTransforms;

    [SerializeField] private Button startQuestListSelectButton;
    [SerializeField] private HideUi hideUI;

    [SerializeField] private BaseQuest questPanelPrefab;

    private void Start()
    {
        CreateQuestPanels();
        SetInActiveQuestUIAtStart();


    }

    private void CreateQuestPanels()
    {
        for (int i = 0; i < questListRectTransforms.Count; ++i)
        {
            foreach (QuestDataBase questData in QuestDataManager.GetInstance().questDataList[i])
            {
                BaseQuest questPanel = Instantiate(questPanelPrefab, questListContentTransforms[i]);
                questPanel.LoadQuest(questData);
            }
        }
    }

    void SetInActiveQuestUIAtStart()
    {
        thisObjRectTransform.offsetMin = thisObjRectTransform.offsetMax = new Vector2(0, 0);

        foreach (RectTransform rectTransform in questListRectTransforms)
        {
            rectTransform.offsetMin = rectTransform.offsetMax = new Vector2(0, 0);
        }
        
        startQuestListSelectButton.onClick.Invoke();
        hideUI.HideUIInVisible();
    }

}
