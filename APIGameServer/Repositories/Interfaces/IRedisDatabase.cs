using APIGameServer.DTO;

namespace APIGameServer.Repository.Interfaces;


public interface IRedisDatabase : IDisposable
{
    public Task<int> GetNewUserUid();
    public  Task<ErrorCode> UpdateUserScore(int uid, int score);
    public Task<(ErrorCode, List<RankingData>)> GetRankingTopFive();


}
