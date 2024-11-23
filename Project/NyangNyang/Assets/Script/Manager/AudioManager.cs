using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource bgmSource;
    public AudioSource miniGamebgmSource;
    public AudioSource sfxSource;
    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

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
        miniGamebgmSource.clip = bgmClips[2];
    }

    public void PlayMainBGM()
    {
        if (bgmSource.clip != bgmClips[1])
        {
            bgmSource.clip = bgmClips[1];
            bgmSource.Play();
        }

    }

    // 외부에서 재생할 때

    // 클립으로 재생
    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }

    // 인덱스로 재생
    public void PlayBGM(int index)
    {
        if (bgmSource.clip != bgmClips[index])
        {
            bgmSource.clip = bgmClips[index];
            bgmSource.Play();
        }
    }

    public void PauseBGM()
    {
        Debug.Log("PauseBGM");
        bgmSource.Pause();
    }
    public void ResumeBGM()
    {
        Debug.Log("ResumeBGM");
        bgmSource.Play();
    }

    // 미니게임은 고정
    public void PlayMiniGameBGM()
    {
        miniGamebgmSource.clip = bgmClips[2];
        miniGamebgmSource.Play();
    }

    public void StopMiniGameBGM()
    {
        miniGamebgmSource.Stop();
    }


    // 효과음 클립으로 재생
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    // 효과음 인덱스로 재생
    public void PlaySFX(int index)
    {
        sfxSource.clip = sfxClips[index];
        sfxSource.PlayOneShot(sfxSource.clip);
    }
}