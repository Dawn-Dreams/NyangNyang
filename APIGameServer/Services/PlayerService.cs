using APIGameServer.Repositories.Interfaces;
using APIGameServer.Services.Interface;

namespace APIGameServer.Services;
using ErrorCode = ServerClientShare.ErrorCode;

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
            //TODO. 에러코드 추가해야한다.
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

    public void Dispose()
    {

    }
}
