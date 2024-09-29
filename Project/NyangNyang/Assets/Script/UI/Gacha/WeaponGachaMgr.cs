using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private void OnDisable()
    {
        BeforePanel.SetActive(true);
        AfterPanel.SetActive(false);
    }

    public void OnClickedDrawButton()
    {
        // ÇÏ³ª¸¸ »Ì±â
        if ( !isRotate )
        {
            BeforePanel.SetActive(true);
            AfterPanel.SetActive(false);
            StartCoroutine(RotateOverTime(1f, 1));
            isRotate = true;
        }
    }

    public void OnClickedDrawAllButton()
    {
        // ÀÏ°ý »Ì±â
        if (!isRotate)
        {
            BeforePanel.SetActive(true);
            AfterPanel.SetActive(false);
            StartCoroutine(RotateOverTime(1f, 10));
            isRotate = true;
        }
    }

    IEnumerator RotateOverTime(float duration, int n)
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


        AfterPanel.GetComponent<PickUpWeapon>().ShowPickUpWeapon(5);
    }
}
