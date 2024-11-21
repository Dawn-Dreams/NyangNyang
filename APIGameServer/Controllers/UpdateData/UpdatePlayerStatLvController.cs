using APIGameServer.DTO;
using APIGameServer.Models;
using APIGameServer.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIGameServer.Controllers.UpdateData;

[Route("[controller]")]
[ApiController]
public class UpdatePlayerStatLvController : ControllerBase
{
    readonly IDreams_Player _playerStat;

    public UpdatePlayerStatLvController(IDreams_Player playerStat)
    {
        _playerStat = playerStat;
    }

    [HttpPost]
    public async Task<ResponseUpdateStat> Create([FromBody] PlayerStatusLevelData req)
    {
        ResponseUpdateStat res = new ResponseUpdateStat();

        var temp = await _playerStat.UpdatePlayerStatusLevel(req);
        if (temp == 0)
        {
            res.result = ErrorCode.FailUpdatePlayerTable;
        }

        return res;


    }

}
