using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if USE_ADMOB_REWARDED_INTERSITIAL
using GoogleMobileAds.Api;
#endif
public class AdmobRewardedInterstitialManager : GenericSingletonClass<AdmobRewardedInterstitialManager>
{

     bool hasGaveReward = false;
#if USE_ADMOB_REWARDED_INTERSITIAL
    private RewardedInterstitialAd _rewardedInterstitialAd;
    private bool isAdBeingLoaded = false;
    private void Start()
    {
        AdmobInitlizationManager.admobSdkInitializationAddition += LoadAd;
    }

    public void LoadAd()
    {
        if (isAdBeingLoaded) {
            return;
        }
        // Clean up the old ad before loading a new one.
        if (_rewardedInterstitialAd != null)
        {
            DestroyAd();
        }

        Debug.Log("Loading rewarded interstitial ad.");
        isAdBeingLoaded = true;
        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // Send the request to load the ad.
        RewardedInterstitialAd.Load(AdsIds.AdmobRewardedIntersitialUnitId(), adRequest,
            (RewardedInterstitialAd ad, LoadAdError error) =>
            {
                    // If the operation failed with a reason.
                    if (error != null)
                {
                    isAdBeingLoaded = false;
                    Debug.Log("Rewarded interstitial ad failed to load an ad with error : "
                                    + error);
                    return;
                }
                    // If the operation failed for unknown reasons.
                    // This is an unexpexted error, please report this bug if it happens.
                    if (ad == null)
                {
                    isAdBeingLoaded = false;
                    Debug.Log("Unexpected error: Rewarded interstitial load event fired with null ad and null error.");
                    return;
                }

                    // The operation completed successfully.
                    Debug.Log("Rewarded interstitial ad loaded with response : "
                    + ad.GetResponseInfo());
                _rewardedInterstitialAd = ad;

                    // Register to ad events to extend functionality.
                    RegisterEventHandlers(ad);
                    isAdBeingLoaded = false;

            });
    }

    /// <summary>
    /// Shows the ad.
    /// </summary>
    public void ShowAd()
    {
        AdConstants.sawRewarded = false;
        if (!AdConstants.shouldDisplayAds())
        {
            return;
        }
        if (_rewardedInterstitialAd != null && _rewardedInterstitialAd.CanShowAd())
        {
            hasGaveReward = false;
            AdConstants.resumeFromAds = true;
            if (GVAnalysisManager.Instance)
                GVAnalysisManager.Instance.AdAnalysis(AdController.AdType.REWARDED_INTERSTITIAL);
            _rewardedInterstitialAd.Show((Reward reward) =>
            {
               
               // Debug.LogError(1);
                AdConstants.sawRewarded = true;
                hasGaveReward = true;
                AdController.instance.DecideForReward();
                Debug.Log("Admob Rewarded interstitial gave rewarded : ");
            });
        }
        else
        {
            AdController.instance.NoAdAvailable();
            LoadAd();
        }

      
    }

    /// <summary>
    /// Destroys the ad.
    /// </summary>
    public void DestroyAd()
    {
        if (_rewardedInterstitialAd != null)
        {
            Debug.Log("Destroying rewarded interstitial ad.");
            _rewardedInterstitialAd.Destroy();
            _rewardedInterstitialAd = null;
        }

      
    }

    /// <summary>
    /// Logs the ResponseInfo.
    /// </summary>
    public void LogResponseInfo()
    {
        if (_rewardedInterstitialAd != null)
        {
            var responseInfo = _rewardedInterstitialAd.GetResponseInfo();
            UnityEngine.Debug.Log(responseInfo);
        }
    }

    protected void RegisterEventHandlers(RewardedInterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
#if USE_ADMOB_PAID_EVENT
            if (GVAnalysisManager.Instance && ad != null)
                GVAnalysisManager.Instance.PaidAdAnalytics(AdController.AdType.REWARDED_INTERSTITIAL.ToString(), ad.GetResponseInfo(), adValue);
#endif

            Debug.Log(String.Format("Rewarded interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {

            Debug.Log("Rewarded interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            AdConstants.resumeFromAds = false;
            if (!hasGaveReward)
            {
                hasGaveReward = true;
                AdController.instance.DecideForReward();
            }
            Debug.Log("Rewarded interstitial ad full screen content closed.");
            LoadAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            AdConstants.resumeFromAds = false;
            Debug.Log("Rewarded interstitial ad failed to open full screen content" +
                           " with error : " + error);
            LoadAd();
            AdController.instance.NoAdAvailable();
        };
    }
#endif
}

