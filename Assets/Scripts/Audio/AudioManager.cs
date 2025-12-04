using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("Sliders (autodetect)")]
    private Slider musicSlider;
    private Slider sfxSlider;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    private float musicVolume = 0.8f;
    private float sfxVolume = 0.8f;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Validar AudioMixer
        if (audioMixer == null)
        {
            
        }

        // Music source
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.outputAudioMixerGroup = musicGroup;

        // Reusable SFX source
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.outputAudioMixerGroup = sfxGroup;

        // Load saved volumes
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        sfxVolume   = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        ApplyMixerVolumes();
        
        SceneManager.activeSceneChanged += OnSceneChanged;
        
    }

    private void Start()
    {
        FindSlidersInScene();
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        FindSlidersInScene();
    }

    private void FindSlidersInScene()
    {
        musicSlider = GameObject.Find("SliderMusic")?.GetComponent<Slider>();
        sfxSlider   = GameObject.Find("SliderSFX")?.GetComponent<Slider>();
        
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveAllListeners();
            musicSlider.SetValueWithoutNotify(musicVolume);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.SetValueWithoutNotify(sfxVolume);
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource == null)
        {
            return;
        }

        if (clip == null)
        {
            return;
        }
        
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
            Debug.Log("🔇 Música detenida");
        }
    }
    

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null)
        {
            return;
        }

        if (clip == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip);
    }
    
    public void SetMusicVolume(float v)
    {
        musicVolume = Mathf.Clamp01(v);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();

        ApplyMixerVolumes();
        
        Debug.Log($"🔊 Volumen música ajustado: {musicVolume:F2}");
    }

    public void SetSfxVolume(float v)
    {
        sfxVolume = Mathf.Clamp01(v);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();

        ApplyMixerVolumes();
        
        Debug.Log($"🔊 Volumen SFX ajustado: {sfxVolume:F2}");
    }

    private void ApplyMixerVolumes()
    {
        if (audioMixer == null)
        {
            Debug.LogWarning("⚠️ AudioMixer es null, no se puede aplicar volumen");
            return;
        }
        
        float musicDB = musicVolume > 0.0001f ? Mathf.Log10(musicVolume) * 20f : -80f;
        float sfxDB = sfxVolume > 0.0001f ? Mathf.Log10(sfxVolume) * 20f : -80f;

        audioMixer.SetFloat("MusicVolume", musicDB);
        audioMixer.SetFloat("SFXVolume", sfxDB);
    }
    
    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }
    
    public float GetMusicVolume() => musicVolume;
    public float GetSfxVolume() => sfxVolume;
}