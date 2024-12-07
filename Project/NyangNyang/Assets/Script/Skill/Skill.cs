using StatusEffects;

public abstract class Skill
{
    protected int ID;
    protected string name;
    protected int count;
    protected string type;
    bool isLock = true;

    protected float effect;
    protected int level;
    protected int coin;

    protected string ment;

    public Skill(int _id, string _name, int _count, string _type, bool _isLock, float _effect,  int _level, int _coin, string _ment)
    {
        ID = _id;
        name = _name;
        count = _count;
        type = _type;
        isLock = _isLock;
        effect = _effect;
        level = _level;
        coin = _coin;
        ment = _ment;
    }

    public int GetID() => ID;
    public string GetName() => name;

    // 소지량 관련
    public int GetCount() => count;
    public void SetCount(int _count) => count += _count;
    public bool HasSkill() => count > 0;

    // 레벨 관련
    public int GetLevel() => level;
    public int AddLevel(int count) => level += count;

    public void SetCoin(int _amount) => coin = _amount;
    public int GetCoin() => coin;


    public void SetEffect(float _amount) => effect = _amount;
    public float GetEffect() => effect;

    public string GetSkillType() => type;

    // Lock 관련
    public bool GetIsLock() => isLock;
    public void SetIsLockToFALSE() => isLock = false;

    // 추상 함수 관련
    public abstract void Activate();
    public abstract void Deactivate();
}
