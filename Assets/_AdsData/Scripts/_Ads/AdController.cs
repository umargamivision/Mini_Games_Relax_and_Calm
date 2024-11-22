using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
#if USE_MAX && UNITY_EDITOR
using AppLovinMax.Scripts.IntegrationManager.Editor;
#endif

#if USE_FIREBASE
using Firebase;
#endif
#if USE_FIREBASE && USE_REMOTE_CONFIG
using Firebase.RemoteConfig;
#endif
using Sirenix.OdinInspector;
using UnityEngine;

public class AdController : MonoBehaviour
{
    #region Variables
    public enum BannerAdTypes
    {
        BANNER, ADAPTIVE, NATIVE,IDLE_NATIVE
    }

    public enum AdType
    {
        STATIC, INTERSTITIAL, REWARDED, REWARDED_INTERSTITIAL, NO_AD, OPEN_AD, OPEN_AD_MAX, INTERSTITIAL_LIMITED
    }

#if USE_MAX
    [BoxGroup("BANNER")]public bool isSmartBanner;
    [BoxGroup("BANNER")]public bool isAdaptiveBanner=false;
    [BoxGroup("BANNER")] public bool showBannerOnIntialization = false;
    [BoxGroup("BANNER")] public bool shouldHaveBannerBg = false;
    [BoxGroup("BANNER")] public MaxSdkBase.BannerPosition bannerPosition;
    [BoxGroup("MREC")] public MaxSdkBase.AdViewPosition NativeBannerPos;
    [BoxGroup("REWARD")] public bool shouldResetIntersititialTimeOnReward = false;
#endif


    
    [BoxGroup("INFO")] public bool dontDestroyOnLoad = true;
    
#if USE_MAX && USE_MAX_OPENADS
    [BoxGroup("OPEN AD")] private bool showMaxOpenAdOnSplashSecondTime = false;
#endif
    [HideInInspector]public bool isSdkInitialized = false;
    private int bannerRetryAttempt;
    [HideInInspector]public int mRecRetryAttempt;
    [HideInInspector] public int interstitialRetryAttempt;
    private int rewardedRetryAttempt;
    private int staticAdCount = 0;

    private DateTime currentTime_banner; // current banner ad time for show ads delay
    public DateTime currentTime_Interstitial; // current interstitial ad time for show ads delay
    private DateTime currentTime_Rewarded; // current rewarded ad time for show ads delay
    private DateTime currentTime_AppOpenAd; // current static ad time for show ads delay

    private bool isBannerAdShowing = false;
    private bool isMRECBannerAdShowing = false;

    private bool isBannerAdisbeingLoaded;
    private bool isMRECBannerAdisbeingLoaded;
    private bool isInterstitialAdisbeingLoaded;
    private bool isRewardedAdisbeingLoaded;
    private bool isAppOpenAdisbeingLoaded;

    public static AdController instance;
    // Property to access the instance
    public static AdController Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("AdController instance is null. Make sure the instance is initialized before accessing it.");
            }

            return instance;
        }
    }

    //========================= BANNER AD CALLBACKS =========================//
    // On Banner failed to Load
    public delegate void bannerLoadingFailed();
    public static event bannerLoadingFailed bannerLoadingFailedMethod;
    // On Banner showing
    public delegate void bannerLoadingSuccessful();
    public static event bannerLoadingSuccessful bannerLoadingSuccessfulMethod;

    //========================= NATIVE BANNER AD CALLBACKS =========================//
    // On Native Banner failed to Load
    public delegate void nativeBannerLoadingFailed();
    public static event nativeBannerLoadingFailed nativeBannerLoadingFailedMethod;
    // On Native Banner showing
    public delegate void nativeBannerShow();
    public static event nativeBannerShow nativeBannerShowMethod;

    //========================= Initilization Addition CALLBACKS =========================//

    public delegate void maxSDKInitializationAddition();
    public static event maxSDKInitializationAddition maxSdkInitializationAddition;

    //========================= REWARDED CALLBACKS =========================//

    public delegate void rewardedVideoWatched();
    public static event rewardedVideoWatched gaveRewardMethod;

    //========================= REVIEW DIALOG CALLBACKS =========================//
    // On Gave rewarded Ad
    public delegate void reviewDialog();
    public static event reviewDialog reviewDialogMethod;
    // On No rewarded Ad
    public delegate void NoRewardedAd();
    public static event NoRewardedAd noRewardedVideoMethod;
    // On rewarded Cancle
    public delegate void cancelRewardedAd();
    public static event cancelRewardedAd cancelRewardedAdMethod;
    // On rewarded Ad Load
    public delegate void rewardedAdLoad();
    public static event rewardedAdLoad rewardedAdLoadMethod;
    // On rewarded Ad Load Failed
    public delegate void rewardedAdLoadFailed();
    public static event rewardedAdLoadFailed rewardedAdLoadFailedMethod;
    // On rewarded Ad Show 
    public delegate void rewardedAdShowing();
    public static event rewardedAdShowing rewardedAdShowingMethod;
    // On rewarded Ad Show Failed
    public delegate void rewardedAdShowingFailed();
    public static event rewardedAdShowingFailed rewardedAdShowingFailedMethod;

    //========================= FIREBASE REMOTE CONFIG CALLBACKS =========================//
