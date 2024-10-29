using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> panels = new List<GameObject>();

    private int curPanel = 0;

    public void OnClickedSkillGachaNavButton()
    {

        Debug.Log("스킬 뽑기");

        if ( curPanel != 0)
        {
            panels[curPanel].SetActive(false);
            curPanel = 0;
            panels[curPanel].SetActive(true);
        }
    }

    public void OnClickedWeaponGachaNavButton()
    {


        NetworkManager.GetStatusManager().UserRegister();
        Debug.Log("무기 뽑기");
        if (curPanel != 1)
        {
            panels[curPanel].SetActive(false);
            curPanel = 1;
            panels[curPanel].SetActive(true);
        }
    }

    public void OnClickedGoldNavButton()
    {

        Debug.Log("재화 상점");
        if (curPanel != 2)
        {
            panels[curPanel].SetActive(false);
            curPanel = 2;
            panels[curPanel].SetActive(true);
        }
    }

    public void OnClickedPassNavButton()
    {
        Debug.Log("패스 상점");
        if (curPanel != 3)
        {
            panels[curPanel].SetActive(false);
            curPanel = 3;
            panels[curPanel].SetActive(true);
        }
    }

    public void OnClickedPackageNavButton()
    {
        Debug.Log("패키지 상점");
        if (curPanel != 4)
        {
            panels[curPanel].SetActive(false);
            curPanel = 4;
            panels[curPanel].SetActive(true);
        }
    }
}
