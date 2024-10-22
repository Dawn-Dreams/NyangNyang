using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public void OnClickedSkillGachaNavButton()
    {
        Debug.Log("스킬 뽑기");
    }

    public void OnClickedWeaponGachaNavButton()
    {
        Debug.Log("무기 뽑기");
    }

    public void OnClickedGoldNavButton()
    {
        Debug.Log("재화 상점");
    }

    public void OnClickedPassNavButton()
    {
        Debug.Log("패스 상점");
    }

    public void OnClickedPackageNavButton()
    {
        Debug.Log("패키지 상점");
    }
}
