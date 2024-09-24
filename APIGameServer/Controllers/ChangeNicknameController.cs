using APIGameServer.DTO;
using APIGameServer.Repositories.Interfaces;
using APIGameServer.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGameServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChangeNicknameController : ControllerBase
{

    readonly IDreams_UserInfo _userInfo;

    public ChangeNicknameController(IDreams_UserInfo userInfo)
    {
        _userInfo = userInfo;
    }

    [HttpPost]
    public async Task<ResponsChangeNick> Create([FromBody] RequestChangeNick req)
    {
        ResponsChangeNick res = new ResponsChangeNick();



    }
}
