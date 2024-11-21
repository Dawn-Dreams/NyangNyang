using APIGameServer.Models;
using APIGameServer.Repositories.Interfaces;
using APIGameServer.Repository.Interfaces;
using APIGameServer.Services.Interface;
using SqlKata.Execution;
using System.Security.Cryptography.X509Certificates;

namespace APIGameServer.Services;

public class UserService : IUserService
{
    readonly IDreams_UserInfo _userInfo;
    readonly IRedisDatabase _redis;
    readonly IMailService _mailService;
    private int[] rewards = new int[30];

    public UserService(IDreams_UserInfo userInfo, IMailService mailService,IRedisDatabase redis)
    {
        _userInfo = userInfo;
        _mailService = mailService;
        _redis = redis;

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
            await _mailService.AddRewardinMailbox(uid, (int)mailType.Reward,rewards[attendence]);
        }
        return rewards[attendence];
    }

    public async Task<ErrorCode> ChanegeUserNickname(int uid, string olNicknamed, string newNickname)
    {
       var res = await _redis.ChangeNickname(uid, olNicknamed, newNickname);

        if(res!= ErrorCode.None)
        {
            return res;
        }

        //이제 DB에도 해야한다.
        if(await _userInfo.ChangeNickname(uid, newNickname) == 0)
        {
            //문제발생
        }

        return ErrorCode.None;
    }


    public void Dispose()
    {

    }
}
