using APIGameServer.DTO;
using APIGameServer.Models;
using APIGameServer.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIGameServer.Controllers;

[Route("[controller]")]
[ApiController]
public class UpdatePlayerStatController : ControllerBase
{
    readonly IDreams_Player _playerStat;

    public UpdatePlayerStatController(IDreams_Player playerStat)
    {
        _playerStat = playerStat;
    }

    [HttpPost]
    public async Task<ResponseUpdateStat> Create([FromBody] RequestUpdateStatLevel req)
    {
        ResponseUpdateStat res = new ResponseUpdateStat();

        var temp = await _playerStat.UpdatePlayerStatusLevel(req.StatData);
        if(temp == 0)
        {
            res.Result = ServerClientShare.ErrorCode.FailUpdatePlayerTable;
        }

        return res;


    }
        
}
