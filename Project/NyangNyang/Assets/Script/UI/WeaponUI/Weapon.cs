public class Weapon
{
    int id;             // �������� �ۼ����� ���� ��� id
    string name;        // ��� �̸�
    int possession;     // ��� ����
    int grade;          // ��� ���
    int subGrade;       // ���� ���

    int status;          // ��� ���� �ɷ�ġ
    int level;          // ��� ����


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
    public void LevelUP()
    {
        // TODO: ������ ���� �����
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
