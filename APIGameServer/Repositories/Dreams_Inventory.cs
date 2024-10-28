using APIGameServer.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using System.Data;

namespace APIGameServer.Repositories;

public class Dreams_Inventory : IDreams_Inventory
{

    readonly IOptions<DBConfig> _dbConfig;

    IDbConnection _dbConn;
    SqlKata.Compilers.MySqlCompiler _compiler;
    QueryFactory _queryFactory;

    string _tableName = "inventory";
    public Dreams_Inventory(IOptions<DBConfig> dbConfing)
    {
        _dbConfig = dbConfing;
        Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new QueryFactory(_dbConn, _compiler);

    }
    public async Task<int> InsertItem(int uid, int item)
    {
        return await _queryFactory.Query(_tableName).InsertAsync(new
        {
            uid=uid, 
            item_id = item,
            item_acquired_dt = DateTime.Now
        });
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

