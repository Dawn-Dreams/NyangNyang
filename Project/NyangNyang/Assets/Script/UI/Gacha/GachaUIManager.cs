using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaUIManager : MonoBehaviour
{

    [SerializeField]
    private GameObject WeaponGachaDetailPanel;

    [SerializeField]
    private GameObject SkillGachaDetailPanel;

    private GameObject currentPanel;

    public void OnClickedWeaponDrawButton()
    {
        if ( currentPanel == null )
        {
            WeaponGachaDetailPanel.SetActive(true);
            currentPanel = WeaponGachaDetailPanel;
        }
    }

    public void OnClickedSkillDrawButton()
    {
        if ( currentPanel == null )
        {
            SkillGachaDetailPanel.SetActive(true);
            currentPanel = SkillGachaDetailPanel;
        }
    }

    public void OnClickedCancleButton()
    {
        if ( currentPanel != null ) {
            currentPanel.SetActive(false);
            currentPanel = null;
        }
    }
}
