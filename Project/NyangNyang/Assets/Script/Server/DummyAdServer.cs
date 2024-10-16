using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAdServer : DummyServerData
{
    private static Dictionary<int, int> _userSnackBuffAdViewCounts = new Dictionary<int, int>
    {
        { 0, 49 },
        { 1, 30 },
        { 2, 4 }
    };

    private static int _maxSnackBuffLevel = 10;
    private static int _snackBuffLevelStep = 5;

    public static int UserRequestSnackLevelData(int userID)
    {
        //전송해주는 로직 진행
        //SendSnackLevelDataToUser(userID);
        return _userSnackBuffAdViewCounts[userID];
    }

    public static void SendSnackLevelDataToUser(int userID)
    {
        // 범위 체크 생략

    }

    public static void UserShowSnackBuffAd(int userID)
    {
        int currentView = _userSnackBuffAdViewCounts[userID];
        currentView = Mathf.Min(currentView + 1, _maxSnackBuffLevel * _snackBuffLevelStep);
        _userSnackBuffAdViewCounts[userID] = currentView;

        // 클라에서도 해당 정보를 갱신하니 다시 send 할 필요는 x
    }
}
