using UnityEngine;

public class ScreenOrientationManager : MonoBehaviour
{
    [SerializeField] private ScreenOrientation orientation = ScreenOrientation.Portrait;
    
    private void Awake()
    {
        Screen.orientation = orientation;
    }
}