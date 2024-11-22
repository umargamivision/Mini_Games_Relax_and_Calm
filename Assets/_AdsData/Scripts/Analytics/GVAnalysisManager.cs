using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
#if USE_FIREBASE
using Firebase.Analytics;
using Firebase;
using System.Threading.Tasks;
using Firebase.Extensions;
#if USE_FIREBASE && USE_REMOTE_CONFIG
using Firebase.RemoteConfig;
#endif
#endif
#if USE_FIREBASE && USE_CRASHLYTICS
using Firebase.Crashlytics;
#endif
#if USE_ADMOB_PAID_EVENT
using GoogleMobileAds.Api;
#endif




using UnityEngine.Analytics;

public class GVAnalysisManager : MonoBehaviour
{
    // Variables
    #region Variables

    public enum AnalyticsType
    {
        UNITY,
        FIREBASE,
        FACEBOOK,
        NONE
    };

    public static GVAnalysisManager Instance;
    // Property to access the instance
    public static GVAnalysisManager instance
    {
        get
        {
            if (Instance == null)
            {
                Debug.LogError("GVAnalysisManager instance is null. Make sure the instance is initialized before accessing it.");
            }

            return Instance;
        }
    }
    [Title("Analysis Provider")]
    public AnalyticsType analyticsType; // States of Analytics  // analytics will be send to the selected type           
    [Serializable]
    public class RemoteEvent : UnityEvent<FirebaseRemoteData>
    {
        // todo
    }
    [Serializable]
    public class FirebaseRemoteData
    {
        public enum DataType
        {
            NUMBER, STRING, BOOLEAN, JSON
        }
        public string name;
        public DataType type;
        [ShowIf("type", DataType.NUMBER)]
        public int DefaultValue_Number;
        [ShowIf("type", DataType.STRING)]
        public string DefaultValue_String;
        [ShowIf("type", DataType.BOOLEAN)]
        public bool DefaultValue_Boolean;
        [ShowIf("type", DataType.JSON)]
        public string DefaultValue_Json;
        public RemoteEvent onFetched;
    }
    [Title("Firebase Remote Config")] // Remote config
    public List<FirebaseRemoteData> remote_Data = new List<FirebaseRemoteData>(); // array containing all the data of remote config
    private FirebaseRemoteData remote_adsSettings, remote_LowEndDevices;
    [Title("Firebase On Complete Processing Callback")] // Remote config
    public UnityEvent OnFirebaseInitialized; // Callback after firebase is Initialized
    [Title("Remote Config On Complete Processing Callback")] // Remote config
    public UnityEvent OnRemoteConfigInitialized; // Callback after firebase is Initialized
    [HideInInspector] public bool sendAnalytics = false;
    [Title("This make sure to keep one instance of ads Initializer keep this true")]

    #endregion

    // Initializations for analtytics
    #region Initialization

    public void Awake()
    {

        // Ensure that only one instance of the singleton class exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        firebaseAnalysis(); // Firebase Initialization calling
        SetupPluginRemoteData();



    }

    #region Firebase

    async void firebaseAnalysis()
    {
        try
        {

#if USE_FIREBASE

            IDictionary<Firebase.Analytics.ConsentType, Firebase.Analytics.ConsentStatus> consentValues = new Dictionary<Firebase.Analytics.ConsentType, Firebase.Analytics.ConsentStatus>
                            {
                                { Firebase.Analytics.ConsentType.AdUserData, Firebase.Analytics.ConsentStatus.Granted },
                                { Firebase.Analytics.ConsentType.AnalyticsStorage, Firebase.Analytics.ConsentStatus.Granted },
                                { Firebase.Analytics.ConsentType.AdPersonalization, Firebase.Analytics.ConsentStatus.Granted },
                                { Firebase.Analytics.ConsentType.AdStorage, Firebase.Analytics.ConsentStatus.Granted }
                                // Add more entries as needed
                            };

            Firebase.Analytics.FirebaseAnalytics.SetConsent(consentValues);

            Debug.Log("Checking Dependencies");
            DependencyStatus dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
#if USE_CRASHLYTICS
            Crashlytics.IsCrashlyticsCollectionEnabled = true;
#endif
            if (dependencyStatus == DependencyStatus.Available)
            {
                var app = FirebaseApp.DefaultInstance;
                sendAnalytics = true;
                Debug.Log("=================> Firebase Initialized Successfully =================>");

            }
            else
            {
                Debug.Log("=================> Firebase Initialization Failed =================>");

            }

#endif
        }
        catch (Exception e)
        {

        }
        if (OnFirebaseInitialized != null)
            OnFirebaseInitialized.Invoke();

    }