#if USE_FIREBASE && USE_REMOTE_CONFIG
    public delegate void FirebaseRemoteConfig(IDictionary<string, ConfigValue> keyValues);
    public static event FirebaseRemoteConfig onFirebaseRemoteConfigSuccess;
#endif

    //========================= GAME SOUND CALLBACKS =========================//
    public delegate void SoundDelegate(bool soundStatus);
    public static event SoundDelegate updateGameSoundMethod;

    //========================= IN APP REVIEW MANAGER =========================//
    [HideInInspector]
    public InappReviewManager iapInstance;


    #endregion

    private void OnValidate()
    {
       
        LoadAdapters();
    }
    [ReadOnly]
    public List<string> adaptersUsed = new List<string>();
    public void LoadAdapters()
    {
#if UNITY_EDITOR && USE_MAX
        AppLovinEditorCoroutine.StartCoroutine(AppLovinIntegrationManager.Instance.LoadPluginData(data =>
        {
            if (data == null) return;
            adaptersUsed.Clear();
            for (int i = 0; i < data.MediatedNetworks.Length; i++)
            {
                var dependencyFilePath = MaxSdkUtils.GetAssetPathForExportPath(data.MediatedNetworks[i].DependenciesFilePath);
                if (File.Exists(dependencyFilePath))
                {
                    adaptersUsed.Add(data.MediatedNetworks[i].DisplayName);

                }
            }
        }));
#endif
    }

    public void Awake()
    {
 
        // Ensure that only one instance of the singleton class exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            if (dontDestroyOnLoad)
                DontDestroyOnLoad(this);
        }

#if USE_MAX 
        MaxSdk.SetHasUserConsent(true);
        MaxSdk.SetDoNotSell(false);
#endif
#if UNITY_IOS && !UNITY_EDITOR
     if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Debug.Log("unity-script: AdSettings.SetDataProcessingOptions");
            AudienceNetwork.AdSettings.SetDataProcessingOptions(new string[] { });
            AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(true);   // Give True or false in value here for authorization
        }

#endif
        AdConstants.CountGameSession();
    }
    public void StartAdController()
    {

        int appOpenSplashCall = PlayerPrefs.GetInt("AppOpenSplash", 0);
        PlayerPrefs.SetInt("AppOpenSplash", 1);
        if (appOpenSplashCall == 0)
            showMaxOpenAdOnSplashSecondTime = false;
        else
            showMaxOpenAdOnSplashSecondTime = true;



        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        iapInstance = GetComponent<InappReviewManager>(); // Gettings in app review manager
        if (iapInstance == null)
            iapInstance = gameObject.AddComponent<InappReviewManager>();
        if (GetComponent<DeviceSettingsManager>())
        {
            GetComponent<DeviceSettingsManager>().ApplyDeviceSettings(); // Applying remote settings
        }
#if USE_MAX
        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
            // AppLovin SDK is initialized, configure and start loading ads.
            isSdkInitialized = true;
            Debug.Log("MAX SDK Initialized");
            InitializeBannerAds();
             InitializeMRecAds();

#if USE_MAX && USE_MAX_OPENADS
           
            if (showMaxOpenAdOnSplashSecondTime)
            {
                InitializeAppOpenAds();
            }
#endif

            InitializeInterstitialAds();
            InitializeRewardedAds();

#if USE_MAX && USE_MAX_OPENADS
            if (!showMaxOpenAdOnSplashSecondTime)
            {
                InitializeAppOpenAds();
            }
#endif
            if (maxSdkInitializationAddition != null) {
                maxSdkInitializationAddition.Invoke();
            }
            // MaxSdk.ShowMediationDebugger();
        };

        MaxSdk.SetSdkKey(AdsIds.SDKKey());
        MaxSdk.InitializeSdk();
#endif


    }

#region Calling Methods



#region New Plugin Methods

    public void ShowBannerAd(BannerAdTypes type, bool IsFirstCall = false)
    {
        if (AdConstants.shouldDisplayAds() == false)
            return;

        switch (type)
        {
            case BannerAdTypes.BANNER:
                ShowBanner();
                break;

            case BannerAdTypes.ADAPTIVE:
                ShowBanner();
                break;

            case BannerAdTypes.NATIVE:

                ShowMRec();

                break;
        }
    }

    public void DestroyBannerAd(BannerAdTypes type)
    {
        switch (type)
        {
            case BannerAdTypes.BANNER:
                HideBanner();
                break;

            case BannerAdTypes.ADAPTIVE:
                HideBanner();
                break;

            case BannerAdTypes.NATIVE:
                HideMRec();
                break;
        }
    }

    public void HideBannerAd(BannerAdTypes type)
    {

        switch (type)
        {
            case BannerAdTypes.BANNER:
                HideBanner();
                break;

            case BannerAdTypes.ADAPTIVE:
                HideBanner();
                break;

            case BannerAdTypes.NATIVE:
                HideMRec();
                break;
        }
    }

    public void LoadAd(AdType type)
    {
        switch (type)
        {
            case AdType.STATIC:
                if (AdConstants.shouldDisplayAds() == false)
                    return;
                LoadInterstitial();
                break;

            case AdType.INTERSTITIAL:
                if (AdConstants.shouldDisplayAds() == false)
                    return;
                LoadInterstitial();
                break;

            case AdType.REWARDED:
                LoadRewardedAd();
                break;

            case AdType.REWARDED_INTERSTITIAL:
                LoadRewardedAd();
                break;
        }
    }

    public void ShowAd(AdType type, string adPlacement)
    {


        switch (type)
        {
            case AdType.STATIC:
                if (AdConstants.shouldDisplayAds() == false)
                    return;
                ShowInterstitial(adPlacement);
                break;

            case AdType.INTERSTITIAL:
                if (AdConstants.shouldDisplayAds() == false)
                    return;
                ShowInterstitial(adPlacement);
                break;

            case AdType.REWARDED:
                ShowRewardedAd(adPlacement);
                break;

            case AdType.REWARDED_INTERSTITIAL:
                ShowRewardedAd(adPlacement);
                break;
        }

    }

