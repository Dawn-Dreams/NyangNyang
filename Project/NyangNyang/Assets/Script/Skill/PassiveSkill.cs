public abstract class PassiveSkill : Skill
{
    public PassiveSkill(int _id, string _name, int _count, string _type, bool _isLock, float _effect, int _level, int _coin, string _ment) 
        : base( _id,  _name,  _count,  _type,  _isLock,  _effect,  _level,  _coin,  _ment)
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
