public class PassiveSkill : Skill
{

    SkillSubType SubType;

    public PassiveSkill(int _id, string _name, int _possession, int _level, int _levelUpCost) 
        : base(_id, _name, _possession, _level, _levelUpCost)
    {
    }

    public override void PlaySkill()
    {
        // subtype에 맞게 설정
    }
}
