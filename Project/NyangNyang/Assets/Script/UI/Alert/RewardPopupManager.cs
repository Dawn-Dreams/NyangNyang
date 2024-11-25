using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardPopupManager : MonoBehaviour
{
    [SerializeField] private GameObject rewardPopUpPrefab;
    private GameObject rewardPopUpInstance;

    private void Awake()
    {
        if (rewardPopUpPrefab == null)
        {
            return;
        }

        if (rewardPopUpInstance == null)
        {
            rewardPopUpInstance = Instantiate(rewardPopUpPrefab, transform);
        }
    }

    public void ShowReward(int gold, int exp, float displayDuration = 3f)
    {
        if (rewardPopUpInstance == null)
        {
            rewardPopUpInstance = Instantiate(rewardPopUpPrefab, transform);

        }

        rewardPopUpInstance.SetActive(true);
        RewardPopUp rewardPopUp = rewardPopUpInstance.GetComponent<RewardPopUp>();
        if (rewardPopUp != null)
        {
            rewardPopUp.SetValues(gold, exp);
        }

        StartCoroutine(HideAfterDelay(displayDuration));
    }
    public void HideReward()
    {
        if (rewardPopUpInstance != null)
        {
            rewardPopUpInstance.SetActive(false);
        }
    }
    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideReward();
    }

}
