using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        musicSlider.value = AudioManager.Instance.GetMusicVolume();
        sfxSlider.value = AudioManager.Instance.GetSfxVolume();

        musicSlider.onValueChanged.AddListener(v => AudioManager.Instance.SetMusicVolume(v));
        sfxSlider.onValueChanged.AddListener(v => AudioManager.Instance.SetSfxVolume(v));
    }

    public void OnResumePressed()
    {
        PauseManager.Instance.ResumeGame();
    }

    public void OnExitPressed()
    {
        PauseManager.Instance.ExitToMainMenu();
    }
}