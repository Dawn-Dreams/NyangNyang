public abstract class Skill
{
    protected int id;
    protected string name;
    protected int possession;
    protected int level;
    protected int levelUpCost;

    public int GetID() => id;
    public string GetName() => name;

    // 소지량 관련
    public int GetPossession() => possession;
    public void SetPossession(int count) => possession += count;
    public bool HasSkill() => possession > 0;

    // 레벨 관련
    public int GetLevel() => level;
    public int AddLevel(int count) => level += count;

    public void SetLevelUpCost(int count) => levelUpCost = count;
    public int GetLevelUpCost() => levelUpCost;


    public Skill(int _id, string _name, int _possession, int _level, int _levelUpCost)
    {
        id = _id;
        name = _name;
        possession = _possession;
        level = _level;
        levelUpCost = _levelUpCost;
    }

    public abstract void PlaySkill();
}
