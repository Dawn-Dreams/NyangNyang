using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenuCanvas;

    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;
    public TMP_InputField uidText;
    public Button copyUIDButton;

    private string userUID = "1234-5678-UID"; // ����� UID

    void Start()
    {
        // �ʱ� ����
        bgmVolumeSlider.value = AudioManager.Instance.bgmVolume;
        sfxVolumeSlider.value = AudioManager.Instance.sfxVolume;
        uidText.text = userUID;

        // �̺�Ʈ ����
        bgmVolumeSlider.onValueChanged.AddListener(delegate { OnBGMVolumeChanged(); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { OnSFXVolumeChanged(); });
        copyUIDButton.onClick.AddListener(CopyUIDToClipboard);
    }

    public void OpenSettingsMenu()
    {
        settingsMenuCanvas.SetActive(true);
        Time.timeScale = 0f;  // ���� â�� ���� �� ������ �Ͻ�����
    }

    public void CloseSettingsMenu()
    {
        settingsMenuCanvas.SetActive(false);
        Time.timeScale = 1f;  // ���� â�� ������ ������ �ٽ� �簳
    }

    public void SubmitCoupon()
    {
        // ���� �ڵ� �Է� �� ó�� ���� �߰� ����
    }

    public void ViewAccountInfo()
    {
        Debug.Log("���� ���� Ȯ��");
        // ���� ���� �ε� ���� �߰� ����
    }

    public void ViewTermsOfService()
    {
        Debug.Log("�̿� ��� Ȯ��");
        // �̿� ��� �ε� ���� �߰� ����
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
        AudioManager.Instance.bgmVolume = bgmVolumeSlider.value;
    }

    public void OnSFXVolumeChanged()
    {
        AudioManager.Instance.sfxVolume = sfxVolumeSlider.value;
    }

    public void CopyUIDToClipboard()
    {
        GUIUtility.systemCopyBuffer = userUID;
        Debug.Log($"UID {userUID} �����");
    }
}
