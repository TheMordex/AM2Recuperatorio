using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public void OnPausePressed()
    {
        PauseManager.Instance.TogglePause();
    }
}