using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
   [SerializeField] string _interstitialAdID = "Interstitial_Android";

   public void LoadInterstitialAd()
   {
      Advertisement.Load(_interstitialAdID, this);
   }

   public void ShowInterstitialAd()
   {
      Advertisement.Show(_interstitialAdID, this);
      LoadInterstitialAd();
   }

   public void OnUnityAdsAdLoaded(string placementId)
   {
      Debug.Log("Interstitial loaded");
   }

   public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
   {
      Debug.Log("Interstitial loading failure");
   }
   
   public void OnUnityAdsShowClick(string placementId)
   {
      Debug.Log("Interstitial ad clicked");
   }

   public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
   {
      Debug.Log("Interstitial ad complete");
   }
   
   public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
      {
         Debug.Log("Interstitial ad failure");
      }

   public void OnUnityAdsShowStart(string placementId)
   {
      Debug.Log("Starting Interstitial ad");
   }
}
