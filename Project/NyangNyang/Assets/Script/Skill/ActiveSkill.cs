public class ActiveSkill : Skill
{
    float coolTime;

    public ActiveSkill(int _id, string _name, int _possession, int _level, int _levelUpCost, float _effect)
    : base(_id, _name, _possession, _level, _levelUpCost, _effect)
    {
    }

    public override void Activate()
    {
        // 각 스킬에 맞게 설정
    }

    public override void Deactivate()
    {
        // 각 스킬에 맞게 설정
    }
}
