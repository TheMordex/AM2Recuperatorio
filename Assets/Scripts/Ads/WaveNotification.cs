using UnityEngine;
using TMPro;

public class WaveNotification : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private float displayDuration = 2f;
    [SerializeField] private float fadeOutDuration = 1f;
    
    private float displayTimer = 0f;
    private float fadeTimer = 0f;
    private bool isFading = false;
    private Color originalColor;
    private int lastWaveShown = 0;
    
    private void Start()
    {
        if (waveText != null)
        {
            originalColor = waveText.color;
            waveText.alpha = 0f;
        }
    }
    
    private void Update()
    {
        if (GameManager.Instance == null) return;
        
        int currentWave = GameManager.Instance.GetCurrentWave();
        
        // Mostrar notificación de nueva oleada
        if (currentWave > lastWaveShown && currentWave <= GameManager.Instance.GetTotalWaves())
        {
            ShowWaveNotification(currentWave);
            lastWaveShown = currentWave;
        }
        
        // Manejar duración de visualización
        if (displayTimer > 0f)
        {
            displayTimer -= Time.deltaTime;
            
            if (displayTimer <= 0f && !isFading)
            {
                isFading = true;
                fadeTimer = fadeOutDuration;
            }
        }
        
        // Fade out
        if (isFading && fadeTimer > 0f)
        {
            fadeTimer -= Time.deltaTime;
            float alpha = fadeTimer / fadeOutDuration;
            waveText.alpha = alpha;
            
            if (fadeTimer <= 0f)
            {
                isFading = false;
                waveText.alpha = 0f;
            }
        }
    }
    
    private void ShowWaveNotification(int waveNumber)
    {
        int totalWaves = GameManager.Instance.GetTotalWaves();
        
        if (waveNumber == totalWaves)
            waveText.text = "Oleada Final";
        else
            waveText.text = $"Oleada {waveNumber}";
        
        waveText.alpha = 1f;
        displayTimer = displayDuration;
        isFading = false;
        fadeTimer = 0f;
    }
}