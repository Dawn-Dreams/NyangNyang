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
    readonly IUserService _userService;

    public LoginController(IPlayerService playerService, IUserService userService)
    {
        _playerService = playerService;
        _userService = userService; 
    }

    [HttpPost]
    public async Task<ResponseLogin> Create([FromBody] RequestLogin req)
    {
        ResponseLogin response = new ResponseLogin();

        //최근 로그인시간 업데이+출석업데이트하기 -> 만약 출석일수 채워지면 우편을 넣어줘아함
        //1. userserviec에서 출석을 업데이트한다.
        //우편함리스트 전송 -> 우편함을 누르면 전송하는거로 하는게 좋을듯
        //2. 출석보상을 줄 만큼인지를 확인한다.
        //출석보상은 배열에다가 30일하고 day를 넣으면 해당하는 상품번호가 나오는거로 하는게 좋을듯
        //만약 0이면 아무것도 안나오는거임 
        //출석보상을 여기서 해줄지 아니면 출석체크하는 함수에서 해줄지가 고민임.
        //출석보상은 체크하는 함수에서 해주자.? 여기서해줄꺼ㅏ?

        //인벤토리 전송 - 이거는 흠냐 이것은 전투시작하면? 아니면 언제하는게좋을까 고민중
        var res = await _userService.UpdateRecentLoginDateTime(req.uid);
        if(res!= ErrorCode.None)
        {
            //여기서 뭐를 처리해줘야한다.
        }

        int cnt = await _userService.CheckAttendenceAndReward(req.uid);
        Console.WriteLine(cnt);



        //플레이어 상태 전송
        (response.result, response.status, response.statusLv, response.goods) =
            await _playerService.GetPlayerTable(req.uid);

    
        return response;
    }


}
