using APIGameServer.DTO;
using APIGameServer.Models;
using APIGameServer.Repositories.Interfaces;
using APIGameServer.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace APIGameServer.Controllers;

[Route("[controller]")]
[ApiController]
public class GachaWeaponController : ControllerBase
{
    readonly IItemService _itemService;
    public GachaWeaponController(IItemService itemService)
    {
       _itemService = itemService;
    }

    [HttpPost]
    public async Task<ResWeaponGacha> Create([FromBody] ReqWeaponGacha req)
    {
        //장비뽑기 스킬

        ResWeaponGacha res = new ResWeaponGacha();

        //TODO. 확률받는코드 변경해야한다.
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
            res.result = ErrorCode.FailSavePlayerTable;
        }

        return res;
    }
}