#endregion

#endregion
#region Banner Ad Methods
#if USE_MAX

    private void InitializeBannerAds()
    {
        if (IsBannerAdReadyToLoad())
        {
            Debug.Log("-------- Initializing Banner ad --------");
        
           
            // Attach Callbacks
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdFailedEvent;
            MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;

            // Banners are automatically sized to 320x50 on phones and 728x90 on tablets.
            // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments.
            if (isSmartBanner)
                MaxSdk.SetBannerWidth(AdsIds.BannerAdUnitId(), Screen.width);
            else
            {
                MaxSdk.SetBannerWidth(AdsIds.BannerAdUnitId(), 320);
            }
           


            isBannerAdShowing = false;
            MaxSdk.CreateBanner(AdsIds.BannerAdUnitId(), bannerPosition);
            if (isAdaptiveBanner)
            {
                MaxSdk.SetBannerExtraParameter(AdsIds.BannerAdUnitId(), "adaptive_banner", "true");
            }
            if (shouldHaveBannerBg)
            {
                MaxSdk.SetBannerBackgroundColor(AdsIds.BannerAdUnitId(), Color.black);
            }

            if (showBannerOnIntialization)
                LoadBanner();
        }
    }
    private void unregisterBannerEvents() 
    {
        MaxSdkCallbacks.Banner.OnAdLoadedEvent -= OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent -= OnBannerAdFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent -= OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent -= OnBannerAdRevenuePaidEvent;
    }
#endif

    private void ShowBanner()
    {
#if USE_MAX
        //if (IsBannerAdAvailable())
        //{
    //    isBannerAdShowing = false;
        Debug.Log("-------- Showing Banner ad --------");
        isBannerAdisbeingLoaded = true;
        MaxSdk.ShowBanner(AdsIds.BannerAdUnitId());
        //}
#endif
    }

    private void LoadBanner()
    {
#if USE_MAX
        //if (IsBannerAdAvailable())
        //{
       // isBannerAdShowing = false;
        Debug.Log("-------- Loading Banner ad --------");
        isBannerAdisbeingLoaded = true;
        MaxSdk.LoadBanner(AdsIds.BannerAdUnitId());
        //}
#endif
    }

    private void HideBanner()
    {
#if USE_MAX
        //if (IsBannerAdAvailable())
        //{
        isBannerAdShowing = false;
        Debug.Log("-------- Hiding Banner ad --------");
        MaxSdk.HideBanner(AdsIds.BannerAdUnitId());
        //}
#endif
    }

    private void ToggleBannerVisibility(bool show)
    {
     
#if USE_MAX
        if (isBannerAdShowing)
        {
            if (show)
            {
                MaxSdk.ShowBanner(AdsIds.BannerAdUnitId());
            }
            else
            {
                
                MaxSdk.HideBanner(AdsIds.BannerAdUnitId());
            }
        }
#endif
    }

    public bool IsBannerAdAvailable()
    {
#if USE_MAX
        return !string.IsNullOrEmpty(AdsIds.BannerAdUnitId()) && AdConstants.shouldDisplayAds() && AdConstants.showBannerAd && !isBannerAdisbeingLoaded && isSdkInitialized;
#else
        return false;

#endif
    }

    public bool IsBannerAdReadyToLoad()
    {
#if USE_MAX
        return !string.IsNullOrEmpty(AdsIds.BannerAdUnitId()) && AdConstants.shouldDisplayAds() && AdConstants.showBannerAd && isSdkInitialized;
#else
        return false;

#endif
    }

#if USE_MAX

    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Banner ad is ready to be shown.
        // If you have already called MaxSdk.ShowBanner(BannerAdUnitId) it will automatically be shown on the next ad refresh.
        if (GVAnalysisManager.Instance)
            GVAnalysisManager.Instance.AdAnalysis(BannerAdTypes.BANNER);
        Debug.Log("-------- Banner ad loaded --------");
        isBannerAdisbeingLoaded = false;
        isBannerAdShowing = true;
        bannerRetryAttempt = 0;
        if (showBannerOnIntialization) {
            ShowBannerAd(BannerAdTypes.BANNER);
        }
            
        showBannerOnIntialization = false;
        BannerAdLoadingSuccessful();
    }

    private void OnBannerAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Banner ad failed to load. MAX will automatically try loading a new ad internally.
        Debug.Log("-------- Banner ad failed to load with error code: " + errorInfo.Code + " --------");
        isBannerAdisbeingLoaded = false;
        isBannerAdShowing = false;
        bannerRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, bannerRetryAttempt));
        Invoke("LoadBanner", (float)bannerRetryAttempt);
        showBannerOnIntialization = false;
        BannerLoadingFailed();
    }

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("-------- Banner ad clicked --------");
      //  MaxSdk.StopBannerAutoRefresh(AdsIds.BannerAdUnitId());
    }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Banner ad revenue paid. Use this callback to track user revenue.
        Debug.Log("-------- Banner ad revenue paid --------");

        // Ad revenue
        double revenue = adInfo.Revenue;

        // Miscellaneous data
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
        string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
        string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
        string placement = adInfo.Placement; // The placement this ad's postbacks are tied to

        TrackAdRevenue("Banner", adInfo);
    }

