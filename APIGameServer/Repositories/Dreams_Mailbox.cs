using APIGameServer.Models;
using APIGameServer.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using System.Data;

namespace APIGameServer.Repositoriesl;

public class Dreams_Mailbox : IDreams_Mailbox
{
    readonly IOptions<DBConfig> _dbConfig;

    IDbConnection _dbConn;
    SqlKata.Compilers.MySqlCompiler _compiler;
    QueryFactory _queryFactory;

    public Dreams_Mailbox(IOptions<DBConfig> dbConfing)
    {
        _dbConfig = dbConfing;
        Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new QueryFactory(_dbConn, _compiler);

    }

    public async Task<int> InsertMailbox(mail mail)
    {
        return await _queryFactory.Query("mailbox").InsertAsync(mail);
    }

    public async Task<List<mail>> GetAllMailList(int uid)
    {
        var result = await _queryFactory.Query("mailbox")
            .Where("uid", uid)
            .Select()
            .GetAsync<mail>();

        return result.ToList();

    
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
