using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum OwningSelectButtonType
{
    NotOwning, Owning, CurrentSelect, Count
}

public class CostumeElement : MonoBehaviour
{
    private static Dictionary<OwningSelectButtonType, string> buttonTypeText =
        new Dictionary<OwningSelectButtonType, string>
        {
            { OwningSelectButtonType.NotOwning, "미보유" },
            { OwningSelectButtonType.Owning, "보유중" },
            { OwningSelectButtonType.CurrentSelect, "선택중" }
        };

    public TextMeshProUGUI costumeNameText;
    public Button selectButton;
    public TextMeshProUGUI selectButtonText;

    private AddressableHandle<Sprite> _currentSelectSprite;
    private AddressableHandle<Sprite> _owningSprite;
    private AddressableHandle<Sprite> _notOwningSprite;

    private Dictionary<OwningSelectButtonType, AddressableHandle<Sprite>> _sprites =
        new Dictionary<OwningSelectButtonType, AddressableHandle<Sprite>>();


    void Awake()
    {
        AssetLoad();    
    }

    void OnDestroy()
    {
        foreach (var VARIABLE in _sprites)
        {
            VARIABLE.Value.Release();
        }
    }

    private void AssetLoad()
    {
        _sprites.Add(OwningSelectButtonType.CurrentSelect, new AddressableHandle<Sprite>().Load("Sprite/Button/CurrentSelect"));
        _sprites.Add(OwningSelectButtonType.Owning, new AddressableHandle<Sprite>().Load("Sprite/Button/Owning"));
        _sprites.Add(OwningSelectButtonType.NotOwning, new AddressableHandle<Sprite>().Load("Sprite/Button/NotOwning"));
    }

    public void SetNameText(string nameString)
    {
        if (costumeNameText)
        {
            costumeNameText.text = nameString;
        }
    }

    public void SetSelectButtonType(OwningSelectButtonType type)
    {
        selectButton.image.sprite = _sprites[type].obj;
        selectButtonText.text = buttonTypeText[type];
    }
}
