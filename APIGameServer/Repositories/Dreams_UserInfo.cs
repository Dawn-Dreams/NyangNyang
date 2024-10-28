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
            attendence_cnt=1,
            is_active = true
        });
    }

    public async Task<int> ChangeNickname(long uid, string changeNickname)
    {
        return await _queryFactory.Query(_tableName).Where("uid", uid)
            .UpdateAsync(new { nickname = changeNickname });
    }

    public async Task<int> UpdateRecentLoginCnt(int uid)
    {
        return await _queryFactory.StatementAsync($"UPDATE user_info " +
                                              $"SET recent_login_dt = '{DateTime.Now:yyyy-MM-dd-HH:mm:ss}', " +
                                              $"attendence_cnt = CASE " +
                                              $"WHEN recent_login_dt < '{DateTime.Now:yyyy-MM-dd-HH:mm:ss}' " +
                                                    $"THEN attendence_cnt+1 " +
                                                    $"ELSE attendence_cnt END " +
                                              $"WHERE uid = {uid}");
    }
    public async Task<int> GetAttendenceCnt(int uid)
    {
        return await _queryFactory.Query("user_info")
            .Where("uid", uid).Select("attendence_cnt").FirstOrDefaultAsync<int>();

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
