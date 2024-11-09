using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inven : MonoBehaviour
{
    [SerializeField]
    private GameObject WeaponPanel;

    [SerializeField]
    private GameObject SkillPanel;

    public void OnClickedWeaponButton()
    {
        WeaponPanel.SetActive(true);
        SkillPanel.SetActive(false);
    }

    public void OnClickedSkillButton()
    {
        SkillPanel.SetActive(true);
        WeaponPanel.SetActive(false);
    }
}
