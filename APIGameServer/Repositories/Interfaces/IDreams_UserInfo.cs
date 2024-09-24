namespace APIGameServer.Repositories.Interfaces;

public interface IDreams_UserInfo : IDisposable
{
    public Task<int> CreateUserInfo(long uid, string nickname);

    public Task<int> ChangeNickname(long uid, string changeNickname);
}
