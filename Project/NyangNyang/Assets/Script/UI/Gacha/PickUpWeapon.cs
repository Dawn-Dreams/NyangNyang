using JetBrains.Annotations;
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

        // TODO: OneContent ���� ���� �ۼ��ϱ� �̱⿡�� ���� �������**
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

        Transform _allT = AllContent.transform;
        foreach (Transform child in _allT)
        {
            // TODO: child ���� ���� �ۼ��ϱ� �̱⿡�� ���� �������**
            int id = 0; // return���� id �˷��ֱ�

            Image img = child.transform.Find("Image").GetComponent<Image>();
            img.sprite = WeaponManager.GetInstance().GetSprite(id);
            child.GetComponent<WeaponUnlock>().Unlock();
            WeaponManager.GetInstance().AddWeaponCount(id, 1);

            Weapon weapon = WeaponManager.GetInstance().GetWeapon(id);

            Slider slider = child.transform.Find("Slider").GetComponent<Slider>();
            slider.value = (float)weapon.GetWeaponCount() / 5 >= 1 ? 1 : (float)weapon.GetWeaponCount() / 5;

            Text text = child.transform.Find("possession").GetComponent<Text>();
            text.text = weapon.GetWeaponCount().ToString() + "/5";
        }

    }
}
