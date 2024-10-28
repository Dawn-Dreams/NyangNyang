
namespace APIGameServer.DTO;

public class ReqEquipGacha
{
    public int uid;
}
public class ResEquipGacha
{
    public ErrorCode result;
    public int itemId;
}

public class ReqSkiilGacha
{
    public int uid;

}
public class ResSkillGacha
{
    public ErrorCode result;
    public int skillId;
}
