using System;
using UnityEngine;
using Sirenix.OdinInspector;
public class IdleMrecManager  : GenericSingletonClass<IdleMrecManager>
{

#if USE_IDLE_MREC && USE_MAX
    private bool isIdleMRECBannerAdShowing = false;
    private bool isIdleMRECBannerAdisbeingLoaded;

    [BoxGroup("MREC")] public MaxSdkBase.AdViewPosition idleNativeBannerPos;


    private void OnEnable()
    {
        if (string.IsNullOrEmpty(AdsIds.IdleMRECBannerAdUnitId())) {
            return;
        }
        AdController.maxSdkInitializationAddition += InitializeIdleMRecAds;
    }

    private void InitializeIdleMRecAds()
    {
        if (string.IsNullOrEmpty(AdsIds.IdleMRECBannerAdUnitId()))
        {
            return;
        }
        Debug.Log("-------- Initializing MRec Banner Ad  --------");

        // Attach Callbacks
        MaxSdkCallbacks.MRec.OnAdLoadedEvent += OnIdleMRecAdLoadedEvent;
        MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += OnIdleMRecAdFailedEvent;
        MaxSdkCallbacks.MRec.OnAdClickedEvent += OnIdleMRecAdClickedEvent;
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnIdleMRecAdRevenuePaidEvent;

        // MRECs are automatically sized to 300x250.
        isIdleMRECBannerAdShowing = false;
        MaxSdk.CreateMRec(AdsIds.IdleMRECBannerAdUnitId(), idleNativeBannerPos);

    }



    public void ShowIdleMRec()
    {

    if (!AdConstants.shouldDisplayAds()) {
            return;
        }
        if (string.IsNullOrEmpty(AdsIds.IdleMRECBannerAdUnitId()))
        {
            return;
        }

        isIdleMRECBannerAdShowing = false;
        isIdleMRECBannerAdisbeingLoaded = true;
        //Debug.LogError(1);
        MaxSdk.ShowMRec(AdsIds.IdleMRECBannerAdUnitId());

    }

    private void LoadMRec()
    {
        if (string.IsNullOrEmpty(AdsIds.IdleMRECBannerAdUnitId()))
        {
            return;
        }

        isIdleMRECBannerAdShowing = false;
        isIdleMRECBannerAdisbeingLoaded = true;
        //   Debug.LogError(1);
        MaxSdk.ShowMRec(AdsIds.IdleMRECBannerAdUnitId());

    }

    public void HideIdleMRec()
    {


        isIdleMRECBannerAdShowing = false;
        MaxSdk.HideMRec(AdsIds.IdleMRECBannerAdUnitId());

    }


    private void ToggleMRecVisibility(bool show)
    {

        if (isIdleMRECBannerAdShowing)
        {
            if (show)
            {
                // Debug.LogError(1);
                MaxSdk.ShowMRec(AdsIds.IdleMRECBannerAdUnitId());
            }
            else
            {
                //  Debug.LogError(1);
                MaxSdk.HideMRec(AdsIds.IdleMRECBannerAdUnitId());
            }
        }

    }

    public bool IsIdleMRecBannerAdAvailable()
    {
#if USE_MAX
        return !string.IsNullOrEmpty(AdsIds.IdleMRECBannerAdUnitId()) && AdConstants.shouldDisplayAds() && AdConstants.showNativeBannerAd && !isIdleMRECBannerAdisbeingLoaded &&AdController.instance.isSdkInitialized;
#else
        return false;

#endif
    }

    public bool IsIdleMRecBannerAdReadToLoad()
    {
#if USE_MAX
        return !string.IsNullOrEmpty(AdsIds.IdleMRECBannerAdUnitId()) && AdConstants.shouldDisplayAds() && AdConstants.showNativeBannerAd && AdController.instance.isSdkInitialized;
#else
        return false;

#endif
    }

    private void OnIdleMRecAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // MRec ad is ready to be shown.
        // If you have already called MaxSdk.ShowMRec(MRecAdUnitId) it will automatically be shown on the next MRec refresh.
        if (GVAnalysisManager.Instance)
            GVAnalysisManager.Instance.AdAnalysis(AdController.BannerAdTypes.IDLE_NATIVE);
        Debug.Log("-------- MRec ad loaded --------");
        isIdleMRECBannerAdisbeingLoaded = false;
        isIdleMRECBannerAdShowing = true;
        AdController.instance.mRecRetryAttempt = 0;
        AdController.instance.NativeBannerLoadSuccessful();
    }

    private void OnIdleMRecAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // MRec ad failed to load. MAX will automatically try loading a new ad internally.
        Debug.Log("-------- MRec ad failed to load with error code: " + errorInfo.Code + " --------");
        isIdleMRECBannerAdisbeingLoaded = false;
        isIdleMRECBannerAdShowing = false;
        AdController.instance.mRecRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, AdController.instance.mRecRetryAttempt));
        Invoke("LoadMRec", (float)AdController.instance.mRecRetryAttempt);
      AdController.instance. NativeBannerLoadingFailed();
    }

    private void OnIdleMRecAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("-------- MRec ad clicked --------");
    }

    private void OnIdleMRecAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // MRec ad revenue paid. Use this callback to track user revenue.
        Debug.Log("-------- MRec ad revenue paid --------");

        // Ad revenue
        double revenue = adInfo.Revenue;

        // Miscellaneous data
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD"!
        string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
        string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
        string placement = adInfo.Placement; // The placement this ad's postbacks are tied to

        TrackAdRevenue("IdleNative", adInfo);
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
