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

   
}
