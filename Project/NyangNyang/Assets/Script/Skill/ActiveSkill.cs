public class ActiveSkill : Skill
{
    float coolTime;

    public ActiveSkill(int _id, string _name, int _possession, int _level, int _levelUpCost, float _effect, string _type)
    : base(_id, _name, _possession, _level, _levelUpCost, _effect, _type)
    {
    }

    public override void Activate()
    {
        // �� ��ų�� �°� ����
    }

    public override void Deactivate()
    {
        // �� ��ų�� �°� ����
    }
}
