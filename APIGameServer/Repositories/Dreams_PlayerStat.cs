using APIGameServer.Models;
using APIGameServer.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using System.Data;
using System.Formats.Asn1;

namespace APIGameServer.Repositories;

public class Dreams_PlayerStat : IDreams_PlayerStat
{
    readonly IOptions<DBConfig> _dbConfig;

    IDbConnection _dbConn;
    SqlKata.Compilers.MySqlCompiler _compiler;
    QueryFactory _queryFactory;

    string _tableName = "player_stat";

    public Dreams_PlayerStat(IOptions<DBConfig> dbConfing)
    {
        _dbConfig = dbConfing;
        Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new QueryFactory(_dbConn,_compiler);
        
    }

    public async Task<ErrorCode> GetPlayerStat(int uid)
    {
        //uid를 이용해 가져와야한다.

        var res = await _queryFactory.Query(_tableName).
            Where("uid", uid).FirstOrDefaultAsync<PlayerStatData>();

        if(res == null)
        {
            //todo error 코드 만들어야한다.
            return ErrorCode.FailRegistByUid;
        }
        return ErrorCode.None;

    }

    public async Task<ErrorCode> UpdatePlayerStat(PlayerStatData data)
    {
        //업데이트 해야한다.
        var uid = data.uid;

        var count = await _queryFactory.Query(_tableName).Where("uid", uid).UpdateAsync(data);

        if(count == 0)
        {
            //업데이트 실패한거임
            Console.WriteLine("플레이어 스탯 업데이트 실패");
            return ErrorCode.FailRegistByUid;

        }
        Console.WriteLine("플레이어 스탯 업데이트 성공");

        return ErrorCode.None;

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
