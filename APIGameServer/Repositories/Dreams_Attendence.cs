using APIGameServer.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using System.Data;

namespace APIGameServer.Repositories;

public class Dreams_Attendence : IDreams_Attendence
{
    readonly IOptions<DBConfig> _dbConfig;

    IDbConnection _dbConn;
    SqlKata.Compilers.MySqlCompiler _compiler;
    QueryFactory _queryFactory; public Dreams_Attendence(IOptions<DBConfig> dbConfing)
    {
        _dbConfig = dbConfing;
        Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new QueryFactory(_dbConn, _compiler);

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
