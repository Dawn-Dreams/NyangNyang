using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
    public GameObject floatingDamage;

    private Slider _healthSlider;
    private TextMeshProUGUI _healthText;

    protected override void Awake()
    {
        characterID = 1;
        base.Awake();

        _healthSlider = gameObject.GetComponentInChildren<Slider>();
        _healthText = _healthSlider.GetComponentInChildren<TextMeshProUGUI>();

        _healthSlider.maxValue = status.hp;
        _healthSlider.minValue = 0;
        _healthSlider.value = currentHP;

        SetHealthBarText();
    }

    protected override bool TakeDamage(int damage)
    {
        bool getDamaged = base.TakeDamage(damage);

        if (getDamaged)
        {
            // 대미지 출력
            GameObject textObject = Instantiate(floatingDamage, transform.position, Quaternion.identity);
            textObject.GetComponentInChildren<TextMesh>().text = " " + damage + " ";
            Destroy(textObject,2.0f);
        }

        _healthSlider.value = currentHP;
        SetHealthBarText();

        return getDamaged;
    }

    private void SetHealthBarText()
    {
        if (_healthText== null) return;
        _healthText.SetText(_healthSlider.value + " / " + _healthSlider.maxValue);
    }
}

