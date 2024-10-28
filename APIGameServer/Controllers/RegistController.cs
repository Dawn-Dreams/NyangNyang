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
            response.result = ErrorCode.FailRegistByUid;
            return response;
        }

        response.uid = uid;

        var nickname = "nyang" + uid;
        var res = await _userInfo.CreateUserInfo(uid, nickname);
        if(res == 0)
        {
            response.result = ErrorCode.FailSaveUserInfoTable;
            return response;
        }
       
        if(await _redis.SaveUserNickname(uid,nickname)==false)
        {
            response.result = ErrorCode.FailSavePlayerTable;
            return response;
        }

        var temp = await _playerServiece.CreatePlayerTables(uid);
        if (temp == 0)
        {
            response.result = ErrorCode.FailSavePlayerTable;
            return response;
        }

        response.result = ErrorCode.None;

        return response;

    }
}
