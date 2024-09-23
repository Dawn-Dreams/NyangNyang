namespace APIGameServer.Models;

using Microsoft.AspNetCore.Connections.Features;
using System;
using System.ComponentModel.DataAnnotations;

public class RequestRegist
{
    //클라가 처음 접속하면 이 패킷보내기 
}
public class RequestLogin
{
    [Required]
    public int Uid { get; set; }
}

public class ResponseRegist
{

    [Required]
    public ErrorCode Result { get; set; } = 0;

    [Required]
    public long Uid { get; set; } 
}

public class ResponseLogin
{

    [Required]
    public ErrorCode Result { get; set; } = 0;
    [Required]
    public int Uid { get; set; }
    [Required]
    public string NickName { get; set; }
}

