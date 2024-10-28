using APIGameServer.DTO;
using APIGameServer.Models;
using APIGameServer.Repositories.Interfaces;
using APIGameServer.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIGameServer.Controllers;

[Route("[controller]")]
[ApiController]
public class UpdatePlayerScore : ControllerBase
{

    IRedisDatabase _redisDb;

    public UpdatePlayerScore(IRedisDatabase redisDb)
    {
        _redisDb = redisDb;
    }

    [HttpPost]
    public async Task<ResponseUpdateScore> Create([FromBody] RequestUpdateScore req)
    {
        ResponseUpdateScore res = new ResponseUpdateScore();
        Console.WriteLine("UID : {0}, SCORE : {1}", req.uid, req.score);

        //받은 uid랑 score 레디스에 저장하자.
        res.result = await _redisDb.UpdateUserScore(req.uid, req.score);


        return res;

    }

}
