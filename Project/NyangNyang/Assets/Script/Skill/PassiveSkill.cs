public abstract class PassiveSkill : Skill
{
    public PassiveSkill(int _id, string _name, int _possession, int _level, int _levelUpCost) 
        : base(_id, _name, _possession, _level, _levelUpCost)
    {
    }

    public override void Activate()
    {
        // passive skill이 실행된다는 문구 띄우기

        ApplyEffect();
    }

    public override void Deactivate()
    {
        // passive skill이 끝났다는 문구 띄우기

        DetachEffect();
    }

    public abstract void ApplyEffect();
    
    public abstract void DetachEffect();

    
}
