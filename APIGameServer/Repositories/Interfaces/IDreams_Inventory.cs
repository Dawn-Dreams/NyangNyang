namespace APIGameServer.Repositories.Interfaces;

public interface IDreams_Inventory : IDisposable
{
    public Task<int> InsertItem(int uid, int item);

}
