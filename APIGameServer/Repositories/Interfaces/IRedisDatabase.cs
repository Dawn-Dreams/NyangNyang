namespace APIGameServer.Repository.Interfaces;


public interface IRedisDatabase : IDisposable
{
    public Task<long> GetNewUserUid();

}
