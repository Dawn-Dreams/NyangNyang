
public enum ErrorCode : int
{
    None = 0,

    //���� �ʱ�ȭ ����
    FailRedisInit = 1,
    FailConnectDB = 2,


    //DB���� ����
    FailSaveUserInfoTable = 1001,
    FailSavePlayerTable = 1002,
    FailSaveInventoryTable = 1003,
    FailUpdatePlayerTable = 1004,

    //ȸ������ �� �α��� ���� �����ڵ�
    FailRegistByUid = 2001,

}
