using APIGameServer.DTO;
using APIGameServer.Repositories.Interfaces;
using APIGameServer.Repository.Interfaces;
using APIGameServer.Services.Interface;

using APIGameServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGameServer.Controllers;

[Route("[controller]")]
[ApiController]
public class RegistController : ControllerBase
{
    readonly IRedisDatabase _redis;
    readonly IDreams_UserInfo _userInfo;

    readonly IPlayerService _playerServiece;

    public RegistController(IRedisDatabase redis, IDreams_UserInfo userInfo, IPlayerService playerService)
    {  
        _redis = redis;
        _userInfo = userInfo;
        _playerServiece = playerService;

    }

    [HttpPost]
    public async Task<ResponseRegist> Create([FromBody] RequestRegist req)
    {
        ResponseRegist response = new ResponseRegist();

        var uid = await _redis.GetNewUserUid();
        if (uid < 0)
        {
            response.Result = ServerClientShare.ErrorCode.FailRegistByUid;
            return response;
        }

        response.Uid = uid;

        var nickname = "nyang" + uid;
        var res = await _userInfo.CreateUserInfo(uid, nickname);
        
        if(res == 0)
        {
            response.Result = ServerClientShare.ErrorCode.FailSaveUserInfoTable;
            return response;
        }

        var temp = await _playerServiece.CreatePlayerTables(uid);
        if (temp == 0)
        {
            response.Result = ServerClientShare.ErrorCode.FailSavePlayerTable;
            return response;
        }


        //닉네임도 보내야하나...?흠냐리~~ 
        //초기닉네임은 uid만 붙힌거라 uid만 보내도된다고 판단하긴했눈데 쩝
        response.Result = ServerClientShare.ErrorCode.None;

        return response;

    }
}
