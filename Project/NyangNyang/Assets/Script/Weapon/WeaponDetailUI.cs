using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public TextMeshProUGUI weaponCoinTxt;
    
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
        weaponMgrUI = gameObject.GetComponent<WeaponMgrUI>();
        
        wNameTxt = weaponPanel.transform.Find("name_txt").GetComponent<Text>();
        wPossessionTxt = weaponPanel.transform.Find("possession_txt").GetComponent<TextMeshProUGUI>();
        wLevelTxt = weaponPanel.transform.Find("levelNum_txt").GetComponent<Text>();
        wImage = weaponPanel.transform.Find("weapon_img").GetComponent<Image>();
        wPossessionSlider = weaponPanel.transform.Find("possession_slider").GetComponent<Slider>();
        wLockImage = weaponPanel.transform.Find("lock_img").gameObject;
    
        eCurStatusTxt = effectPanel.transform.Find("prev_txt").GetComponent<Text>();
        eNextStatusTxt = effectPanel.transform.Find("next_txt").GetComponent<Text>();
    }

    // 디테일 장비 창 열릴 때마다 불리는 함수
    private void OnEnable()
    {
        choosedWeapon = null;
    }

    // 장비 인벤 창에서 장비 선택 시 불리는 함수
    public void OnClickedWeapon(GameObject _obj)
    {
        if ( choosedWeapon == null ) // 선택된 장비가 없는 경우
        {  
            // 선택된 장비 정보 받아오기
            choosedWeapon = WeaponManager.GetInstance().GetWeapon(_obj.name);
            // 장비 정보가 잘 불려온 경우
            if ( choosedWeapon != null )
            {
                // 장비 디테일 창 열기
                detailPanel.SetActive(true);
                UpdateDetailUI();
            }

        }
    }

    // 다음 단계로의 장비로 합성하는 함수
    public void OnClickedMerge()
    {
        // 다음 단계로의 merge 과정
        if (choosedWeapon != null && choosedWeapon.GetCount() >= 5)
        {
            if (WeaponManager.GetInstance().CombineWeapon(choosedWeapon.GetID()))
            {
                // Debug.Log("성공");
                UpdatePossessionUI();
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

    // 장비 Lv 올리는 함수
    public void OnClickedEnhance()
    {
        if (choosedWeapon != null && !choosedWeapon.GetIsLock() && choosedWeapon.GetLevel() < 100)
        {
            if (Player.Gold >= int.Parse(weaponCoinTxt.text))
            {
                Player.Gold -= int.Parse(weaponCoinTxt.text);
                // LevelUpWeapon 함수가 int 값으로 다음 단계에 필요한 코인의 양 return 함.
                int num = WeaponManager.GetInstance().LevelUpWeapon(choosedWeapon.GetID());
                // TODO: 능력치 증가 로직 만들기
                //WeaponManager.GetInstance().EnhanceEffectWeapon(choosedWeapon.GetID());
                //eCurStatusTxt.text = choosedWeapon.GetCurStatus().ToString();
                //eNextStatusTxt.text = choosedWeapon.GetNextStatus().ToString();
                if (choosedWeapon.GetLevel() == 100)
                {
                    UpdateMaxLevelUI();
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

    // 이전 무기의 디테일 창으로 넘어가는 함수
    public void OnClickedShowPreviousWeapon()
    {
        if ( choosedWeapon != null )
        {
            if ( choosedWeapon.GetID() > 0)
            {
                // 선택된 장비 정보 받아오기
                choosedWeapon = WeaponManager.GetInstance().GetWeapon(choosedWeapon.GetID() - 1);

                // 장비 정보가 잘 불려온 경우
                if (choosedWeapon != null)
                {
                    UpdateDetailUI();
                }
            }
        }
    }

    // 다음 무기의 디테일 창으로 넘어가는 함수
    public void OnClickedShowNextWeapon()
    {
        if (choosedWeapon != null)
        {
            if (choosedWeapon.GetID() < 31)
            {
                // 선택된 장비 정보 받아오기
                choosedWeapon = WeaponManager.GetInstance().GetWeapon(choosedWeapon.GetID() + 1);

                // 장비 정보가 잘 불려온 경우
                if (choosedWeapon != null)
                {
                    UpdateDetailUI();
                }
            }
        }
    }

    // 디테일 장비 창 닫는 함수
    public void OnClickedCancle()
    {
        if ( choosedWeapon != null )
        {
            choosedWeapon = null;
            wLockImage.SetActive(true); 
            detailPanel.SetActive(false);
        }
    }

    // 다음 단계의 장비로 합성 시, 불리는 소지량 UI 변경 함수
    void UpdatePossessionUI()
    {
        weaponMgrUI.UpdatePossession(choosedWeapon.GetID());
        weaponMgrUI.UpdatePossession(choosedWeapon.GetID() + 1);

        int count = choosedWeapon.GetCount();
        wPossessionTxt.text = count + "/5";
        wPossessionSlider.value = (float)count / 5 >= 1 ? 1 : (float)count / 5;
    }

    // UpdateDetailUI 할 시, Max Lv인 경우 불리는 함수
    public void UpdateMaxLevelUI()
    {
        weaponCoinTxt.text = "max";
        wLevelTxt.text = "100";
    }

    // 디테일 창에 새로운 장비의 정보를 띄우는 함수
    public void UpdateDetailUI()
    {
        // UI - 잠금 확인하기
        wLockImage.SetActive(choosedWeapon.GetIsLock());

        // UI - 이름 및 이미지 변경하기
        wNameTxt.text = choosedWeapon.GetName();
        wImage.sprite = WeaponManager.GetInstance().GetSprite(choosedWeapon.GetID());

        // UI - 소지량 변경하기
        if (choosedWeapon.GetLevel() == 100)
        {
            // 장비의 레벨이 max인 경우
            UpdateMaxLevelUI();
        }
        else
        {
            // 레벨이 max가 아닌 경우
            wLevelTxt.text = choosedWeapon.GetLevel() + "/100";
            int count = choosedWeapon.GetCount();
            weaponCoinTxt.text = choosedWeapon.GetCoin().ToString();
            wPossessionTxt.text = count + "/5";
            wPossessionSlider.value = (float)count / 5 >= 1 ? 1 : (float)count / 5;
        }

        // UI - 효과 변경하기
        //eCurStatusTxt.text = choosedWeapon.GetCurStatus().ToString();
        //eNextStatusTxt.text = choosedWeapon.GetNextStatus().ToString();
    }

}
