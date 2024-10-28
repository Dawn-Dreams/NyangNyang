namespace APIGameServer.Services.Interface;

public interface IItemService : IDisposable
{
    public Task<int> EquipmentGacha();
    public Task<int> SkillGacha();
    public Task<ErrorCode> SaveIteminInventory(int uid, int item);
}
