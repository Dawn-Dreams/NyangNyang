using APIGameServer.Models;
using System.ComponentModel.DataAnnotations;

namespace APIGameServer.DTO;

public class RequestUpdateStat
{
    [Required]
    public PlayerStatData StatData { get; set; }

}

public class ResponseUpdateStat
{
    public ErrorCode Result { get; set; }    
}