namespace APIGameServer.Models;

public class UserInfo
{
    public int uid { get; set; }
    public string nickname { get; set; }
    public DateTime regist_dt{  get; set; }
    public DateTime recent_login_dt { get; set; }
    public int attendence_cnt {  get; set; }
    public bool is_active {  get; set; }
}
