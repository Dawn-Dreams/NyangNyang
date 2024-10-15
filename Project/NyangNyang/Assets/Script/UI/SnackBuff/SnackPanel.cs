using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SnackType
{
    Atk, Hp, Gold
}
public class SnackPanel : MonoBehaviour
{
    public SnackType snackType;

    public Button showAdButton;
    public GameObject eatingImageObject;

}
