using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaManager : MonoBehaviour
{

    enum type { weapon, skill, none };

    [SerializeField]
    private GameObject GachaDetailPanel;

    [SerializeField]
    private GameObject ProbabilityPanel;

    [SerializeField]
    private GameObject BeforePanel;

    [SerializeField]
    private GameObject AfterPanel;

    GameObject board;
    float rotationSpeed = 360f;
    bool isRotate = false;

    type curType = type.none;
    int weaponGachaLevel = 1;
    int skillGachaLevel = 1;

    int showGachaLevel = 1;

    // �뷱�� ��ġ �ʿ�
    int cost = 10000;

    Text GachaLevelTxt;

    private void Start()
    {
        board = BeforePanel.transform.Find("Board").gameObject;
        GachaLevelTxt = ProbabilityPanel.transform.Find("Level").GetComponent<Text>();
        // TODO: �������� �̱� ���� �޾ƿ���
    }

    public void OnClickedWeaponProbButton()
    {
        ProbabilityPanel.SetActive(true);
        showGachaLevel = weaponGachaLevel;
        GachaLevelTxt.text = "Lv. " + showGachaLevel;
    }

    public void OnClickedSkillProbButton()
    {
        ProbabilityPanel.SetActive(true);
        showGachaLevel = skillGachaLevel;
        GachaLevelTxt.text = "Lv. " + showGachaLevel;
    }

    public void OnClickedProbPanelCancleButton()
    {
        ProbabilityPanel.SetActive(false);
    }

    public void OnClickedNextLevelButton()
    {
        if ( showGachaLevel < 10)
        {
            GachaLevelTxt.text = "Lv. " + (showGachaLevel + 1);
            showGachaLevel++;
        }
    }

    public void OnClickedPrevLevelButton()
    {
        if ( showGachaLevel > 1)
        {
            GachaLevelTxt.text = "Lv. " + (showGachaLevel - 1);
            showGachaLevel--;
        }
    }

    public void OnClickedWeaponDrawButton()
    {
        if ( curType == type.none)
        {
            curType = type.weapon;
            GachaDetailPanel.SetActive(true);
        }
    }

    public void OnClickedSkillDrawButton()
    {
        if (curType == type.none)
        {
            curType = type.skill;
            GachaDetailPanel.SetActive(true);
        }
    }

    public void OnClickedCancleButton()
    {
        if ( curType != type.none)
        {
            curType = type.none;
            BeforePanel.SetActive(false);
            AfterPanel.SetActive(false);
            GachaDetailPanel.SetActive(false);
        }
    }

    public void OnClickedDrawButton()
    {
        // �ϳ��� �̱�
        if (!isRotate && Player.Gold >= cost)
        {
            BeforePanel.SetActive(true);
            AfterPanel.SetActive(false);
            Player.Gold -= cost;
            StartCoroutine(RotateOverTime(1f, 1));
            isRotate = true;
        }
    }

    public void OnClickedDrawAllButton()
    {
        // �ϰ� �̱�
        if (!isRotate && Player.Gold >= cost * 10)
        {
            BeforePanel.SetActive(true);
            AfterPanel.SetActive(false);
            Player.Gold -= cost * 10;
            StartCoroutine(RotateOverTime(1f, 10));
            isRotate = true;
        }
    }

    IEnumerator RotateOverTime(float duration, int n)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            board.transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime * 2);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        board.transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(1f);

        isRotate = false;
        BeforePanel.SetActive(false);
        AfterPanel.SetActive(true);

        if (n == 1 && curType == type.weapon)
        {
            AfterPanel.GetComponent<PickUpWeapon>().ShowPickUpWeapon();
        }
        else if ( n != 1 && curType == type.weapon)
        {
            AfterPanel.GetComponent<PickUpWeapon>().ShowPickUpWeapons();
        }
        else if ( n == 1 && curType == type.skill)
        {
            AfterPanel.GetComponent<PickUpSkill>().ShowPickUpSkill();
        }
        else
        {
            AfterPanel.GetComponent<PickUpSkill>().ShowPickUpSkills();
        }
    }
}
