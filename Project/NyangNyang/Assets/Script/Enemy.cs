using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : Character
{
    public GameObject floatingDamage;

    protected override bool TakeDamage(int damage)
    {
        bool getDamaged = base.TakeDamage(damage);

        if (getDamaged)
        {
            // 대미지 출력
            GameObject textObject = Instantiate(floatingDamage, transform.position, Quaternion.identity);
            textObject.GetComponentInChildren<TextMesh>().text = " " + damage + " ";
            
            Destroy(textObject,2.0f);
            //textObject.GetComponentInChildren<TextMeshProUGUI>().SetText();
        }

        return getDamaged;
    }
}
