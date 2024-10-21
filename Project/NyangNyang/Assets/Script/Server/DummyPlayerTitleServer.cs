using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        { 0, new List<int>(){0,999} }
    };

    // 유저의 현재 선택된 타이틀 ID를 요구하는 함수
    public static int UserRequestCurrentSelectedTitleID(int userID)
    {
        return _userCurrentSelectedTitle[userID];
    }

    // 유저가 {타이틀ID}의 타이틀을 착용하겠다고 하는 함수
    public static void UserRequestEquipTitle(int userID, int titleID)
    {
        // 유저가 해당 타이틀을 보유하고 있지 않다면 경고 및 불법사용자 의심 코드 추가..

        _userCurrentSelectedTitle[userID] = titleID;
    }

    public static void UserRequestAcquireTitle(int userID, int acquireTitleID)
    {
        _userOwningTitles[userID].Add(acquireTitleID);
    }

    public static List<int> UserRequestOwningTitles(int userID)
    {
        //int[] returnData = _userOwningTitles[userID].ToArray();
        List<int> returnData = new List<int>(_userOwningTitles[userID]);
        return returnData;
    }
}
