using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelTriggerContent
{
    public ContentAlert content;
    public int contentOpenLevel;
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

        foreach (LevelTriggerContent content in levelTriggerContents)
        {
            if (playerLevel >= content.contentOpenLevel)
            {
                content.content.ChangeAlertState(AlertState.Null);
            }
            else
            {
                content.content.ChangeAlertState(AlertState.Locked);
            }
        }
    }
}
