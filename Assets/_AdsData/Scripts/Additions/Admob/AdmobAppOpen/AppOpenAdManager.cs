using System;
using System.Collections;
using GVAdmobOpenAds;
#if !USE_MAX_OPENADS
#if USE_ADMOB_OPEN_AD_7_2_0 || USE_ADMOB_OPEN_AD_8_5
using GoogleMobileAds.Api;
#endif
#endif
using UnityEngine;

public class AppOpenAdManager
{
#if !USE_MAX_OPENADS
#if USE_ADMOB_OPEN_AD_7_2_0

    private static AppOpenAdManager instance;

    private AppOpenAd ad;

    private DateTime loadTime;

    private bool isShowingAd = false;

    public bool showFirstOpen = false;

    public static bool ConfigOpenApp = true;
    public static bool ConfigResumeApp = true;

    public static bool hasDisplayedFirstAd = false;

    public static bool ResumeFromAds = false;

    public static AppOpenAdManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AppOpenAdManager();
            }

            return instance;
        }
    }

    private bool IsAdAvailable => ad != null && (System.DateTime.UtcNow - loadTime).TotalHours < 4;

    private int tierIndex = 1;

    public void LoadAd()
    {

        // if (IAP_AD_REMOVED)
        //     return;

        LoadAOA();
    }

    public void LoadAOA()
    {

        string id = AdsIds.AppOpenAdTier1();
        if (tierIndex == 2)
            id = AdsIds.AppOpenAdTier2();
        else if (tierIndex == 3)
            id = AdsIds.AppOpenAdTier3();

        // Debug.LogError($"Start request Open App Ads: Tier{tierIndex}- ID:{id}");

        AdRequest request = new AdRequest.Builder().Build();

        AppOpenAd.LoadAd(id, ScreenOrientation.LandscapeLeft, request, ((appOpenAd, error) =>
        {
            if (error != null)
            {
                // Handle the error.
                Debug.LogFormat(
                    $"Failed to load AOA tier {tierIndex} - id: {id}. Reason: {error.LoadAdError.GetMessage()}");
                tierIndex++;
                if (tierIndex <= 3)
                    LoadAOA();
                else
                    tierIndex = 1;
                return;
            }

            //Debug.LogError("LoadAppOpenAd App open ad is loaded");
            ad = appOpenAd;
            tierIndex = 1;
            loadTime = DateTime.UtcNow;
            if (!showFirstOpen && ConfigOpenApp)
            {
                //  Debug.LogError("LoadAppOpenAd show");
                ShowAdIfAvailable();
                showFirstOpen = true;
            }
        }));
    }

    public void ShowAdIfAvailable()
    {

        //Debug.LogError("call 1");
        if (!AdConstants.shouldDisplayAds())
        {
            // Debug.Log("shouldDisplayAds");
            return;
        }

        if (!IsAdAvailable || isShowingAd)
        {
            if (!IsAdAvailable)
            {
                LoadAd();
            }
            //  Debug.Log("IsAdAvailable"+(!IsAdAvailable).ToSafeString()+(isShowingAd).ToSafeString());
            return;
        }

        ad.OnAdDidDismissFullScreenContent += HandleAdDidDismissFullScreenContent;
        ad.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresentFullScreenContent;
        ad.OnAdDidPresentFullScreenContent += HandleAdDidPresentFullScreenContent;
        ad.OnAdDidRecordImpression += HandleAdDidRecordImpression;
        ad.OnPaidEvent += HandlePaidEvent;
        //Debug.LogError("=== Show AOA ===");
        if (GVAnalysisManager.Instance)
        {
            GVAnalysisManager.Instance?.AdAnalysis(AdController.AdType.OPEN_AD);
            ad.Show();
        }

    }

    private void HandleAdDidDismissFullScreenContent(object sender, EventArgs args)
    {

        // Debug.LogError("Closed app open ad");
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        isShowingAd = false;
        LoadAd();
    }

    private void HandleAdFailedToPresentFullScreenContent(object sender, AdErrorEventArgs args)
    {

        Debug.LogFormat("Failed to present the ad (reason: {0})", args.AdError.GetMessage());
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        LoadAd();
    }

    private void HandleAdDidPresentFullScreenContent(object sender, EventArgs args)
    {

        //Debug.LogError("Displayed app open ad");
        isShowingAd = true;
    }

    private void HandleAdDidRecordImpression(object sender, EventArgs args)
    {

        //Debug.LogError("Recorded ad impression");
    }

    private void HandlePaidEvent(object sender, AdValueEventArgs args)
    {

#if USE_ADMOB_PAID_EVENT
#if USE_ADMOB_OPEN_AD_7_2_0
        if (GVAnalysisManager.Instance && ad != null)
            GVAnalysisManager.Instance.PaidAdAnalytics(AdController.AdType.OPEN_AD.ToString(), ad.GetResponseInfo(), args.AdValue);
#endif
#endif

        Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
            args.AdValue.CurrencyCode, args.AdValue.Value);
    }