#endif
    #endregion
    #region MREC Ad Methods
#if USE_MAX

    private void InitializeMRecAds()
    {

        if (string.IsNullOrEmpty(AdsIds.MRECBannerAdUnitId()))
        {
            return;
        }

        Debug.Log("-------- Initializing MRec Banner Ad  --------");

        // Attach Callbacks
        MaxSdkCallbacks.MRec.OnAdLoadedEvent += OnMRecAdLoadedEvent;
        MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += OnMRecAdFailedEvent;
        MaxSdkCallbacks.MRec.OnAdClickedEvent += OnMRecAdClickedEvent;
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnMRecAdRevenuePaidEvent;

        // MRECs are automatically sized to 300x250.
        isMRECBannerAdShowing = false;
        MaxSdk.CreateMRec(AdsIds.MRECBannerAdUnitId(), NativeBannerPos);

    }
    private void unregisterMRecEvents() {

        //MaxSdkCallbacks.MRec.OnAdLoadedEvent -= OnMRecAdLoadedEvent;
        //MaxSdkCallbacks.MRec.OnAdLoadFailedEvent -= OnMRecAdFailedEvent;
        //MaxSdkCallbacks.MRec.OnAdClickedEvent -= OnMRecAdClickedEvent;
        //MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent -= OnMRecAdRevenuePaidEvent;
    }

#endif

    private void ShowMRec()
    {
        if (string.IsNullOrEmpty(AdsIds.MRECBannerAdUnitId()))
        {
            return;
        }
#if USE_MAX
        isMRECBannerAdShowing = false;
        isMRECBannerAdisbeingLoaded = true;
        //Debug.LogError(1);
        Debug.LogWarning("$$Tag: Showing MRec " );
        MaxSdk.ShowMRec(AdsIds.MRECBannerAdUnitId());
#endif
    }

    private void LoadMRec()
    {

        if (string.IsNullOrEmpty(AdsIds.MRECBannerAdUnitId()))
        {
            return;
        }
#if USE_MAX
        isMRECBannerAdShowing = false;
        isMRECBannerAdisbeingLoaded = true;
     //   Debug.LogError(1);
        MaxSdk.ShowMRec(AdsIds.MRECBannerAdUnitId());
#endif
    }

    private void HideMRec()
    {

#if USE_MAX
        isMRECBannerAdShowing = false;
        MaxSdk.HideMRec(AdsIds.MRECBannerAdUnitId());
#endif
    }


    private void ToggleMRecVisibility(bool show)
    {
#if USE_MAX
        if (isMRECBannerAdShowing)
        {
            if (show)
            {
               // Debug.LogError(1);
                MaxSdk.ShowMRec(AdsIds.MRECBannerAdUnitId());
            }
            else
            {
              //  Debug.LogError(1);
                MaxSdk.HideMRec(AdsIds.MRECBannerAdUnitId());
            }
        }
#endif
    }

    public bool IsMRecBannerAdAvailable()
    {
       
#if USE_MAX
        return !string.IsNullOrEmpty(AdsIds.MRECBannerAdUnitId()) && AdConstants.shouldDisplayAds() && AdConstants.showNativeBannerAd && !isMRECBannerAdisbeingLoaded && isSdkInitialized;
#else
        return false;

#endif
    }

    public bool IsMRecBannerAdReadToLoad()
    {
    
#if USE_MAX
        return !string.IsNullOrEmpty(AdsIds.MRECBannerAdUnitId()) && AdConstants.shouldDisplayAds() && AdConstants.showNativeBannerAd && isSdkInitialized;
#else
        return false;

#endif
    }

#if USE_MAX

    private void OnMRecAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
       
        // MRec ad is ready to be shown.
        // If you have already called MaxSdk.ShowMRec(MRecAdUnitId) it will automatically be shown on the next MRec refresh.
        if (GVAnalysisManager.Instance)
            GVAnalysisManager.Instance.AdAnalysis(BannerAdTypes.NATIVE);
        Debug.Log("-------- MRec ad loaded --------");
        isMRECBannerAdisbeingLoaded = false;
        isMRECBannerAdShowing = true;
        mRecRetryAttempt = 0;
        NativeBannerLoadSuccessful();
    }

    private void OnMRecAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        
        // MRec ad failed to load. MAX will automatically try loading a new ad internally.
        Debug.Log("-------- MRec ad failed to load with error code: " + errorInfo.Code + " --------");
        isMRECBannerAdisbeingLoaded = false;
        isMRECBannerAdShowing = false;
        mRecRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, mRecRetryAttempt));
       // Invoke("LoadMRec", (float)mRecRetryAttempt);
        NativeBannerLoadingFailed();
    }

    private void OnMRecAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("-------- MRec ad clicked --------");
    }

    private void OnMRecAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
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

        TrackAdRevenue("Native", adInfo);
    }
