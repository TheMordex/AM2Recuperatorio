using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Range(0, 1)] private float musicVolume = 0.8f;
    [Range(0, 1)] private float sfxVolume = 0.8f;

    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Vector3.zero, sfxVolume);
    }

    public void SetMusicVolume(float v)
    {
        musicVolume = v;
        musicSource.volume = musicVolume;
    }

    public void SetSfxVolume(float v)
    {
        sfxVolume = v;
    }

    public float GetMusicVolume() => musicVolume;
    public float GetSfxVolume() => sfxVolume;
}