using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gacha : MonoBehaviour
{
    public enum type { weapon, skill };

    [SerializeField]
    private GameObject BeforePanel;

    [SerializeField]
    private GameObject AfterPanel;

    [SerializeField]
    private GameObject Wheel;

    public AnimationCurve AnimationCurve;

    public bool m_spinning = false;

    public type curType;

    int cost = 1000;

    private void OnEnable()
    {
        BeforePanel.SetActive(true);
        AfterPanel.SetActive(false);
    }

    public void OnClickedDrawButton(int n)
    {
        if ( !m_spinning)
        {
            if ( Player.Gold >= cost * n )
            {
                BeforePanel.SetActive(true);
                AfterPanel.SetActive(false);

               

                Spin(n);

                Player.Gold -= cost * n;
            }
            else
            {
                AlertManager.GetInstance().SetText("돈이 부족합니다.");
                // Debug.Log("돈이 부족합니다.");
            }
        }
        else
        {
            AlertManager.GetInstance().SetText("뽑기 중입니다.");
            // Debug.Log("뽑기 중입니다");
        }
    }

    public void Spin(int n)
    {
        if (!m_spinning)
            StartCoroutine(DoSpin(n));
    }

    private IEnumerator DoSpin(int n)
    {
        m_spinning = true;

        yield return new WaitForSeconds(1f);

        var timer = 0.0f;
        var startAngle = Wheel.transform.eulerAngles.z;

        var time = 3.0f;
        var maxAngle = 360.0f;

        while (timer < time)
        {
            var angle = AnimationCurve.Evaluate(timer / time) * maxAngle;
            Wheel.transform.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Wheel.transform.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
        

        yield return new WaitForSeconds(1f);

        m_spinning = false;
        BeforePanel.SetActive(false);
        AfterPanel.SetActive(true);

        if (n == 1 && curType == type.skill)
        {
            AfterPanel.GetComponent<PickUpSkill>().ShowPickUpSkill();
        }
        else if ( n == 10 && curType == type.skill)
        {
            AfterPanel.GetComponent<PickUpSkill>().ShowPickUpSkills();
        }
        else if (n == 1 && curType == type.weapon)
        {
            AfterPanel.GetComponent<PickUpWeapon>().ShowPickUpWeapon();
        }
        else if (n == 10 && curType == type.weapon)
        {
            AfterPanel.GetComponent<PickUpWeapon>().ShowPickUpWeapons();
        }
    }
}
