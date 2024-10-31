using Microsoft.VisualBasic;

namespace APIGameServer.Models;

enum mailType
{
    Event = 0,
    Reward=1,
    Friend=2
}

public class mail
{
    public int uid {  get; set; }
    public int mail_type { get; set; }
    public int mail_reward_item { get; set; }
    public DateTime mail_create_dt { get; set; }
    public DateTime? mail_read_dt { get; set; }
    public bool is_recived { get; set; }
    public mail() { }
    public mail(int uid, int type, int reward)
    {
        this.uid = uid;
        this.mail_type = type;
        this.mail_reward_item = reward;
        this.mail_create_dt = DateTime.Now;
        this.mail_read_dt = null;
        this.is_recived = false;
    }
    public mail(int uid, int mail_type, int mail_reward_item,
        DateTime mail_create_dt, DateTime mail_read_dt, bool is_recived)
    {
        this.uid = uid;
        this.mail_type = mail_type;
        this.mail_reward_item = mail_reward_item;
        this.mail_create_dt = mail_create_dt;
        this.mail_read_dt = mail_read_dt;

        this.is_recived = is_recived;
    }
    public mail(mail mail)
    {
        this.uid = mail.uid;
        this.mail_type = mail.mail_type;
        this.mail_reward_item = mail.mail_reward_item;
        this.mail_create_dt = mail.mail_create_dt;
        this.mail_read_dt = mail.mail_read_dt;

        this.is_recived = mail.is_recived;
    }
}

public class ResponseMails
{
    public ErrorCode ErrorCode {  get; set; }
    public List<mail> mailList {  get; set; }
}

