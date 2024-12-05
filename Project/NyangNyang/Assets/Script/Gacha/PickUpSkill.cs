using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PickUpSkill : MonoBehaviour
{
    public GameObject AllContent;
    public GameObject OneContent;

    // ��ų ID: 0 ~ 24 (��Ƽ��: 0 ~ 4, �нú�: 5 ~ 24)
    private const int TotalSkills = 25;

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
        for (int level = 1; level <= 12; level++)
        {
            probabilityTable[level] = new float[TotalSkills];

            for (int i = 0; i < TotalSkills; i++)
            {
                if (i < 5) probabilityTable[level][i] = 0.01f / level; // Active skills
                else probabilityTable[level][i] = 0.05f / level; // Passive skills
            }
        }
    }

    // ��ų �̱� �Լ�
    public int DrawSkill(int level)
    {
        if (!probabilityTable.ContainsKey(level))
        {
            Debug.LogError("Invalid level provided for skill draw.");
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

        // ���� ���� ���� ��ų ����
        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulativeProbability += probabilities[i];
            if (randomValue <= cumulativeProbability)
            {
                return i;
            }
        }

        // ���� ó��
        Debug.LogError("Failed to draw skill due to probability calculation error.");
        return -1;
    }

    private void OnDisable()
    {
        AllContent.SetActive(false);
        OneContent.SetActive(false);
    }

    public void ShowPickUpSkill()
    {
        // �� �� �̱�

        OneContent.SetActive(true);

        // TODO: OneContent ���� ���� �ۼ��ϱ� �̱⿡�� ���� �������**
        int id = DrawSkill(PlayInfoManager.GetInstance().GetInfo().skillGachaLevel);

        PlayInfoManager.GetInstance().SetSkillGachaCount(1);
        SetPickUPSkill(id, OneContent);

    }

    public void ShowPickUpSkills()
    {
        // �ϰ� �̱�
        AllContent.SetActive(true);

        Transform _allT = AllContent.transform;
        PlayInfoManager.GetInstance().SetSkillGachaCount(10);
        foreach (Transform child in _allT)
        {
            // TODO: child ���� ���� �ۼ��ϱ� �̱⿡�� ���� �������**
            int id = DrawSkill(PlayInfoManager.GetInstance().GetInfo().skillGachaLevel);

            SetPickUPSkill(id, child.gameObject);
        }
    }

    public void SetPickUPSkill(int id, GameObject _obj)
    {
        Image img = _obj.transform.Find("Image").GetComponent<Image>();
        img.sprite = SkillManager.GetInstance().GetSprite(id);
        _obj.GetComponent<WeaponUnlock>().Unlock();
        SkillManager.GetInstance().AddSkillCount(id, 1);
        SkillManager.GetInstance().MatchSkillDataFromSkill(id);

        Skill skill = SkillManager.GetInstance().GetSkill(id);

        int num = SkillManager.GetInstance().GetLevelUpCostPerLevel(skill.GetLevel() - 1);

        Slider slider = _obj.transform.Find("Slider").GetComponent<Slider>();
        slider.value = (float)skill.GetCount() / num >= 1 ? 1 : (float)skill.GetCount() / num;

        Text text = _obj.transform.Find("possession").GetComponent<Text>();
        text.text = skill.GetCount().ToString() + "/" + num;
    }
}
