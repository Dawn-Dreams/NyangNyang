using APIGameServer.DTO;
using APIGameServer.Repository.Interfaces;
using CloudStructures;
using CloudStructures.Structures;
using Microsoft.Extensions.Options;
using ServerClientShare;
using StackExchange.Redis;

namespace APIGameServer.Repositories;

public class RedisDatabase : IRedisDatabase
{
    public RedisConnection _redisCon;
    private readonly IDatabase _redisDb;

    public RedisDatabase(IOptions<DBConfig> dbCon)
    {
        RedisConfig config = new("defult", dbCon.Value.Redis);
        _redisCon = new RedisConnection(config);
        _redisDb = _redisCon.GetConnection().GetDatabase();
    }

    public async Task<int> GetNewUserUid()
    {
        var UID = await _redisDb.StringIncrementAsync("uid",1);
        if(UID == 0) { return -1; }
        return (int)UID;

    }
    

    public async Task<ErrorCode> UpdateUserScore(int uid, int score)
    {
        //score 갱신할때 사용하는것이다.
        Console.WriteLine("UID : {0}, SCORE : {1}",uid, score);

        var rankSet = new RedisSortedSet<int>(_redisCon, "player_score_ranking", null);
        var ret = await rankSet.AddAsync(uid, score);

        if (ret == false)
        {
            //이게 맞나?
         
            return ErrorCode.FailConnectDB;
        }
        return ErrorCode.None;


    }

    public async Task<(ErrorCode, List<RankingData>)> GetRankingTopFive()
    {
        List<RankingData> ranking = new();

        var rankSet = new RedisSortedSet<int>(_redisCon, "player_score_ranking", null);

        var rankDatas = await rankSet.RangeByRankWithScoresAsync(0, 5, order: StackExchange.Redis.Order.Descending);

        //닉네임은 어떻게 해결해야하죠?? 흠냐
        //일단 레디스에 {uid : socre}가 저장되어있음
        //레디스에 {uid : nickname}도 저장해놓을까?

        //이부분을 모르겟음 -> score가 같으면 왜>
        int rank = 0;
        int score = -1;
        int count = 0;

        foreach (var rankdata in rankDatas)
        {
            if(rankdata.Score == score) //동점인 경우?
            {
                ++count;
            }
            else
            {
                rank += count;
                count= 1;
                score = (int)rankdata.Score;
            }

            ranking.Add(new RankingData
            {
                rank = rank,
                uid =rankdata.Value,
                nickname = "일단 몰라~",
                score = score,

            });
        
        }

        return (ErrorCode.None, ranking);
    }

    public void Dispose()
    {

    }
}
