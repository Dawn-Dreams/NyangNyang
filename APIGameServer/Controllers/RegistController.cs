using APIGameServer.DTO;
using APIGameServer.Repositories.Interfaces;
using APIGameServer.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGameServer.Controllers;

[Route("[controller]")]
[ApiController]
public class RegistController : ControllerBase
{
    readonly IRedisDatabase _redis;
    readonly IDreams_UserInfo _userInfo;
    public RegistController(IRedisDatabase redis, IDreams_UserInfo userInfo)
    {  
        _redis = redis;
        _userInfo = userInfo;

    }

    [HttpPost]
    public async Task<ResponseRegist> Create([FromBody] RequestRegist req)
    {
        ResponseRegist res = new ResponseRegist();

        var uid = await _redis.GetNewUserUid();
        if (uid < 0)
        {
            res.Result = ErrorCode.FailRegistByUid;
            //uid 발급 실패
            return res;
        }
       
        //DB에 계정정보 생성할까-> DB연동하고 해결하자.
        //user_info에 db정보 생성해야한다.
        var nickname = "nyang" + uid;
        var cnt = await _userInfo.CreateUserInfo(uid, nickname);
        
        if(cnt == 0)
        {
            //db에 저장이 안된거임 오류를 리턴해야한다.
            //오류코드를 추가해야한다.
            res.Result = ErrorCode.FailRegistByUid;
            return res;

        }

        Console.WriteLine(nickname);

        //cnt리턴값이 과연 무엇일가 성공한 쿼리cnt?
        Console.WriteLine(cnt);


        //닉네임도 보내야하나...?흠냐리~~ 
        //초기닉네임은 uid만 붙힌거라 uid만 보내도된다고 판단하긴했눈데 쩝
        res.Uid = uid;
        res.Result = ErrorCode.None;

        return res;

    }
}
