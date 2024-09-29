namespace APIGameServer.Services.Interface;

using ErrorCode = ServerClientShare.ErrorCode;
public interface IPlayerService : IDisposable
{
    public Task<ErrorCode> CreatePlayerTables(int uid);

}
