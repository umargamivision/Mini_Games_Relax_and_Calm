using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowIntersititialManager : GenericSingletonClass<LowIntersititialManager>
{
#if USE_MAX_LOW_INTERSITITAL
    private bool isInterstitialAdisbeingLoaded;


    private void OnEnable()
    {
        AdController.maxSdkInitializationAddition += InitializeInterstitialAds;
    }


    private void InitializeInterstitialAds()
    {
       if (IsInterstitialReadyToLoad())
        {
            Debug.Log("-------- Initializing interstitial Ad --------");


            // Attach callbacks
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnLowInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnLowInterstitialFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InLowterstitialFailedToDisplayEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnLowInterstitialDismissedEvent;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnLowInterstitialRevenuePaidEvent;

            // Load the first interstitial
            LoadInterstitial();
        }
    }
    private void unregisterInterstitialEvents()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnLowInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnLowInterstitialFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent -= InLowterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnLowInterstitialDismissedEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent -= OnLowInterstitialRevenuePaidEvent;

    }


    void LoadInterstitial()
    {
#if USE_MAX

        if (IsInterstitialReadyToLoad())
        {
            isInterstitialAdisbeingLoaded = true;
            MaxSdk.LoadInterstitial(AdsIds.LowMaxIntersititialAdUnitId());
        }
#endif
    }

    public void ShowInterstitial()
    {
        if (!AdConstants.shouldDisplayAds()) {
            return;
        }

#if USE_MAX

        if (IsInterstitialAdAvailable())
        {
            //interstitialStatusText.text = "Showing";
            if (GVAnalysisManager.Instance)
                GVAnalysisManager.Instance.AdAnalysis(AdController.AdType.INTERSTITIAL_LIMITED);
            AdController.instance.UpdateGame_Sounds_Banners(false);
            AdConstants.resumeFromAds = true;
            MaxSdk.ShowInterstitial(AdsIds.LowMaxIntersititialAdUnitId());
        }
        else
        {
            Debug.Log("-------- Interstitial Ad is not ready yet --------");
            LoadInterstitial();
            AdController.instance.ShowAd(AdController.AdType.INTERSTITIAL);
            //interstitialStatusText.text = "Ad not ready";
        }
#endif
    }

    public bool IsInterstitialAdAvailable()
    {
//        Debug.LogError(AdConstants.adDelay);
#if USE_MAX
        return !string.IsNullOrEmpty(AdsIds.LowMaxIntersititialAdUnitId()) && MaxSdk.IsInterstitialReady(AdsIds.LowMaxIntersititialAdUnitId()) && AdConstants.shouldDisplayAds() && AdConstants.showInterstitialAd && AdController.instance.isSdkInitialized&& DateTime.Now >=AdController.instance.currentTime_Interstitial.AddSeconds(AdConstants.adDelay);
#else
        return false;

#endif
    }

    public bool IsInterstitialReadyToLoad()
    {
#if USE_MAX
        return !string.IsNullOrEmpty(AdsIds.LowMaxIntersititialAdUnitId()) && AdConstants.shouldDisplayAds() && AdConstants.showInterstitialAd && !isInterstitialAdisbeingLoaded && AdController.instance.isSdkInitialized;
#else
        return false;

#endif
    }



    private void OnLowInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
        //interstitialStatusText.text = "Loaded";
        Debug.Log("--------  Low Interstitial loaded --------");
        isInterstitialAdisbeingLoaded = false;
        // Reset retry attempt
        AdController.instance.interstitialRetryAttempt = 0;
    }

    private void OnLowInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
        AdController.instance.interstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, AdController.instance.interstitialRetryAttempt));

        //interstitialStatusText.text = "Load failed: " + errorInfo.Code + "\nRetrying in " + retryDelay + "s...";
        Debug.Log("-------- Interstitial failed to load with error code: " + errorInfo.Code + " --------");
        isInterstitialAdisbeingLoaded = false;
        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void InLowterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        AdConstants.resumeFromAds = false;
        // Interstitial ad failed to display. We recommend loading the next ad
        Debug.Log("-------- Interstitial failed to display with error code: " + errorInfo.Code + " --------");
        AdController.instance.UpdateGame_Sounds_Banners(true);
        LoadInterstitial();
        AdController.instance.ShowAd(AdController.AdType.INTERSTITIAL);
    }

    private void OnLowInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AdConstants.resumeFromAds = false;
        AdConstants.IsAdWasShowing = true;
        // Interstitial ad is hidden. Pre-load the next ad
        Debug.Log("-------- Low Interstitial dismissed --------");
        AdController.instance.ResetInterstitialTime(); // resetting time
        AdController.instance.UpdateGame_Sounds_Banners(true);
        LoadInterstitial();
  
    }

    private void OnLowInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad revenue paid. Use this callback to track user revenue.
        Debug.Log("-------- Interstitial revenue paid --------");
        AdConstants.IsAdWasShowing = true;
        // Ad revenue
        double revenue = adInfo.Revenue;

        // Miscellaneous data
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
        string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
        string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
        string placement = adInfo.Placement; // The placement this ad's postbacks are tied to

        TrackAdRevenue("Interstitial", adInfo);
    }

    private void TrackAdRevenue(string adString, MaxSdkBase.AdInfo adInfo)
    {
        //Debug.LogFormat("ad_platform: Applovin \n AppLovin ad_source: {0}\nad_unit_name: {1}\nad_format: {2}\nvalue: {3}\ncurrency: USD"
        //             , adInfo.NetworkName, adInfo.AdUnitIdentifier, adInfo.AdFormat, adInfo.Revenue);
        if (GVAnalysisManager.Instance)
            GVAnalysisManager.Instance.PaidAdAnalytics(adString, adInfo);

    }


#endif

}
