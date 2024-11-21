public abstract class ActiveSkill : Skill
{
    float coolTime;

    public ActiveSkill(int _id, string _name, int _possession, int _level, int _levelUpCost)
    : base(_id, _name, _possession, _level, _levelUpCost)
    {
    }

    public override void Activate()
    {
        // 각 스킬에 맞게 설정

        ApplyEffect();
    }

    public override void Deactivate()
    {
        // 각 스킬에 맞게 설정
    }

    public abstract void ApplyEffect();
}
