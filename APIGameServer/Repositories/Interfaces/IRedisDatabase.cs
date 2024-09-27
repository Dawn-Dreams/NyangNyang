namespace APIGameServer.Repository.Interfaces;


public interface IRedisDatabase : IDisposable
{
    public Task<int> GetNewUserUid();

}
