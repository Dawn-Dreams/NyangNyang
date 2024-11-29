using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundEffectManager : MonoBehaviour
{
    private Button _button;
    private Toggle _toggle;

    private void Start()
    {
        _button = GetComponent<Button>();
        _toggle = GetComponent<Toggle>();

        if (_button != null)
        {
            _button.onClick.AddListener(PlaySound);
        }

        if (_toggle != null)
        {
            _toggle.onValueChanged.AddListener((value) => PlaySound()); // ��� ���°� ����� ������ �Ҹ� ���
        }
    }

    private void PlaySound()
    {
        AudioManager.Instance.PlaySFX("SFX_Pop");  // ȿ���� ���
    }

    private void OnDestroy()
    {
        if (_button != null)
        {
            _button.onClick.RemoveListener(PlaySound);
        }

        if (_toggle != null)
        {
            _toggle.onValueChanged.RemoveListener((value) => PlaySound());
        }
    }
}
