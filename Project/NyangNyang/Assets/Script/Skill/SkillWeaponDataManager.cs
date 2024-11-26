using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillWeaponDataManager : MonoBehaviour
{
    private static SkillWeaponDataManager instance;
    public static SkillWeaponDataManager GetInstance() => instance;

    private SkillWeaponData gameData = new SkillWeaponData();
    private SkillWeaponJson jsonManager = new SkillWeaponJson();


    private void Awake()
    {
        if ( instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // 데이터

        // gameData = jsonManager.LoadGameData();   

        //gameData.weapons[0] = new WeaponSaveData { id = 0, name = "낡은 횃불", level = 1, possession = 1, grade = "Normal", subGrade = "A", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[1] = new WeaponSaveData { id = 1, name = "평범한 횃불", level = 1, possession = 1, grade = "Normal", subGrade = "B", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[2] = new WeaponSaveData { id = 2, name = "정교한 횃불", level = 1, possession = 1, grade = "Normal", subGrade = "C", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[3] = new WeaponSaveData { id = 3, name = "전설의 횃불", level = 1, possession = 1, grade = "Normal", subGrade = "D", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[4] = new WeaponSaveData { id = 4, name = "낡은 밀대", level = 1, possession = 1, grade = "Magic", subGrade = "A", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[5] = new WeaponSaveData { id = 5, name = "평범한 밀대", level = 1, possession = 1, grade = "Magic", subGrade = "B", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[6] = new WeaponSaveData { id = 6, name = "정교한 밀대", level = 1, possession = 1, grade = "Magic", subGrade = "C", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[7] = new WeaponSaveData { id = 7, name = "전설의 밀대", level = 1, possession = 1, grade = "Magic", subGrade = "D", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[8] = new WeaponSaveData { id = 8, name = "낡은 잠자리채", level = 1, possession = 1, grade = "Rare", subGrade = "A", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[9] = new WeaponSaveData { id = 9, name = "평범한 잠자리채", level = 1, possession = 1, grade = "Rare", subGrade = "B", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[10] = new WeaponSaveData { id = 10, name = "정교한 잠자리채", level = 1, possession = 1, grade = "Rare", subGrade = "C", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[11] = new WeaponSaveData { id = 11, name = "전설의 잠자리채", level = 1, possession = 1, grade = "Rare", subGrade = "D", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[12] = new WeaponSaveData { id = 12, name = "낡은 공구", level = 1, possession = 1, grade = "Unique", subGrade = "A", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[13] = new WeaponSaveData { id = 13, name = "평범한 공구", level = 1, possession = 1, grade = "Unique", subGrade = "B", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[14] = new WeaponSaveData { id = 14, name = "정교한 공구", level = 1, possession = 1, grade = "Unique", subGrade = "C", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[15] = new WeaponSaveData { id = 15, name = "전설의 공구", level = 1, possession = 1, grade = "Unique", subGrade = "D", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[16] = new WeaponSaveData { id = 16, name = "낡은 삽", level = 1, possession = 1, grade = "Epic", subGrade = "A", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[17] = new WeaponSaveData { id = 17, name = "평범한 삽", level = 1, possession = 1, grade = "Epic", subGrade = "B", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[18] = new WeaponSaveData { id = 18, name = "정교한 삽", level = 1, possession = 1, grade = "Epic", subGrade = "C", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[19] = new WeaponSaveData { id = 19, name = "전설의 삽", level = 1, possession = 1, grade = "Epic", subGrade = "D", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[20] = new WeaponSaveData { id = 20, name = "낡은 갈고리", level = 1, possession = 1, grade = "Legend", subGrade = "A", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[21] = new WeaponSaveData { id = 21, name = "평범한 갈고리", level = 1, possession = 1, grade = "Legend", subGrade = "B", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[22] = new WeaponSaveData { id = 22, name = "정교한 갈고리", level = 1, possession = 1, grade = "Legend", subGrade = "C", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[23] = new WeaponSaveData { id = 23, name = "전설의 갈고리", level = 1, possession = 1, grade = "Legend", subGrade = "D", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[24] = new WeaponSaveData { id = 24, name = "낡은 도끼", level = 1, possession = 1, grade = "Star", subGrade = "A", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[25] = new WeaponSaveData { id = 25, name = "평범한 도끼", level = 1, possession = 1, grade = "Star", subGrade = "B", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[26] = new WeaponSaveData { id = 26, name = "정교한 도끼", level = 1, possession = 1, grade = "Star", subGrade = "C", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[27] = new WeaponSaveData { id = 27, name = "전설의 도끼", level = 1, possession = 1, grade = "Star", subGrade = "D", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[28] = new WeaponSaveData { id = 28, name = "낡은 톱", level = 1, possession = 1, grade = "Galaxy", subGrade = "A", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[29] = new WeaponSaveData { id = 29, name = "평범한 톱", level = 1, possession = 1, grade = "Galaxy", subGrade = "B", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[30] = new WeaponSaveData { id = 30, name = "정교한 톱", level = 1, possession = 1, grade = "Galaxy", subGrade = "C", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };
        //gameData.weapons[31] = new WeaponSaveData { id = 31, name = "전설의 톱", level = 1, possession = 1, grade = "Galaxy", subGrade = "D", coin = 0, status = 0, nextStatus = 0, levelUpCost = 0, effect = 0, isLock = true };

        //gameData.skills[0] = new SkillSaveData { id = 0, name = "우주 냥경찰 출동!", level = 1, possession = 1, type = "Active", effect = 0, levelUpCost = 0, isLock = true, subType = "None" };
        //gameData.skills[1] = new SkillSaveData { id = 1, name = "캣닢비가 내려와", level = 1, possession = 1, type = "Active", effect = 0, levelUpCost = 0, isLock = true, subType = "None" };
        //gameData.skills[2] = new SkillSaveData { id = 2, name = "냥냥대원들아 도와줘!", level = 1, possession = 1, type = "Active", effect = 0, levelUpCost = 0, isLock = true, subType = "None" };
        //gameData.skills[3] = new SkillSaveData { id = 3, name = "자린고비냥", level = 1, possession = 1, type = "Active", effect = 0, levelUpCost = 0, isLock = true, subType = "None" };
        //gameData.skills[4] = new SkillSaveData { id = 4, name = "실타래 폭탄", level = 1, possession = 1, type = "Active", effect = 0, levelUpCost = 0, isLock = true, subType = "None" };
        //gameData.skills[5] = new SkillSaveData { id = 5, name = "냥냥 마법빵", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "Recover" };
        //gameData.skills[6] = new SkillSaveData { id = 6, name = "생선 간식 타임", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "Recover" };
        //gameData.skills[7] = new SkillSaveData { id = 7, name = "햇볕 낮잠", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "Recover" };
        //gameData.skills[8] = new SkillSaveData { id = 8, name = "핑크 젤리 힐링", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "Recover" };
        //gameData.skills[9] = new SkillSaveData { id = 9, name = "털복숭이 갑옷", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "DefenceUp" };
        //gameData.skills[10] = new SkillSaveData { id = 10, name = "꼬리 방패", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "DefenceUp" };
        //gameData.skills[11] = new SkillSaveData { id = 11, name = "퐁퐁 쿠션 아머", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "DefenceUp" };
        //gameData.skills[12] = new SkillSaveData { id = 12, name = "튼튼한 발톱 가드", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "DefenceUp" };
        //gameData.skills[13] = new SkillSaveData { id = 13, name = "뾰족뾰족 수염", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "AttackUp" };
        //gameData.skills[14] = new SkillSaveData { id = 14, name = "냥편치 파워업!", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "AttackUp" };
        //gameData.skills[15] = new SkillSaveData { id = 15, name = "캣닢의 힘", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "AttackUp" };
        //gameData.skills[16] = new SkillSaveData { id = 16, name = "우다다 돌진", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "AttackUp" };
        //gameData.skills[17] = new SkillSaveData { id = 17, name = "시간 멈춰라냥", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "CoolTime" };
        //gameData.skills[18] = new SkillSaveData { id = 18, name = "우다다 가속모드", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "CoolTime" };
        //gameData.skills[19] = new SkillSaveData { id = 19, name = "사뿐사뿐 이동", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "CoolTime" };
        //gameData.skills[20] = new SkillSaveData { id = 20, name = "깜찍한 깜짝 점프", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "CoolTime" };
        //gameData.skills[21] = new SkillSaveData { id = 21, name = "젤리 파워업", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "HPUp" };
        //gameData.skills[22] = new SkillSaveData { id = 22, name = "모래 목욕", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "HPUp" };
        //gameData.skills[23] = new SkillSaveData { id = 23, name = "포동포동 냥이", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "HPUp" };
        //gameData.skills[24] = new SkillSaveData { id = 24, name = "별빛 힐링", level = 1, possession = 1, type = "Passive", effect = 0, levelUpCost = 0, isLock = true, subType = "HPUp" };

        //jsonManager.SaveSkillData(gameData);

        gameData = jsonManager.LoadGameData();
        
    }

    public void GetSkillSaveData()
    {
        Debug.Log(gameData.skills[0].name);
    }
}
