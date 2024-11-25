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

}
