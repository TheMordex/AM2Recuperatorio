using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu Panel")]
    [SerializeField] private GameObject mainMenuPanel;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    [Header("Shop Panel")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Button backFromShopButton;

    [Header("Options Panel")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Button backFromOptionsButton;

    [Header("Audio")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip panelOpenSFX;
    [SerializeField] private AudioClip panelCloseSFX;

    private void Start()
    {
        // Resetear GameState al entrar al menú principal
        GameState.IsPaused = false;
        GameState.IsDead = false;
        GameState.IsVictorious = false;
        
        // Reproducir música del menú
        if (AudioManager.Instance != null && menuMusic != null)
        {
            AudioManager.Instance.PlayMusic(menuMusic);
            Debug.Log("🎵 Música del menú principal iniciada");
        }

        // Activamos SOLO el menú principal al inicio
        mainMenuPanel.SetActive(true);
        shopPanel.SetActive(false);
        optionsPanel.SetActive(false);

        // Listeners menú principal
        playButton.onClick.AddListener(() => { PlayButtonSound(); PlayGame(); });
        shopButton.onClick.AddListener(() => { PlayButtonSound(); OpenShop(); });
        optionsButton.onClick.AddListener(() => { PlayButtonSound(); OpenOptions(); });
        quitButton.onClick.AddListener(() => { PlayButtonSound(); QuitGame(); });

        // Botones de regreso
        backFromShopButton.onClick.AddListener(() => { PlayButtonSound(); CloseShop(); });
        backFromOptionsButton.onClick.AddListener(() => { PlayButtonSound(); CloseOptions(); });

        // Asegurar escalas iniciales
        mainMenuPanel.transform.localScale = Vector3.one;
        shopPanel.transform.localScale = Vector3.zero;
        optionsPanel.transform.localScale = Vector3.zero;
    }
    

    private void PlayButtonSound()
    {
        if (AudioManager.Instance != null && buttonClickSFX != null)
        {
            AudioManager.Instance.PlaySFX(buttonClickSFX);
        }
    }

    private void PlayPanelOpenSound()
    {
        if (AudioManager.Instance != null && panelOpenSFX != null)
        {
            AudioManager.Instance.PlaySFX(panelOpenSFX);
        }
    }

    private void PlayPanelCloseSound()
    {
        if (AudioManager.Instance != null && panelCloseSFX != null)
        {
            AudioManager.Instance.PlaySFX(panelCloseSFX);
        }
    }

    private void AnimateOpen(GameObject panel)
    {
        RectTransform rt = panel.GetComponent<RectTransform>();

        panel.SetActive(true);
        rt.localScale = Vector3.zero;
        
        PlayPanelOpenSound();

        LeanTween.scale(rt, Vector3.one, 0.25f)
            .setEaseOutBack();    
    }

    private void AnimateClose(GameObject panel)
    {
        RectTransform rt = panel.GetComponent<RectTransform>();
        
        PlayPanelCloseSound();

        LeanTween.scale(rt, Vector3.zero, 0.20f)
            .setEaseInBack()       // Contracción suave hacia adentro
            .setOnComplete(() =>
            {
                panel.SetActive(false);
            });
    }

    private void CloseAllPanelsExcept(GameObject panelToKeepOpen)
    {
        if (mainMenuPanel != panelToKeepOpen && mainMenuPanel.activeSelf)
            AnimateClose(mainMenuPanel);

        if (shopPanel != panelToKeepOpen && shopPanel.activeSelf)
            AnimateClose(shopPanel);

        if (optionsPanel != panelToKeepOpen && optionsPanel.activeSelf)
            AnimateClose(optionsPanel);
    }
    private void PlayGame()
    {
        // Verificar si tiene stamina
        if (StaminaManager.Instance != null && !StaminaManager.Instance.CanPlayLevel())
        {
            return;
        }
    
        // Gastar stamina
        if (StaminaManager.Instance != null)
            StaminaManager.Instance.UseLevelStamina();
    
        Debug.Log("🎮 Iniciando juego...");
        SceneManager.LoadScene("First Level");
    }

    private void OpenShop()
    {
        CloseAllPanelsExcept(shopPanel);
        AnimateOpen(shopPanel);
    }

    private void CloseShop()
    {
        AnimateClose(shopPanel);
        AnimateOpen(mainMenuPanel);
    }

    private void OpenOptions()
    {
        CloseAllPanelsExcept(optionsPanel);
        AnimateOpen(optionsPanel);
    }

    private void CloseOptions()
    {
        AnimateClose(optionsPanel);
        AnimateOpen(mainMenuPanel);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}