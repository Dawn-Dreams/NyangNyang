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

    private string userUID = "1234-5678-UID"; // 예시 UID

    void Start()
    {
        // 초기 설정
        //bgmVolumeSlider.value = AudioManager.Instance.bgmVolume;
        //sfxVolumeSlider.value = AudioManager.Instance.sfxVolume;
        uidText.text = userUID;

        notificationToggle.isOn = PlayerPrefs.GetInt("notificationsEnabled", 1) == 1;
        vibrationToggle.isOn = PlayerPrefs.GetInt("vibrationEnabled", 1) == 1;

        // 이벤트 연결
        bgmVolumeSlider.onValueChanged.AddListener(delegate { OnBGMVolumeChanged(); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { OnSFXVolumeChanged(); });
        copyUIDButton.onClick.AddListener(CopyUIDToClipboard);
    }

    public void OpenSettingsMenu()
    {
        settingsMenuCanvas.SetActive(true);
        Time.timeScale = 0f;  // 설정 창을 여는 동안 게임 일시정지
    }

    public void CloseSettingsMenu()
    {
        settingsMenuCanvas.SetActive(false);
        Time.timeScale = 1f;  // 설정 창을 닫으면 게임 재개
    }

    public void SubmitCoupon()
    {
        string couponCode = couponInputField.text;
        Debug.Log($"쿠폰 코드 {couponCode} 제출됨");
        // 쿠폰 코드 검증 및 보상 지급 로직 추가 가능
    }

    public void ViewAccountInfo()
    {
        Debug.Log("계정 정보 확인");
        accountInfoText.text = "계정: 사용자123 (Level 45)";
        // 실제 계정 정보 로드 로직 추가
    }

    public void ViewTermsOfService()
    {
        Debug.Log("게임 이용 약관 확인");
        termsOfServiceText.text = "여기에 게임 이용 약관을 표시합니다...";
        // 약관 불러오기 로직 추가
    }

    public void ToggleNotifications(bool isOn)
    {
        PlayerPrefs.SetInt("notificationsEnabled", isOn ? 1 : 0);
        Debug.Log($"알림 설정: {(isOn ? "켜짐" : "꺼짐")}");
    }

    public void ToggleVibration(bool isOn)
    {
        PlayerPrefs.SetInt("vibrationEnabled", isOn ? 1 : 0);
        Debug.Log($"진동 설정: {(isOn ? "켜짐" : "꺼짐")}");
    }

    public void OnBGMVolumeChanged()
    {
        //AudioManager.Instance.bgmVolume = bgmVolumeSlider.value;
        Debug.Log($"배경음악 볼륨: {bgmVolumeSlider.value}");
    }

    public void OnSFXVolumeChanged()
    {
        //AudioManager.Instance.sfxVolume = sfxVolumeSlider.value;
        Debug.Log($"효과음 볼륨: {sfxVolumeSlider.value}");
    }

    public void CopyUIDToClipboard()
    {
        GUIUtility.systemCopyBuffer = userUID;
        Debug.Log($"UID {userUID} 복사됨");
    }
}
