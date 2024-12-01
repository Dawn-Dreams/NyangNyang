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

    public static string GetStatusTypeText(StatusLevelType type)
    {
        if (!statusTypeDescription.ContainsKey(type))
        {
            return "NULL";
        }

        return statusTypeDescription[type];
    }
}
