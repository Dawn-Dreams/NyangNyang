
using System.ComponentModel.DataAnnotations;

namespace APIGameServer.DTO;

public class ReqWeaponGacha
{
    [Required]
    public int uid { get; set; }
}
public class ResWeaponGacha
{
    [Required]
    public ErrorCode result { get; set; }

    [Required]
    public int itemId { get; set; }
}

public class ReqSkiilGacha
{
    [Required]
    public int uid { get; set; }

}
public class ResSkillGacha
{
    [Required]
    public ErrorCode result { get; set; }
    [Required]
    public int skillId { get; set; }
}
