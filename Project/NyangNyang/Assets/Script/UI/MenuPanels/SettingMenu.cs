using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenuCanvas;

    // UI Elements
    public InputField couponInputField;
    public Text accountInfoText;
    public Text termsOfServiceText;
    public Toggle notificationToggle;
    public Toggle vibrationToggle;
    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;
    public Text uidText;
    public Button copyUIDButton;

    private string userUID = "1234-5678-UID"; // ���� UID

    void Start()
    {
        // �ʱ� ����
        //bgmVolumeSlider.value = AudioManager.Instance.bgmVolume;
        //sfxVolumeSlider.value = AudioManager.Instance.sfxVolume;
        uidText.text = userUID;

        notificationToggle.isOn = PlayerPrefs.GetInt("notificationsEnabled", 1) == 1;
        vibrationToggle.isOn = PlayerPrefs.GetInt("vibrationEnabled", 1) == 1;

        // �̺�Ʈ ����
        bgmVolumeSlider.onValueChanged.AddListener(delegate { OnBGMVolumeChanged(); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { OnSFXVolumeChanged(); });
        copyUIDButton.onClick.AddListener(CopyUIDToClipboard);
    }

    public void OpenSettingsMenu()
    {
        settingsMenuCanvas.SetActive(true);
        Time.timeScale = 0f;  // ���� â�� ���� ���� ���� �Ͻ�����
    }

    public void CloseSettingsMenu()
    {
        settingsMenuCanvas.SetActive(false);
        Time.timeScale = 1f;  // ���� â�� ������ ���� �簳
    }

    public void SubmitCoupon()
    {
        string couponCode = couponInputField.text;
        Debug.Log($"���� �ڵ� {couponCode} �����");
        // ���� �ڵ� ���� �� ���� ���� ���� �߰� ����
    }

    public void ViewAccountInfo()
    {
        Debug.Log("���� ���� Ȯ��");
        accountInfoText.text = "����: �����123 (Level 45)";
        // ���� ���� ���� �ε� ���� �߰�
    }

    public void ViewTermsOfService()
    {
        Debug.Log("���� �̿� ��� Ȯ��");
        termsOfServiceText.text = "���⿡ ���� �̿� ����� ǥ���մϴ�...";
        // ��� �ҷ����� ���� �߰�
    }

    public void ToggleNotifications(bool isOn)
    {
        PlayerPrefs.SetInt("notificationsEnabled", isOn ? 1 : 0);
        Debug.Log($"�˸� ����: {(isOn ? "����" : "����")}");
    }

    public void ToggleVibration(bool isOn)
    {
        PlayerPrefs.SetInt("vibrationEnabled", isOn ? 1 : 0);
        Debug.Log($"���� ����: {(isOn ? "����" : "����")}");
    }

    public void OnBGMVolumeChanged()
    {
        //AudioManager.Instance.bgmVolume = bgmVolumeSlider.value;
        Debug.Log($"������� ����: {bgmVolumeSlider.value}");
    }

    public void OnSFXVolumeChanged()
    {
        //AudioManager.Instance.sfxVolume = sfxVolumeSlider.value;
        Debug.Log($"ȿ���� ����: {sfxVolumeSlider.value}");
    }

    public void CopyUIDToClipboard()
    {
        GUIUtility.systemCopyBuffer = userUID;
        Debug.Log($"UID {userUID} �����");
    }
}
