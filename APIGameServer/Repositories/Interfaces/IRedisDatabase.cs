using APIGameServer.DTO;

namespace APIGameServer.Repository.Interfaces;


public interface IRedisDatabase : IDisposable
{
    public Task<int> GetNewUserUid();
    public Task<bool> SaveUserNickname(int uid, string nickname);
    public Task<bool> CheckUserUid (int uid);
    public Task<ErrorCode> ChangeNickname(int uid, string oldNickname, string newNickname);

    public Task<ErrorCode> UpdateUserScore(int uid, int score);
    public Task<(ErrorCode, List<RankingData>)> GetRankingTop100();


}
