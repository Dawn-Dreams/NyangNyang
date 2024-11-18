using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class LevelTriggerContent
{
    public ContentAlert content;
    public int contentOpenLevel;
    public Image contentImage;
    public string contentName;
}

public class LevelTriggerManager : MonoBehaviour
{
    public List<LevelTriggerContent> levelTriggerContents;

    void Start()
    {
        if (Player.UserLevel)
        {
            Player.UserLevel.OnExpChange += CheckOpenContent;
            CheckOpenContent(Player.UserLevel);
        }
        
    }

    void CheckOpenContent(UserLevelData levelData)
    {
        int playerLevel = levelData.currentLevel;

        for (int index = 0; index < levelTriggerContents.Count; ++index)
        {
            LevelTriggerContent content = levelTriggerContents[index];
            
            if (playerLevel >= content.contentOpenLevel)
            {
                content.content.ChangeAlertState(AlertState.Null);
                PlayerLevelUpUI.GetInstance().AddNewContent(content.contentImage.sprite, content.contentName);
                levelTriggerContents.RemoveAt(index);
                --index;
                continue;
            }
            else
            {
                content.content.ChangeAlertState(AlertState.Locked);
            }
        }
    }
}
