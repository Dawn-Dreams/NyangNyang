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
            _toggle.onValueChanged.AddListener((value) => PlaySound()); // 토글 상태가 변경될 때마다 소리 재생
        }
    }

    private void PlaySound()
    {
        AudioManager.Instance.PlaySFX("SFX_Pop");  // 효과음 재생
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
