using APIGameServer.DTO;
using APIGameServer.Repository.Interfaces;
using CloudStructures;
using CloudStructures.Structures;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace APIGameServer.Repositories;

public class RedisDatabase : IRedisDatabase
{
    public RedisConnection _redisCon;
    private readonly IDatabase _redisDb;
    private string allnicknameKey = "nicknames";

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

    public async Task<bool> SaveUserNickname(int uid, string nickname)
    {
        if(await _redisDb.StringSetAsync(uid.ToString(), nickname))
        {
            return await _redisDb.SetAddAsync(allnicknameKey, nickname);
        }

        return false;
    }

    public async Task<ErrorCode> ChangeNickname(int uid,string oldNickname, string newNickname)
    {
        //먼저 있는지를 찾아보자.
        if(await _redisDb.SetContainsAsync(allnicknameKey, newNickname))
        {
            //사용중이다. 사용중이라는 에러값 내보내면될듯
        }

        //없다면 이제 기존값을 삭제하고 새롭게 추가하자.
        var res = await _redisDb.SetRemoveAsync(allnicknameKey, oldNickname);
        res =await SaveUserNickname(uid, newNickname);

        return ErrorCode.None;
    }

    public async Task<bool> CheckUserUid(int uid)
    {
        return await _redisDb.KeyExistsAsync(uid.ToString());
    }
    public async Task<ErrorCode> UpdateUserScore(int uid, int score)
    {
        //score 갱신할때 사용하는것이다.
        Console.WriteLine("UID : {0}, SCORE : {1}",uid, score);

        var rankSet = new RedisSortedSet<int>(_redisCon, "player_score_ranking", null);
        var ret = await rankSet.AddAsync(uid, score);

        if (ret == false)
        {
        
            return ErrorCode.FailConnectDB;
        }
        return ErrorCode.None;


    }

    public async Task<(ErrorCode, List<RankingData>)> GetRankingTop100()
    {
        List<RankingData> ranking = new();

        var rankSet = new RedisSortedSet<int>(_redisCon, "player_score_ranking", null);

        var rankDatas = await rankSet.RangeByRankWithScoresAsync(0, 100, order: StackExchange.Redis.Order.Descending);

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
            string nickname = await _redisDb.StringGetAsync(rankdata.Value.ToString());

            ranking.Add(new RankingData
            {
                rank = rank,
                uid =rankdata.Value,

                nickname = nickname,
                score = score,

            });
        
        }

        return (ErrorCode.None, ranking);
    }

    public void Dispose()
    {

    }
}
