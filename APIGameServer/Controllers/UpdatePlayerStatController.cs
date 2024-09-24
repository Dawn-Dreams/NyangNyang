using APIGameServer.DTO;
using APIGameServer.Models;
using APIGameServer.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIGameServer.Controllers;

[Route("[controller]")]
[ApiController]
public class UpdatePlayerStatController : ControllerBase
{
    readonly IDreams_PlayerStat _playerStat;

    public UpdatePlayerStatController(IDreams_PlayerStat playerStat)
    {
        _playerStat = playerStat;
    }

    [HttpPost]
    public async Task<ResponseUpdateStat> Create([FromBody] RequestUpdateStat req)
    {
        ResponseUpdateStat res = new ResponseUpdateStat();

        res.Result = await _playerStat.UpdatePlayerStat(req.StatData);

        return res;


    }
        
}
