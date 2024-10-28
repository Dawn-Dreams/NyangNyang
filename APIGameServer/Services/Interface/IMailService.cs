namespace APIGameServer.Services.Interface;

public interface IMailService : IDisposable
{
    public Task<ErrorCode> AddRewardinMailbox(int uid, int type, int reward);

}
