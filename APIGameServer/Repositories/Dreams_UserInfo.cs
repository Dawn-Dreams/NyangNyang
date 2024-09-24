using APIGameServer.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APIGameServer.Repositories;

public class Dreams_UserInfo : IDreams_UserInfo
{

    readonly IOptions<DBConfig> _dbConfig;

    IDbConnection _dbConn;
    SqlKata.Compilers.MySqlCompiler _compiler;
    QueryFactory _queryFactory;

    string _tableName = "user_info";
    public Dreams_UserInfo(IOptions<DBConfig> dbConfing)
    {
        _dbConfig = dbConfing;
        Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new QueryFactory(_dbConn, _compiler);

    }


    public async Task<int> CreateUserInfo(long uid, string nickname)
    {

        return await _queryFactory.Query(_tableName).InsertAsync(new
        {
            uid,
            nickname,
            regist_dt = DateTime.Now,
            recent_login_dt = DateTime.Now,
            is_active = true
        });


    }

    public async Task<int> ChangeNickname(long uid, string changeNickname)
    {
        return await _queryFactory.Query(_tableName).Where("uid", uid)
            .UpdateAsync(new { nickname = changeNickname });
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
