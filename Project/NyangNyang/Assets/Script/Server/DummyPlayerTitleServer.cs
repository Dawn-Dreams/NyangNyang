using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayerTitleServer : MonoBehaviour
{
    private static Dictionary<int, int> _userCurrentSelectedTitle = new Dictionary<int, int>
    {
        // { 유저 ID, 타이틀 ID}
        { 0, 999 },
    };

    private static Dictionary<int, List<int>> _userOwningTitles = new Dictionary<int, List<int>>
    {
        // { 유저 ID, 보유중인 타이틀 ID int list }
        { 0, new List<int>(){0,1,999} }
    };

    public static int UserRequestCurrentSelectedTitleID(int userID)
    {
        return _userCurrentSelectedTitle[userID];
    }

    public static void UserRequestAcquireTitle(int userID, int acquireTitleID)
    {
        _userOwningTitles[userID].Add(acquireTitleID);
    }

    public static int[] UserRequestOwningTitles(int userID)
    {
        int[] returnData = _userOwningTitles[userID].ToArray();
        return returnData;
    }
}
