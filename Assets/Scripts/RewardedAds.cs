using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public abstract class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
   [SerializeField] string _rewardedAdID = "Rewarded_Android";
   public void LoadRewardedAd()
   {
      Advertisement.Load(_rewardedAdID, this);
   }

   public void ShowRewardedAd()
   {
      Advertisement.Show(_rewardedAdID, this);
   }

   public void OnUnityAdsAdLoaded(string placementId)
   {
      Debug.Log("Rewarded loaded");
   }

   public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
   {
      Debug.Log("Rewarded loading failure: ");
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
         if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)) Debug.Log("Full rewards");
         else if (showCompletionState.Equals(UnityAdsShowCompletionState.SKIPPED)) Debug.Log("Some rewards");
         else if(showCompletionState.Equals(UnityAdsShowCompletionState.UNKNOWN)) Debug.Log("Error");
      }
   }

   public void OnUnityAdsShowFailure(string placeId, UnityAdsShowError error, string message)
   {
      Debug.Log("Reward ad failure");
   }

   public void OnUnityAdsShowStart(string placementId)
   {
      Debug.Log("Starting reward ad");
   }
}
