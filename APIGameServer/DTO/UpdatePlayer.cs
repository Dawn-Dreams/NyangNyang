using APIGameServer.Models;
using System.ComponentModel.DataAnnotations;

namespace APIGameServer.DTO;

using ErrorCode = ServerClientShare.ErrorCode;
public class RequestUpdateStatLevel
{
    [Required]
    public PlayerStatusLevelData StatData { get; set; }

}

public class ResponseUpdateStat
{
    public ErrorCode Result { get; set; }    
}