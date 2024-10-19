using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayerTitleServer : MonoBehaviour
{
    private static Dictionary<int, int> _userSelectedTitle = new Dictionary<int, int>
    {
        // { 유저 ID, 타이틀 ID}
        { 0, 0 },
    };

    private static Dictionary<int, List<int>> _userAcquireTitles = new Dictionary<int, List<int>>
    {
        // { 유저 ID, 보유중인 타이틀 ID array }
        { 0, new List<int>(){0,1,999} }
    };

    public int UserRequestSelectedTitleID(int userID)
    {
        return _userSelectedTitle[userID];
    }

    public void UserRequestAcquireTitle(int userID, int acquireTitleID)
    {
        _userAcquireTitles[userID].Add(acquireTitleID);
    }
}
