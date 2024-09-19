public class Weapon
{
    int id;             // 서버와의 송수신을 위한 장비 id
    string name;        // 장비 이름
    int possession;     // 장비 개수
    int grade;          // 장비 등급
    int subGrade;       // 세부 등급

    int status;          // 장비 세부 능력치
    int level;          // 장비 레벨


    public Weapon(string _name, int _grade, int _subGrade, int _level, int _possession)
    {
        name = _name;
        possession = _possession;
        grade = _grade;
        subGrade = _subGrade;
        level = _level;
    }

    public void SetWeaponData(string _name, int _grade, int _subGrade, int _level, int _possession)
    {
        name = _name;
        possession = _possession;
        grade = _grade;
        subGrade = _subGrade;
        level = _level;
    }

    public void LevelUP()
    {
        // TODO: 레벨업 로직 만들기
        level++;
    }

    public void AddWeapon(int count)
    {
        possession += count;
    }

    public bool HasWeapon()
    {
        return possession > 0;
    }

    public int GetWeaponCount()
    {
        return possession;
    }
}
