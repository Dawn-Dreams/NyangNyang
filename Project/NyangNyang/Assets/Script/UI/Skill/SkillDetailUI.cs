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
           
    public Text skillCoinTxt;

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
            wImage.sprite = SkillManager.GetInstance().GetSprite(choosedSkill.GetID());

            if ( choosedSkill.GetLevel() == 10)
            {
                MaxLevelOfSKill();
            }
            else
            {
                wLevelTxt.text = choosedSkill.GetLevel() + "/10";
                skillCoinTxt.text = choosedSkill.GetLevelUpCost().ToString();
                int count = choosedSkill.GetPossession();
                int num = SkillManager.GetInstance().GetLevelUpCostPerLevel(choosedSkill.GetLevel() - 1);
                wPossessionTxt.text = count + "/" + num;
                wPossessionSlider.value = (float)count / num >= 1 ? 1 : (float)count / num;
            }

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

    public void OnClickedLevelUP()
    {
        if ( choosedSkill != null && choosedSkill.HasSkill() && choosedSkill.GetLevel() < 10 )
        {
            if ( Player.Gold >= int.Parse(skillCoinTxt.text))
            {
                int result = SkillManager.GetInstance().LevelUpSkill(choosedSkill.GetID());
                Player.Gold -= int.Parse(skillCoinTxt.text);

                if ( result == 10)
                {
                    MaxLevelOfSKill();
                }
                else if (result != -1) {
                    wLevelTxt.text = choosedSkill.GetLevel() + "/10";
                    int count = choosedSkill.GetPossession();

                    int coin = choosedSkill.GetLevelUpCost();
                    skillCoinTxt.text = coin.ToString();

                    int num = SkillManager.GetInstance().GetLevelUpCostPerLevel(choosedSkill.GetLevel() - 1);
                    wPossessionTxt.text = count + "/" + num;
                    wPossessionSlider.value = (float)count / num >= 1 ? 1 : (float)count / num;
                }
                else
                {
                    Debug.Log("레벨 업에 실패했습니다.");
                }
            }
            else
            {
                Debug.Log("돈이 부족합니다.");
            }
        }
    }

    public void MaxLevelOfSKill()
    {
        wPossessionSlider.value = 1;
        wPossessionTxt.text = "max";
        skillCoinTxt.text = "max";
        wLevelTxt.text = "10";
    }
}
