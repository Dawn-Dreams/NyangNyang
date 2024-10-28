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
            response.Result = ErrorCode.FailRegistByUid;
            return response;
        }

        response.Uid = uid;

        var nickname = "nyang" + uid;
        //여기서 redis에 uid랑 닉네임도 pair로 저장할까?
        var res = await _userInfo.CreateUserInfo(uid, nickname);
        
        if(res == 0)
        {
            response.Result = ErrorCode.FailSaveUserInfoTable;
            return response;
        }

        var temp = await _playerServiece.CreatePlayerTables(uid);
        if (temp == 0)
        {
            response.Result = ErrorCode.FailSavePlayerTable;
            return response;
        }

        response.Result = ErrorCode.None;

        return response;

    }
}
