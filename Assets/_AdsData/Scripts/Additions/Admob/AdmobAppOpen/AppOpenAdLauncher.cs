
#if USE_ADMOB_OPEN_AD_7_2_0 || USE_ADMOB_OPEN_AD_8_5
using GoogleMobileAds.Api;
using Sirenix.OdinInspector;
#endif
using UnityEngine;
namespace GVAdmobOpenAds
{
    public class AppOpenAdLauncher : GenericSingletonClass<AppOpenAdLauncher>
    {
#if !USE_MAX_OPENADS
#if USE_ADMOB_OPEN_AD_7_2_0 || USE_ADMOB_OPEN_AD_8_5
       [BoxGroup("INFO")] public bool showShowOpenAdOnOpen = true;

#endif
#endif
        private void OnEnable()
        {
            DontDestroyOnLoad(gameObject);

        }
        private void Start()
        {
#if !USE_MAX_OPENADS
#if USE_ADMOB_OPEN_AD_7_2_0 || USE_ADMOB_OPEN_AD_8_5
            AppOpenAdManager.Instance.showFirstOpen = showShowOpenAdOnOpen;
            AdmobInitlizationManager.admobSdkInitializationAddition += LoadAd;
#endif
#endif
        }

        public void LoadAd() {
#if USE_ADMOB_OPEN_AD_7_2_0 || USE_ADMOB_OPEN_AD_8_5
            AppOpenAdManager.Instance.LoadAd();
#endif
        }


#if !USE_MAX_OPENADS
#if USE_ADMOB_OPEN_AD_7_2_0 || USE_ADMOB_OPEN_AD_8_5

#if UNITY_ANDROID
        private void OnApplicationPause(bool pause)
        {

            if (!pause)
            {

                if (!pause && AppOpenAdManager.ConfigResumeApp && !AdConstants.resumeFromAds)
                {

                    AppOpenAdManager.Instance.ShowAdIfAvailable();
                }
                AdConstants.resumeFromAds = false;
            }

        }
#endif
#if UNITY_IOS
    private void OnApplicationFocus(bool focus)
    {
            if (Application.isEditor) {
                return;
            }
        if (focus)
        {
            

            if (AppOpenAdManager.ConfigResumeApp && !AdConstants.resumeFromAds)
            {

                AppOpenAdManager.Instance.ShowAdIfAvailable();

            }
           AdConstants.resumeFromAds = false;


        }

    }
#endif

#endif
#endif

    }
}