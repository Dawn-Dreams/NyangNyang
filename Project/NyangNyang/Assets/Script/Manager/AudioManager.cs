using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject audioManagerObject = new GameObject("AudioManager");
                _instance = audioManagerObject.AddComponent<AudioManager>();
                //DontDestroyOnLoad(audioManagerObject);
            }
            return _instance;
        }
    }

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    private float bgmVolume = 1f;
    private float sfxVolume = 1f;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            bgmSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // BGM 재생
    public void PlayBGM(string address)
    {
        Addressables.LoadAssetAsync<AudioClip>(address).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                bgmSource.clip = handle.Result;
                bgmSource.loop = true;
                bgmSource.volume = bgmVolume;
                bgmSource.Play();
            }
            else
            {
                Debug.LogError($"Failed to load BGM at address: {address}");
            }
        };
    }

    // BGM 일시 정지
    public void PauseBGM()
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Pause();
        }
    }

    // BGM 정지
    public void StopBGM()
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
    }

    // BGM 재개
    public void ResumeBGM()
    {
        if (!bgmSource.isPlaying && bgmSource.clip != null)
        {
            bgmSource.Play();
        }
    }

    // SFX 재생
    public void PlaySFX(string address)
    {
        Addressables.LoadAssetAsync<AudioClip>(address).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                sfxSource.PlayOneShot(handle.Result, sfxVolume);
            }
            else
            {
                Debug.LogError($"Failed to load SFX at address: {address}");
            }
        };
    }

    // BGM 볼륨 설정 및 가져오기
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        if (bgmSource != null)
        {
            bgmSource.volume = bgmVolume;
        }
    }

    public float GetBGMVolume()
    {
        return bgmVolume;
    }

    // SFX 볼륨 설정 및 가져오기
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }
}
