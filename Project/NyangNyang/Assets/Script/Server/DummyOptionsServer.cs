using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyOptionsServer
{
    // ����, ����, ģ��, ��ŷ, �Խ��ǿ� ���� �����ͺ��̽� (���� �����ʹ� ����)
    protected static List<NoticeData> NoticeDataList = new List<NoticeData>
    {
        new NoticeData(1, "������Ʈ ����", "���ο� ������Ʈ�� ����Ǿ����ϴ�.", "2024-10-13"),
        new NoticeData(2, "�̺�Ʈ ����", "�ҷ��� �̺�Ʈ�� ���۵˴ϴ�.", "2024-10-20")
    };

    protected static List<MailData> MailDataList = new List<MailData>
    {
        new MailData(1, "���� ����", "���� ���� ����", "2024-10-12", false),
        new MailData(2, "�̺�Ʈ ����", "�̺�Ʈ ���� ����", "2024-10-13", false)
    };

    protected static List<FriendData> FriendDataList = new List<FriendData>
    {
        new FriendData(1, "�����ģ��1", 10),
        new FriendData(2, "�����ģ��2", 15)
    };

    protected static List<RankingData> RankingDataList = new List<RankingData>
    {
        new RankingData(1, "���������1", 1000),
        new RankingData(2, "���������2", 950)
    };

    protected static List<BoardData> BoardDataList = new List<BoardData>
    {
        new BoardData(1, "ù ��° �Խñ�", "�ȳ��ϼ���! ������ �ʹ� ��վ��!", "2024-10-11"),
        new BoardData(2, "�� ��° �Խñ�", "���� �� �����մϴ�.", "2024-10-12")
    };

    // ���� ������ ���� �Լ�
    public static List<NoticeData> SendNoticeDataToUser(int userID)
    {
        // ����ڰ� ������ �� ���� �����͸� ����
        return NoticeDataList;
    }

    // ���� ������ ���� �Լ�
    public static List<MailData> SendMailDataToUser(int userID)
    {
        // ����ڰ� ������ �� ���� �����͸� ����
        return MailDataList;
    }

    // ģ�� ������ ���� �Լ�
    public static List<FriendData> SendFriendDataToUser(int userID)
    {
        // ����ڰ� ������ �� ģ�� �����͸� ����
        return FriendDataList;
    }

    // ��ŷ ������ ���� �Լ�
    public static List<RankingData> SendRankingDataToUser(int userID)
    {
        // ����ڰ� ������ �� ��ŷ �����͸� ����
        return RankingDataList;
    }

    // �Խ��� ������ ���� �Լ�
    public static List<BoardData> SendBoardDataToUser(int userID)
    {
        // ����ڰ� ������ �� �Խ��� �����͸� ����
        return BoardDataList;
    }

    // ���� ���� ó�� �Լ�
    public static void ReceiveMail(int userID, int mailID)
    {
        foreach (var mail in MailDataList)
        {
            if (mail.mailID == mailID && !mail.isReceived)
            {
                mail.isReceived = true;
                Debug.Log($"���� {userID}�� ���� {mailID}��(��) �����߽��ϴ�.");
                break;
            }
        }
    }

    // �� ���� �߰� �Լ�
    public static void AddNewNotice(string title, string content, string date)
    {
        int newID = NoticeDataList.Count + 1;
        NoticeDataList.Add(new NoticeData(newID, title, content, date));
        Debug.Log($"�� ���� �߰�: {title}");
    }

    // �� ģ�� �߰� �Լ�
    public static void AddFriend(int userID, string friendName, int friendLevel)
    {
        int newID = FriendDataList.Count + 1;
        FriendDataList.Add(new FriendData(newID, friendName, friendLevel));
        Debug.Log($"���� {userID}�� ���ο� ģ�� �߰�: {friendName}");
    }

    // ��ŷ ���� �Լ� (���Ƿ� ��ŷ ���� ������Ʈ)
    public static void UpdateRanking(int userID, int newScore)
    {
        foreach (var rank in RankingDataList)
        {
            if (rank.userID == userID)
            {
                rank.score = newScore;
                Debug.Log($"���� {userID}�� ��ŷ ���� ����: {newScore}");
                break;
            }
        }
    }

    // �Խñ� �ۼ� �Լ�
    public static void AddNewBoardPost(string title, string content, string date)
    {
        int newID = BoardDataList.Count + 1;
        BoardDataList.Add(new BoardData(newID, title, content, date));
        Debug.Log($"�� �Խñ� �߰�: {title}");
    }
}

// ������ ����ü (����, ����, ģ��, ��ŷ, �Խ���)
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
    public string date;
    public bool isReceived;

    public MailData(int id, string title, string content, string date, bool isReceived)
    {
        this.mailID = id;
        this.title = title;
        this.content = content;
        this.date = date;
        this.isReceived = isReceived;
    }
}

[System.Serializable]
public class FriendData
{
    public int friendID;
    public string friendName;
    public int friendLevel;

    public FriendData(int id, string name, int level)
    {
        this.friendID = id;
        this.friendName = name;
        this.friendLevel = level;
    }
}

[System.Serializable]
public class RankingData
{
    public int userID;
    public string userName;
    public int score;

    public RankingData(int id, string name, int score)
    {
        this.userID = id;
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
