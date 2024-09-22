using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUnlock : MonoBehaviour
{
    public GameObject unlockImg;

    public void Unlock()
    {
        unlockImg.SetActive(false);
    }
}
