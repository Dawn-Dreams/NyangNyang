using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenuCanvas;

    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider brightnessSlider;
    public Toggle bgmSoundOnOffToggle; // BGM ��� �߰�
    public Toggle sfxSoundOnOffToggle; // SFX ��� �߰�
    public Toggle alertToggle;
    public Toggle vibrationToggle;
    public TMP_InputField uidText;
    public TMP_InputField nicknameText;
    public TMP_InputField couponText;
    public Button copyUIDButton;
    public Button changeNicknameButton;
    public Button submitCouponButton;
    public Button accountButton;
    public Button termsOfServiceButton;
    public Button makersButton;
    public Button askButton;

    private string userUID = "1234-5678-UID"; // ����� UID
    private string userNickname = "Player";  // �⺻ �г���
    private string normalCoupon = "DAWNDREAMS2024";  // ���� ���� ��ȣ
    private float brightness = 1f;  // �⺻ ��� �� (1.0 = �ִ� ���)

    void Start()
    {
        // �ʱ� ����
        bgmVolumeSlider.value = AudioManager.Instance.bgmVolume;
        sfxVolumeSlider.value = AudioManager.Instance.sfxVolume;
        brightnessSlider.value = brightness;
        bgmSoundOnOffToggle.isOn = bgmVolumeSlider.value > 0;
        sfxSoundOnOffToggle.isOn = sfxVolumeSlider.value > 0;
        alertToggle.isOn = PlayerPrefs.GetInt("notificationsEnabled", 1) == 1;
        vibrationToggle.isOn = PlayerPrefs.GetInt("vibrationEnabled", 1) == 1;
        uidText.text = userUID;
        nicknameText.text = userNickname;
        couponText.text = normalCoupon;

        // �̺�Ʈ ����
        bgmVolumeSlider.onValueChanged.AddListener(delegate { OnBGMVolumeChanged(); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { OnSFXVolumeChanged(); });
        brightnessSlider.onValueChanged.AddListener(delegate { OnBrightnessChanged(); });
        bgmSoundOnOffToggle.onValueChanged.AddListener(delegate { ToggleBGSoundOnOff(); });
        sfxSoundOnOffToggle.onValueChanged.AddListener(delegate { ToggleSFXSoundOnOff(); });
        alertToggle.onValueChanged.AddListener(ToggleNotifications);
        vibrationToggle.onValueChanged.AddListener(ToggleVibration);
        copyUIDButton.onClick.AddListener(CopyUIDToClipboard);
        changeNicknameButton.onClick.AddListener(ChangeNickname);
        submitCouponButton.onClick.AddListener(SubmitCoupon);

        accountButton.onClick.AddListener(ViewAccountInfo);
        termsOfServiceButton.onClick.AddListener(ViewTermsOfService);
        makersButton.onClick.AddListener(ViewMakersInfo);
        askButton.onClick.AddListener(ViewAskService);
    }

    public void OpenSettingsMenu()
    {
        settingsMenuCanvas.SetActive(true);
        //Time.timeScale = 0f;  // �Ͻ�����
    }

    public void CloseSettingsMenu()
    {
        settingsMenuCanvas.SetActive(false);
        //Time.timeScale = 1f;  // �簳
    }

    public void SubmitCoupon()
    {
        Debug.Log("���� ����");
        // ���� �ڵ� �Է� �� ó��
    }
    
    public void ViewAccountInfo()
    {
        Debug.Log("���� ���� Ȯ��");
        // ���� ���� �ε�
    }

    public void ViewTermsOfService()
    {
        Debug.Log("�̿� ��� Ȯ��");
        // �̿� ��� �ε�
    }

      public void ViewMakersInfo()
    {
        Debug.Log("�������� Ȯ��");

    }

    public void ViewAskService()
    {
        Debug.Log("���� ���� Ȯ��");

    }

    public void ToggleNotifications(bool isOn)
    {
        PlayerPrefs.SetInt("notificationsEnabled", isOn ? 1 : 0);
    }

    public void ToggleVibration(bool isOn)
    {
        PlayerPrefs.SetInt("vibrationEnabled", isOn ? 1 : 0);
    }

    public void ToggleBGSoundOnOff()
    {
        if (bgmSoundOnOffToggle.isOn)
        {
            AudioManager.Instance.bgmVolume = 0.5f;
        }
        else
        {
            AudioManager.Instance.bgmVolume = 0.0f;
        }

        bgmVolumeSlider.value = AudioManager.Instance.bgmVolume;
    }

    public void ToggleSFXSoundOnOff()
    {
        if (sfxSoundOnOffToggle.isOn)
        {
            AudioManager.Instance.sfxVolume = 0.5f;
        }
        else
        {
            AudioManager.Instance.sfxVolume = 0.0f;
        }

        sfxVolumeSlider.value = AudioManager.Instance.sfxVolume;
    }

    public void OnBGMVolumeChanged()
    {
        AudioManager.Instance.bgmVolume = bgmVolumeSlider.value;
        if (bgmVolumeSlider.value != 0.0f)
        {
            bgmSoundOnOffToggle.isOn = true;
        }
        else
        {
            bgmSoundOnOffToggle.isOn = false;
        }
    }

    public void OnSFXVolumeChanged()
    {
        AudioManager.Instance.sfxVolume = sfxVolumeSlider.value;
        if (sfxVolumeSlider.value != 0.0f)
        {
            sfxSoundOnOffToggle.isOn = true;
        }
        else
        {
            sfxSoundOnOffToggle.isOn = false;
        }
    }

    public void OnBrightnessChanged()
    {
        brightness = brightnessSlider.value;
        Screen.brightness = brightness;
    }

    public void CopyUIDToClipboard()
    {
        GUIUtility.systemCopyBuffer = userUID;
        Debug.Log($"UID {userUID} �����");
    }

    public void ChangeNickname()
    {
        userNickname = nicknameText.text;
        Debug.Log($"�г����� {userNickname}(��)�� �����");
        // �г��� ���� �� ������ ����
    }
}
