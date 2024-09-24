using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class MenuButton : MonoBehaviour
{
    private Image _image;
    [SerializeField]
    private Sprite _activeSprite;
    [SerializeField]
    private Sprite _inactiveSprite;

    public bool isActive = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _image = gameObject.GetComponent<Image>();
        isActive = _image.sprite == _activeSprite;
    }

    public void SetButtonActivity(bool toActive)
    {
        isActive = toActive;
        _image.sprite = toActive ? _activeSprite :  _inactiveSprite;
    }

    // 에디터 property 변경 시 호출되는 함수
    private void OnValidate()
    {
        Image imageComponent = gameObject.GetComponent<Image>();
        if (imageComponent && imageComponent.sprite != _inactiveSprite)
        {
            imageComponent.sprite = _inactiveSprite;
        }
    }

    public void SetButtonActivityOnDebug(bool toActive)
    {
        Image imageComponent = gameObject.GetComponent<UnityEngine.UI.Image>();
        imageComponent.sprite = toActive ? _activeSprite : _inactiveSprite;
        isActive = imageComponent.sprite == _activeSprite;
    }
}
