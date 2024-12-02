using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum OwningSelectButtonType
{
    NotOwning, Owning, CurrentSelect, CurrentPreview, Count
}

public class CostumeElement : MonoBehaviour
{
    private static Dictionary<OwningSelectButtonType, string> buttonTypeText =
        new Dictionary<OwningSelectButtonType, string>
        {
            { OwningSelectButtonType.NotOwning, "미보유" },
            { OwningSelectButtonType.Owning, "보유중" },
            { OwningSelectButtonType.CurrentSelect, "선택중" },
            { OwningSelectButtonType.CurrentPreview , "미리보기" }
        };

    public TextMeshProUGUI costumeNameText;
    public Button selectButton;
    public Image owningImage;
    public TextMeshProUGUI selectButtonText;
    public Image previewImage;

    private AddressableHandle<Sprite> _currentSelectSprite;
    private AddressableHandle<Sprite> _owningSprite;
    private AddressableHandle<Sprite> _notOwningSprite;

    private Dictionary<OwningSelectButtonType, AddressableHandle<Sprite>> _sprites =
        new Dictionary<OwningSelectButtonType, AddressableHandle<Sprite>>();

    // 프리뷰 스프라이트
    private AddressableHandle<Sprite> _previewSprite ;

    private CatCostumePart _costumePart;
    private int _costumeIndex;

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

        if (_previewSprite != null)
        {
            _previewSprite.Release();
        }
    }

    private void AssetLoad()
    {
        for (int i = 0; i < (int)OwningSelectButtonType.Count; ++i)
        {
            OwningSelectButtonType type = (OwningSelectButtonType)i;
            _sprites.Add(type,new AddressableHandle<Sprite>().Load("Sprite/Button/"+type));
        }
        //_sprites.Add(OwningSelectButtonType.CurrentSelect, new AddressableHandle<Sprite>().Load("Sprite/Button/CurrentSelect"));
        //_sprites.Add(OwningSelectButtonType.Owning, new AddressableHandle<Sprite>().Load("Sprite/Button/Owning"));
        //_sprites.Add(OwningSelectButtonType.NotOwning, new AddressableHandle<Sprite>().Load("Sprite/Button/NotOwning"));
        
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
        owningImage.sprite = _sprites[type].obj;
        selectButtonText.text = buttonTypeText[type];
        
        // 보유중/선택중 버튼은 선택 가능하게, (미리보기로 인해 선택중도 추가)
        bool selectable = (type == OwningSelectButtonType.Owning || type == OwningSelectButtonType.CurrentSelect);
        selectButton.interactable = selectable;
        
    }

    public void SetCostumeData(CatCostumePart catCostumePart, int index)
    {
        _costumePart = catCostumePart;
        _costumeIndex = index;

        // 이름 변경
        //SetNameText(CostumeManager.GetInstance().GetCostumeName(_costumePart, _costumeIndex));
        SetNameText(EnumTranslator.GetCostumeText(_costumePart, _costumeIndex));

        // 프리뷰 스프라이트 설정
        if (_previewSprite != null)
        {
            _previewSprite.Release();
            _previewSprite = null;
        }
        _previewSprite = new AddressableHandle<Sprite>();
        _previewSprite.Load("Sprite/Costume/" + catCostumePart + "/" +
                            CostumeManager.GetInstance().GetCostumeName(catCostumePart, index, false));
        if (_previewSprite.obj != null)
        {
            previewImage.gameObject.SetActive(true);
            previewImage.sprite = _previewSprite.obj;
        }
        else
        {
            previewImage.gameObject.SetActive(false);
        }
        
    }
}
