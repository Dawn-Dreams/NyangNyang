using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NyangNyangPower : MonoBehaviour
{

    [SerializeField]
    int nyangnyangLevel = 1;

    [SerializeField]
    int nyangnyangPower = 0;

    [SerializeField]
    Slider nyangSliderValue;

    [SerializeField]
    public TextMeshProUGUI nyangPowerTxt;

    private void Start()
    {
        UpdateNyangNyangSlider();
    }

    public void OnClickedNyangNyangButton()
    {
        nyangnyangPower += nyangnyangLevel;
        UpdateNyangNyangSlider();
    }

    public void UpdateNyangNyangSlider()
    {
        nyangSliderValue.value = nyangnyangPower / 1000/*레벨에 따른 총량 값*/;
        nyangPowerTxt.text = nyangnyangPower + "/" + 1000/*레벨에 따른 총량 값*/;
    }

    public bool CanUseNyangNyangPower(int amount)
    {
        if ( nyangnyangPower < amount )
        {
            return false;
        }
        return true;
    }

    public void UseNyangNyangPower(int amount)
    {
        nyangnyangPower -= amount;
        UpdateNyangNyangSlider();
    }
}
