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
        owningImage.sprite = _sprites[type].obj;
        selectButtonText.text = buttonTypeText[type];
    }

    public void SetCostumeData(CatCostumePart catCostumePart, int index)
    {
        _costumePart = catCostumePart;
        _costumeIndex = index;

        // 이름 변경
        SetNameText(CostumeManager.GetInstance().GetCostumeName(_costumePart, _costumeIndex));

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
        

        // 보유중인지 변경
        // TODO: 10/27 임시코드. 추후 서버에서 보유중인 코스튬 정보를 받아와 갱신 예정
        SetSelectButtonType(OwningSelectButtonType.Owning);
    }
}
