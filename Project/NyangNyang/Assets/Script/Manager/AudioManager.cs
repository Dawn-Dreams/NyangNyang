using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource bgmSource;
    public AudioSource miniGamebgmSource;
    public AudioSource sfxSource;

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

    public void PlayMainBGM(AudioClip clip)
    {
        if (bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }
    public void PauseMainBGM()
    {
        bgmSource.Pause();
    }
    public void ResumeMainBGM()
    {
        bgmSource.Play();
    }

    public void PlayMiniGameBGM(AudioClip clip)
    {
        miniGamebgmSource.clip = clip;
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