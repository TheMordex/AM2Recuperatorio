using UnityEngine;
using UnityEngine.Advertisements;
using System;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
   [SerializeField] string _rewardedAdID = "Rewarded_Android";
   
   private Action _onAdCompletedCallback;

   public void LoadRewardedAd()
   {
      Advertisement.Load(_rewardedAdID, this);
   }

   public void ShowRewardedAd(Action onAdCompleted = null)
   {
      _onAdCompletedCallback = onAdCompleted;
      Advertisement.Show(_rewardedAdID, this);
   }

   public void OnUnityAdsAdLoaded(string placementId)
   {
      Debug.Log("Rewarded loaded");
   }

   public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
   {
      Debug.Log("Rewarded loading failure: " + message);
   }

   public void OnUnityAdsShowClick(string placementId)
   {
      Debug.Log("Rewarded ad clicked");
   }

   public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
   {
      if (placementId == _rewardedAdID)
      {
         Debug.Log("Time for reward");
         
         if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
         {
            Debug.Log("Full rewards - Usuario vio el anuncio completo");
            
            _onAdCompletedCallback?.Invoke();
         }
         else if (showCompletionState.Equals(UnityAdsShowCompletionState.SKIPPED))
         {
            Debug.Log("Usuario salteó el anuncio - No hay recompensa");
         }
         else if(showCompletionState.Equals(UnityAdsShowCompletionState.UNKNOWN))
         {
            Debug.Log("Error desconocido");
         }
         
         _onAdCompletedCallback = null;
         
         LoadRewardedAd();
      }
   }

   public void OnUnityAdsShowFailure(string placeId, UnityAdsShowError error, string message)
   {
      Debug.Log("Reward ad failure: " + message);
      _onAdCompletedCallback = null;
      
      LoadRewardedAd();
   }

   public void OnUnityAdsShowStart(string placementId)
   {
      Debug.Log("Starting reward ad");
   }
}
