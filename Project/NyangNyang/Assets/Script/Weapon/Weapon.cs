public class Weapon
{
    int ID;             // �������� �ۼ����� ���� ��� id
    string name;        // ��� �̸�
    int count;     // ��� ����
    string grade;          // ��� ���
    string subGrade;       // ���� ���
    bool isLock = true;

    float effect;
    int level;
    int coin;

    string ment;


    public Weapon(int _id, string _name, int _count, string _grade, string _subGrade, bool _isLock, float _effect, int _level, int _coin, string _ment)
    {
        ID = _id;
        name = _name;
        count = _count;
        grade = _grade;
        subGrade = _subGrade;
        isLock = _isLock;
        effect = _effect;
        level = _level;
        coin = _coin;
        ment = _ment;
    }

    //public void SetWeaponData(string _name, int _grade, int _subGrade, int _level, int _count)
    //{
    //    name = _name;
    //    count = _count;
    //    grade = _grade;
    //    subGrade = _subGrade;
    //    level = _level;
    //}
    public int GetID() {  return ID; }

    public string GetName()
    {
        return name;
    }

    public bool GetIsLock()
    {
        return isLock;
    }

    public void SetIsLockToFALSE()
    {
        isLock = false;
    }

    public void AddWeapon(int _count)
    {
        count += _count;
    }

    public bool HasWeapon()
    {
        return count > 0;
    }

    public int SetLevel()
    {
        level++;
        coin += level;
        // �뷱�� ��ġ �ʿ�
        return coin + level;
    }

    //public int StatusUpgrade()
    //{
    //    status = nextStatus;
    //    nextStatus += 10;
    //    return nextStatus;
    //}

    public int GetCount()
    {
        return count;
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetCoin()
    {
        return coin;
    }

    //public int GetCurStatus()
    //{
    //    return status;
    //}

    //public int GetNextStatus()
    //{
    //    return nextStatus;
    //}

    public float GetEffect()
    {
        return effect;
    }
}
