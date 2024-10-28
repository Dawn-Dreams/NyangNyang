namespace APIGameServer.Models;

enum MailType
{
    Event = 0,
    Reward=1,
    Friend=2
}

public class Mail
{
    public int uid {  get; set; }
    public int mail_type { get; set; }
    public int mail_reward_item { get; set; }
    public DateTime mail_create_dt { get; set; }
    public DateTime? mail_read_dt { get; set; }
    public bool is_recived { get; set; }

    public Mail(int uid, int type, int reward)
    {
        this.uid = uid;
        this.mail_type = type;
        this.mail_reward_item = reward;
        this.mail_create_dt = DateTime.Now;
        this.mail_read_dt = null;

        this.is_recived = false;
    }


}

public class ResponseMails
{
    public ErrorCode ErrorCode {  get; set; }
    public List<Mail> mailList {  get; set; }
}

