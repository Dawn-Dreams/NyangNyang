using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cat : Character
{
    [SerializeField] 
    private Slider _healthBarSlider;
    [SerializeField]
    private TextMeshProUGUI _textMeshPro;

    protected override void Awake()
    {
        base.Awake();

        _healthBarSlider.maxValue = status.hp;
        _healthBarSlider.minValue = 0;
        _healthBarSlider.value = currentHP;

       SetHealthBarText();
    }

    public override void InitialSettings()
    {
        base.InitialSettings();


    }

    protected override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        _healthBarSlider.value = currentHP;
        SetHealthBarText();
    }

    private void SetHealthBarText()
    {
        if (_textMeshPro == null) return;
        _textMeshPro.SetText(_healthBarSlider.value + " / " + _healthBarSlider.maxValue);
    }
}
