using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyOptionsServer
{
    // 공지, 우편, 친구, 랭킹, 게시판에 대한 데이터베이스 (현재 데이터는 예시)
    protected static List<NoticeData> NoticeDataList = new List<NoticeData>
    {
        new NoticeData(1, "업데이트 공지", "새로운 업데이트가 적용되었습니다.", "2024-10-13"),
        new NoticeData(2, "이벤트 공지", "할로윈 이벤트가 시작됩니다.", "2024-10-20")
    };

    protected static List<MailData> MailDataList = new List<MailData>
    {
        new MailData(1, "보상 우편", "전투 보상 지급", 1231, "2024-10-12", false),
        new MailData(2, "이벤트 우편", "이벤트 참여 보상", 5322, "2024-10-13", false)
    };

    protected static List<FriendData> FriendDataList = new List<FriendData>
    {
        new FriendData(12323, "고양이친구1", 1110),
        new FriendData(2323, "고양이친구2", 234234),
        new FriendData(23492, "고양이친구3", 13434),
        new FriendData(23232, "고양이친구4", 45466),
        new FriendData(78687, "고양이친구5", 3432),
        new FriendData(3421, "고양이친구6", 232123)
    };

    protected static List<RankingData> RankingDataList = new List<RankingData>
    {
        new RankingData(0808, 1, "고양이유저1", 311000),
        new RankingData(123123, 2, "고양이유저2", 221000),
        new RankingData(28928, 3, "고양이유저3", 111000),
        new RankingData(23928, 4, "고양이유저1", 31000),
        new RankingData(12938, 5, "고양이유저2", 3950)
    };

    protected static List<BoardData> BoardDataList = new List<BoardData>
    {
        new BoardData(1, "첫 번째 게시글", "안녕하세요! 게임이 너무 재밌어요!", "2024-10-11"),
        new BoardData(2, "두 번째 게시글", "좋은 팁 공유합니다.", "2024-10-12")
    };

    // 공지 데이터 전송 함수
    public static List<NoticeData> SendNoticeDataToUser(int userID)
    {
        // 사용자가 접속할 때 공지 데이터를 전송
        return NoticeDataList;
    }

    // 우편 데이터 전송 함수
    public static List<MailData> SendMailDataToUser(int userID)
    {
        // 사용자가 접속할 때 우편 데이터를 전송
        return MailDataList;
    }

    // 친구 데이터 전송 함수
    public static List<FriendData> SendFriendDataToUser(int userID)
    {
        // 사용자가 접속할 때 친구 데이터를 전송
        return FriendDataList;
    }

    // 랭킹 데이터 전송 함수
    public static List<RankingData> SendRankingDataToUser(int userID)
    {
        // 사용자가 접속할 때 랭킹 데이터를 전송
        return RankingDataList;
    }

    // 게시판 데이터 전송 함수
    public static List<BoardData> SendBoardDataToUser(int userID)
    {
        // 사용자가 접속할 때 게시판 데이터를 전송
        return BoardDataList;
    }

    // 우편 수령 처리 함수
    public static void ReceiveMail(int userID, int mailID)
    {
        foreach (var mail in MailDataList)
        {
            if (mail.mailID == mailID && !mail.isReceived)
            {
                mail.isReceived = true;
                Debug.Log($"유저 {userID}가 우편 {mailID}을(를) 수령했습니다.");
                break;
            }
        }
    }

    // 새 공지 추가 함수
    public static void AddNewNotice(string title, string content, string date)
    {
        int newID = NoticeDataList.Count + 1;
        NoticeDataList.Add(new NoticeData(newID, title, content, date));
        Debug.Log($"새 공지 추가: {title}");
    }

    // 새 친구 추가 함수
    public static void AddFriend(int userID, string friendName, int friendLevel)
    {
        int newID = FriendDataList.Count + 1;
        FriendDataList.Add(new FriendData(newID, friendName, friendLevel));
        Debug.Log($"유저 {userID}의 새로운 친구 추가: {friendName}");
    }

    // 랭킹 갱신 함수 (임의로 랭킹 점수 업데이트)
    public static void UpdateRanking(int userID, int newScore)
    {
        foreach (var rank in RankingDataList)
        {
            if (rank.userUID == userID)
            {
                rank.score = newScore;
                Debug.Log($"유저 {userID}의 랭킹 점수 갱신: {newScore}");
                break;
            }
        }
    }

    // 게시글 작성 함수
    public static void AddNewBoardPost(string title, string content, string date)
    {
        int newID = BoardDataList.Count + 1;
        BoardDataList.Add(new BoardData(newID, title, content, date));
        Debug.Log($"새 게시글 추가: {title}");
    }

    // -----------------------Get-----------------------------
    public static List<NoticeData> GetNoticeData()
    {
        return NoticeDataList;
    }

    public static List<RankingData> GetRankingData()
    {
        return RankingDataList;
    }

    public static List<MailData> GetMailData()
    {
        return MailDataList;
    }

    public static List<FriendData> GetFriendData()
    {
        return FriendDataList;
    }

    public static List<BoardData> GetBoardData()
    {
        return BoardDataList;
    }
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
public class FriendData
{
    public int friendUID;
    public string friendName;
    public int friendLevel;

    public FriendData(int uid, string name, int level)
    {
        this.friendUID = uid;
        this.friendName = name;
        this.friendLevel = level;
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
