public class ActiveSkill : Skill
{
    float coolTime;

    public ActiveSkill(int _id, string _name, int _count, string _type, bool _isLock, float _effect, int _level, int _coin, string _ment)
        : base(_id, _name, _count, _type, _isLock, _effect, _level, _coin, _ment)
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
