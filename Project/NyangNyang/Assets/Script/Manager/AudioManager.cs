using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public bool sfxSoundOn = true;
    public bool bgSoundOn = true;
    //public AudioClip touchSound; // 터치 시 재생될 효과음

    public float bgmVolume
    {
        get { return PlayerPrefs.GetFloat("BGMVolume", 1f); }
        set
        {
            PlayerPrefs.SetFloat("BGMVolume", value);
            bgmSource.volume = value;
        }
    }

    public float sfxVolume
    {
        get { return PlayerPrefs.GetFloat("SFXVolume", 1f); }
        set
        {
            PlayerPrefs.SetFloat("SFXVolume", value);
            sfxSource.volume = value;
        }
    }

    void Awake()
    {
        // 싱글톤 패턴을 적용하여 AudioManager가 하나만 존재하도록
        if (Instance == null)
        {
            Instance = this;

            // AudioManager가 부모 오브젝트에서 분리되어 최상위로 이동되도록 설정
            transform.SetParent(null);

            // 씬 이동 시 파괴되지 않도록 설정
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //private void Update()
    //{
    //// 모바일 터치 감지
    //if (Input.touchCount > 0)
    //{
    //    for (int i = 0; i < Input.touchCount; i++)
    //    {
    //        if (Input.GetTouch(i).phase == TouchPhase.Began)
    //        {
    //            PlayTouchSound();
    //        }
    //    }
    //}
    //// PC 마우스 클릭 감지
    //if (Input.GetMouseButtonDown(0))
    //{
    //    PlayTouchSound();
    //}
    //}

    private void PlayTouchSound()
    {
        //if (touchSound != null)
        //{
        //    PlaySFX(touchSound); // 터치나 클릭 시 설정된 효과음 재생
        //}
    }
    private void Start()
    {
        // 게임 시작 시 저장된 볼륨 적용
        bgmSource.volume = bgmVolume;
        sfxSource.volume = sfxVolume;
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
