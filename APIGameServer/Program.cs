using System;
using System.IO;
using System.Text.Json;
using APIGameServer.Repositories;
using APIGameServer.Repositories.Interfaces;
using APIGameServer.Repository.Interfaces;
using APIGameServer.Services;
using APIGameServer.Services.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
builder.Services.Configure<DBConfig>(configuration.GetSection(nameof(DBConfig)));


// Add services to the container.
builder.Services.AddSingleton<IRedisDatabase, RedisDatabase>();
builder.Services.AddControllers();

builder.Services.AddTransient<IDreams_Inventory,Dreams_Inventory>();
builder.Services.AddTransient<IDreams_Player, Dreams_Player>();
builder.Services.AddTransient<IDreams_UserInfo, Dreams_UserInfo>();

builder.Services.AddTransient<IPlayerService, PlayerService>();
builder.Services.AddTransient<IItemService, ItemService>();

var app = builder.Build();

app.UseRouting();
#pragma warning disable ASP0014
app.UseEndpoints(endpoints => { _ = endpoints.MapControllers(); });
#pragma warning restore ASP0014


app.Run(configuration["ServerAddress"]);



public class DBConfig
{
    public string? GameDB { get; set; }
    public string? Redis { get; set; }

}