#endif


#if USE_ADMOB_OPEN_AD_8_5

    private static AppOpenAdManager instance;
    private AppOpenAd ad;

    private DateTime loadTime;

    private bool isShowingAd = false;

    public bool showFirstOpen = false;

    public static bool ConfigOpenApp = true;
    public static bool ConfigResumeApp = true;

    public static bool hasDisplayedFirstAd = false;

    public static bool ResumeFromAds = false;

    public static AppOpenAdManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AppOpenAdManager();
            }

            return instance;
        }
    }

    private bool IsAdAvailable
    {
        get
        {
            return

            ad != null && (System.DateTime.UtcNow - loadTime).TotalHours < 4;

        }
    }

    private int tierIndex = 1;

    public void LoadAd()
    {

        // if (IAP_AD_REMOVED)
        //     return;

        LoadAOA();
    }

    public void LoadAOA()
    {

        string id = AdsIds.AppOpenAdTier1();
        if (tierIndex == 2)
            id = AdsIds.AppOpenAdTier2();
        else if (tierIndex == 3)
            id = AdsIds.AppOpenAdTier3();

        // Debug.LogError($"Start request Open App Ads: Tier{tierIndex}- ID:{id}");
        AdRequest request = new AdRequest();
        
        AppOpenAd.Load(id, request, ((appOpenAd, error) =>
        {
            if (error != null)
            {
                // Handle the error.
                Debug.LogFormat(
                    $"Failed to load AOA tier {tierIndex} - id: {id}. Reason: {error.GetMessage()}");
                tierIndex++;
                if (tierIndex <= 3)
                    LoadAOA();
                else
                    tierIndex = 1;
                return;
            }

            //Debug.LogError(AppOpenAdLauncher.Instance != null);
            //Debug.LogError(AppOpenAdLauncher.Instance.showShowOpenAdOnOpen;
            ad = appOpenAd;
            tierIndex = 1;
            loadTime = DateTime.UtcNow;
            if (AppOpenAdLauncher.Instance!=null&&AppOpenAdLauncher.Instance.showShowOpenAdOnOpen && ConfigOpenApp)
            {
                //  Debug.LogError("LoadAppOpenAd show");
                ShowAdIfAvailable();
                AppOpenAdLauncher.Instance.showShowOpenAdOnOpen = false;
            }
        }));
    }
    public void ShowAdIfAvailable()
    {

      //  Debug.LogError("call 1");
        if (!AdConstants.shouldDisplayAds())
        {
            // Debug.Log("shouldDisplayAds");
            return;
        }
        //Debug.Log($"BILLI : Trying to ShowOpenAd AppOpenAdLauncher");
        Debug.Log("BILLI : IsAdAvailable" + IsAdAvailable + "isShowingAd" + isShowingAd);
        if (!IsAdAvailable || isShowingAd)
        {
            if (!IsAdAvailable)
            {
                LoadAd();
            }
            return;
        }
        ad.OnAdFullScreenContentClosed += HandleAdDidDismissFullScreenContent;
        ad.OnAdFullScreenContentFailed += HandleAdFailedToPresentFullScreenContent;
        ad.OnAdFullScreenContentOpened += HandleAdDidPresentFullScreenContent;
        ad.OnAdImpressionRecorded += HandleAdDidRecordImpression;
        ad.OnAdPaid += HandlePaidEvent;
        Debug.Log("BILLI : === Show AOA ===");
        if (GVAnalysisManager.Instance)
        {
            GVAnalysisManager.Instance?.AdAnalysis(AdController.AdType.OPEN_AD);
            ad.Show();
        }


    }

    private void HandleAdDidDismissFullScreenContent()
    {

        // Debug.LogError("Closed app open ad");
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        isShowingAd = false;
        LoadAd();
    }

    private void HandleAdFailedToPresentFullScreenContent(AdError adError)
    {

        Debug.LogFormat("Failed to present the ad (reason: {0})", adError.GetMessage());
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        LoadAd();
    }

    private void HandleAdDidPresentFullScreenContent()
    {

        //Debug.LogError("Displayed app open ad");
        isShowingAd = true;
    }

    private void HandleAdDidRecordImpression()
    {

        //Debug.LogError("Recorded ad impression");
    }

    private void HandlePaidEvent(AdValue args)
    {

#if USE_ADMOB_PAID_EVENT
#if USE_ADMOB_OPEN_AD_8_5
        if (GVAnalysisManager.Instance && ad != null)
            GVAnalysisManager.Instance.PaidAdAnalytics(AdController.AdType.OPEN_AD.ToString(), ad.GetResponseInfo(), args);
#endif
#endif

        Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
                args.CurrencyCode, args.Value);
    }
#endif

    
#endif

}