using APIGameServer.Models;
using APIGameServer.Repositories.Interfaces;
using APIGameServer.Services.Interface;
using SqlKata.Execution;
using System.Security.Cryptography.X509Certificates;

namespace APIGameServer.Services;

public class UserService : IUserService
{
    readonly IDreams_UserInfo _userInfo;
    readonly IMailService _mailService;
    private int[] rewards = new int[30];

    public UserService(IDreams_UserInfo userInfo, IMailService mailService)
    {
        _userInfo = userInfo;
        _mailService = mailService;

        for(int i=0;i<rewards.Length;i++)
        {
            rewards[i] = -1;
        }
    }

    public async Task<ErrorCode> UpdateRecentLoginDateTime(int uid)
    {
        int res = await _userInfo.UpdateRecentLoginCnt(uid);
        if(res == 0)
        {
            //실패임
            
        }
        return ErrorCode.None;

    }

    public async Task<int> CheckAttendenceAndReward(int uid)
    {
        //여기서 출석일수 가져와서 
        int attendence = await _userInfo.GetAttendenceCnt(uid);
 
        if (attendence != 0 && rewards[attendence] >=0 )
        {
            await _mailService.AddRewardinMailbox(uid, (int)MailType.Reward,rewards[attendence]);
        }
        return rewards[attendence];
    }

    public void Dispose()
    {

    }
}