    #endregion

    #endregion

    // public Analytics Methods
    #region Analytics Methods

    #region Ads Analysis
    public void AdAnalysis(AdController.BannerAdTypes adType)
    {
        if (sendAnalytics == false)  // if adcontroller is not intialized or analytics are not enabled from user
            return;

        switch (adType)
        {
            case AdController.BannerAdTypes.BANNER:
                AdAnalysis("ad_banner");

                break;

            case AdController.BannerAdTypes.ADAPTIVE:
                AdAnalysis("ad_adaptive_banner");

                break;


            case AdController.BannerAdTypes.NATIVE:
                AdAnalysis("ad_native");

                break;


        }
    }


    public void AdAnalysis(AdController.AdType adType)
    {
        if (sendAnalytics == false)  // if adcontroller is not intialized or analytics are not enabled from user
            return;

        switch (adType)
        {
            case AdController.AdType.STATIC:
                AdAnalysis("ad_static");


                break;


            case AdController.AdType.INTERSTITIAL:
                AdAnalysis("ad_interstitial");

                break;

            case AdController.AdType.INTERSTITIAL_LIMITED:
                AdAnalysis("ad_limited_interstitial");

                break;
            case AdController.AdType.REWARDED:
                AdAnalysis("ad_rewarded");


                break;

            case AdController.AdType.REWARDED_INTERSTITIAL:
                AdAnalysis("ad_rewarded_interstitial");

                break;

            case AdController.AdType.NO_AD:
                AdAnalysis("ad_no_reward");

                break;

            case AdController.AdType.OPEN_AD:
                AdAnalysis("ad_app_open");

                break;
            case AdController.AdType.OPEN_AD_MAX:
                AdAnalysis("ad_app_open_max");

                break;
        }
    }

    private void AdAnalysis(string adType)
    {
        if (sendAnalytics == false)  // if adcontroller is not intialized or analytics are not enabled from user
            return;

        switch (analyticsType)
        {
            case AnalyticsType.FIREBASE:

#if USE_FIREBASE

                try
                {
                    //FirebaseAnalytics.LogEvent("Ads_Analysis", // Sending Analytics to firebase
                    // new Parameter("Ads", adType) // sending ads type send from Adcontroller
                    // );

                   // FirebaseAnalytics.LogEvent(adType); // sending ads type send from Adcontroller

                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }

#endif
                break;

            case AnalyticsType.UNITY:

                Analytics.CustomEvent("Ads_Analysis", new Dictionary<string, object> { // Sending Analytics to Unity
                    {
                        Application.version.ToString() ,adType // sending ads type send from Adcontroller with app version
                    } }
                );

                break;

            case AnalyticsType.NONE:

                Debug.Log("No analytics sent");

                break;
        }
    }

    #endregion

