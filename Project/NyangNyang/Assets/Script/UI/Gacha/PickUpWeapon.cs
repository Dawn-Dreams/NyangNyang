using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpWeapon : MonoBehaviour
{
    public GameObject AllContent;
    public GameObject OneContent;

    private void OnDisable()
    {
        AllContent.SetActive(false);
        OneContent.SetActive(false);
    }

    public void ShowPickUpWeapon()
    {
        // �� �� �̱�

        OneContent.SetActive(true);

        // TODO: newUI ���� ���� �ۼ��ϱ� �̱⿡�� ���� �������**
        int id = 0;

        Image img = OneContent.transform.Find("Image").GetComponent<Image>();
        img.sprite = WeaponManager.GetInstance().GetSprite(id);
        OneContent.GetComponent<WeaponUnlock>().Unlock();
        WeaponManager.GetInstance().AddWeaponCount(id, 1);

        Weapon weapon = WeaponManager.GetInstance().GetWeapon(id);

        Slider slider = OneContent.transform.Find("Slider").GetComponent<Slider>();
        slider.value = (float)weapon.GetWeaponCount() / 5 >= 1 ? 1 : (float)weapon.GetWeaponCount() / 5;

        Text text = OneContent.transform.Find("possession").GetComponent<Text>();
        text.text = weapon.GetWeaponCount().ToString() + "/5";
    }

    public void ShowPickUpWeapons()
    {
        // �ϰ� �̱�

        AllContent.SetActive(true);
        //for ( int i = 0; i < uiObjects.Count; ++i )
        //{
            
        //    // TODO: newUI ���� ���� �ۼ��ϱ� �̱⿡�� ���� �������**
        //}
    }
}
