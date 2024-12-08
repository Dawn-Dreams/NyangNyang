using System.Collections.Generic;

public class EnumTranslator
{
    private static Dictionary<StatusLevelType, string> statusTypeDescription = new Dictionary<StatusLevelType, string>()
    {
        { StatusLevelType.HP, "최대 생명력" },
        { StatusLevelType.MP, "최대 마나" },
        { StatusLevelType.STR, "공격력" },
        { StatusLevelType.DEF, "방어력" },
        { StatusLevelType.HEAL_HP, "초당 체력 회복" },
        { StatusLevelType.HEAL_MP, "초당 마나 회복" },
        { StatusLevelType.CRIT, "치명타 확률" },
        { StatusLevelType.ATTACK_SPEED, "공격속도" },
        { StatusLevelType.GOLD, "골드 획득량" },
        { StatusLevelType.EXP, "경험치 획득량" },
    };

    // 코스튬 - 머리
    private static Dictionary<HeadCostumeType, string> headCostumeDescription =
        new Dictionary<HeadCostumeType, string>()
        {
            { HeadCostumeType.Meat, "만화 고기" },
            { HeadCostumeType.NotEquip, " " },
            { HeadCostumeType.HikingHat, "등산모" },
            { HeadCostumeType.Grass, "잔디" },
            { HeadCostumeType.Glass_Red, "안경" }
        };
    // 코스튬 - 머리
    private static Dictionary<BodyCostumeType, string> bodyCostumeDescription = new Dictionary<BodyCostumeType, string>()
    {
        { BodyCostumeType.NotEquip, ""},
        {BodyCostumeType.Bag, "가방"},
        {BodyCostumeType.Fish, "물고기"},
        {BodyCostumeType.Pan, "후라이팬"},
    };
    // 코스튬 - 머리
    private static Dictionary<HandRCostumeType, string> handRCostumeDescription =
        new Dictionary<HandRCostumeType, string>()
        {
            { HandRCostumeType.NotEquip, "" },
            { HandRCostumeType.Flower, "꽃" },
            { HandRCostumeType.Fork, "포크" },
            { HandRCostumeType.PinWheel, "바람개비" },
            { HandRCostumeType.Wood, "지팡이" },
        };
    // 코스튬 - 머리
    private static Dictionary<CatFurSkin, string> catFurSkinCostumeDescription = new Dictionary<CatFurSkin, string>()
    {
        { CatFurSkin.White, "하얀 냥이" },
        { CatFurSkin.Sand, "모래 냥이" },
        { CatFurSkin.Cheese, "치즈 냥이" },
        { CatFurSkin.Mackerel, "고등어 냥이" },
        { CatFurSkin.Siamese, "샴 냥이" },
        { CatFurSkin.RagDoll, "랙돌 냥이" },
        { CatFurSkin.ThreeColor, "삼색 냥이" },
        { CatFurSkin.Socks, "양말 냥이" },
        { CatFurSkin.Tuxedo, "턱시도 냥이" },
        { CatFurSkin.Snow, "눈 냥이" },
    };
    // 코스튬 - 머리
    private static Dictionary<EnemyMonsterType, string> enemyCostumeDescription =
        new Dictionary<EnemyMonsterType, string>()
        {
            { EnemyMonsterType.Porin_A, "포린" }, { EnemyMonsterType.Porin_B, "포포린" }, { EnemyMonsterType.Racco, "라코" },
            { EnemyMonsterType.Platopo, "플래토" }, { EnemyMonsterType.Snailo, "스네일로" },
            { EnemyMonsterType.StarFish, "별가사리" }, { EnemyMonsterType.Octopus, "옥토" }, { EnemyMonsterType.Puffe, "푸페" },
            { EnemyMonsterType.Shellfish, "껍데기조개" }, { EnemyMonsterType.Krake, "크라켄" },
            { EnemyMonsterType.Rato, "라토" }, { EnemyMonsterType.Cactuso, "케투스" }, { EnemyMonsterType.Wormo, "워모" },
            { EnemyMonsterType.Scopionto, "스콜피온" }, { EnemyMonsterType.MegaGolem, "메가 골렘" },
            { EnemyMonsterType.MiniIceCube, "작은 얼음" }, { EnemyMonsterType.MiniIceBear, "작은 곰" },
            { EnemyMonsterType.IceBear, "설산 곰" }, { EnemyMonsterType.IceStarflake, "설산 정령" },
            { EnemyMonsterType.IceGolem, "설산 골렘" },
            { EnemyMonsterType.Gazar, "가자르" }, { EnemyMonsterType.Tempora_A, "템포" },
            { EnemyMonsterType.Tempora_B, "템포라" }, { EnemyMonsterType.FirePig, "화산 돼지" },
            { EnemyMonsterType.FireGolem, "화산 골렘" },
        };
    // 코스튬 - 머리
    private static Dictionary<EmotionCostumeType, string> emotionCostumeDescription =
        new Dictionary<EmotionCostumeType, string>()
        {
            { EmotionCostumeType.Smile, "미소" },
            { EmotionCostumeType.Questioning, "의문" },
            { EmotionCostumeType.CatMouth, "고양이 입" },
            { EmotionCostumeType.BrightlySmile, "환한 미소" },
            { EmotionCostumeType.WhatsWrong, "왜?" },
            { EmotionCostumeType.Ggyu, "뀨" },
        };

    private static Dictionary<CatCostumePart, string> catCostumePartDescription =
        new Dictionary<CatCostumePart, string>()
        {
            { CatCostumePart.Body, "몸" },
            { CatCostumePart.Head, "머리" },
            { CatCostumePart.Hand_R, "손" },
            { CatCostumePart.FurSkin, "털색" },
            { CatCostumePart.Pet, "펫" },
            { CatCostumePart.Emotion, "표정" },
        };

    public static string GetStatusTypeText(StatusLevelType type)
    {
        if (!statusTypeDescription.ContainsKey(type))
        {
            return "NULL";
        }

        return statusTypeDescription[type];
    }

    public static string GetCatCostumePartText(CatCostumePart part)
    {
        if (!catCostumePartDescription.ContainsKey(part))
        {
            return "NULL";
        }

        return catCostumePartDescription[part];
    }
    public static string GetCostumeText(CatCostumePart part, int index)
    {
        string retStr = "";
        switch (part)
        {
            case CatCostumePart.Head:
                retStr = headCostumeDescription[(HeadCostumeType)index];
                break;
            case CatCostumePart.Hand_R:
                retStr = handRCostumeDescription[(HandRCostumeType)index];
                break;
            case CatCostumePart.Body:
                retStr = bodyCostumeDescription[(BodyCostumeType)index];
                break;
            case CatCostumePart.FurSkin:
                retStr = catFurSkinCostumeDescription[(CatFurSkin)index];
                break;
            case CatCostumePart.Pet:
                if (enemyCostumeDescription.ContainsKey((EnemyMonsterType)index))
                {
                    retStr = enemyCostumeDescription[(EnemyMonsterType)index];
                }
                break;
            case CatCostumePart.Emotion:
                retStr = emotionCostumeDescription[(EmotionCostumeType)index];
                break;

            default:
                break;
        }

        return retStr;
    }
}
