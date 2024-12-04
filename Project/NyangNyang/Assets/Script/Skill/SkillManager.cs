using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private static SkillManager instance;
    public static SkillManager GetInstance() => instance;

    private Skill[] skills = new Skill[25];
    private SkillInfo[] skillDatas = new SkillInfo[25];
    private Dictionary<string, int> skillDic = new Dictionary<string, int>();
    public Sprite[] sprites;

    // 밸런스 패치 필요
    private int[] levelUpCosts = new int[9] { 5000, 10000, 50000, 100000, 250000, 500000, 1000000, 2500000, 5000000};
    private int[] levelUpNeeds = new int[9] { 5, 10, 20, 20, 30, 50, 100, 500, 1000 };

    private void Awake()
    {
        if ( instance == null)
        {
            instance = this;
        }
        InitializedSkills();
    }

    public void InitializedSkills()
    {
        /*
         
        //skillDatas[0] = new SkillInfo { ID = 0, name = "우주 냥경찰 출동!", count = 0, type = "Active", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"고양이가 탑승한 우주선이 상층부에서 날아와서 수직 우주선 빔\"" };
        //skillDatas[1] = new SkillInfo { ID = 1, name = "캣닢비가 내려와", count = 0, type = "Active", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"하늘에서 캣닢이 낙엽 떨어지듯이 떨어져서 고양이 공격속도 n배 증가\"" };
        //skillDatas[2] = new SkillInfo { ID = 2, name = "냥냥대원들아 도와줘!", count = 0, type = "Active", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"고양이 여러마리 달려오면서 먼지 이는 이펙트\"" };
        //skillDatas[3] = new SkillInfo { ID = 3, name = "자린고비냥", count = 0, type = "Active", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"눈 앞에 날개달린 치즈, 따라가면서 속도 증가 + 체력증가 + 공격력 증가 (n초)\"" };
        //skillDatas[4] = new SkillInfo { ID = 4, name = "실타래 폭탄", count = 0, type = "Active", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"맵 왼쪽 밖에서 실타래가 포물선으로 날아와서 적한테서 펑 터지기\"" };
        //skillDatas[5] = new SkillInfo { ID = 5, name = "냥냥 마법빵", count = 0, type = "Recover", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"냥이의 최애 간식! 마법 빵을 먹으며 체력을 회복한다.\"" };
        //skillDatas[6] = new SkillInfo { ID = 6, name = "생선 간식 타임", count = 0, type = "Recover", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"먹으면 기운이 불끈! 체력을 회복한다.\"" };
        //skillDatas[7] = new SkillInfo { ID = 7, name = "햇볕 낮잠", count = 0, type = "Recover", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"따뜻한 햇볕에서 낮잠을 자며 체력을 회복한다.\"" };
        //skillDatas[8] = new SkillInfo { ID = 8, name = "핑크 젤리 힐링", count = 0, type = "Recover", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"발바닥 젤리의 귀여운 에너지로 체력을 회복한다.\"" };
        //skillDatas[9] = new SkillInfo { ID = 9, name = "털복숭이 갑옷", count = 0, type = "Defence", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"폭신한 털 덕분에 냥이의 방어력이 증가한다.\"" };
        //skillDatas[10] = new SkillInfo { ID = 10, name = "꼬리 방패", count = 0, type = "Defence", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"냥이의 꼬리 방어술로 방어력이 증가한다.\"" };
        //skillDatas[11] = new SkillInfo { ID = 11, name = "퐁퐁 쿠션 아머", count = 0, type = "Defence", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"푹신푹신한 쿠션으로 방어력이 증가한다.\"" };
        //skillDatas[12] = new SkillInfo { ID = 12, name = "튼튼한 발톱 가드", count = 0, type = "Defence", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"발톱이 강해지며 방어력이 증가한다.\"" };
        //skillDatas[13] = new SkillInfo { ID = 13, name = "뾰족뾰족 수염", count = 0, type = "Attack", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"수염이 반짝이며 적에게 더 강력한 타격을 가한다.\"" };
        //skillDatas[14] = new SkillInfo { ID = 14, name = "냥편치 파워업!", count = 0, type = "Attack", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"펀치력이 강화되며 적에게 더 강력한 타격을 가한다.\"" };
        //skillDatas[15] = new SkillInfo { ID = 15, name = "캣닢의 힘", count = 0, type = "Attack", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"캣닢을 먹어 적에게 더 강력한 타격을 가한다.\"" };
        //skillDatas[16] = new SkillInfo { ID = 16, name = "우다다 돌진", count = 0, type = "Attack", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"우다다 속도로 달려가 적에게 더 강력한 타격을 가한다.\"" };
        //skillDatas[17] = new SkillInfo { ID = 17, name = "시간 멈춰라냥", count = 0, type = "CoolTime", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"냥이의 능력으로 스킬 쿨타임이 빨라진다.\"" };
        //skillDatas[18] = new SkillInfo { ID = 18, name = "우다다 가속모드", count = 0, type = "CoolTime", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"우다다 스피드 덕분에 스킬 쿨타임이 빨라진다.\"" };
        //skillDatas[19] = new SkillInfo { ID = 19, name = "사뿐사뿐 이동", count = 0, type = "CoolTime", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"조용하고 빠르게 움직이며 스킬 쿨타임이 빨라진다.\"" };
        //skillDatas[20] = new SkillInfo { ID = 20, name = "깜찍한 깜짝 점프", count = 0, type = "CoolTime", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"냥이의 깜찍한 점프로 스킬 쿨타임이 빨라진다.\"" };
        //skillDatas[21] = new SkillInfo { ID = 21, name = "젤리 파워업!", count = 0, type = "Health", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"발바닥 젤리가 더욱 강력해지며 기본 체력이 증가한다.\"" };
        //skillDatas[22] = new SkillInfo { ID = 22, name = "모래 목욕", count = 0, type = "Health", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"모래 목욕을 하면 피로가 풀리고 체력이 증가한다.\"" };
        //skillDatas[23] = new SkillInfo { ID = 23, name = "포동포동 냥이", count = 0, type = "Health", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"체중이 늘어나며 기본 체력이 증가한다.\"" };
        //skillDatas[24] = new SkillInfo { ID = 24, name = "별빛 힐링", count = 0, type = "Health", isLock = true, effect = 0f, level = 1, coin = 1, ment = "\"별빛을 받으면 기본 체력이 증가한다.\"" };
       
         */
        skillDatas = SaveDataManager.GetInstance().LoadSkills();
        
        for (int i = 0; i < 25; ++i)
        {
            MatchSkillFromSkillData(i);
        }
    }

    public void MatchSkillDataFromSkill(int id)
    {
        skillDatas[id].count = skills[id].GetCount();
        skillDatas[id].isLock = skills[id].GetIsLock();
        skillDatas[id].effect = skills[id].GetEffect();
        skillDatas[id].level = skills[id].GetLevel();
        skillDatas[id].coin = skills[id].GetCoin();
    }

    public void MatchSkillFromSkillData(int id)
    {
        switch (skillDatas[id].type)
        {
            case "Active":
                {
                    skills[id] = new ActiveSkill(skillDatas[id].ID, skillDatas[id].name, skillDatas[id].count, skillDatas[id].type, skillDatas[id].isLock, skillDatas[id].effect, skillDatas[id].level, skillDatas[id].coin, skillDatas[id].ment);

                    break;
                }
            case "Recover":
                {
                    skills[id] = new RecoverHPSkill(skillDatas[id].ID, skillDatas[id].name, skillDatas[id].count, skillDatas[id].type, skillDatas[id].isLock, skillDatas[id].effect, skillDatas[id].level, skillDatas[id].coin, skillDatas[id].ment);
                    break;
                }
            case "Defence":
                {
                    skills[id] = new DefenseUpSkill(skillDatas[id].ID, skillDatas[id].name, skillDatas[id].count, skillDatas[id].type, skillDatas[id].isLock, skillDatas[id].effect, skillDatas[id].level, skillDatas[id].coin, skillDatas[id].ment);
                    break;
                }
            case "Attack":
                {
                    skills[id] = new AttackUpSkill(skillDatas[id].ID, skillDatas[id].name, skillDatas[id].count, skillDatas[id].type, skillDatas[id].isLock, skillDatas[id].effect, skillDatas[id].level, skillDatas[id].coin, skillDatas[id].ment);
                    break;
                }
            case "CoolTime":
                {
                    skills[id] = new CoolTimeDownSkill(skillDatas[id].ID, skillDatas[id].name, skillDatas[id].count, skillDatas[id].type, skillDatas[id].isLock, skillDatas[id].effect, skillDatas[id].level, skillDatas[id].coin, skillDatas[id].ment);
                    break;
                }
            case "Health":
                {
                    skills[id] = new HealthUpSkill(skillDatas[id].ID, skillDatas[id].name, skillDatas[id].count, skillDatas[id].type, skillDatas[id].isLock, skillDatas[id].effect, skillDatas[id].level, skillDatas[id].coin, skillDatas[id].ment);
                    break;
                }
        }
    }

    public Skill GetSkill(int id)
    {
        if ( id >= 0 && id < skills.Length)
        {
            return skills[id];
        }
        return null;
    }

    public Skill GetSkill(string name)
    {
        int id;
        if (skillDic.TryGetValue(name, out id))
        {
            return skills[id];
        }
        return null;
    }

    public Sprite GetSprite(int id)
    {
        if ( id >= 0 && id < skills.Length)
        {
            return sprites[id];
        }
        return null;
    }

    public int LevelUpSkill(int id)
    {
        Skill skill = GetSkill(id);
        if ( skill != null && skill.GetLevel() < 10 && skill.GetCount() >= levelUpNeeds[skill.GetLevel()-1] )
        {
            skill.SetCount(-levelUpNeeds[skill.GetLevel() - 1]);
            skill.SetCoin(levelUpCosts[skill.GetLevel() - 1]);
            skill.AddLevel(1);
            return skill.GetLevel();
        }
        return -1;
    }

    public void AddSkillPossession(int id, int count)
    {
        Skill skill = GetSkill(id);
        if ( skill != null )
        {
            skill.SetCount(count);

            // 11.12 이윤석 - 스킬 획득 퀘스트
            if (QuestManager.GetInstance().OnUserGetSkill != null)
            {
                QuestManager.GetInstance().OnUserGetSkill(count);
            }
        }
    }

    public int GetLevelUpCostPerLevel(int level)
    {
        if ( level >= 0 && level < levelUpCosts.Length)
        {
            return levelUpNeeds[level];
        }
        return -1;
    }

    public void LetSkillActivate(int id)
    {
        Skill skill = GetSkill(id);
        if ( skill != null)
        {
            skill.Activate();
        }
    }

    public void LetSkillDeActivate(int id)
    {
        Skill skill = GetSkill(id);
        if (skill != null)
        {
            skill.Deactivate();
        }
    }

}
