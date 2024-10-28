namespace APIGameServer.Services.Interface;

public interface IUserService :  IDisposable
{
    public Task<ErrorCode> UpdateRecentLoginDateTime(int uid);
    public Task<int> CheckAttendenceAndReward(int uid);


}
