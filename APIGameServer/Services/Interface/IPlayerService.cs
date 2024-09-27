namespace APIGameServer.Services.Interface;

public interface IPlayerService : IDisposable
{
    public Task<ErrorCode> CreatePlayerTables(int uid);

}
