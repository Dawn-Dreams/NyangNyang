using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelUpUI : MonoBehaviour
{
    private static PlayerLevelUpUI _instance;
    public static PlayerLevelUpUI GetInstance()
    {
        return _instance;
    }
    public GameObject newContentPanel;
    public GridLayoutGroup grid;

    public GameObject iconPrefab;
    private List<GameObject> newContentIconPool = new List<GameObject>();
    public GameObject iconObjectPool;

    public Animation levelUpObjectAnimation;
    public Button newContentAlertButton;

    private void Awake()
    {
        _instance = this;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.localPosition = Vector3.zero;
        gameObject.SetActive(false);

        for (int i = 0; i < 12; ++i)
        {
            newContentIconPool.Add(Instantiate(iconPrefab, iconObjectPool.transform));
        }

    }

    public void UserLevelUp()
    {
        // 레벨업 UI 애니메이션 재생
        levelUpObjectAnimation.gameObject.SetActive(true);
        levelUpObjectAnimation.Play();

        if (!GameManager.isMiniGameActive)
            AudioManager.Instance.PlaySFX("SFX_LevelUP");

        // 새로운 컨텐츠가 있다면 좌하단 ! UI
        if (grid.transform.childCount > 0)
        {
            newContentAlertButton.gameObject.SetActive(true);
        }


    }

    public void OpenLevelUpUI()
    {
        gameObject.SetActive(true);
        newContentAlertButton.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        for (int i = 0; i < newContentIconPool.Count; ++i)
        {
            Destroy(newContentIconPool[i].gameObject);
        }
    }

    void OnEnable()
    {
        newContentPanel.SetActive(true);
    }

    void CloseLevelUpUIWithNoNewContent()
    {
        if (grid.transform.childCount == 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void CloseLevelUpUI()
    {
        int childCount = grid.transform.childCount;

        for (int i = 0; i < childCount; ++i)
        {
            GameObject obj = grid.transform.GetChild(0).gameObject;
            obj.transform.SetParent(iconObjectPool.transform);
            newContentIconPool.Add(obj);
        }
        gameObject.SetActive(false);
    }

    public void AddNewContent(Sprite newContentSprite, string newContentName)
    {
        // 마지막 인덱스
        GameObject res = newContentIconPool[^1];
        res.transform.SetParent(grid.transform);

        newContentIconPool.RemoveAt(newContentIconPool.Count - 1);

        res.GetComponent<Image>().sprite = newContentSprite;
        res.GetComponentInChildren<TextMeshProUGUI>().text = newContentName;
        res.transform.localScale = new Vector3(1, 1, 1);
    }
}
