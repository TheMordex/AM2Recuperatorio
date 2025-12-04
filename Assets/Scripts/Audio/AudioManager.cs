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

        // ✅ Validar AudioMixer
        if (audioMixer == null)
        {
            Debug.LogError("❌ AudioMixer no asignado en AudioManager!");
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

        // Detect sliders on scene load
        SceneManager.activeSceneChanged += OnSceneChanged;
        
        Debug.Log($"✅ AudioManager inicializado - Music: {musicVolume}, SFX: {sfxVolume}");
    }

    private void Start()
    {
        FindSlidersInScene();
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        Debug.Log($"🎬 Cambio de escena: {oldScene.name} → {newScene.name}");
        FindSlidersInScene();
    }

    private void FindSlidersInScene()
    {
        // ✅ Buscar sliders
        musicSlider = GameObject.Find("SliderMusic")?.GetComponent<Slider>();
        sfxSlider   = GameObject.Find("SliderSFX")?.GetComponent<Slider>();

        // ✅ CRÍTICO: Configurar sliders sin disparar eventos
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveAllListeners();
            // ✅ PRIMERO asignar el valor (sin listeners)
            musicSlider.SetValueWithoutNotify(musicVolume);
            // ✅ DESPUÉS agregar el listener
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            Debug.Log($"✅ MusicSlider encontrado y configurado: {musicVolume}");
        }
        else
        {
            Debug.Log("ℹ️ SliderMusic no encontrado en esta escena");
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveAllListeners();
            // ✅ PRIMERO asignar el valor (sin listeners)
            sfxSlider.SetValueWithoutNotify(sfxVolume);
            // ✅ DESPUÉS agregar el listener
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
            Debug.Log($"✅ SFXSlider encontrado y configurado: {sfxVolume}");
        }
        else
        {
            Debug.Log("ℹ️ SliderSFX no encontrado en esta escena");
        }
    }

    // --------------------------------------------------
    // MUSIC
    // --------------------------------------------------

    public void PlayMusic(AudioClip clip)
    {
        // ✅ Validaciones completas
        if (musicSource == null)
        {
            Debug.LogError("❌ MusicSource es null!");
            return;
        }

        if (clip == null)
        {
            Debug.LogWarning("⚠️ AudioClip es null en PlayMusic");
            return;
        }

        // ✅ Solo asignar clip y hacer Play
        musicSource.clip = clip;
        musicSource.Play();
        Debug.Log($"🎵 Reproduciendo música: {clip.name}");
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
            Debug.Log("🔇 Música detenida");
        }
    }

    // --------------------------------------------------
    // SFX
    // --------------------------------------------------

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null)
        {
            Debug.LogError("❌ SFXSource es null!");
            return;
        }

        if (clip == null)
        {
            Debug.LogWarning("⚠️ AudioClip es null en PlaySFX");
            return;
        }

        sfxSource.PlayOneShot(clip);
    }

    // --------------------------------------------------
    // VOLUMEN + MIXER
    // --------------------------------------------------

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

        // ✅ Convertir 0-1 a -80dB a 0dB (escala logarítmica)
        float musicDB = musicVolume > 0.0001f ? Mathf.Log10(musicVolume) * 20f : -80f;
        float sfxDB = sfxVolume > 0.0001f ? Mathf.Log10(sfxVolume) * 20f : -80f;

        audioMixer.SetFloat("MusicVolume", musicDB);
        audioMixer.SetFloat("SFXVolume", sfxDB);
    }

    // --------------------------------------------------
    // Cleanup
    // --------------------------------------------------
    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    // --------------------------------------------------
    // Getters
    // --------------------------------------------------
    public float GetMusicVolume() => musicVolume;
    public float GetSfxVolume() => sfxVolume;
}