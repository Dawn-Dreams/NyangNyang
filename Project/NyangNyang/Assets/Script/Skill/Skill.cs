using StatusEffects;

public abstract class Skill
{
    protected int id;
    protected string name;
    protected int possession;
    protected int level;
    protected int levelUpCost;
    protected float effect;
    bool isLock = true;

    public Skill(int _id, string _name, int _possession, int _level, int _levelUpCost, float _effect)
    {
        id = _id;
        name = _name;
        possession = _possession;
        level = _level;
        levelUpCost = _levelUpCost;
        effect = _effect;
    }

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

    // Lock 관련
    public bool GetIsLock() => isLock;
    public void SetIsLockToTrue() => isLock = false;

    // 추상 함수 관련
    public abstract void Activate();
    public abstract void Deactivate();
}
