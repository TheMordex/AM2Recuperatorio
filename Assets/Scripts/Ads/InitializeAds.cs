using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeAds : MonoBehaviour, IUnityAdsInitializationListener
{
  [SerializeField] string _androidID = "5993645";
  [SerializeField] bool _testingMode;
  void Awake()
  {
    if (!Advertisement.isInitialized && Advertisement.isSupported)
    {
      Advertisement.Initialize(_androidID, _testingMode, this);
    }
  }
  public void OnInitializationComplete()
  {
    Debug.Log("OnInitialization success");
  }
  public void OnInitializationFailed(UnityAdsInitializationError error, string message)
  {
    Debug.Log("OnInitialization failure");
  }
}
