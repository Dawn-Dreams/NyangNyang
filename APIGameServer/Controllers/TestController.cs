using APIGameServer.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGameServer.Controllers;

[Route("[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpPost]
    public async Task<ResponseTest> Create([FromBody] RequestTest req)
    {
        ResponseTest res = new ResponseTest();

        res.message = "server connect success!";

        return res;
    }

}
