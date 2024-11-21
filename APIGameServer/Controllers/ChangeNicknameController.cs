using APIGameServer.DTO;
using APIGameServer.Repositories.Interfaces;
using APIGameServer.Repository.Interfaces;
using APIGameServer.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGameServer.Controllers;

[Route("[controller]")]
[ApiController]
public class ChangeNicknameController : ControllerBase
{

    readonly IUserService _userService;

    public ChangeNicknameController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<ResChangeNickname> Create([FromBody] ReqChangeNickname req)
    {
        ResChangeNickname res = new ResChangeNickname();

   
        res.result = await _userService.ChanegeUserNickname(req.uid,
            req.oldNickname, req.newNickname);

        
        return res;

    }
}
