using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DummyOptionsServer
{
   
}

// 데이터 구조체 (공지, 우편, 친구, 랭킹-해결, 게시판)
[System.Serializable]
public class NoticeData
{
    public int noticeID;
    public string title;
    public string content;
    public string date;

    public NoticeData(int id, string title, string content, string date)
    {
        this.noticeID = id;
        this.title = title;
        this.content = content;
        this.date = date;
    }
}

[System.Serializable]
public class MailData
{
    public int mailID;
    public string title;
    public string content;
    public int itemID;
    public string date;
    public bool isReceived;

    public MailData(int id, string title, string content, int itemID, string date, bool isReceived)
    {
        this.mailID = id;
        this.title = title;
        this.content = content;
        this.itemID = itemID;
        this.date = date;
        this.isReceived = isReceived;
    }
}

[System.Serializable]
public class DungeonData
{
    public int dungeonLevel1;
    public int dungeonLevel2;
    public int dungeonLevel3;

    public DungeonData(int level1, int level2, int level3)
    {
        this.dungeonLevel1 = level1;
        this.dungeonLevel2 = level2;
        this.dungeonLevel3 = level3;
    }
}

[System.Serializable]
public class RankingData
{
    public int userUID;
    public int rank;
    public string userName;
    public int score;

    public RankingData(int uid, int rank, string name, int score)
    {
        this.userUID = uid;
        this.rank = rank;
        this.userName = name;
        this.score = score;
    }
}

[System.Serializable]
public class BoardData
{
    public int postID;
    public string title;
    public string content;
    public string date;

    public BoardData(int id, string title, string content, string date)
    {
        this.postID = id;
        this.title = title;
        this.content = content;
        this.date = date;
    }
}
