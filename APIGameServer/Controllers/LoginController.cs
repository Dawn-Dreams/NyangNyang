using APIGameServer.DTO;
using APIGameServer.Repository.Interfaces;
using APIGameServer.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlKata;
using System.Reflection.PortableExecutable;

namespace APIGameServer.Controllers;

[Route("[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    readonly IPlayerService _playerService;
    readonly IUserService _userService;
    readonly IMailService _mailService;

    readonly IRedisDatabase _redis;
    public LoginController(IPlayerService playerService, IUserService userService,IRedisDatabase redis, IMailService mailService)
    {
        _playerService = playerService;
        _userService = userService;
        _redis = redis;
        _mailService = mailService; 
    }

    [HttpPost]
    public async Task<ResponseLogin> Create([FromBody] RequestLogin req)
    {
        ResponseLogin response = new ResponseLogin();
  
        if(await _redis.CheckUserUid(req.uid) == false)
        {
            response.result= ErrorCode.FailRedisInit;
            return response;
        }

        var res = await _userService.UpdateRecentLoginDateTime(req.uid);
        if(res!= ErrorCode.None)
        {
            //여기서 뭐를 처리해줘야한다.
        }

        int cnt = await _userService.CheckAttendenceAndReward(req.uid);
        Console.WriteLine(cnt);

        //여기서 우편함에 있는 모든것들 전송하기 메일리스트를 가져와야한다.
        (res,response.mailList) = await _mailService.GetMailList(req.uid);
       

        //플레이어 상태 전송
        (response.result, response.status, response.statusLv, response.goods) =
            await _playerService.GetPlayerTable(req.uid);

    
        return response;
    }


}
