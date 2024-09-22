using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
    public GameObject floatingDamage;

    protected EnemyDropData DropData = null;

    private Slider _healthSlider;
    private TextMeshProUGUI _healthText;

    protected override void Awake()
    {
        Debug.Log("enemy 생성");
        // 09.23 - EnemyID 별개로 관리하도록 변경
        characterID = 0;
        IsEnemy = true;

        base.Awake();

        _healthSlider = gameObject.GetComponentInChildren<Slider>();
        _healthText = _healthSlider.GetComponentInChildren<TextMeshProUGUI>();

        _healthSlider.maxValue = status.hp;
        _healthSlider.minValue = 0;
        _healthSlider.value = currentHP;

        SetHealthBarText();

        // enemy drop data 받기
        if (DropData == null)
        {
            DropData = ScriptableObject.CreateInstance<EnemyDropData>().SetEnemyDropData(DummyServerData.GetEnemyDropData(characterID));
        }
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

    // 적군이 사망했는지 여부를 반환
    public bool IsDead()
    {
        return currentHP <= 0;
    }

    private void SetHealthBarText()
    {
        if (_healthText== null) return;
        _healthText.SetText(_healthSlider.value + " / " + _healthSlider.maxValue);
    }

    protected override void Death()
    {
        if (DropData)
        {
            DropData.GiveItemToPlayer();
        }

        base.Death();

    }
}

