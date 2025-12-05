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

    private Slider musicSlider;
    private Slider sfxSlider;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    private float musicVolume = 0.8f;
    private float sfxVolume = 0.8f;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeManager()
    {
        if (Instance == null)
        {
            GameObject managerObj = new GameObject("AudioManager");
            managerObj.AddComponent<AudioManager>();
            DontDestroyOnLoad(managerObj);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        CreateAudioSources();
        LoadVolumes();
        ApplyMixerVolumes();
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void CreateAudioSources()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.outputAudioMixerGroup = musicGroup;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.outputAudioMixerGroup = sfxGroup;
    }

    private void LoadVolumes()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
    }

    private void Start()
    {
        FindAndConnectSliders();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindAndConnectSliders();
        ApplyMixerVolumes();
    }

    private void FindAndConnectSliders()
    {
        musicSlider = GameObject.Find("SliderMusic")?.GetComponent<Slider>();
        sfxSlider = GameObject.Find("SliderSFX")?.GetComponent<Slider>();

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

    public void PlayMusic(AudioClip clip, bool forceRestart = false)
    {
        if (musicSource == null || clip == null)
            return;

        if (!forceRestart && musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void PauseMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
            musicSource.Pause();
    }

    public void ResumeMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
            musicSource.UnPause();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    public void SetMusicVolume(float v)
    {
        musicVolume = Mathf.Clamp01(v);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();

        ApplyMixerVolumes();
    }

    public void SetSfxVolume(float v)
    {
        sfxVolume = Mathf.Clamp01(v);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();

        ApplyMixerVolumes();
    }

    private void ApplyMixerVolumes()
    {
        if (audioMixer == null)
            return;

        float musicDB = musicVolume > 0.0001f ? Mathf.Log10(musicVolume) * 20f : -80f;
        float sfxDB = sfxVolume > 0.0001f ? Mathf.Log10(sfxVolume) * 20f : -80f;

        audioMixer.SetFloat("MusicVolume", musicDB);
        audioMixer.SetFloat("SFXVolume", sfxDB);
    }

    public float GetMusicVolume() => musicVolume;
    public float GetSfxVolume() => sfxVolume;
    public bool IsMusicPlaying() => musicSource != null && musicSource.isPlaying;

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
        if (Instance == this)
            Instance = null;
    }
}