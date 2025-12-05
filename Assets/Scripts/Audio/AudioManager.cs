using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Slider musicSlider;
    public Slider sfxSlider;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    private float musicVolume;
    private float sfxVolume;

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
        sfxSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;

        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        ApplyVolume();
    }

    private void Start()
    {
        if (musicSlider != null)
        {
            musicSlider.value = musicVolume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    public void SetMusicVolume(float v)
    {
        musicVolume = Mathf.Clamp01(v);
        if (musicSource) musicSource.volume = musicVolume;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void SetSFXVolume(float v)
    {
        sfxVolume = Mathf.Clamp01(v);
        if (sfxSource) sfxSource.volume = sfxVolume;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    private void ApplyVolume()
    {
        if (musicSource) musicSource.volume = musicVolume;
        if (sfxSource) sfxSource.volume = sfxVolume;
    }

    public void PlayMusic(AudioClip clip, bool restart = true)
    {
        if (clip == null || musicSource == null) return;

        if (!restart && musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void FadeToDefeat(AudioClip clip, float fadeTime = 1.5f)
    {
        StartCoroutine(FadeAndSwapMusic(clip, fadeTime));
    }

    public void FadeToVictory(AudioClip clip, float fadeTime = 1.5f)
    {
        StartCoroutine(FadeAndSwapMusic(clip, fadeTime));
    }

    private IEnumerator FadeAndSwapMusic(AudioClip newClip, float fadeTime)
    {
        if (musicSource == null) yield break;

        float startVolume = musicSource.volume;

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            if (!musicSource) yield break;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
            yield return null;
        }

        if (!musicSource) yield break;

        musicSource.volume = 0f;
        musicSource.Stop();

        if (newClip != null)
        {
            musicSource.clip = newClip;
            musicSource.Play();
        }

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            if (!musicSource) yield break;
            musicSource.volume = Mathf.Lerp(0f, musicVolume, t / fadeTime);
            yield return null;
        }

        if (musicSource)
            musicSource.volume = musicVolume;
    }
}
