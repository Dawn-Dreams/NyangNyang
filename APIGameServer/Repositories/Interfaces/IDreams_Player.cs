using APIGameServer.DTO;
using APIGameServer.Models;

namespace APIGameServer.Repositories.Interfaces;

public interface IDreams_Player : IDisposable
{
    public Task<int> CreatePlayerStat(int uid);
    public Task<int> CreatePlayerStatusLevel(int uid);
    public Task<int> CreatePlayerGoods(int uid);

    public Task<PlayerStatusData> GetPlayerStatus(int uid);
    public Task<PlayerStatusLevelData> GetPlayerStatusLevel(int uid);
    public Task<PlayerGoodsData> GetPlayerGoods(int uid);


    public Task<int> UpdatePlayerStatus(PlayerStatusData data);
    public Task<int> UpdatePlayerStatusLevel(PlayerStatusLevelData data);
    public Task<int> UpdatePlayerGoods(PlayerGoodsData data);

}
