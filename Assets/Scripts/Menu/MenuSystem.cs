using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void Jugar(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void Salir()
    {
        Application.Quit();
    }
}
