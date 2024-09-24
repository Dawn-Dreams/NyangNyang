using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDetailUI : MonoBehaviour
{
    private Weapon choosedWeapon;
    private WeaponMgrUI weaponMgrUI;

    // ���� â�� ��� �̹��� �迭
    public Sprite[] sprites;

    // �г�
    public GameObject detailPanel;          // ������ �г� ��ü set active�� ����
    public GameObject weaponPanel;          // weapon �̸�, �̹���, ����, ������
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
        // Active �Ǵ� �� ���� �ҷ���
        choosedWeapon = null;
    }

    public void OnClickedWeapon(GameObject _obj)
    {
        if ( choosedWeapon == null)
        {
            detailPanel.SetActive(true);
            
            choosedWeapon = WeaponManager.GetInstance().GetWeapon(_obj.name);

            wNameTxt.text = choosedWeapon.GetName();
            wLevelTxt.text = choosedWeapon.GetLevel() + "/100";
            wImage.sprite = sprites[choosedWeapon.GetID()];

            int count = choosedWeapon.GetWeaponCount();
            weaponCoinTxt.text = choosedWeapon.GetNeedCoin().ToString();
            wPossessionTxt.text = count + "/5";
            wPossessionSlider.value = (float)count / 5 >= 1 ? 1 : (float)count / 5;

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
        // ���� �ܰ���� merge ����
        if (choosedWeapon != null && choosedWeapon.GetWeaponCount() >= 5)
        {
            if (WeaponManager.GetInstance().CombineWeapon(choosedWeapon.GetID()))
            {
                // Debug.Log("����");
                UpdatePossessionText();
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

    public void OnClickedEnhance()
    {
        if ( choosedWeapon != null && choosedWeapon.HasWeapon())
        {
            // TODO: ���� ��� & ���� �����
            // LevelUpWeapon �Լ��� int ������ ���� �ܰ迡 �ʿ��� ������ �� return ��.
            weaponCoinTxt.text = WeaponManager.GetInstance().LevelUpWeapon(choosedWeapon.GetID()).ToString();
            wLevelTxt.text = choosedWeapon.GetLevel() + "/100";

            // TODO: �ɷ�ġ ���� ���� �����
            WeaponManager.GetInstance().EnhanceEffectWeapon(choosedWeapon.GetID());
            eCurStatusTxt.text = choosedWeapon.GetCurStatus().ToString();
            eNextStatusTxt.text = choosedWeapon.GetNextStatus().ToString();
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
}
