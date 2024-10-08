using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDetailUI : MonoBehaviour
{
    private Weapon choosedWeapon;
    private WeaponMgrUI weaponMgrUI;

    // 패널
    public GameObject detailPanel;          // 디테일 패널 자체 set active를 위함
    public GameObject weaponPanel;          // weapon 이름, 이미지, 레벨, 보유량
    public GameObject effectPanel;
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
        weaponMgrUI = gameObject.GetComponent<WeaponMgrUI>();
        
        wNameTxt = weaponPanel.transform.Find("name_txt").GetComponent<Text>();
        wPossessionTxt = weaponPanel.transform.Find("possession_txt").GetComponent<Text>();
        wLevelTxt = weaponPanel.transform.Find("levelNum_txt").GetComponent<Text>();
        wImage = weaponPanel.transform.Find("weapon_img").GetComponent<Image>();
        wPossessionSlider = weaponPanel.transform.Find("possession_slider").GetComponent<Slider>();
    
        eCurStatusTxt = effectPanel.transform.Find("prev_txt").GetComponent<Text>();
        eNextStatusTxt = effectPanel.transform.Find("next_txt").GetComponent<Text>();
    }

    private void OnEnable()
    {
        // Active 되는 매 순간 불려짐
        choosedWeapon = null;
    }

    public void OnClickedWeapon(GameObject _obj)
    {
        if ( choosedWeapon == null )
        {
            detailPanel.SetActive(true);
            
            choosedWeapon = WeaponManager.GetInstance().GetWeapon(_obj.name);

            wNameTxt.text = choosedWeapon.GetName();
            wImage.sprite = WeaponManager.GetInstance().GetSprite(choosedWeapon.GetID());

            if ( choosedWeapon.GetLevel() == 100)
            {
                MaxLevelOfWeapon();
            }
            else
            {
                wLevelTxt.text = choosedWeapon.GetLevel() + "/100";
                int count = choosedWeapon.GetWeaponCount();
                weaponCoinTxt.text = choosedWeapon.GetNeedCoin().ToString();
                wPossessionTxt.text = count + "/5";
                wPossessionSlider.value = (float)count / 5 >= 1 ? 1 : (float)count / 5;
            }

            eCurStatusTxt.text = choosedWeapon.GetCurStatus().ToString();
            eNextStatusTxt.text = choosedWeapon.GetNextStatus().ToString();
        }
    }

    public void OnClickedCancle()
    {
        if ( choosedWeapon != null )
        {
            choosedWeapon = null;
            detailPanel.SetActive(false);
        }
    }

    public void OnClickedMerge()
    {
        // 다음 단계로의 merge 과정
        if (choosedWeapon != null && choosedWeapon.GetWeaponCount() > 5)
        {
            if (WeaponManager.GetInstance().CombineWeapon(choosedWeapon.GetID()))
            {
                // Debug.Log("성공");
                UpdatePossessionText();
            }
            else
            {
                // TODO: 최고 단계라는 팝업 띄우기
                // Debug.Log("최고 단계");
            }
        }
        else
        {
            // TODO: 개수 부족 팝업 띄우기
            // Debug.Log("개수 부족");
        }
    }

    public void OnClickedEnhance()
    {
        if ( choosedWeapon != null && choosedWeapon.HasWeapon() && choosedWeapon.GetLevel() < 100)
        {
            if ( Player.Gold >= int.Parse(weaponCoinTxt.text))
            {
                Player.Gold -= int.Parse(weaponCoinTxt.text);
                // LevelUpWeapon 함수가 int 값으로 다음 단계에 필요한 코인의 양 return 함.
                int num = WeaponManager.GetInstance().LevelUpWeapon(choosedWeapon.GetID());
                // TODO: 능력치 증가 로직 만들기
                WeaponManager.GetInstance().EnhanceEffectWeapon(choosedWeapon.GetID());
                eCurStatusTxt.text = choosedWeapon.GetCurStatus().ToString();
                eNextStatusTxt.text = choosedWeapon.GetNextStatus().ToString();
                if ( choosedWeapon.GetLevel() == 100)
                {
                    MaxLevelOfWeapon();
                }
                else
                {
                    weaponCoinTxt.text = num.ToString();
                    wLevelTxt.text = choosedWeapon.GetLevel() + "/100";
                }
            }
            else
            {
                Debug.Log("돈이 부족합니다.");
            }
        }
        else
        {
            Debug.Log("레벨 업에 실패했습니다.");
        }
    }

    void UpdatePossessionText()
    {
        weaponMgrUI.UpdatePossession(choosedWeapon.GetID());
        weaponMgrUI.UpdatePossession(choosedWeapon.GetID() + 1);

        int count = choosedWeapon.GetWeaponCount();
        wPossessionTxt.text = count + "/5";
        wPossessionSlider.value = (float)count / 5 >= 1 ? 1 : (float)count / 5;
    }

    public void MaxLevelOfWeapon()
    {
        weaponCoinTxt.text = "max";
        wLevelTxt.text = "100";
    }
}
