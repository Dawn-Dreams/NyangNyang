using APIGameServer.Models;
using APIGameServer.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using System.Data;
using System.Formats.Asn1;

namespace APIGameServer.Repositories;

public class Dreams_Player : IDreams_Player
{
    readonly IOptions<DBConfig> _dbConfig;

    IDbConnection _dbConn;
    SqlKata.Compilers.MySqlCompiler _compiler;
    QueryFactory _queryFactory;

    public Dreams_Player(IOptions<DBConfig> dbConfing)
    {
        _dbConfig = dbConfing;
        Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new QueryFactory(_dbConn,_compiler);
        
    }
    public async Task<int> CreatePlayerStat(int uid)
    {
        return await _queryFactory.Query("player_status").InsertAsync(new
        {
            uid,   
        });
    }

    public async Task<PlayerStatusData> GetPlayerStatus(int uid)
    {

        return await _queryFactory.Query("player_status").
             Where("uid", uid).FirstOrDefaultAsync<PlayerStatusData>();
    }
    public async Task<int> UpdatePlayerStatus(PlayerStatusData data)
    {

        return await _queryFactory.Query("player_status")
                        .Where("uid", data.uid).UpdateAsync(data);
    }


    public async Task<int> CreatePlayerStatusLevel(int uid)
    {
        return await _queryFactory.Query("player_status_lv").InsertAsync(new
        {
            uid,    
        });

    }

    public async Task<PlayerStatusLevelData> GetPlayerStatusLevel(int uid)
    {

        return await _queryFactory.Query("player_status_lv").
             Where("uid", uid).FirstOrDefaultAsync<PlayerStatusLevelData>();
    }
    public async Task<int> UpdatePlayerStatusLevel(PlayerStatusLevelData data)
    {

        return await _queryFactory.Query("player_status_lv")
                        .Where("uid", data.uid).UpdateAsync(data);
    }




    public async Task<int> CreatePlayerGoods(int uid)
    {
        return await _queryFactory.Query("player_goods").InsertAsync(new
        {
            uid,  
        });

    }


    public async Task<PlayerGoodsData> GetPlayerGoods(int uid)
    {

        return await _queryFactory.Query("player_goods").
             Where("uid", uid).FirstOrDefaultAsync<PlayerGoodsData>();
    }

    public async Task<int> UpdatePlayerGoods(PlayerGoodsData data)
    {

        return await _queryFactory.Query("player_goods")
                        .Where("uid", data.uid).UpdateAsync(data);
    }


    public void Dispose()
    {
        Close();
    }
    void Open()
    {
        _dbConn = new MySqlConnection(_dbConfig.Value.GameDB);
        _dbConn.Open();
    }
    void Close()
    {
        _dbConn.Close();

    }


}