#endif

    #endregion
    #region App Open Ads Max
#if USE_MAX && USE_MAX_OPENADS
    int appOpenRetryAttempt = 0;
    void reloadAppOpen()
    {
        MaxSdk.LoadAppOpenAd(AdsIds.AppOpenAdUnitId_MAX());

    }
    private void InitializeAppOpenAds()
    {
        if (IsAppOpenAdReadyToLoad())
        {
            Debug.Log("-------- Initializing App Open Ad --------");

            ResetAppOpenTime(); // resetting time
            // attach callbacks
            MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;
            MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += OnAppOpenLoadedEvent;
            MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += OnAppOpenLoadFailedEvent;
            MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent += OnAppOpenDisplayFailedEvent;
            MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnAppOpenPaidEvent;
            reloadAppOpen();
        }
        
    }

    public void ShowAppLovinAppOpen()
    {

        if (AdConstants.IsAdWasShowing == true)
        {
            AdConstants.IsAdWasShowing = false;
            return;
        }

        if (AdConstants.shouldDisplayAds() == false)
        {
            return;
        }

#if USE_MAX && USE_MAX_OPENADS

        if (MaxSdk.IsAppOpenAdReady(AdsIds.AppOpenAdUnitId_MAX()))
        {
            if (GVAnalysisManager.Instance)
            {
                GVAnalysisManager.Instance.AdAnalysis(AdType.OPEN_AD_MAX);
            }
            AdConstants.IsAdWasShowing = true;
            MaxSdk.ShowAppOpenAd(AdsIds.AppOpenAdUnitId_MAX());
        }
        else
        {
            Debug.Log("Load App Open Ad");

            MaxSdk.LoadAppOpenAd(AdsIds.AppOpenAdUnitId_MAX());
        }


#endif
    }

    public bool IsAppOpenAdAvailable()
    {
#if USE_MAX && USE_MAX_OPENADS
        return !string.IsNullOrEmpty(AdsIds.AppOpenAdUnitId_MAX()) && MaxSdk.IsAppOpenAdReady(AdsIds.AppOpenAdUnitId_MAX()) && AdConstants.shouldDisplayAds() && AdConstants.showApplovinAppOpen && DateTime.Now >= currentTime_AppOpenAd.AddSeconds(AdConstants.adDelay) && isSdkInitialized;
#else
        return false;

#endif
    }

    public bool IsAppOpenAdReadyToLoad()
    {
#if USE_MAX && USE_MAX_OPENADS
        return !string.IsNullOrEmpty(AdsIds.AppOpenAdUnitId_MAX()) && AdConstants.shouldDisplayAds() && AdConstants.showApplovinAppOpen && !isAppOpenAdisbeingLoaded && isSdkInitialized;
#else
        return false;

#endif
    }

    private void OnAppOpenLoadedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        int SplashIndex = SceneManager.GetActiveScene().buildIndex;
        isAppOpenAdisbeingLoaded = false;
        if (showMaxOpenAdOnSplashSecondTime && SplashIndex == 0)
        {
            ShowAppLovinAppOpen();
            showMaxOpenAdOnSplashSecondTime = false;
        }
        Debug.Log("=============> Loaded App Open Ads <===============");
    }

    private void OnAppOpenLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2)
    {
        isAppOpenAdisbeingLoaded = false;
        Debug.Log("=============> Load Failed App Open Ads <===============");
        appOpenRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, appOpenRetryAttempt));
        Invoke("reloadAppOpen", (float)retryDelay);
    }

    private void OnAppOpenDisplayFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo arg3)
    {
        Debug.Log("=============> Display Failed App Open Ads <===============");
        //UpdateGame_Sounds_Banners(true);
        AdConstants.IsAdWasShowing = false;

    }

    public void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        if (IsAppOpenAdReadyToLoad())
        {
            Debug.Log("=============> Closed App Open Ads <===============");
            ResetAppOpenTime(); // resetting time
            //UpdateGame_Sounds_Banners(true);
            isAppOpenAdisbeingLoaded = true;
            reloadAppOpen();
        }
    }

    private void OnAppOpenPaidEvent(string addUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("=============> App Open Ads Renevue Paid <===============");
#if USE_MAX
        TrackAdRevenue("App_Open", adInfo);
#endif
    }


#endif

#if USE_MAX


#if USE_MAX_OPENADS && UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            Debug.Log("=============> Calling App Open Ads <===============");
            ShowAppLovinAppOpen();
        }
    }
#elif USE_MAX_OPENADS && UNITY_IOS //&& UNITY_EDITOR
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Debug.Log("=============> Calling App Open Ads <===============");
            ShowAppLovinAppOpen();
           // MaxSdk.StartBannerAutoRefresh(AdsIds.BannerAdUnitId());
        }
    }
#endif
#endif
    #endregion

    #region Interstitial Ad Methods
