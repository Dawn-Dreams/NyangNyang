using APIGameServer.Models;
using APIGameServer.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGameServer.Controllers;

[Route("[controller]")]
[ApiController]
public class RegistController : ControllerBase
{
    readonly IRedisDatabase _redis;
    public RegistController(IRedisDatabase redis)
    {  
        _redis = redis; 
    }

    [HttpPost]
    public async Task<ResponseRegist> Create([FromBody] RequestRegist req)
    {
        ResponseRegist res = new ResponseRegist();

        var uid = await _redis.GetNewUserUid();
        if (uid < 0)
        {
            res.Result = ErrorCode.FailRegistByUid;
        }

        //DB에 계정정보 생성할까-> DB연동하고 해결하자.

        res.Uid = uid;
        res.Result = ErrorCode.None;

        return res;

    }
}
