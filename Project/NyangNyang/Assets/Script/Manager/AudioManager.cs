using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource bgmSource;
    public AudioSource sfxSource;

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