#if USE_MAX

    private void InitializeInterstitialAds()
    {
        if (IsInterstitialReadyToLoad())
        {
            Debug.Log("-------- Initializing interstitial Ad --------");

            ResetInterstitialTime(); // resetting time
            // Attach callbacks
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialFailedToDisplayEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;

            // Load the first interstitial
            LoadInterstitial();
        }
    }
    private void unregisterInterstitialEvents() {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnInterstitialFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent -= InterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnInterstitialDismissedEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent -= OnInterstitialRevenuePaidEvent;

    }
#endif

    void LoadInterstitial()
    {
#if USE_MAX

        if (IsInterstitialReadyToLoad())
        {
            isInterstitialAdisbeingLoaded = true;
            MaxSdk.LoadInterstitial(AdsIds.InterstitialAdUnitId());
        }
#endif
    }

    void ShowInterstitial(string adPlacement)
    {
#if USE_MAX

        if (IsInterstitialAdAvailable())
        {
            //interstitialStatusText.text = "Showing";
            if (GVAnalysisManager.Instance)
                GVAnalysisManager.Instance.AdAnalysis(AdType.INTERSTITIAL);
            UpdateGame_Sounds_Banners(false);
            AdConstants.resumeFromAds = true;
            MaxSdk.ShowInterstitial(AdsIds.InterstitialAdUnitId(), adPlacement);
        }
        else
        {
            Debug.Log("-------- Interstitial Ad is not ready yet --------");
            LoadInterstitial();
            //interstitialStatusText.text = "Ad not ready";
        }
#endif
    }

    public bool IsInterstitialAdAvailable()
    {
#if USE_MAX
        return !string.IsNullOrEmpty(AdsIds.InterstitialAdUnitId()) && MaxSdk.IsInterstitialReady(AdsIds.InterstitialAdUnitId()) && AdConstants.shouldDisplayAds() && AdConstants.showInterstitialAd && isSdkInitialized&& DateTime.Now >= currentTime_Interstitial.AddSeconds(AdConstants.adDelay);
#else
        return false;

#endif
    }

    public bool IsInterstitialReadyToLoad()
    {
#if USE_MAX
        return !string.IsNullOrEmpty(AdsIds.InterstitialAdUnitId()) && AdConstants.shouldDisplayAds() && AdConstants.showInterstitialAd && !isInterstitialAdisbeingLoaded && isSdkInitialized;
#else
        return false;

#endif
    }

#if USE_MAX

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
        //interstitialStatusText.text = "Loaded";
        Debug.Log("-------- Interstitial loaded --------");
        isInterstitialAdisbeingLoaded = false;
        // Reset retry attempt
        interstitialRetryAttempt = 0;
    }

    private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
        interstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));

        //interstitialStatusText.text = "Load failed: " + errorInfo.Code + "\nRetrying in " + retryDelay + "s...";
        Debug.Log("-------- Interstitial failed to load with error code: " + errorInfo.Code + " --------");
        isInterstitialAdisbeingLoaded = false;
        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void InterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        AdConstants.resumeFromAds = false;
        // Interstitial ad failed to display. We recommend loading the next ad
        Debug.Log("-------- Interstitial failed to display with error code: " + errorInfo.Code + " --------");
        UpdateGame_Sounds_Banners(true);
        LoadInterstitial();
    }

    private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AdConstants.resumeFromAds = false;
        AdConstants.IsAdWasShowing = true;
        // Interstitial ad is hidden. Pre-load the next ad
        Debug.Log("-------- Interstitial dismissed --------");
        ResetInterstitialTime(); // resetting time
        UpdateGame_Sounds_Banners(true);
        LoadInterstitial();
       
    }

    private void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
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
#endif

#endregion
#region Rewarded Ad Methods
#if USE_MAX

    private void InitializeRewardedAds()
    {
        if (IsRewardedAdReadyToLoad())
        {
            Debug.Log("-------- Initializing Rewarded Ad --------");

            ResetRewardedTime(); // resetting time
            // Attach callbacks
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;

            // Load the first RewardedAd
            LoadRewardedAd();
        }
    }
    private void unregisterRewardedEvents() {
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnRewardedAdFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent -= OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent -= OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnRewardedAdReceivedRewardEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= OnRewardedAdRevenuePaidEvent;
     
    }
#endif
    public void resetRewardedAdisbeingLoaded() {
        isRewardedAdisbeingLoaded = false;
    }
    private void LoadRewardedAd()
    {
#if USE_MAX

        if (IsRewardedAdReadyToLoad())
        {        //rewardedStatusText.text = "Loading...";
            isRewardedAdisbeingLoaded = true;
            Invoke("resetRewardedAdisbeingLoaded", 60);
            MaxSdk.LoadRewardedAd(AdsIds.RewardedAdUnitId());
        }
#endif
    }

    private void ShowRewardedAd(string adPlacement)
    {
        AdConstants.sawRewarded = false;
#if USE_MAX

        if (IsRewardedAdAvailable())
        {
            //rewardedStatusText.text = "Showing";
            if (GVAnalysisManager.Instance)
                GVAnalysisManager.Instance.AdAnalysis(AdType.REWARDED);
              UpdateGame_Sounds_Banners(false);
            AdConstants.resumeFromAds = true;
            MaxSdk.ShowRewardedAd(AdsIds.RewardedAdUnitId(), adPlacement);
        }
        else
        {
            Debug.Log("-------- Rewarded Ad is not ready yet --------");

#if !USE_ADMOB_REWARDED_INTERSITIAL

            NoAdAvailable();
#else
            AdmobRewardedInterstitialManager.Instance.ShowAd();
#endif
            LoadRewardedAd();
        }
#endif
        }


    public bool IsRewardedAdAvailable()
    {
#if USE_MAX
        return !string.IsNullOrEmpty(AdsIds.RewardedAdUnitId()) && MaxSdk.IsRewardedAdReady(AdsIds.RewardedAdUnitId()) && AdConstants.showRewardedAd && DateTime.Now >= currentTime_Rewarded.AddSeconds(AdConstants.AdDelayReward) && isSdkInitialized;
#else
        return false;

#endif
    }

    public bool IsRewardedAdReadyToLoad()
    {
#if USE_MAX
        return !string.IsNullOrEmpty(AdsIds.RewardedAdUnitId()) && AdConstants.showRewardedAd && !isRewardedAdisbeingLoaded && isSdkInitialized;
#else
        return false;

#endif
    }

