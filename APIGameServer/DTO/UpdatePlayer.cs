using APIGameServer.Models;
using System.ComponentModel.DataAnnotations;

namespace APIGameServer.DTO;


public class RequestUpdateStatLevel
{
    [Required]
    public PlayerStatusLevelData StatData { get; set; }

}


public class ResponseUpdateStat
{
    public ErrorCode Result { get; set; }    
}

public class RequestUpdateScore
{
    public int uid {  get; set; }   
    public int score {get; set; }
}

public class ResponseUpdateScore
{
    public ErrorCode Result { get; set; }

}