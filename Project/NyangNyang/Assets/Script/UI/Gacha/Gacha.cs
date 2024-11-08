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

    private bool m_spinning = false;

    public type curType;

    int cost = 1000;

    private void OnEnable()
    {
        BeforePanel.SetActive(true);
        AfterPanel.SetActive(false);
    }

    public void OnClickedDrawButton(int n)
    {
        if ( Player.Gold >= cost * n)
        {
            BeforePanel.SetActive(true);
            AfterPanel.SetActive(false);

            Spin();

            Player.Gold -= cost * n;
        }
    }

    public void Spin()
    {
        if (!m_spinning)
            StartCoroutine(DoSpin());
    }

    private IEnumerator DoSpin()
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
        m_spinning = false;

        yield return new WaitForSeconds(1f);

        BeforePanel.SetActive(false);
        AfterPanel.SetActive(true);
    }
}
