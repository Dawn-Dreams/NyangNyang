using APIGameServer.DTO;
using APIGameServer.Models;
using APIGameServer.Repositories.Interfaces;
using APIGameServer.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace APIGameServer.Controllers;

[Route("[controller]")]
[ApiController]
public class GachaEquipmentController : ControllerBase
{
    readonly IItemService _itemService;
    public GachaEquipmentController(IItemService itemService)
    {
       _itemService = itemService;
    }

    [HttpPost]
    public async Task<ResEquipGacha> Create([FromBody] ReqEquipGacha req)
    {
        //장비뽑기 스킬

        ResEquipGacha res = new ResEquipGacha();

        //클라이언트한테는 아이템을 넘겨주자.
        //1. 확률그거 하고 아이템뽑는다.
        //2. 유저 인벤토리에 추가한다.

        res.itemId= await _itemService.EquipmentGacha();
        if (res.itemId < 0)
        {
            //TODO. 실패에러코드 변경해야함
            res.result = ErrorCode.FailSavePlayerTable;
        }


        //여기까지온거는 성공이니까 유저db에 저장
        res.result = await _itemService.SaveIteminInventory(req.uid, res.itemId);
        if (res.result != ErrorCode.None)
        {
            //실패일때 해결책을 쓰자. 
        }


        return res;


    }
}
