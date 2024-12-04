using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        // 한 개 뽑기

        OneContent.SetActive(true);

        // TODO: OneContent 속의 내용 작성하기 뽑기에서 나온 결과물로**
        int id = 0;

        SetPickUPWeapon(id, OneContent);
    }

    public void ShowPickUpWeapons()
    {
        // 일괄 뽑기

        AllContent.SetActive(true);

        Transform _allT = AllContent.transform;
        foreach (Transform child in _allT)
        {
            // TODO: child 속의 내용 작성하기 뽑기에서 나온 결과물로**
            int id = 0; // return으로 id 알려주기

            SetPickUPWeapon(id, child.gameObject);
        }

    }
    public void SetPickUPWeapon(int id, GameObject _obj)
    {
        Image img = _obj.transform.Find("Image").GetComponent<Image>();
        img.sprite = WeaponManager.GetInstance().GetSprite(id);
        _obj.GetComponent<WeaponUnlock>().Unlock();
        WeaponManager.GetInstance().AddWeaponCount(id, 1);

        Weapon weapon = WeaponManager.GetInstance().GetWeapon(id);

        Slider slider = _obj.transform.Find("Slider").GetComponent<Slider>();
        slider.value = (float)weapon.GetCount() / 5 >= 1 ? 1 : (float)weapon.GetCount() / 5;

        Text text = _obj.transform.Find("possession").GetComponent<Text>();
        text.text = weapon.GetCount().ToString() + "/5";
    }
}
