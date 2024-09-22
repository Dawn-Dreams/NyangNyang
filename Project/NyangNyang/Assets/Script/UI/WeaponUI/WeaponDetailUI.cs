using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDetailUI : MonoBehaviour
{
    private Weapon choosedWeapon;
    private WeaponMgrUI weaponMgrUI;
    
    // TODO: ���߿� detailPanel�� private���� �޾ƿ���s
    public GameObject detailPanel;
    public GameObject weaponInfo;
    public GameObject coinInfo;

    private Text dWeaponNameText;
    private Text dWeaponPossessionText;
    private Slider dWeaponPossessionSlider;

    private void Start()
    {
        weaponMgrUI = gameObject.GetComponent<WeaponMgrUI>();
        dWeaponNameText = weaponInfo.transform.Find("name_txt").GetComponent<Text>();
        dWeaponPossessionText = weaponInfo.transform.Find("possession_txt").GetComponent<Text>();
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
                Debug.Log("����");
                UpdatePossessionText();
            }
            else
            {
                Debug.Log("�ְ� �ܰ�");
            }
        }
        else
        {
            Debug.Log("���� ����");
        }
    }

    public void OnClickedEnhance()
    {

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
