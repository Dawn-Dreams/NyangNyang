using APIGameServer.DTO;
using APIGameServer.Models;
using APIGameServer.Repositories.Interfaces;
using APIGameServer.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIGameServer.Controllers.UpdateData;

[Route("[controller]")]
[ApiController]
public class UpdateRankingController : ControllerBase
{

    IRedisDatabase _redisDb;

    public UpdateRankingController(IRedisDatabase redisDb)
    {
        _redisDb = redisDb;
    }

    [HttpPost]
    public async Task<ResponseRanking> Create()
    {
        ResponseRanking res = new ResponseRanking();

        (res.ErrorCode, res.RankingData) = await _redisDb.GetRankingTop100();

        return res;

    }

}
