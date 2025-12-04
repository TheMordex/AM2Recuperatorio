using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Canvas canvas;

    [Header("Buttons")]
    [SerializeField] private Button pauseButton;     
    [SerializeField] private Button resumeButton;    
    [SerializeField] private Button mainMenuButton;  

    private AudioSource[] allAudioSources;
    private bool isPaused = false;

    private void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
        
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
        }
        
        if (pauseButton != null)
            pauseButton.onClick.AddListener(OpenPauseMenu);
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ClosePauseMenu);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(BackToMainMenu);
    }

    public void OpenPauseMenu()
    {
        if (isPaused) return;

        isPaused = true;

        
        allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (var a in allAudioSources)
        {
            if (a.isPlaying)
                a.Pause();
        }
        
        if (pausePanel != null)
            pausePanel.SetActive(true);
        
        Time.timeScale = 0f;
    }

    public void ClosePauseMenu()
    {
        if (!isPaused) return;

        isPaused = false;
        
        Time.timeScale = 1f;
        
        if (allAudioSources != null)
        {
            foreach (var a in allAudioSources)
                a.UnPause();
        }
        
        if (pausePanel != null)
            pausePanel.SetActive(false);

        Debug.Log("Menú de pausa cerrado");
    }

    private void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ClosePauseMenu();
            else
                OpenPauseMenu();
        }
    }
}