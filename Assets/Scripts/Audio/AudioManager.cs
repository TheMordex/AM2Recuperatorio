using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;
    [Range(0, 1)] private float musicVolume = 0.8f;
    [Range(0, 1)] private float sfxVolume = 0.8f;
    
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

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

    private void Start()
    {
        // Buscar sliders si no están asignados
        if (musicSlider == null)
            musicSlider = GameObject.Find("SliderMusic")?.GetComponent<Slider>();
        if (sfxSlider == null)
            sfxSlider = GameObject.Find("SliderSFX")?.GetComponent<Slider>();
        
        // Conectar sliders
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            musicSlider.value = musicVolume;
        }
        
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
            sfxSlider.value = sfxVolume;
        }
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
        musicVolume = Mathf.Clamp01(v);
        musicSource.volume = musicVolume;
        
        if (audioMixer != null)
        {
            float dB = Mathf.Lerp(-80f, 0f, musicVolume);
            audioMixer.SetFloat("MusicVolume", dB);
        }
    }

    public void SetSfxVolume(float v)
    {
        sfxVolume = Mathf.Clamp01(v);
        
        if (audioMixer != null)
        {
            float dB = Mathf.Lerp(-80f, 0f, sfxVolume);
            audioMixer.SetFloat("SFXVolume", dB);
        }
    }

    public float GetMusicVolume() => musicVolume;
    public float GetSfxVolume() => sfxVolume;
}