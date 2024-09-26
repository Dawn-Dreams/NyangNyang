using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGachaMgr : MonoBehaviour
{
    [SerializeField]
    private GameObject BeforePanel;

    [SerializeField] 
    private GameObject AfterPanel;

    GameObject board;
    float rotationSpeed = 360f;
    bool isRotate = false;

    private void Start()
    {
        board = BeforePanel.transform.Find("Board").gameObject;
    }
    private void OnEnable()
    {
        isRotate = false;
    }

    public void OnClickedDrawButton()
    {
        if ( !isRotate )
        {
            StartCoroutine(RotateOverTime(1f));
            isRotate = true;
        }
    }

    IEnumerator RotateOverTime(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            board.transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime * 2);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        board.transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(1f);

        isRotate = false;
        BeforePanel.SetActive(false);
        AfterPanel.SetActive(true);
    }
}
