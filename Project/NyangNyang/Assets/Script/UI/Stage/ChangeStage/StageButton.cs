using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum StageButtonType
{
    Normal, Select, Current, Close
}

public class StageButton : MonoBehaviour
{
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite currentSprite;
    [SerializeField] private Sprite closeSprite;

    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI stageNumberText;
    [SerializeField] private Image buttonImage;
    private int stageNumber = 1;
    private StageButtonType _buttonType = StageButtonType.Normal;

    [SerializeField] private Image selectButtonImage;

    

    public void Initialize(int newStageNumber)
    {
        stageNumber = newStageNumber;
        stageNumberText.text = stageNumber.ToString();

    }

    public Button GetButton()
    {
        return button;
    }

    public void SetButtonType(StageButtonType type)
    {
        _buttonType = type;
        SetImageSprite(type);
    }

    public void UnSelect()
    {
        selectButtonImage.gameObject.SetActive(false);
    }

    public StageButtonType GetButtonType()
    {
        return _buttonType;
    }

    private void SetImageSprite(StageButtonType type)
    {
        Sprite newSprite = normalSprite;
        selectButtonImage.gameObject.SetActive(false);
        switch (type)
        {
            case StageButtonType.Normal:
                newSprite = normalSprite;
                break;
            case StageButtonType.Select:
                newSprite = null;
                selectButtonImage.gameObject.SetActive(true);
                break;
            case StageButtonType.Current:
                newSprite = currentSprite;
                break;
            case StageButtonType.Close:
                newSprite = closeSprite;
                break;
        }

        if (newSprite != null)
        {
            buttonImage.sprite = newSprite;
        }
        
    }
}
