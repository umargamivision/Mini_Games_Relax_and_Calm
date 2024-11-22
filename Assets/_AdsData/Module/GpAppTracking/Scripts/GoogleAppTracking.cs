//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using GoogleMobileAds.Api;
//using UnityEngine.SceneManagement;
//using System.Collections;
//using GoogleMobileAds.Ump.Api;
//using Sirenix.OdinInspector;

//namespace GpAppTrackingg
//{
//    [AddComponentMenu("GV Max Plugin/GpAppTrackingg/GoogleAppTracking")]
//    public class GoogleAppTracking : MonoBehaviour
//    {

//        [BoxGroup("INFO")] [SerializeField] private bool shouldShowAppTracking = true;
//        [BoxGroup("INFO")] [SerializeField] [ReadOnly] private bool isInitialized = false;
//        [BoxGroup("INFO")] [SerializeField] [ReadOnly] private ConsentForm _consentForm;


//        [BoxGroup("APP TRACKING")] [SerializeField] private DebugGeography debugGeography = DebugGeography.Disabled;
//        [BoxGroup("APP TRACKING")] [SerializeField] private bool tagForUnderAgeOfConsent = false;

//        [BoxGroup("TESTING")]
//        [SerializeField]
//        private readonly List<string> TEST_DEVICE_IDS = new List<string>
//        {
//            AdRequest.TestDeviceSimulator,
//            // Add your test device IDs (replace with your own device IDs).
//            #if UNITY_IPHONE
//                "96e23e80653bb28980d3f40beb58915c",
//            #elif UNITY_ANDROID
//                "75EF8D155528C04DACBBA6F36F433035"
//            #endif
//        };

    


//        private void Awake()
//        {

//            initializeAdmob();
//        }

//        public void showGoogleConsent()
//        {
//            if (!shouldShowAppTracking || GoogleAppTrackingConstants.hasShownGpAppTrackiing()) {
//                switchToPluginScene();
//                return;

//            }
//            // For dispatching events back onto the Unity main thread.
//            MobileAds.RaiseAdEventsOnUnityMainThread = true;

//            // On Android, Unity is paused when displaying interstitial or rewarded video.
//            // This behavior should be made consistent with iOS.
//            MobileAds.SetiOSAppPauseOnBackground(true);

//            UpdateConsentInformation();

//        }


//        #region Admob
//        private void initializeAdmob()
//        {
//            //   yield return new WaitForSeconds(0);
//            // Demonstrates how to configure Google Mobile Ads.
//            // Google Mobile Ads needs to be run only once and before loading any ads.
//            if (isInitialized)
//            {
//                switchToPluginScene();
//                return;
//            }

//            // On Android, Unity is paused when displaying interstitial or rewarded video.
//            // This setting makes iOS behave consistently with Android.
//            MobileAds.SetiOSAppPauseOnBackground(true);

//            // When true all events raised by GoogleMobileAds will be raised
//            // on the Unity main thread. The default value is false.
//            // https://developers.google.com/admob/unity/quick-start#raise_ad_events_on_the_unity_main_thread
//            MobileAds.RaiseAdEventsOnUnityMainThread = true;

//            // Set your test devices.
//            // https://developers.google.com/admob/unity/test-ads
//            List<string> deviceIds = new List<string>()
//            {
//                AdRequest.TestDeviceSimulator,
//                // Add your test device IDs (replace with your own device IDs).
//                #if UNITY_IPHONE
//                "96e23e80653bb28980d3f40beb58915c"
//                #elif UNITY_ANDROID
//                "75EF8D155528C04DACBBA6F36F433035"
//                #endif
//            };

//            // Configure your RequestConfiguration with Child Directed Treatment
//            // and the Test Device Ids.
//            RequestConfiguration requestConfiguration = new RequestConfiguration
//            {
//                TestDeviceIds = deviceIds
//            };
//            MobileAds.SetRequestConfiguration(requestConfiguration);

//            // Initialize the Google Mobile Ads SDK.
//            Debug.Log("Google Mobile Ads Initializing.");
//            MobileAds.Initialize((InitializationStatus initstatus) =>
//            {
//                if (initstatus == null)
//                {
//                    Debug.LogError("Google Mobile Ads initialization failed.");

//                    switchToPluginScene();
//                    return;
//                }

//            // If you use mediation, you can check the status of each adapter.
//            var adapterStatusMap = initstatus.getAdapterStatusMap();
//                if (adapterStatusMap != null)
//                {
//                    foreach (var item in adapterStatusMap)
//                    {
//                        Debug.Log(string.Format("Adapter {0} is {1}",
//                            item.Key,
//                            item.Value.InitializationState));
//                    }
//                }

//                Debug.Log("Google Mobile Ads initialization complete.");

//                isInitialized = true;
//                showGoogleConsent();

