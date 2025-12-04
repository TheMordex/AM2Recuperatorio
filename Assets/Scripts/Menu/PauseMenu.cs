using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pausePanel;  // Solo el PausePanel
    [SerializeField] private Canvas canvas;

    [Header("Buttons")]
    [SerializeField] private Button pauseButton;     // Button Pause (no se tocará)
    [SerializeField] private Button resumeButton;    // ButtonResume
    [SerializeField] private Button mainMenuButton;  // Botón para volver al menú

    private AudioSource[] allAudioSources;
    private bool isPaused = false;

    private void Start()
    {
        // ✅ Asegurar que el PausePanel esté desactivado
        if (pausePanel != null)
            pausePanel.SetActive(false);

        // ✅ Configurar Canvas
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
        }

        // ✅ Configurar botones
        if (pauseButton != null)
            pauseButton.onClick.AddListener(OpenPauseMenu);
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ClosePauseMenu);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(BackToMainMenu);
    }

    public void OpenPauseMenu()
    {
        // ✅ Si ya está pausado, no hacer nada (evita clics dobles)
        if (isPaused) return;

        isPaused = true;

        // ✅ 1. Pausar audios PRIMERO
        allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (var a in allAudioSources)
        {
            if (a.isPlaying)
                a.Pause();
        }

        // ✅ 2. Activar SOLO el PausePanel (NO tocar Button Pause)
        if (pausePanel != null)
            pausePanel.SetActive(true);

        // ✅ 3. Congelar tiempo AL FINAL
        Time.timeScale = 0f;

        Debug.Log("Menú de pausa abierto");
    }

    public void ClosePauseMenu()
    {
        // ✅ Si ya está cerrado, no hacer nada
        if (!isPaused) return;

        isPaused = false;

        // ✅ 1. Restaurar tiempo PRIMERO
        Time.timeScale = 1f;

        // ✅ 2. Reactivar audios
        if (allAudioSources != null)
        {
            foreach (var a in allAudioSources)
                a.UnPause();
        }

        // ✅ 3. Desactivar SOLO el PausePanel
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