using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public bool sfxSoundOn = true;
    public bool bgSoundOn = true;
    //public AudioClip touchSound; // ��ġ �� ����� ȿ����

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
        // �̱��� ������ �����Ͽ� AudioManager�� �ϳ��� �����ϵ���
        if (Instance == null)
        {
            Instance = this;

            // AudioManager�� �θ� ������Ʈ���� �и��Ǿ� �ֻ����� �̵��ǵ��� ����
            transform.SetParent(null);

            // �� �̵� �� �ı����� �ʵ��� ����
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //private void Update()
    //{
    //// ����� ��ġ ����
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
    //// PC ���콺 Ŭ�� ����
    //if (Input.GetMouseButtonDown(0))
    //{
    //    PlayTouchSound();
    //}
    //}

    private void PlayTouchSound()
    {
        //if (touchSound != null)
        //{
        //    PlaySFX(touchSound); // ��ġ�� Ŭ�� �� ������ ȿ���� ���
        //}
    }
    private void Start()
    {
        // ���� ���� �� ����� ���� ����
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
