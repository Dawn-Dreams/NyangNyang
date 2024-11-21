
public enum ErrorCode : int
{
    None = 0,
    FailRedisInit = 1,
    FailConnectDB = 2,


    FailSaveUserInfoTable = 1001,
    FailSavePlayerTable = 1002,
    FailSaveInventoryTable = 1003,
    FailUpdatePlayerTable = 1004,

    FailRegistByUid = 2001,

}
