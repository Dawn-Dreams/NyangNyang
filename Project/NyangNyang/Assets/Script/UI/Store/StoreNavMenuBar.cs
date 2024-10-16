using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreNavMenuBar : MonoBehaviour
{

    [SerializeField]
    private GameObject WeaponGachaPanel;
    [SerializeField]
    private GameObject SkillGachaPanel;
    [SerializeField]
    private GameObject GoldPanel;
    [SerializeField]
    private GameObject PassPanel;
    [SerializeField]
    private GameObject PackagePanel;

    enum type { weapon, skill, gold, pass, package, none };

    private type curPanel;

    public void OnClickedWeaponGachaButton()
    {
        curPanel = type.weapon;
        WeaponGachaPanel.SetActive(true);
        WeaponGachaPanel.GetComponent<GachaManager>().OnClickedWeaponDrawButton();
    }

    public void OnClickedSkillGachaButton()
    {
        curPanel = type.skill;
        SkillGachaPanel.SetActive(true);
        SkillGachaPanel.GetComponent<GachaManager>().OnClickedSkillDrawButton();
    }

    public void OnClickedGoldButton()
    {
        GoldPanel.SetActive(true);
    }

    public void OnClickedPassButton()
    {
        PassPanel.SetActive(true);
    }

    public void OnClickedPackageButton()
    {
        PackagePanel.SetActive(true);
    }
}
