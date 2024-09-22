using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDetailUI : MonoBehaviour
{
    private Weapon choosedWeapon;
    private WeaponMgrUI weaponMgrUI;

    public Sprite[] sprites;

    public GameObject detailPanel;
    public GameObject weaponInfo;
    public GameObject coinInfo;
    public Text needCoinTxt;

    private Text dWeaponNameText;
    private Text dWeaponPossessionText;
    private Text dWeaponlevelText;
    private Image dWeaponImage;
    private Text dWeaponlevel_coinText;
    private Slider dWeaponPossessionSlider;

    private void Start()
    {
        weaponMgrUI = gameObject.GetComponent<WeaponMgrUI>();
        dWeaponNameText = weaponInfo.transform.Find("name_txt").GetComponent<Text>();
        dWeaponPossessionText = weaponInfo.transform.Find("possession_txt").GetComponent<Text>();
        dWeaponlevelText = weaponInfo.transform.Find("levelNum_txt").GetComponent<Text>();
        dWeaponImage = weaponInfo.transform.Find("weapon_img").GetComponent<Image>();
        dWeaponlevel_coinText = coinInfo.transform.Find("coin_txt").GetComponent<Text>();
        dWeaponPossessionSlider = weaponInfo.transform.Find("possession_slider").GetComponent<Slider>();
    }

    private void OnEnable()
    {
        choosedWeapon = null;
    }

    public void OnClickedWeapon(GameObject _obj)
    {
        if ( choosedWeapon == null)
        {
            choosedWeapon = WeaponManager.GetInstance().GetWeapon(_obj.name);
            detailPanel.SetActive(true);
            dWeaponNameText.text = choosedWeapon.GetName();
            int count = choosedWeapon.GetWeaponCount();
            needCoinTxt.text = choosedWeapon.GetNeedCoin().ToString();
            dWeaponlevelText.text = choosedWeapon.GetLevel() + "/100";
            dWeaponImage.sprite = sprites[choosedWeapon.GetID()];
            dWeaponPossessionText.text = count + "/5";
            dWeaponPossessionSlider.value = (float)count / 5 >= 1 ? 1 : (float)count / 5;
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

    public void OnClickedUpgrade()
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
                // Debug.Log("�ְ� �ܰ�");
            }
        }
        else
        {
            // Debug.Log("���� ����");
        }
    }

    public void OnClickedEnhance()
    {
        if ( choosedWeapon != null && choosedWeapon.HasWeapon())
        {
            // TODO: ���� ��� & ���� �����

            // LevelUpWeapon �Լ��� int ������ ���� �ܰ迡 �ʿ��� ������ �� return ��.
            needCoinTxt.text = WeaponManager.GetInstance().LevelUpWeapon(choosedWeapon.GetID()).ToString();
            dWeaponlevelText.text = choosedWeapon.GetLevel() + "/100";
        }
    }

    void UpdatePossessionText()
    {
        weaponMgrUI.UpdatePossession(choosedWeapon.GetID());
        weaponMgrUI.UpdatePossession(choosedWeapon.GetID() + 1);

        dWeaponNameText.text = choosedWeapon.GetName();
        int count = choosedWeapon.GetWeaponCount();
        dWeaponPossessionText.text = count + "/5";
        dWeaponPossessionSlider.value = (float)count / 5 >= 1 ? 1 : (float)count / 5;
    }
}
