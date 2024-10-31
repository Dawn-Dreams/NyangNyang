using APIGameServer.Models;

namespace APIGameServer.Services.Interface;

public interface IMailService : IDisposable
{
    public Task<ErrorCode> AddRewardinMailbox(int uid, int type, int reward);

    public Task<ErrorCode> RemoveinMailbox(string uid, int type, int reward);

    public Task<(ErrorCode, List<mail>)> GetMailList(int uid);

}
