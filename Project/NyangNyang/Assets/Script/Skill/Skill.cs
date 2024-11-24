using StatusEffects;

public abstract class Skill
{
    protected int id;
    protected string name;
    protected int possession;
    protected int level;
    protected int levelUpCost;
    protected float effect;

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

    // ������ ����
    public int GetPossession() => possession;
    public void SetPossession(int count) => possession += count;
    public bool HasSkill() => possession > 0;

    // ���� ����
    public int GetLevel() => level;
    public int AddLevel(int count) => level += count;

    public void SetLevelUpCost(int count) => levelUpCost = count;
    public int GetLevelUpCost() => levelUpCost;


    // �߻� �Լ� ����
    public abstract void Activate();
    public abstract void Deactivate();
}
