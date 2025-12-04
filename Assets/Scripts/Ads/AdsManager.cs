using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;

public class AdsManager : MonoBehaviour
{
  public static AdsManager instance;

  [SerializeField] InitializeAds _initializeAds;
  [SerializeField] RewardedAds _rewardedAds;
  [SerializeField] BannerAds _bannerAds;
  [SerializeField] InterstitialAds _interstitialAds;

  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else 
    {
      Destroy(gameObject);
      return;
    }

    _rewardedAds.LoadRewardedAd();

    StartCoroutine(BannerAds());

    _interstitialAds.LoadInterstitialAd();
    StartCoroutine(InterstitialAds());
  }

  public void ShowRewardedAd(Action onAdCompleted = null)
  {
    _rewardedAds.ShowRewardedAd(onAdCompleted);
  }

  IEnumerator BannerAds()
  {
    while (true)
    {
      _bannerAds.LoadBannerAd();
      yield return new WaitForSeconds(5f);
      _bannerAds.ShowBannerAd();
      yield return new WaitForSeconds(30f);
      _bannerAds.HideBannerAd();
      yield return new WaitForSeconds(5f);
    }
  }

  IEnumerator InterstitialAds()
  {
    yield return new WaitForSeconds(10f);
    _interstitialAds.ShowInterstitialAd();
  }
}
