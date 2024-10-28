using APIGameServer.Models;
using APIGameServer.Repositories.Interfaces;
using APIGameServer.Services.Interface;
using System.Formats.Asn1;

namespace APIGameServer.Services;


public class PlayerService : IPlayerService
{

    readonly IDreams_Player _playerDB;
    public PlayerService(IDreams_Player playerDB)
    {
        _playerDB = playerDB;
    }

    public async Task<ErrorCode> CreatePlayerTables(int uid)
    {

        var res = await _playerDB.CreatePlayerStat(uid);
        if(res == 0)
        {
            return ErrorCode.FailRegistByUid;
        }

        res = await _playerDB.CreatePlayerStatusLevel(uid);
        if (res == 0)
        {
            return ErrorCode.FailRegistByUid;
        }

        res = await _playerDB.CreatePlayerGoods(uid);
        if (res == 0)
        {
            return ErrorCode.FailRegistByUid;
        }

        return ErrorCode.None;
    }
    public async Task<(ErrorCode,PlayerStatusData,PlayerStatusLevelData,PlayerGoodsData)> GetPlayerTable(int uid)
    {
        var status = await _playerDB.GetPlayerStatus(uid);
        if (status == null)
        {
            return (ErrorCode.FailConnectDB, null, null, null);
        }

        var lv = await _playerDB.GetPlayerStatusLevel(uid);
        if (lv == null)
        {
            return (ErrorCode.FailConnectDB, null, null, null);
        }

        var goods = await _playerDB.GetPlayerGoods(uid);
        if (goods == null)
        {
            return (ErrorCode.FailConnectDB, null, null, null);
        }

        return (ErrorCode.None, status,lv, goods);
    }


    //출석 업데이트 하자




    public void Dispose()
    {

    }
}