#if USE_MAX

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
        //rewardedStatusText.text = "Loaded";
        Debug.Log("-------- Rewarded ad loaded --------");
        isRewardedAdisbeingLoaded = false;
        CancelInvoke("resetRewardedAdisbeingLoaded");
        RewardedAdLoaded();
        // Reset retry attempt
        rewardedRetryAttempt = 0;
    }

    private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
        rewardedRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));

        //rewardedStatusText.text = "Load failed: " + errorInfo.Code + "\nRetrying in " + retryDelay + "s...";
        Debug.Log("-------- Rewarded ad failed to load with error code: " + errorInfo.Code + " --------");
        isRewardedAdisbeingLoaded = false;
        CancelInvoke("resetRewardedAdisbeingLoaded");
        RewardedAdLoadFailed();
        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        AdConstants.resumeFromAds = false;
        // Rewarded ad failed to display. We recommend loading the next ad
        Debug.Log("-------- Rewarded ad failed to display with error code: " + errorInfo.Code + " --------");
        UpdateGame_Sounds_Banners(true);
#if !USE_ADMOB_REWARDED_INTERSITIAL

            NoAdAvailable();
#else
        AdmobRewardedInterstitialManager.Instance.ShowAd();
#endif
        RewardedAdShowingFailed();
        LoadRewardedAd();
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("-------- Rewarded ad displayed --------");
        AdConstants.IsAdWasShowing = true;
        RewardedAdShowing();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("-------- Rewarded ad clicked --------");
    }

    private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AdConstants.resumeFromAds = false;
        // Rewarded ad is hidden. Pre-load the next ad
        Debug.Log("-------- Rewarded ad dismissed --------");
        ResetRewardedTime(); // resetting time
        UpdateGame_Sounds_Banners(true);
        AdConstants.IsAdWasShowing = true;
        DecideForReward();
        LoadRewardedAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad was displayed and user should receive the reward
        Debug.Log("-------- Rewarded ad received reward --------");
        AdConstants.sawRewarded = true;
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad revenue paid. Use this callback to track user revenue.
        Debug.Log("-------- Rewarded ad revenue paid --------");
        AdConstants.IsAdWasShowing = true;
        // Ad revenue
        double revenue = adInfo.Revenue;

        // Miscellaneous data
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
        string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
        string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
        string placement = adInfo.Placement; // The placement this ad's postbacks are tied to

        TrackAdRevenue("Rewarded", adInfo);
    }

#endif

    #endregion

    private void OnDestroy()
    {
#if USE_MAX
        unregisterBannerEvents();
        unregisterInterstitialEvents();
        unregisterMRecEvents();
        unregisterRewardedEvents();
#endif
    }

    #region Paid Impression
#if USE_MAX
    private void TrackAdRevenue(string adString, MaxSdkBase.AdInfo adInfo)
    {
        //Debug.LogFormat("ad_platform: Applovin \n AppLovin ad_source: {0}\nad_unit_name: {1}\nad_format: {2}\nvalue: {3}\ncurrency: USD"
        //             , adInfo.NetworkName, adInfo.AdUnitIdentifier, adInfo.AdFormat, adInfo.Revenue);
        if (GVAnalysisManager.Instance)
            GVAnalysisManager.Instance.PaidAdAnalytics(adString, adInfo);

    }
#endif
    #endregion
    #region RATE US
    IEnumerator showingIOSRate(bool ratePopUp = false)
    {
#if UNITY_IOS

        Ping googPing = new Ping("8.8.8.8");
        // keep in mind that 'Ping' only accepts IP addresses, it doesn't 
        // do DNS lookup. This address may not work for your location-
        // Google owns many servers the world over.


        while (!googPing.isDone)
        {
            yield return new WaitForSecondsRealtime(2);
        }
        // Debug.Log(googPing.time);

        if (googPing.time > 0 && googPing.time < 500)
        {
            yield return new WaitForSecondsRealtime(1);

            if (UnityEngine.iOS.Device.RequestStoreReview())
            {
                if (ratePopUp) // this is for controlling time scale
                    Instantiate(Resources.Load("IOSRateMenuSupport"));

            }
        }


#else
        yield return null;
#endif

    }

    public void PromptRateMenu(bool ratePopUp = false)
    {

#if UNITY_IOS
        // Show Rate us here
        if (AdConstants.GetInternetStatus())
        {
           // Debug.LogWarning("$$Tag: Rate us called " );
            StartCoroutine(showingIOSRate(ratePopUp));

        }
#elif UNITY_ANDROID //&& !UNITY_EDITOR

        if (AdConstants.shouldDisplayRateMenu())
        {
            DestroyBannerAd(BannerAdTypes.NATIVE);
#if !SHOW_NATIVE_RATE
            if (reviewDialogMethod != null)
                reviewDialogMethod();
#elif SHOW_NATIVE_RATE
            AdController.instance?.iapInstance.showRateMenu();
#endif

            //  AdConstants.userHasRatedApp();
        }

#endif


    }

