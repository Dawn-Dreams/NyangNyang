using ServerClientShare;

namespace APIGameServer.Models;

public class mail
{
    public int uid;
    public int mail_type;
    public string mail_content;
    public DateTime mali_create_dt;
    public DateTime mali_read_dt;
    public int mail_reward_item;
    public bool is_recived;

}

public class ResponseMails
{
    public ErrorCode ErrorCode {  get; set; }
    public List<mail> mailList {  get; set; }
}

