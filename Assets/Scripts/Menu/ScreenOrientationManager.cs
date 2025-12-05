using UnityEngine;

public class ScreenOrientationManager : MonoBehaviour
{
    [SerializeField] private ScreenOrientation orientation = ScreenOrientation.Portrait;
    
    private void Awake()
    {
        // Forzar orientación
        Screen.orientation = ScreenOrientation.AutoRotation;
        
        // Según la escena, aplicar rotación específica
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        
        if (sceneName == "MenuScene")
        {
            Screen.orientation = ScreenOrientation.Portrait;
            Debug.Log("📱 Orientación: PORTRAIT (1080x1920)");
        }
        else if (sceneName == "First Level" || sceneName == "GameScene")
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            Debug.Log("📱 Orientación: LANDSCAPE (1920x1080)");
        }
    }
}