#endregion
#region User Callback


    private void BannerAdLoadingSuccessful()
    {
#if USE_DELEGATES
        if (bannerLoadingSuccessfulMethod != null)
        {
            bannerLoadingSuccessfulMethod.Invoke();
        }
#endif
    }

    private void BannerLoadingFailed()
    {
#if USE_DELEGATES

        if (bannerLoadingFailedMethod != null)
        {
            bannerLoadingFailedMethod.Invoke();
        }
#endif
    }

    public void NativeBannerLoadingFailed()
    {
#if USE_DELEGATES

        if (nativeBannerLoadingFailedMethod != null)
        {
            nativeBannerLoadingFailedMethod.Invoke();
        }
#endif
    }

    public void NativeBannerLoadSuccessful()
    {
#if USE_DELEGATES

        if (nativeBannerShowMethod != null)
        {
            nativeBannerShowMethod.Invoke();
        }
#endif
    }




    public void DecideForReward()
    {
        //Debug.LogError(2+ "AdConstants.sawRewarded"+ AdConstants.sawRewarded);
        if (AdConstants.sawRewarded)
        {
           // Debug.LogError(3 + "AdConstants.sawRewarded" + AdConstants.sawRewarded);
            GaveReward();
        }
        else
        {
            //Debug.LogError(4 + "AdConstants.sawRewarded" + AdConstants.sawRewarded);
            CancelReward();
        }


       
    }

    public void RewardedAdLoaded()
    {
#if USE_DELEGATES

        if (rewardedAdLoadMethod != null)
        {
            rewardedAdLoadMethod.Invoke();
        }
#endif
    }


    private void RewardedAdLoadFailed()
    {
#if USE_DELEGATES

        if (rewardedAdLoadFailedMethod != null)
        {
            rewardedAdLoadFailedMethod.Invoke();
        }
#endif
    }

    private void RewardedAdShowing()
    {
#if USE_DELEGATES

        if (rewardedAdShowingMethod != null)
            rewardedAdShowingMethod.Invoke();
#endif
    }

    private void RewardedAdShowingFailed()
    {
#if USE_DELEGATES

        if (rewardedAdShowingFailedMethod != null)
            rewardedAdShowingFailedMethod.Invoke();
#endif
    }

    private void GaveReward()
    {
        if (shouldResetIntersititialTimeOnReward) {
            ResetInterstitialTime();
        }
       
        Debug.Log("-------- Gave Reward --------");
        if (gaveRewardMethod != null)
            gaveRewardMethod();
    }

    private void CancelReward()
    {
        Debug.Log("-------- Reward Skipped --------");

        if (cancelRewardedAdMethod != null)
            cancelRewardedAdMethod();
    }


    public void NoAdAvailable()
    {
        Debug.Log("-------- No Rewarded Ad Available --------");

        ShowNativeAlert();

        if (noRewardedVideoMethod != null)
            noRewardedVideoMethod();
    }

    EasyMobile.NativeUI.AlertPopup alert;

    void ShowNativeAlert()
    {
        alert = EasyMobile.NativeUI.Alert("Try Later", "Ad is Not Available right now.");

        if (alert != null)
            alert.OnComplete += OnAlertCompleteHandler;

    }

    private void OnAlertCompleteHandler(int obj)
    {
        if (alert != null)
            alert.OnComplete -= OnAlertCompleteHandler;

    }

    private void UpdateGameSound(bool status)
    {
        Debug.Log("-------- Game Sound => " + (status == true ? "ON" : "OFF") + " --------");
        if (updateGameSoundMethod != null)
            updateGameSoundMethod(status);
    }


#if USE_FIREBASE && USE_REMOTE_CONFIG
    public void OnFirebaseCompleteFetching(IDictionary<string, ConfigValue> fetchValue)
    {
        if (onFirebaseRemoteConfigSuccess != null)
        {
            onFirebaseRemoteConfigSuccess(fetchValue);
        }
    }
#endif

#endregion
#region Ads Delay Management

    public void ResetInterstitialTime()
    {
        currentTime_Interstitial = DateTime.Now;
    }
    private void ResetBannerTime()
    {
        currentTime_banner = DateTime.Now;
    }
    private void ResetRewardedTime()
    {
        currentTime_Rewarded = DateTime.Now;
    }

    private void ResetAppOpenTime()
    {
        currentTime_AppOpenAd = DateTime.Now;
    }

#endregion
#region Ads Optimization
    public void UpdateGame_Sounds_Banners(bool status)
    {
        ToggleBannerVisibility(status);
        //ToggleAdaptiveBannerVisibility(status);
      //  ToggleMRecVisibility(status);
        UpdateGameSound(status);
    }
    #endregion




 
    
}


