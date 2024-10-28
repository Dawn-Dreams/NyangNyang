using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIMgr : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> MenuPanels = new List<GameObject>();

    private ToggleGroup ToggleGroup;

    private void Start()
    {
        ToggleGroup = GetComponent<ToggleGroup>();
    }

    public void OnClickedStatusToggle(Toggle toggle)
    {
        if (toggle.isOn)
        {
            MenuPanels[0].SetActive(true);
        }
        else
        {
            MenuPanels[0].SetActive(false);
        }
    }
    public void OnClickedInvenToggle(Toggle toggle)
    {
        if (toggle.isOn)
        {
            MenuPanels[1].SetActive(true);
        }
        else
        {
            MenuPanels[1].SetActive(false);
        }
    }
    public void OnClickedDunjeonToggle(Toggle toggle)
    {
        if (toggle.isOn)
        {
            MenuPanels[2].SetActive(true);
        }
        else
        {
            MenuPanels[2].SetActive(false);
        }
    }
    public void OnClickedStoreToggle(Toggle toggle)
    {
        if (toggle.isOn)
        {
            MenuPanels[3].SetActive(true);
        }
        else
        {
            MenuPanels[3].SetActive(false);
        }
    }
}
