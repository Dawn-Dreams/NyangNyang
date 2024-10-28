using APIGameServer.Models;

namespace APIGameServer.Repositories.Interfaces;

public interface IDreams_Mailbox : IDisposable
{
    public  Task<int> InsertMailbox(Mail mail);
    public Task<List<Mail>> GetAllMailList(int uid);

}
