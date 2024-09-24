using APIGameServer.Models;

namespace APIGameServer.Repositories.Interfaces;

public interface IDreams_PlayerStat : IDisposable
{
    public Task<ErrorCode> GetPlayerStat(int uid);

    public Task<ErrorCode> UpdatePlayerStat(PlayerStatData data);
}
