using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDetailUI : MonoBehaviour
{
    private Skill choosedSkill;
    private SkillMgrUI skillMgrUI;

    public GameObject detailPanel;
    public GameObject skillPanel;
    public GameObject effectPanel;

    public Text playerCoinTxt;              //  
    public Text weaponCoinTxt;

    private Text wNameTxt;
    private Text wPossessionTxt;
    private Text wLevelTxt;
    private Image wImage;
    private Slider wPossessionSlider;

    private Text eCurStatusTxt;
    private Text eNextStatusTxt;

    private void Start()
    {
        skillMgrUI = gameObject.GetComponent<SkillMgrUI>();

        wNameTxt = skillPanel.transform.Find("name_txt").GetComponent<Text>();
        wPossessionTxt = skillPanel.transform.Find("possession_txt").GetComponent<Text>();
        wLevelTxt = skillPanel.transform.Find("levelNum_txt").GetComponent<Text>();
        wImage = skillPanel.transform.Find("weapon_img").GetComponent<Image>();
        wPossessionSlider = skillPanel.transform.Find("possession_slider").GetComponent<Slider>();

        eCurStatusTxt = effectPanel.transform.Find("prev_txt").GetComponent<Text>();
        eNextStatusTxt = effectPanel.transform.Find("next_txt").GetComponent<Text>();
    }

    private void OnEnable()
    {
        // 스킬 인벤 창이 열릴 때 active 됨 디테일 창 X
        choosedSkill = null;
    }

    public void OnClickedSkill(GameObject _obj)
    {
        if ( choosedSkill == null )
        {
            detailPanel.SetActive(true);

            choosedSkill = SkillManager.GetInstance().GetSkill(_obj.name);

            wNameTxt.text = choosedSkill.GetName();
            wLevelTxt.text = choosedSkill.GetLevel() + "/100";
            wImage.sprite = SkillManager.GetInstance().GetSprite(choosedSkill.GetID());

            int count = choosedSkill.GetPossession();
            // weaponCoinTxt.text = choosedSkill.GetNeedCoin().ToString();
            wPossessionTxt.text = count + "/5";
            wPossessionSlider.value = (float)count / 5 >= 1 ? 1 : (float)count / 5;

            // eCurStatusTxt.text = choosedSkill.GetCurStatus().ToString();
            // eNextStatusTxt.text = choosedSkill.GetNextStatus().ToString();
        }
    }

    public void OnClickedCancle()
    {
        if ( choosedSkill != null )
        {
            choosedSkill = null;
            detailPanel.SetActive(false);
        }
    }

}
