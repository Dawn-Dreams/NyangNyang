using APIGameServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;

namespace APIGameServer.Controllers;

[Route("[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    //레디스 참조해야한다

    public LoginController()
    {

    }

    [HttpPost]
    public async Task<ResponseLogin> UserLogin([FromBody] RequestLogin req)
    {
        ResponseLogin response = new();


       
        return response;
    }


}
