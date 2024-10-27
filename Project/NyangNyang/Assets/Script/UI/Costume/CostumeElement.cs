using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Rendering.FilterWindow;

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

    public void SetCostumeData(CatCostumePart catCostumePart, int index)
    {
        _costumePart = catCostumePart;
        _costumeIndex = index;

        // 이름 변경
        SetNameText(CostumeManager.GetInstance().GetCostumeName(_costumePart, _costumeIndex));


        // 프리뷰 오브젝트 변경
        GameObject prefab = CostumeManager.GetInstance().GetCatCostumePrefab(_costumePart, _costumeIndex);
        if (prefab)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.transform.localScale = new Vector3(200, 200, 200);
            obj.layer = LayerMask.NameToLayer("UI");
            foreach (Transform child in obj.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("UI");
            }
        }
        
        //// UI 레이어로 변경
        

        // 보유중인지 변경
        // TODO: 10/27 임시코드. 추후 서버에서 보유중인 코스튬 정보를 받아와 갱신 예정
        SetSelectButtonType(OwningSelectButtonType.Owning);
    }
}
