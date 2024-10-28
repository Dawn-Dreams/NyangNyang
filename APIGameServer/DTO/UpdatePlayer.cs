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
    public ErrorCode result { get; set; }    
}

public class RequestUpdateScore
{
    [Required]
    public int uid {  get; set; }   
    [Required]
    public int score {get; set; }
}

public class ResponseUpdateScore
{
    [Required]
    public ErrorCode result { get; set; }

}