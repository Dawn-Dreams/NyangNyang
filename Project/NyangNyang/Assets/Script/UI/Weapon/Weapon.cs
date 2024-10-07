public class Weapon
{
    int id;             // �������� �ۼ����� ���� ��� id
    string name;        // ��� �̸�
    int possession;     // ��� ����
    int grade;          // ��� ���
    int subGrade;       // ���� ���

    int status;          // ��� ���� �ɷ�ġ
    int nextStatus = 10;     // ���� ����� ���� �ɷ�ġ
    int level;          // ��� ����
    int coin = 1;           // ��� ���� �� �� �ʿ��� ���� �ʱⰪ


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