//            });




//        }
//        #endregion

//        #region Google Mobile Ads UMP API

//        /// <summary>
//        /// Clears all consent information from persistent storage.
//        /// </summary>
//        public void ResetConsentInformation()
//        {
//            ConsentInformation.Reset();
//            Debug.Log("Consent information has been reset.");
//        }

//        /// <summary>
//        /// Updates the consent information.
//        /// </summary>
//        public void UpdateConsentInformation()
//        {
//            Debug.Log("Updating consent information.");

//            //    var debugGeography = (DebugGeography)SelectDebugGeography.value;
//            //    var tagForUnderAgeOfConsent = SelectChildUser.value == 1;

//            // Confugre the ConsentDebugSettings.
//            // The ConsentDebugSettings is serializable so you may expose this to your monobehavior.
//            var consentDebugSettings = new ConsentDebugSettings();
//            consentDebugSettings.DebugGeography = debugGeography;
//            consentDebugSettings.TestDeviceHashedIds = TEST_DEVICE_IDS;

//            // Set tag for under age of consent. Here false means users are not under age.
//            var consentRequestParameters = new ConsentRequestParameters();
//            consentRequestParameters.ConsentDebugSettings = consentDebugSettings;
//            consentRequestParameters.TagForUnderAgeOfConsent = tagForUnderAgeOfConsent;

//            ConsentInformation.Update(consentRequestParameters,
//                // OnConsentInformationUpdate
//                (FormError error) =>
//                {


//                    if (error == null)
//                    {
//                    // The consent information updated successfully.
//                    Debug.Log(string.Format(
//                            "Consent information updated to {0}. You may load the consent " +
//                            "form.", ConsentInformation.ConsentStatus));

//                        LoadConsentForm();
//                    }
//                    else
//                    {
//                    // The consent information failed to update.
//                    Debug.LogError("Failed to update consent information with error: " +
//                            error.Message);

//                        switchToPluginScene();
//                    }

//                });
//        }

//        /// <summary>
//        /// Loads a consent form.
//        /// </summary>
//        /// <remarks>
//        /// This should be done before it is needed
//        /// so that you can show the consent form without delay when needed.
//        /// </remarks>
//        public void LoadConsentForm()
//        {
//            Debug.Log("Loading consent form.");

//            ConsentForm.Load(
//                // OnConsentFormLoad
//                (ConsentForm form, FormError error) =>
//                {

//                    if (form != null)
//                    {
//                    // The consent form was loaded.
//                    // We cache the consent form for showing later.
//                    _consentForm = form;
//                        Debug.Log("Consent form is loaded and is ready to show.");
//                        ShowConsentForm();
//                    }
//                    else
//                    {
//                    // The consent form failed to load.
//                    Debug.LogError("Failed to load consent form with error: " +
//                        error == null ? "unknown error" : error.Message);
//                        switchToPluginScene();
//                    }
//                });
//        }

//        /// <summary>
//        /// Shows the consent form. The consent form must be loaded first.
//        /// </summary>
//        public void ShowConsentForm()
//        {
//            _consentForm.Show(
//                 // OnConsentFormShow
//                 (FormError error) =>
//                 {
//                     if (error == null)
//                     {
//                         IDictionary<Firebase.Analytics.ConsentType, Firebase.Analytics.ConsentStatus> consentValues = new Dictionary<Firebase.Analytics.ConsentType, Firebase.Analytics.ConsentStatus>
//                            {
//                                { Firebase.Analytics.ConsentType.AdUserData, Firebase.Analytics.ConsentStatus.Granted },
//                                { Firebase.Analytics.ConsentType.AnalyticsStorage, Firebase.Analytics.ConsentStatus.Granted },
//                                { Firebase.Analytics.ConsentType.AdPersonalization, Firebase.Analytics.ConsentStatus.Granted },
//                                { Firebase.Analytics.ConsentType.AdStorage, Firebase.Analytics.ConsentStatus.Granted }
//                                // Add more entries as needed
//                            };

//                         Firebase.Analytics.FirebaseAnalytics.SetConsent(consentValues);
//                         GoogleAppTrackingConstants.setAppTracking(true);
//                     // If the error parameter is null,
//                     // we showed the consent form without error.
//                     switchToPluginScene();

//                     // Load another consent form for use later.
//                     //  LoadConsentForm();
//                 }
//                     else
//                     {
//                     // The consent form failed to show.

//                     Debug.LogError("Failed to show consent form with error: " +
//                                           error.Message);
//                         switchToPluginScene();

//                     }
//                 });


//        }

//        #endregion

//        #region Helper Functiions
//        private void switchToPluginScene()
//        {
//#if USE_MAX
//            MaxSdk.SetHasUserConsent(true);
//#endif
//            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

//        }
//        #endregion
//    }
//}
