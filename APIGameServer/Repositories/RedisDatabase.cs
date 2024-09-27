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
    public void Dispose()
    {

    }
}
