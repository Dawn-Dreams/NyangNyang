
public enum ErrorCode : int
{
    None = 0,

    //서버 초기화 엥러
    FailRedisInit = 1,
    FailConnectDB = 2,


    //DB관련 에러
    FailSaveUserInfoTable = 1001,
    FailSavePlayerTable = 1002,
    FailSaveInventoryTable = 1003,
    FailUpdatePlayerTable = 1004,

    //회원가입 및 로그인 관련 에러코드
    FailRegistByUid = 2001,

}
