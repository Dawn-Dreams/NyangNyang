using APIGameServer.Repositories.Interfaces;
using APIGameServer.Services.Interface;

using System.Formats.Asn1;

namespace APIGameServer.Services;

public class ItemService : IItemService
{
    //아이템 관련 작업 수행할 서비스
    readonly IDreams_Inventory _inventory;

    public ItemService(IDreams_Inventory inventory)
    {
        _inventory = inventory;
    }
    //가챠도 여기서 수행하자
    public async Task<int> EquipmentGacha()
    {
        //아이템뽑아서 아이템번호만 준다.

        return 5;

    }
    public async Task<int> SkillGacha()
    {
        return 0;
    }

    public async Task<ErrorCode> SaveIteminInventory(int uid, int item)
    {
        
        //유저 invetnroy에 아이템정보 넣는다.
        var res = await _inventory.InsertItem(uid, item);

        return ErrorCode.None;
    }
    public void Dispose()
    {

    }
}
