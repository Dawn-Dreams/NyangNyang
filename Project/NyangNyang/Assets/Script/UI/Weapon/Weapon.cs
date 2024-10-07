public class Weapon
{
    int id;             // 서버와의 송수신을 위한 장비 id
    string name;        // 장비 이름
    int possession;     // 장비 개수
    int grade;          // 장비 등급
    int subGrade;       // 세부 등급

    int status;          // 장비 세부 능력치
    int nextStatus = 10;     // 다음 장비의 세부 능력치
    int level;          // 장비 레벨
    int coin = 1;           // 장비 레벨 업 시 필요한 코인 초기값


    public Weapon(int _id, string _name, int _grade, int _subGrade, int _level, int _possession)
    {
        id = _id;
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
    public int GetID() {  return id; }

    public string GetName()
    {
        return name;
    }

    public void AddWeapon(int count)
    {
        possession += count;
    }

    public bool HasWeapon()
    {
        return possession > 0;
    }

    public int LevelUP()
    {
        level++;
        return coin + level * (grade * 10 + subGrade);
    }

    public int StatusUpgrade()
    {
        status = nextStatus;
        nextStatus += 10;
        return nextStatus;
    }

    public int GetWeaponCount()
    {
        return possession;
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetNeedCoin()
    {
        return coin;
    }

    public int GetCurStatus()
    {
        return status;
    }

    public int GetNextStatus()
    {
        return nextStatus;
    }
}
