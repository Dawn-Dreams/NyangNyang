using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillDetailUI : MonoBehaviour
{
    private Skill choosedSkill;
    private SkillMgrUI skillMgrUI;

    public GameObject detailPanel;
    public GameObject skillPanel;
    public GameObject effectPanel;
    public TextMeshProUGUI skillCoinTxt;

    private Text wNameTxt;
    private TextMeshProUGUI wPossessionTxt;
    private Text wLevelTxt;
    private Image wImage;
    private Slider wPossessionSlider;
    private GameObject wLockImage;

    private Text eCurStatusTxt;
    private Text eNextStatusTxt;

    private void Start()
    {
        skillMgrUI = gameObject.GetComponent<SkillMgrUI>();

        wNameTxt = skillPanel.transform.Find("name_txt").GetComponent<Text>();
        wPossessionTxt = skillPanel.transform.Find("possession_txt").GetComponent<TextMeshProUGUI>();
        wLevelTxt = skillPanel.transform.Find("levelNum_txt").GetComponent<Text>();
        wImage = skillPanel.transform.Find("skill_img").GetComponent<Image>();
        wPossessionSlider = skillPanel.transform.Find("possession_slider").GetComponent<Slider>();
        wLockImage = skillPanel.transform.Find("lock_img").gameObject;

        eCurStatusTxt = effectPanel.transform.Find("prev_txt").GetComponent<Text>();
        eNextStatusTxt = effectPanel.transform.Find("next_txt").GetComponent<Text>();
    }

    private void OnEnable()
    {
        // 스킬 인벤 창이 열릴 때 active 됨 디테일 창 X
        choosedSkill = null;
    }

    // 스킬 인벤 창에서 스킬 선택 시 불리는 함수
    public void OnClickedSkill(GameObject _obj)
    {
        if ( choosedSkill == null )
        {
            choosedSkill = SkillManager.GetInstance().GetSkill(_obj.name);
            
            // 스킬이 잘 불려온 경우
            if ( choosedSkill != null )
            {
                detailPanel.SetActive(true);
                UpdateDetailUI();
            }
        }
    }

    // 이전 스킬의 디테일 창으로 넘어가는 함수
    public void OnClickedShowPreviousWeapon()
    {
        if (choosedSkill != null)
        {
            if (choosedSkill.GetID() > 0)
            {
                // 선택된 스킬 정보 받아오기
                choosedSkill = SkillManager.GetInstance().GetSkill(choosedSkill.GetID() - 1);

                // 스킬 정보가 잘 불려온 경우
                if (choosedSkill != null)
                {
                    UpdateDetailUI();
                }
            }
            else
            {
                AlertManager.GetInstance().SetText("첫 번째 스킬입니다.");
            }
        }
    }
    
    // 다음 스킬의 디테일 창으로 넘어가는 함수
    public void OnClickedShowNextWeapon()
    {
        if (choosedSkill != null)
        {
            if (choosedSkill.GetID() < 24)
            {
                // 선택된 스킬 정보 받아오기
                choosedSkill = SkillManager.GetInstance().GetSkill(choosedSkill.GetID() + 1);

                // 스킬 정보가 잘 불려온 경우
                if (choosedSkill != null)
                {
                    UpdateDetailUI();
                }
            }
            else
            {
                AlertManager.GetInstance().SetText("마지막 스킬입니다.");
            }
        }
    }

    // 스킬 레벨 업하는 함수
    public void OnClickedLevelUP()
    {
        if ( choosedSkill != null && choosedSkill.HasSkill() && choosedSkill.GetLevel() < 10 )
        {
            if ( Player.Gold >= int.Parse(skillCoinTxt.text))
            {
                int result = SkillManager.GetInstance().LevelUpSkill(choosedSkill.GetID());
                Player.Gold -= int.Parse(skillCoinTxt.text);


                // 11.12 이윤석 - 스킬 레벨업 퀘스트
                if (QuestManager.GetInstance().OnUserSkillLevelUp != null)
                {
                    QuestManager.GetInstance().OnUserSkillLevelUp(result);
                }


                if ( result == 10)
                {
                    UpdateMaxLevelUI();
                }
                else if (result != -1) {
                    wLevelTxt.text = choosedSkill.GetLevel() + "/10";
                    int count = choosedSkill.GetCount();

                    int coin = choosedSkill.GetCoin();
                    skillCoinTxt.text = coin.ToString();

                    int num = SkillManager.GetInstance().GetLevelUpCostPerLevel(choosedSkill.GetLevel() - 1);
                    wPossessionTxt.text = count + "/" + num;
                    wPossessionSlider.value = (float)count / num >= 1 ? 1 : (float)count / num;
                }
                else
                {
                    AlertManager.GetInstance().SetText("레벨업에 실패했습니다.");
                    // Debug.Log("레벨 업에 실패했습니다.");
                }
            }
            else
            {
                AlertManager.GetInstance().SetText("돈이 부족합니다.");
                // Debug.Log("돈이 부족합니다.");
            }
        }
    }

    // 디테일 스킬 창 닫는 함수
    public void OnClickedCancle()
    {
        if ( choosedSkill != null )
        {
            choosedSkill = null;
            wLockImage.SetActive(true);
            detailPanel.SetActive(false);
        }
    }

    // UpdateDetailUI 할 시, Max Lv인 경우 불리는 함수
    public void UpdateMaxLevelUI()
    {
        wPossessionSlider.value = 1;
        wPossessionTxt.text = "max";
        skillCoinTxt.text = "max";
        wLevelTxt.text = "10";
    }

    // 디테일 창에 새로운 스킬의 정보를 띄우는 함수
    public void UpdateDetailUI()
    {
        // UI - 잠금 확인하기
        wLockImage.SetActive(choosedSkill.GetIsLock());

        // UI - 이름 및 이미지 변경하기
        wNameTxt.text = choosedSkill.GetName();
        wImage.sprite = SkillManager.GetInstance().GetSprite(choosedSkill.GetID());

        // UI - 소지량 변경하기
        if (choosedSkill.GetLevel() == 10)
        {
            // 장비의 레벨이 max인 경우
            UpdateMaxLevelUI();
        }
        else
        {
            // 레벨이 max가 아닌 경우
            wLevelTxt.text = choosedSkill.GetLevel() + "/10";
            skillCoinTxt.text = choosedSkill.GetCoin().ToString();
            int count = choosedSkill.GetCount();
            int num = SkillManager.GetInstance().GetLevelUpCostPerLevel(choosedSkill.GetLevel() - 1);
            wPossessionTxt.text = count + "/" + num;
            wPossessionSlider.value = (float)count / num >= 1 ? 1 : (float)count / num;
        }

        // UI - 효과 변경하기
        // eCurStatusTxt.text = choosedSkill.GetCurStatus().ToString();
        // eNextStatusTxt.text = choosedSkill.GetNextStatus().ToString();
    }
}
