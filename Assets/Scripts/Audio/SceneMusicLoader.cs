using UnityEngine;

public class SceneMusicLoader : MonoBehaviour
{
    [Header("Música de esta escena")]
    [SerializeField] private AudioClip sceneMusic;
    
    [Header("Opciones")]
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private bool stopPreviousMusic = false;
    
    private void Start()
    {
        if (!playOnStart)
            return;
        
        if (AudioManager.Instance == null || sceneMusic == null)
            return;
        
        if (stopPreviousMusic)
            AudioManager.Instance.StopMusic();
        
        AudioManager.Instance.PlayMusic(sceneMusic, forceRestart: false);
    }
    
    public void PlaySceneMusic()
    {
        if (AudioManager.Instance != null && sceneMusic != null)
            AudioManager.Instance.PlayMusic(sceneMusic);
    }
    
    public void StopSceneMusic()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.StopMusic();
    }
}