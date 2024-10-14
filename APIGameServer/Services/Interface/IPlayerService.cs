namespace APIGameServer.Services.Interface;

using APIGameServer.Models;
using ErrorCode = ServerClientShare.ErrorCode;
public interface IPlayerService : IDisposable
{
    public Task<ErrorCode> CreatePlayerTables(int uid);
    public Task<(ErrorCode, PlayerStatusData, PlayerStatusLevelData, PlayerGoodsData)> GetPlayerTable(int uid);


}
