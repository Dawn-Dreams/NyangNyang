using APIGameServer.DTO;
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

    public LoginController(IPlayerService playerService)
    {
        _playerService = playerService;

    }

    [HttpPost]
    public async Task<ResponseLogin> Create([FromBody] RequestLogin req)
    {
        ResponseLogin response = new();

        (response.Result, response.status, response.statusLv, response.goods) =
            await _playerService.GetPlayerTable(req.Uid);

        //출석관련 코드도 추가해야한다 

        return response;
    }


}
