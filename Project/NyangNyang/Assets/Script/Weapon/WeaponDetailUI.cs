using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDetailUI : MonoBehaviour
{
    private Weapon choosedWeapon;
    private WeaponMgrUI weaponMgrUI;

    // �г�
    public GameObject detailPanel;          // ������ �г� ��ü set active�� ����
    public GameObject weaponPanel;          // weapon �̸�, �̹���, ����, ������
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

    // ������ ��� â ���� ������ �Ҹ��� �Լ�
    private void OnEnable()
    {
        choosedWeapon = null;
    }

    // ��� �κ� â���� ��� ���� �� �Ҹ��� �Լ�
    public void OnClickedWeapon(GameObject _obj)
    {
        if ( choosedWeapon == null ) // ���õ� ��� ���� ���
        {  
            // ���õ� ��� ���� �޾ƿ���
            choosedWeapon = WeaponManager.GetInstance().GetWeapon(_obj.name);
            // ��� ������ �� �ҷ��� ���
            if ( choosedWeapon != null )
            {
                // ��� ������ â ����
                detailPanel.SetActive(true);
                UpdateDetailUI();
            }

        }
    }

    // ���� �ܰ���� ���� �ռ��ϴ� �Լ�
    public void OnClickedMerge()
    {
        // ���� �ܰ���� merge ����
        if (choosedWeapon != null && choosedWeapon.GetCount() >= 5)
        {
            if (WeaponManager.GetInstance().CombineWeapon(choosedWeapon.GetID()))
            {
                // Debug.Log("����");
                UpdatePossessionUI();
            }
            else
            {
                // TODO: �ְ� �ܰ��� �˾� ����
                // Debug.Log("�ְ� �ܰ�");
            }
        }
        else
        {
            // TODO: ���� ���� �˾� ����
            // Debug.Log("���� ����");
        }
    }

    // ��� Lv �ø��� �Լ�
    public void OnClickedEnhance()
    {
        if (choosedWeapon != null && !choosedWeapon.GetIsLock() && choosedWeapon.GetLevel() < 100)
        {
            if (Player.Gold >= int.Parse(weaponCoinTxt.text))
            {
                Player.Gold -= int.Parse(weaponCoinTxt.text);
                // LevelUpWeapon �Լ��� int ������ ���� �ܰ迡 �ʿ��� ������ �� return ��.
                int num = WeaponManager.GetInstance().LevelUpWeapon(choosedWeapon.GetID());
                // TODO: �ɷ�ġ ���� ���� �����
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
                Debug.Log("���� �����մϴ�.");
            }
        }
        else
        {
            Debug.Log("���� ���� �����߽��ϴ�.");
        }
    }

    // ���� ������ ������ â���� �Ѿ�� �Լ�
    public void OnClickedShowPreviousWeapon()
    {
        if ( choosedWeapon != null )
        {
            if ( choosedWeapon.GetID() > 0)
            {
                // ���õ� ��� ���� �޾ƿ���
                choosedWeapon = WeaponManager.GetInstance().GetWeapon(choosedWeapon.GetID() - 1);

                // ��� ������ �� �ҷ��� ���
                if (choosedWeapon != null)
                {
                    UpdateDetailUI();
                }
            }
        }
    }

    // ���� ������ ������ â���� �Ѿ�� �Լ�
    public void OnClickedShowNextWeapon()
    {
        if (choosedWeapon != null)
        {
            if (choosedWeapon.GetID() < 31)
            {
                // ���õ� ��� ���� �޾ƿ���
                choosedWeapon = WeaponManager.GetInstance().GetWeapon(choosedWeapon.GetID() + 1);

                // ��� ������ �� �ҷ��� ���
                if (choosedWeapon != null)
                {
                    UpdateDetailUI();
                }
            }
        }
    }

    // ������ ��� â �ݴ� �Լ�
    public void OnClickedCancle()
    {
        if ( choosedWeapon != null )
        {
            choosedWeapon = null;
            wLockImage.SetActive(true); 
            detailPanel.SetActive(false);
        }
    }

    // ���� �ܰ��� ���� �ռ� ��, �Ҹ��� ������ UI ���� �Լ�
    void UpdatePossessionUI()
    {
        weaponMgrUI.UpdatePossession(choosedWeapon.GetID());
        weaponMgrUI.UpdatePossession(choosedWeapon.GetID() + 1);

        int count = choosedWeapon.GetCount();
        wPossessionTxt.text = count + "/5";
        wPossessionSlider.value = (float)count / 5 >= 1 ? 1 : (float)count / 5;
    }

    // UpdateDetailUI �� ��, Max Lv�� ��� �Ҹ��� �Լ�
    public void UpdateMaxLevelUI()
    {
        weaponCoinTxt.text = "max";
        wLevelTxt.text = "100";
    }

    // ������ â�� ���ο� ����� ������ ���� �Լ�
    public void UpdateDetailUI()
    {
        // UI - ��� Ȯ���ϱ�
        wLockImage.SetActive(choosedWeapon.GetIsLock());

        // UI - �̸� �� �̹��� �����ϱ�
        wNameTxt.text = choosedWeapon.GetName();
        wImage.sprite = WeaponManager.GetInstance().GetSprite(choosedWeapon.GetID());

        // UI - ������ �����ϱ�
        if (choosedWeapon.GetLevel() == 100)
        {
            // ����� ������ max�� ���
            UpdateMaxLevelUI();
        }
        else
        {
            // ������ max�� �ƴ� ���
            wLevelTxt.text = choosedWeapon.GetLevel() + "/100";
            int count = choosedWeapon.GetCount();
            weaponCoinTxt.text = choosedWeapon.GetCoin().ToString();
            wPossessionTxt.text = count + "/5";
            wPossessionSlider.value = (float)count / 5 >= 1 ? 1 : (float)count / 5;
        }

        // UI - ȿ�� �����ϱ�
        //eCurStatusTxt.text = choosedWeapon.GetCurStatus().ToString();
        //eNextStatusTxt.text = choosedWeapon.GetNextStatus().ToString();
    }

}
