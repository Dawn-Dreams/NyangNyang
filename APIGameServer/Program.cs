using System;
using System.IO;
using System.Text.Json;
using APIGameServer.Repositories;
using APIGameServer.Repositories.Interfaces;
using APIGameServer.Repository.Interfaces;
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
builder.Services.AddTransient<IDreams_PlayerStat, Dreams_PlayerStat>();
builder.Services.AddTransient<IDreams_UserInfo, Dreams_UserInfo>();



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