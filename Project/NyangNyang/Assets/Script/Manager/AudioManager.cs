using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource bgmSource;
    public AudioSource miniGamebgmSource;
    public AudioSource sfxSource;
    public AudioClip[] clips;

    public float bgmVolume
    {
        get { return PlayerPrefs.GetFloat("BGMVolume", 1f); }
        set
        {
            PlayerPrefs.SetFloat("BGMVolume", value);
            bgmSource.volume = value;
            miniGamebgmSource.volume = value;
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
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        bgmSource.volume = bgmVolume;
        miniGamebgmSource.volume = bgmVolume;
        sfxSource.volume = sfxVolume;
    }

    public void PlayMainBGM()
    {
        if (bgmSource.clip != clips[0])
        {
            bgmSource.clip = clips[0];
            bgmSource.Play();
        }

    }

    // 외부에서 재생할 때
    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }

    public void PauseBGM()
    {
        bgmSource.Pause();
    }
    public void ResumeBGM()
    {
        bgmSource.Play();
    }

    public void PlayMiniGameBGM()
    {
        miniGamebgmSource.clip = clips[1];
        miniGamebgmSource.Play();
    }

    public void StopMiniGameBGM()
    {
        miniGamebgmSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}