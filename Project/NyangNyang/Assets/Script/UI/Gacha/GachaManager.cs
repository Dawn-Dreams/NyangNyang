using Ricimi;
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
    private GameObject SkillBeforePanel;

    [SerializeField]
    private GameObject SkillAfterPanel;

    [SerializeField]
    private GameObject wheel;

    // This animation curve drives the spin wheel motion.
    public AnimationCurve AnimationCurve;

    private bool m_spinning = false;

    type curType = type.none;
    int weaponGachaLevel = 1;
    int skillGachaLevel = 1;

    int showGachaLevel = 1;

    // �뷱�� ��ġ �ʿ�
    int cost = 10000;

    Text GachaLevelTxt;

    private void Start()
    {
       // GachaLevelTxt = ProbabilityPanel.transform.Find("Level").GetComponent<Text>();
        // TODO: �������� �̱� ���� �޾ƿ���
    }

    private void OnEnable()
    {
        Debug.Log("dd");

        SkillBeforePanel.SetActive(true);
        SkillAfterPanel.SetActive(false);
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
            //BeforePanel.SetActive(false);
            //AfterPanel.SetActive(false);
            GachaDetailPanel.SetActive(false);
        }
    }

    public void OnClickedDrawButton()
    {
        // �ϳ��� �̱�
        if (Player.Gold >= cost)
        {
            SkillBeforePanel.SetActive(true);
            SkillAfterPanel.SetActive(false);

            Spin();

            Player.Gold -= cost;
        }
    }

    public void OnClickedDrawAllButton()
    {
        // �ϰ� �̱�
        if (Player.Gold >= cost * 10)
        {
            //BeforePanel.SetActive(true);
            //AfterPanel.SetActive(false);
            Player.Gold -= cost * 10;
        }
    }

    public void Spin()
    {
        if (!m_spinning)
            StartCoroutine(DoSpin());
    }

    private IEnumerator DoSpin()
    {

        m_spinning = true;

        yield return new WaitForSeconds(1f);

        var timer = 0.0f;
        var startAngle = wheel.transform.eulerAngles.z;

        var time = 3.0f;
        var maxAngle = 360.0f;

        while (timer < time)
        {
            var angle = AnimationCurve.Evaluate(timer / time) * maxAngle;
            wheel.transform.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        wheel.transform.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
        m_spinning = false;

        yield return new WaitForSeconds(1f);

        SkillBeforePanel.SetActive(false);
        SkillAfterPanel.SetActive(true);
    }
}