    #region Paid Events
#if USE_MAX
    public void PaidAdAnalytics(string adString, MaxSdkBase.AdInfo adInfo)
    {
        // if adcontroller is not intialized or analytics are not enabled from user or any args or parameter is send with null
        if (sendAnalytics == false)
            return;

        switch (analyticsType)
        {
            case AnalyticsType.FIREBASE:

                // UNITY_IOS
#if USE_FIREBASE
#if UNITY_IOS
                double revenue = adInfo.Revenue;
                var impressionParameters = new[]
                {
                     new Parameter("ad_platform", "AppLovin"),
                     new Parameter("ad_source", adInfo.NetworkName),
                     new Parameter("ad_unit_name", adInfo.AdUnitIdentifier),
                     new Parameter("ad_format", adInfo.AdFormat),
                     new Parameter("value", revenue),
                     new Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
                };
                FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
                Debug.Log("ad_impression" + impressionParameters);
                //--------AppsFlyer AdRevenue Events--------
#if USE_APPSFLYER


                Dictionary<string, string> AdParameters = new Dictionary<string, string>();
                AdParameters.Add("ad_platform", "Applovin"); //
                AdParameters.Add("ad_source", adInfo.NetworkName); //
                AdParameters.Add("ad_unit_name", adInfo.AdUnitIdentifier); //
                AdParameters.Add("ad_format", adInfo.AdFormat); //
                AdParameters.Add("value", revenue.ToString()); //
                AdParameters.Add("currency", "USD"); //
                AppsFlyerSDK.AppsFlyer.sendEvent("paid_ad_impressions_new", AdParameters);
                Debug.Log("paid_ad_impressions_new" + AdParameters);

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("ad_unit_name", adInfo.AdUnitIdentifier);
                dic.Add("ad_format", adInfo.AdFormat);
                AppsFlyerSDK.AppsFlyerAdRevenue.logAdRevenue(adInfo.NetworkName,
                    AppsFlyerSDK.AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeApplovinMax, revenue, "USD", dic);
#endif
#else
                double revenue = adInfo.Revenue;
                var impressionParameters = new[]
                {
                     new Parameter("ad_platform", "AppLovin"),
                     new Parameter("ad_source", adInfo.NetworkName),
                     new Parameter("ad_unit_name", adInfo.AdUnitIdentifier),
                     new Parameter("ad_format", adInfo.AdFormat),
                     new Parameter("value", revenue),
                     new Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
                };
                FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
                Debug.Log("ad_impression" + impressionParameters);

                //--------AppsFlyer AdRevenue Events--------
#if USE_APPSFLYER
                Dictionary<string, string> AdParameters = new Dictionary<string, string>();
                AdParameters.Add("ad_platform", "Applovin"); //
                AdParameters.Add("ad_source", adInfo.NetworkName); //
                AdParameters.Add("ad_unit_name", adInfo.AdUnitIdentifier); //
                AdParameters.Add("ad_format", adInfo.AdFormat); //
                AdParameters.Add("value", revenue.ToString()); //
                AdParameters.Add("currency", "USD"); //
                AppsFlyerSDK.AppsFlyer.sendEvent("paid_ad_impressions_new", AdParameters);
                Debug.Log("paid_ad_impressions_new" + AdParameters);

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("ad_unit_name", adInfo.AdUnitIdentifier);
                dic.Add("ad_format", adInfo.AdFormat);
                AppsFlyerSDK.AppsFlyerAdRevenue.logAdRevenue(adInfo.NetworkName,
                    AppsFlyerSDK.AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeApplovinMax, revenue, "USD", dic);
#endif
#endif
#endif
                break;

            case AnalyticsType.NONE:

                Debug.Log("No analytics sent");

                break;
        }
    }

#endif

#if USE_ADMOB_PAID_EVENT
    public void PaidAdAnalytics(string adString, ResponseInfo info, GoogleMobileAds.Api.AdValue adValue)
    {
        // if adcontroller is not intialized or analytics are not enabled from user or any args or parameter is send with null
        if (!sendAnalytics || info == null || adValue == null)
            return;

        switch (analyticsType)
        {
            case AnalyticsType.FIREBASE:

#if USE_FIREBASE

                decimal currentImpressionRevenue = (decimal)(adValue.Value / Mathf.Pow(10, 6)); // calculation impression revenue with 10^6 decimals
                decimal previousTroasCache = decimal.Parse(getAdValue(adString), System.Globalization.NumberStyles.Float); // previously cached troas
                decimal currentTroasCache = (decimal)(previousTroasCache + currentImpressionRevenue); // summing up previous and current troas to get estimated value
                if (currentTroasCache >= (decimal)0.01) // avoiding minor values we do'nt need those
                {

#if USE_FIREBASE
                    try
                    {
                        FirebaseAnalytics.LogEvent("paid_ad_impressions_new", // sending Paid events details to Firebase
                        new Parameter("value", (double)currentTroasCache),
                        new Parameter("currency", "" + adValue.CurrencyCode.ToString()),
                        new Parameter("precision", "" + adValue.Precision.ToString()),
                        new Parameter("network", "" + info.GetMediationAdapterClassName().ToString())
                        );

                        setAdValue(adString, "0");

#endif
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
                else
                {
                    setAdValue(adString, currentTroasCache.ToString()); // else update Troas in cache
                }

#endif
                break;

            case AnalyticsType.UNITY:

                Analytics.CustomEvent("paid_ad_impressions", new Dictionary<string, object> { // sending Paid events details to Unity
                    {"valueMicros" ,adValue.Value.ToString()},
                    {"currency",adValue.CurrencyCode.ToString()},
                    {"precision",adValue.Precision.GetType().ToString()},
                    {"network", info.GetMediationAdapterClassName().ToString()}
                }
                );

                break;

            case AnalyticsType.NONE:

                Debug.Log("No analytics sent");

                break;
        }
    } // Paid events Admob

#endif

    #region Playerpref

    private void setAdValue(string str, string val)
    {
        PlayerPrefs.SetString("tROAS" + str, val); // Saving Troas values
    }

    private string getAdValue(string str)
    {
        return PlayerPrefs.GetString("tROAS" + str, "0"); // Getting Troas values
    }



    #endregion

    #endregion

    #region Custom Events
    public void CustomEvent(string eventName)
    {
        if (sendAnalytics == false)
            return;
#if USE_FIREBASE
        try
        {
            Debug.Log(eventName);
            FirebaseAnalytics.LogEvent(eventName);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
#endif

    }

    #endregion

    #endregion

    // Firebase Remote Config
    #region Remote Configurations


    #region Remote Config 

    private void SetupPluginRemoteData()
    {
        if (GetComponent<DeviceSettingsManager>() == null)
        {
            return;
        }
        AdsSettings adsettings = GetComponent<DeviceSettingsManager>()._adsSettings;
        if (adsettings != null)
        {
            remote_adsSettings = new FirebaseRemoteData();
            remote_adsSettings.type = FirebaseRemoteData.DataType.JSON;
            remote_adsSettings.name = "Device_Settings";
            remote_adsSettings.DefaultValue_Json = JsonUtility.ToJson(adsettings.deviceSettings);
            remote_adsSettings.onFetched = new RemoteEvent();
            remote_adsSettings.onFetched.RemoveAllListeners();
            remote_adsSettings.onFetched.AddListener(OnFetchAdsSettings);
            if (string.IsNullOrEmpty(remote_adsSettings.DefaultValue_Json) == false)
            {
                remote_Data.Add(remote_adsSettings);
                Debug.LogFormat("Defaul Device Json Settings {0}", remote_adsSettings.DefaultValue_Json);
            }
            else
                Debug.LogErrorFormat("Json Devices Formatting Failed", remote_adsSettings.DefaultValue_Json);


            remote_LowEndDevices = new FirebaseRemoteData();
            remote_LowEndDevices.name = "LowEndDevices";
            remote_LowEndDevices.type = FirebaseRemoteData.DataType.JSON;
            remote_LowEndDevices.DefaultValue_Json = JsonUtility.ToJson(adsettings.lowEndDevices);
            remote_LowEndDevices.onFetched = new RemoteEvent();
            remote_LowEndDevices.onFetched.RemoveAllListeners();
            remote_LowEndDevices.onFetched.AddListener(OnFetchLowEndDevicesSettings);
            if (string.IsNullOrEmpty(remote_LowEndDevices.DefaultValue_Json) == false)
            {
                remote_Data.Add(remote_LowEndDevices);
                Debug.LogFormat("Defaul Low End Json Settings {0}", remote_LowEndDevices.DefaultValue_Json);
            }
            else
                Debug.LogErrorFormat("Json Low End Formatting Failed", remote_LowEndDevices.DefaultValue_Json);
        }

    }


    public void FetchFireBase()
    {
#if USE_REMOTE_CONFIG && USE_FIREBASE
        try
        {
            FetchDataAsync();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

#else
        if (OnRemoteConfigInitialized != null)
            OnRemoteConfigInitialized.Invoke();

#endif
    }
#if USE_REMOTE_CONFIG && USE_FIREBASE
    private Task FetchDataAsync()
    {

        System.Threading.Tasks.Task fetchTask =
        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
            TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);

    }
#endif
#if USE_REMOTE_CONFIG && USE_FIREBASE
    private void FetchComplete(Task fetchTask)
    {


        try
        {
            if (fetchTask.IsCanceled)
            {
                Debug.Log("Fetch canceled.");

            }
            else if (fetchTask.IsFaulted)
            {
                Debug.Log("Fetch encountered an error.");

            }
            else if (fetchTask.IsCompleted)
            {
                Debug.Log("Fetch completed successfully!");

            }


            var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
            switch (info.LastFetchStatus)
            {
                case Firebase.RemoteConfig.LastFetchStatus.Success:
                    Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                    .ContinueWithOnMainThread(task =>
                    {
                        Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).", info.FetchTime));
                    });
                    OnRemoteDataFetched();
                    if (OnRemoteConfigInitialized != null)
                        OnRemoteConfigInitialized.Invoke();
                    break;
                case Firebase.RemoteConfig.LastFetchStatus.Failure:

                    Debug.Log(AdConstants.Colorize("Firebase Local Json Activated", "Red", true));

                    // On failed
                    if (OnRemoteConfigInitialized != null)
                        OnRemoteConfigInitialized.Invoke();
                    switch (info.LastFetchFailureReason)
                    {
                        case Firebase.RemoteConfig.FetchFailureReason.Error:
                            Debug.Log("Fetch failed for unknown reason");
                            break;
                        case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                            Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                            break;
                    }
                    break;
                case Firebase.RemoteConfig.LastFetchStatus.Pending:
                    Debug.Log("Latest Fetch call still pending.");
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

    }
#endif

    #endregion

#if USE_REMOTE_CONFIG && USE_FIREBASE

    private void OnRemoteDataFetched()
    {
        try
        {
            IDictionary<string, ConfigValue> fetchValue = FirebaseRemoteConfig.DefaultInstance.AllValues;

            foreach (var item in fetchValue)
            {

                for (int i = 0; i < remote_Data.Count; i++)
                {

                    if (remote_Data[i].name.Contains(item.Key))
                    {
                        switch (remote_Data[i].type)
                        {
                            case FirebaseRemoteData.DataType.NUMBER:

                                //Debug.Log(string.Format("Name: {0} Value: {1}", item.Key, item.Value.DoubleValue));
                                remote_Data[i].DefaultValue_Number = (int)item.Value.DoubleValue;

                                break;

                            case FirebaseRemoteData.DataType.STRING:

                                //Debug.Log(string.Format("Name: {0} Value: {1}", item.Key, item.Value.StringValue));
                                remote_Data[i].DefaultValue_String = item.Value.StringValue;

                                break;

                            case FirebaseRemoteData.DataType.BOOLEAN:

                                //Debug.Log(string.Format("Name: {0} Value: {1}", item.Key, item.Value.BooleanValue));
                                remote_Data[i].DefaultValue_Boolean = item.Value.BooleanValue;

                                break;

                            case FirebaseRemoteData.DataType.JSON:

                                //Debug.Log(string.Format("Name: {0} Value: {1}", item.Key, item.Value.StringValue));
                                remote_Data[i].DefaultValue_Json = item.Value.StringValue;

                                break;
                        }
                        // Calling the Callback with updated data 
                        if (remote_Data[i].onFetched != null)
                            remote_Data[i].onFetched.Invoke(remote_Data[i]);
                    }
                }
            }


            if (AdController.instance)
                AdController.instance.OnFirebaseCompleteFetching(fetchValue); // Calling delegate method for user firebase cusstom remote config data
            Debug.Log(AdConstants.Colorize("Custom Firebase Fetched", "Green", true));
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);

            Debug.Log(AdConstants.Colorize("Firebase Local Json Activated Reason:" + e.Message, "Red", true));



        }

    }
#endif





    #endregion



    

    #region Remote Methods

    public void OnFetchAdsSettings(FirebaseRemoteData data)
    {
        Debug.Log(AdConstants.Colorize("Android Ads Settings Fetched Sucessfully Data: " + data.DefaultValue_Json, "Green", true));
        GetComponent<DeviceSettingsManager>().deviceSettingsJson = data.DefaultValue_Json;
    }

    public void OnFetchLowEndDevicesSettings(FirebaseRemoteData data)
    {
        Debug.Log(AdConstants.Colorize("Low End Devices Ads Settings Fetched Sucessfully Data: " + data.DefaultValue_Json, "Green", true));

    }

    #endregion
}
