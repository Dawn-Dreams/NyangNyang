using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DummyPlayerCostumeServer : MonoBehaviour
{
    private static Dictionary<int, Dictionary<CatCostumePart, int>> _userCurrentEquiCostume = new Dictionary<int, Dictionary<CatCostumePart, int>>
    {
        // { 유저 ID, {파츠, 인덱스} }
        { 0, new Dictionary<CatCostumePart, int> { { CatCostumePart.Head , 0}, { CatCostumePart.Hand_R , 0}, {CatCostumePart.Body,0}, {CatCostumePart.FurSkin, 0},
            { CatCostumePart.Pet , 0}, {CatCostumePart.Emotion, 0}}},
        { 1, new Dictionary<CatCostumePart, int> { { CatCostumePart.Head , 0}, { CatCostumePart.Hand_R , 0}, {CatCostumePart.Body,0}, {CatCostumePart.FurSkin, 0}}},
    };

    private static Dictionary<int, Dictionary<CatCostumePart, List<int>>> _userOwningCostumes = new Dictionary<int, Dictionary<CatCostumePart, List<int>>>
    {
        // { 유저 ID, { 파츠, 인덱스 리스트} }
        // 0번 (NotEquip) 은 무조건 보유
        { 0, new Dictionary<CatCostumePart, List<int>>
        {
            { CatCostumePart.Head , new List<int>{0,1,2}},
            { CatCostumePart.Body, new List<int>{0,1,2}},
            { CatCostumePart.Hand_R , new List<int>{0,1,2}},
            { CatCostumePart.FurSkin , new List<int>{0,3,4,5}},
            { CatCostumePart.Pet , new List<int>{0,1}},
            {CatCostumePart.Emotion, new List<int>{0,1,2}}
        } }
    };

    // 유저의 현재 선택된 코스튬 데이터를 요구하는 함수
    public static Dictionary<CatCostumePart, int> UserRequestCurrentEquipCostumes(int userID)
    {
        return _userCurrentEquiCostume[userID];
    }

    // 유저가 { 파츠, 인덱스 } 의 코스튬 착용에 대한 요청 함수
    // 클라 내에서도 장착이 적용되므로 반환 필요 x
    // 실제 서버에서는 파트가 분리되어 정보를 전송받지 말고 한번에 받을 수 있도록
    public static void UserRequestEquipCostume(int userID, CatCostumePart part, int index)
    {
        // 유저가 해당 타이틀을 보유하고 있지 않다면 경고 및 불법사용자 의심 코드 추가..
        _userCurrentEquiCostume[userID][part] = index;
    }

    public static void UserRequestAcquireCostume(int userID, CatCostumePart part, int index)
    {
        // 보유중이라면 에러 처리 필요

        _userOwningCostumes[userID][part].Add(index);
    }

    public static Dictionary<CatCostumePart, List<int>> UserRequestOwningCostumes(int userID)
    {
        //int[] returnData = _userOwningTitles[userID].ToArray();
        return _userOwningCostumes[userID];
    }
}
