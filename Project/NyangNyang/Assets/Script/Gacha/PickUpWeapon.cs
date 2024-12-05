using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PickUpWeapon : MonoBehaviour
{
    public GameObject AllContent;
    public GameObject OneContent;

    private void OnDisable()
    {
        AllContent.SetActive(false);
        OneContent.SetActive(false);
    }
    // ���� ID: 0 ~ 31
    private const int TotalWeapons = 32;

    // Ȯ�� ���̺� (������ Ȯ��)
    private Dictionary<int, float[]> probabilityTable;

    private void Awake()
    {
        InitializeProbabilityTable();

    }
    // Ȯ�� ���̺� �ʱ�ȭ
    private void InitializeProbabilityTable()
    {
        probabilityTable = new Dictionary<int, float[]>();

        // ������ Ȯ�� ���� (���ǰ�, �ʿ�� ���� ����)
        for (int level = 1; level <= 10; level++)
        {
            probabilityTable[level] = new float[TotalWeapons];

            for (int i = 0; i < TotalWeapons; i++)
            {
                if (i < 8) probabilityTable[level][i] = 0.02f / level; // Normal
                else if (i < 16) probabilityTable[level][i] = 0.03f / level; // Magic
                else if (i < 24) probabilityTable[level][i] = 0.04f / level; // Rare
                else probabilityTable[level][i] = 0.05f / level; // Unique ~ Galaxy
            }
        }
    }
    // ���� �̱� �Լ�
    public int DrawWeapon(int level)
    {
        if (!probabilityTable.ContainsKey(level))
        {
            Debug.LogError("Invalid level provided for weapon draw.");
            return -1;
        }

        float[] probabilities = probabilityTable[level];
        float totalProbability = 0;

        // Ȯ�� �հ� ���
        foreach (float prob in probabilities)
        {
            totalProbability += prob;
        }

        // ���� �� ����
        float randomValue = UnityEngine.Random.Range(0f, totalProbability);
        float cumulativeProbability = 0f;

        // ���� ���� ���� ���� ����
        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulativeProbability += probabilities[i];
            if (randomValue <= cumulativeProbability)
            {
                return i;
            }
        }

        // ���� ó��
        Debug.LogError("Failed to draw weapon due to probability calculation error.");
        return -1;
    }
    public void ShowPickUpWeapon()
    {
        // �� �� �̱�

        OneContent.SetActive(true);

        // TODO: OneContent ���� ���� �ۼ��ϱ� �̱⿡�� ���� �������**
        int id = DrawWeapon(1);

        SetPickUPWeapon(id, OneContent);
    }

    public void ShowPickUpWeapons()
    {
        // �ϰ� �̱�

        AllContent.SetActive(true);

        Transform _allT = AllContent.transform;
        foreach (Transform child in _allT)
        {
            // TODO: child ���� ���� �ۼ��ϱ� �̱⿡�� ���� �������**
            int id = DrawWeapon(2); // return���� id �˷��ֱ�

            SetPickUPWeapon(id, child.gameObject);
        }

    }
    public void SetPickUPWeapon(int id, GameObject _obj)
    {
        Image img = _obj.transform.Find("Image").GetComponent<Image>();
        img.sprite = WeaponManager.GetInstance().GetSprite(id);
        _obj.GetComponent<WeaponUnlock>().Unlock();
        WeaponManager.GetInstance().AddWeaponCount(id, 1);
        WeaponManager.GetInstance().MatchWeaponDataFromWeapon(id);

        Weapon weapon = WeaponManager.GetInstance().GetWeapon(id);

        Slider slider = _obj.transform.Find("Slider").GetComponent<Slider>();
        slider.value = (float)weapon.GetCount() / 5 >= 1 ? 1 : (float)weapon.GetCount() / 5;

        Text text = _obj.transform.Find("possession").GetComponent<Text>();
        text.text = weapon.GetCount().ToString() + "/5";
    }
}
