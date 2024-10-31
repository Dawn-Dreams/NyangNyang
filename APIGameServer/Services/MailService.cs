using APIGameServer.Models;
using APIGameServer.Repositories.Interfaces;
using APIGameServer.Services.Interface;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using System.Data;

namespace APIGameServer.Services;

public class MailService : IMailService
{
    readonly IDreams_Mailbox _mailbox;
    public MailService(IDreams_Mailbox mailbox)
    {
        _mailbox = mailbox;

    }
    public async Task<ErrorCode> AddRewardinMailbox(int uid,int type, int reward)
    {

        var res = await _mailbox.InsertMailbox(new mail(uid, type, reward));
        if(res == 0)
        {
            //잘못된거임 에러코드를 어쩌구해야한다.
        }
        return ErrorCode.None;
    }
    public async Task<ErrorCode> RemoveinMailbox(string uid, int type, int reward)
    {
       
        return ErrorCode.None;
    }

    public async Task<(ErrorCode, List<mail>)> GetMailList(int uid )
    {

        //db에서 가져와야함
        List<mail> mails = await _mailbox.GetAllMailList(uid);

       if(mails.Count == 0)
        {

        }

       return (ErrorCode.None, mails);
    }

    public void Dispose()
    {
        
    }

}
