namespace APIGameServer.DTO;

using APIGameServer.Models;
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
    public int uid { get; set; }
}

public class ResponseRegist
{

    [Required]
    public ErrorCode result { get; set; } = 0;

    [Required]
     public int uid { get; set; } 
    //클라도 자신의 uid를 알고 클라측에서 저정해야함
}

public class ResponseLogin
{

    [Required]
    public ErrorCode result { get; set; } = 0;

    public PlayerStatusData status {  get; set; }
    public PlayerStatusLevelData statusLv {  get; set; }
    public PlayerGoodsData goods {  get; set; }

    //메일리스트도 같이 받자. 
    public List<mail> mailList { get; set; }


}

