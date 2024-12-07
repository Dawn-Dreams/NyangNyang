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

    private int[] LevelList = new int[12] { 0, 150, 300, 550, 1000, 2000, 4000, 10000, 20000, 40000, 40000, 40000 };

    public void InitializedNyangNyang()
    {

        nyangnyangLevel = PlayInfoManager.GetInstance().GetInfo().nyangnyangLevel;
        nyangnyangPower = PlayInfoManager.GetInstance().GetInfo().nyangnyangCount;
        UpdateNyangNyangSlider();
    }

    public void OnClickedNyangNyangButton()
    {
        nyangnyangPower += nyangnyangLevel;
        PlayInfoManager.GetInstance().SetNyangNyangCount();
        nyangnyangLevel = PlayInfoManager.GetInstance().GetInfo().nyangnyangLevel;
        UpdateNyangNyangSlider();
    }

    public void UpdateNyangNyangSlider()
    {
        if ( nyangnyangLevel < 12)
        {
            nyangSliderValue.value = (float)nyangnyangPower / (float)LevelList[nyangnyangLevel]/*레벨에 따른 총량 값*/;
            nyangPowerTxt.text = nyangnyangPower + "/" + LevelList[nyangnyangLevel]/*레벨에 따른 총량 값*/;
        }
        else
        {
            nyangSliderValue.value = 1;
            nyangPowerTxt.text = "MAX" + nyangnyangPower;
        }
    }

    public bool CanUseNyangNyangPower(int amount)
    {
        if ( nyangnyangPower < amount )
        {
            AlertManager.GetInstance().SetText("냥냥력이 부족합니다!");
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
